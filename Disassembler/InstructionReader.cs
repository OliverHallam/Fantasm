using System;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace Fantasm.Disassembler
{
    /// <summary>
    /// A class used to read a stream of X86 assembly instructions.
    /// </summary>
    public partial class InstructionReader
    {
        private enum Size
        {
            Byte,
            Word,
            Dword,
            Qword,
        }

        private readonly Stream stream;
        private readonly ExecutionModes executionMode;
        private readonly bool default32BitOperands;

        private readonly byte[] buffer = new byte[4];

        private InstructionPrefixes prefixes;
        private RexPrefix rex = RexPrefix.Magic;
        private Instruction instruction = Instruction.Unknown;
        private OperandType operand1;
        private OperandType operand2;
        private int operandCount;

        private Register register;
        private Register baseRegister;
        private Register indexRegister;
        private int scale;
        private int displacement;
        private int immediate;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstructionReader"/> class.
        /// </summary>
        /// <param name="stream">The stream to read the instruction data from.</param>
        /// <param name="executionMode">The execution mode.</param>
        /// <param name="default32BitOperands">
        /// <see langword="true"/> if the default operand size is 32 bits; otherwise <see langword="false" />.  This
        /// corresponds to the D flag in segment descriptor for the code segment block.
        /// </param>
        public InstructionReader(Stream stream, ExecutionMode executionMode, bool default32BitOperands)
        {
            this.stream = stream;
            this.executionMode = executionMode.ToExecutionModes();
            this.default32BitOperands = default32BitOperands || executionMode == ExecutionMode.Long64Bit;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstructionReader"/> class.
        /// </summary>
        /// <param name="stream">The stream to read the instruction data from.</param>
        /// <param name="executionMode">The execution mode.</param>
        public InstructionReader(Stream stream, ExecutionMode executionMode)
            : this(stream, executionMode, true)
        {
        }

        /// <summary>
        /// Gets the current instruction.
        /// </summary>
        public Instruction Instruction
        {
            get
            {
                return this.instruction;
            }
        }

        /// <summary>
        /// Gets the number of operands for the current instruction.
        /// </summary>
        public int OperandCount
        {
            get
            {
                return this.operandCount;
            }
        }

        /// <summary>
        /// Gets the type of the specified operand.
        /// </summary>
        /// <param name="index">
        /// The index of the operand.
        /// </param>
        /// <returns>
        /// A member of the <see cref="OperandType"/> enumeration.
        /// </returns>
        public OperandType GetOperandType(int index)
        {
            if (index < 0 || index >= this.operandCount)
                throw new ArgumentException("The index is not valid for this instruction", "index");

            switch (index)
            {
                default:
                    return operand1;

                case 1:
                    return operand2;
            }
        }

        /// <summary>
        /// Gets the value of the immediate operand.
        /// </summary>
        /// <returns>
        /// The value of the immediate operand, or <c>0</c> if there was none.
        /// </returns>
        /// <remarks>
        /// This is the value of any operand with type <see cref="OperandType.ImmediateByte"/>,
        /// <see cref="OperandType.ImmediateWord"/> or <see cref="OperandType.ImmediateDword"/>
        /// </remarks>
        public int GetImmediateValue()
        {
            return this.immediate;
        }

        /// <summary>
        /// Gets The value of the register operand.
        /// </summary>
        /// <returns>
        /// The value of the register operand, or <see cref="Disassembler.Register.None"/> if there was none.
        /// </returns>
        /// <remarks>
        /// This is the value of any operand with type <see cref="OperandType.Register"/>.
        /// </remarks>
        public Register GetRegister()
        {
            return this.register;
        }

        /// <summary>
        /// Gets the base register of a memory access operand, or the register in place of a memory access.
        /// </summary>
        /// <returns>
        /// A member of the <see cref="register"/> enumeration.
        /// </returns>
        /// <remarks>
        /// This is <c>baseRegister</c> in the formula <c>[baseRegister + indexRegister*Scale + Displacement]</c>.
        /// </remarks>
        public Register GetBaseRegister()
        {
            return this.baseRegister;
        }

        /// <summary>
        /// Gets the index register of a memory access operand.
        /// </summary>
        /// <returns>
        /// A member of the <see cref="register"/> enumeration.
        /// </returns>
        /// <remarks>
        /// This is <c>indexRegister</c> in the formula <c>[baseRegister + indexRegister*Scale + Displacement]</c>.
        /// </remarks>
        public Register GetIndexRegister()
        {
            return this.indexRegister;
        }

        /// <summary>
        /// Gets the scale parameter of a memory access operand.
        /// </summary>
        /// <returns>
        /// Either 1, 2, 4 or 8.
        /// </returns>
        /// <remarks>
        /// This is <c>Scale</c> in the formula <c>[baseRegister + indexRegister*Scale + Displacement]</c>.
        /// </remarks>
        public int GetScale()
        {
            return this.scale;
        }

        /// <summary>
        /// Gets the displacement parameter of a memory access operand.
        /// </summary>
        /// <returns>
        /// The displacement, in bytes.
        /// </returns>
        /// <remarks>
        /// This is <c>Displacement</c> in the formula <c>[baseRegister + indexRegister*Scale + Displacement]</c>.
        /// </remarks>
        public int GetDisplacement()
        {
            return this.displacement;
        }

        /// <summary>
        /// Attempts to read the next instruction from the stream.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if there are more isntructions; <see langword="false" /> if the reader has reached
        /// the end of the stream.
        /// </returns>
        public bool Read()
        {
            this.prefixes = InstructionPrefixes.None;
            this.rex = 0;
            this.instruction = Instruction.Unknown;
            this.operandCount = 0;
            this.immediate = this.displacement = 0;
            this.register = this.baseRegister = this.indexRegister = Disassembler.Register.None;
            this.scale = 1;

            bool readAny = false;

            while (true)
            {
                var nextByte = this.stream.ReadByte();
                if (nextByte < 0)
                {
                    if (readAny)
                    {
                        throw InvalidInstructionBytes();
                    }
                    return false;
                }
                readAny = true;

                switch (nextByte)
                {
                    case 0x00:
                    case 0x01:
                    case 0x02:
                    case 0x03:
                    case 0x04:
                    case 0x05:
                        return this.ReadAdd(nextByte);

                    case 0x26:
                        this.ReadPrefix(InstructionPrefixes.Group2Mask, InstructionPrefixes.SegmentES);
                        continue;
                    case 0x2E:
                        this.ReadPrefix(InstructionPrefixes.Group2Mask, InstructionPrefixes.SegmentCS);
                        continue;
                    
                    case 0x36:
                        this.ReadPrefix(InstructionPrefixes.Group2Mask, InstructionPrefixes.SegmentSS);
                        continue;
                    case 0x37:
                        return this.ReadInstructionNoOperands(Instruction.Aaa, ExecutionModes.CompatibilityMode);

                    case 0x3E:
                        this.ReadPrefix(InstructionPrefixes.Group2Mask, InstructionPrefixes.SegmentDS);
                        continue;
                    case 0x3F:
                        return this.ReadInstructionNoOperands(Instruction.Aas, ExecutionModes.CompatibilityMode);

                       
                    case 0x40:
                    case 0x41:
                    case 0x42:
                    case 0x43:
                    case 0x44:
                    case 0x45:
                    case 0x46:
                    case 0x47:
                    case 0x48:
                    case 0x49:
                    case 0x4A:
                    case 0x4B:
                    case 0x4C:
                    case 0x4D:
                    case 0x4E:
                    case 0x4F:
                        if (this.executionMode == ExecutionModes.Long64Bit)
                        {
                            this.ReadRexPrefix(nextByte);
                            continue;
                        }
                        goto default;

                    case 0x64:
                        this.ReadPrefix(InstructionPrefixes.Group2Mask, InstructionPrefixes.SegmentES);
                        continue;
                    case 0x65:
                        this.ReadPrefix(InstructionPrefixes.Group2Mask, InstructionPrefixes.SegmentGS);
                        continue;
                    case 0x66:
                        this.ReadPrefix(InstructionPrefixes.Group3Mask, InstructionPrefixes.OperandSizeOverride);
                        continue;
                    case 0x67:
                        this.ReadPrefix(InstructionPrefixes.Group4Mask, InstructionPrefixes.AddressSizeOverride);
                        continue;

                    case 0x80:
                    case 0x81:
                    case 0x82:
                    case 0x83:
                    {
                        var operandSize = this.GetOperandSize();
                        var registerSize = ((nextByte & 1) != 0) ? operandSize : Size.Byte;
                        var immediateSize = nextByte == 0x81 ? operandSize : Size.Byte;
                        
                        var modrm = this.ReadByte();
                        this.instruction = this.GetGroup1Opcode(modrm);
                        this.operandCount = 2;
                        this.operand1 = this.RegisterOrMemory(modrm, registerSize);
                        this.operand2 = this.Immediate(immediateSize);
                        this.CheckLockValid();
                        return true;
                    }

                    case 0x90:
                        return this.ReadInstructionNoOperands(Instruction.Nop, ExecutionModes.All);

                    case 0xD4:
                        return this.ReadAsciiAdjustWithBase(Instruction.Aam);
                    case 0xD5:
                        return this.ReadAsciiAdjustWithBase(Instruction.Aad);

                    case 0xF0:
                        this.ReadPrefix(InstructionPrefixes.Group1Mask, InstructionPrefixes.Lock);
                        continue;
                    case 0xF1:
                        throw InvalidInstructionBytes();
                    case 0xF2:
                        this.ReadPrefix(InstructionPrefixes.Group1Mask, InstructionPrefixes.RepNE);
                        continue;
                    case 0xF3:
                        this.ReadPrefix(InstructionPrefixes.Group1Mask, InstructionPrefixes.Rep);
                        continue;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private bool ReadAdd(int opCode)
        {
            var size = (opCode & 1) == 0 ? Size.Byte : this.GetOperandSize();
            var operandBaseRegister = this.GetBaseRegister(size);

            this.instruction = Instruction.Add;
            this.operandCount = 2;

            switch (opCode & 7)
            {
                case 0x00:
                case 0x01:
                {
                    var modrm = this.ReadByte();
                    this.operand1 = this.RegisterOrMemory(modrm, operandBaseRegister);
                    this.operand2 = this.Register(modrm, operandBaseRegister);
                    this.CheckLockValid();
                    break;
                }

                case 0x02:
                case 0x03:
                {
                    var modrm = this.ReadByte();
                    this.operand1 = this.Register(modrm, operandBaseRegister);
                    this.operand2 = this.RegisterOrMemory(modrm, operandBaseRegister);
                    this.CheckLockValid();
                    break;
                }

                case 0x04:
                case 0x05:
                    this.operand1 = this.ImplicitRegister(operandBaseRegister);
                    this.operand2 = this.Immediate(size);
                    this.DisallowLock();
                    break;
            }

            return true;
        }

        private void CheckLockValid()
        {
            if (this.operand1 != OperandType.Memory)
            {
                this.DisallowLock();
            }
        }
        
        private void DisallowLock()
        {
            if ((this.prefixes & InstructionPrefixes.Lock) != 0)
            {
                throw InvalidInstructionBytes();
            }
        }
        
        private Instruction GetGroup1Opcode(int modrm)
        {
            var opCode = (modrm & 0x38) >> 3;
            switch (opCode)
            {
                case 0:
                    return Instruction.Add;

                default:
                    throw new NotImplementedException();
            }
        }

        private byte ReadByte()
        {
            var modrm = this.stream.ReadByte();
            if (modrm < 0)
            {
                throw InvalidInstructionBytes();
            }
            return (byte)modrm;
        }

        private void ReadRexPrefix(int nextByte)
        {
            this.rex = (RexPrefix)nextByte;
        }

        private Size GetOperandSize()
        {
            var operandSizeOverride = ((this.prefixes & InstructionPrefixes.OperandSizeOverride) != 0);

            if ((this.rex & (RexPrefix.W)) != 0)
            {
                return Size.Qword;
            }
            
            return (this.default32BitOperands ^ operandSizeOverride) ? Size.Dword : Size.Word;
        }

        private Size GetAddressSize()
        {
            var addressSizeOverride = ((this.prefixes & InstructionPrefixes.AddressSizeOverride) != 0);

            if ((this.executionMode & ExecutionModes.Long64Bit) != 0)
            {
                return addressSizeOverride ? Size.Dword : Size.Qword;
            }
            
            return (this.default32BitOperands ^ addressSizeOverride) ? Size.Dword : Size.Word;
        }

        private void ReadPrefix(InstructionPrefixes group, InstructionPrefixes prefix)
        {
            if (this.rex != 0)
            {
                // REX prefix must appear after all other prefixes.
                throw InvalidInstructionBytes();
            }

            if ((this.prefixes & group) != 0)
            {
                throw InvalidInstructionBytes();
            }

            this.prefixes |= prefix;
        }

        /// <summary>
        /// Reads an instruction that takes no operands.
        /// </summary>
        /// <param name="instruction">The instruction mnemonic.</param>
        /// <param name="executionModes">The execution modes that are supported for the instruction.</param>
        /// <returns>
        /// Always returns <see langword="true" />
        /// </returns>
        private bool ReadInstructionNoOperands(Instruction instruction, ExecutionModes executionModes)
        {
            this.ReadInstruction(instruction, executionModes);

            this.operandCount = 0;
            return true;
        }

        private bool ReadAsciiAdjustWithBase(Instruction instruction)
        {
            this.ReadInstruction(instruction, ExecutionModes.CompatibilityMode);
            this.operandCount = 1;
            this.operand1 = this.ImmediateByte();
            // special case: AAD is a synonym for AAD 0AH
            if (this.immediate == 0x0A)
            {
                this.operandCount = 0;
                this.immediate = 0;
            }
            return true;
        }

        private void ReadInstruction(Instruction instruction, ExecutionModes executionModes)
        {
            if ((this.prefixes & InstructionPrefixes.Lock) != 0)
            {
                throw InvalidInstructionBytes();
            }

            if ((this.executionMode & executionModes) == 0)
            {
                throw InvalidInstructionBytes();
            }

            this.instruction = instruction;
        }

        private short ReadWord()
        {
            var bytesRead = this.stream.Read(this.buffer, 0, 2);
            if (bytesRead < 2)
            {
                throw InvalidInstructionBytes();
            }

            return BitConverter.ToInt16(this.buffer, 0);
        }

        private int ReadDword()
        {
            var bytesRead = this.stream.Read(this.buffer, 0, 4);
            if (bytesRead < 4)
            {
                throw InvalidInstructionBytes();
            }

            return BitConverter.ToInt32(this.buffer, 0);
        }

        private OperandType Immediate(Size size)
        {
            switch (size)
            {
                case Size.Byte:
                    return this.ImmediateByte();
                case Size.Word:
                    return this.ImmediateWord();
                default:
                    return this.ImmediateDword();
            }
        }

        private OperandType ImmediateDword()
        {
            this.immediate = this.ReadDword();
            return OperandType.ImmediateDword;
        }

        private OperandType ImmediateWord()
        {
            this.immediate = this.ReadWord();
            return OperandType.ImmediateWord;
        }

        private OperandType ImmediateByte()
        {
            this.immediate = this.ReadByte();
            return OperandType.ImmediateByte;
        }

        private OperandType ImplicitRegister(Register register)
        {
            this.register = register;
            return OperandType.Register;
        }

        private Register GetBaseRegister(Size operandSize)
        {
            switch (operandSize)
            {
                case Size.Qword:
                    return Disassembler.Register.Rax;
                case Size.Dword:
                    return Disassembler.Register.Eax;
                case Size.Word:
                    return Disassembler.Register.Ax;
                default:
                    return Disassembler.Register.Al;
            }
        }

        private OperandType RegisterOrMemory(byte modrm, Size operandSize)
        {
            return this.RegisterOrMemory(modrm, this.GetBaseRegister(operandSize));
        }

        private static Exception InvalidInstructionBytes()
        {
            return new FormatException("The data does not represent a valid instruction.");
        }
    }
}
