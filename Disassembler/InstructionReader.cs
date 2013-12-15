using System;
using System.IO;

namespace Fantasm.Disassembler
{
    /// <summary>
    /// A class used to read a stream of X86 assembly instructions.
    /// </summary>
    public partial class InstructionReader
    {
        private enum Size
        {
            Word,
            Dword,
            Qword,
        }

        private struct Operand
        {
            public OperandType Type;
            
            /// <summary>
            /// The base register if this is a memory operand, or the register if this is a register operand.
            /// </summary>
            public Register BaseRegister;

            /// <summary>
            /// The index register if this is a memory operand.
            /// </summary>
            public Register IndexRegister;

            /// <summary>
            /// The displacement if this is a memory operand, or the value if this is is an immediate operand.
            /// </summary>
            public int Displacement;

            /// <summary>
            /// The scaling factor if this is a memory operand.
            /// </summary>
            public int Scale;
        }

        private readonly Stream stream;
        private readonly ExecutionModes executionMode;
        private readonly bool default32BitOperands;

        private readonly byte[] buffer = new byte[4];

        private InstructionPrefixes prefixes;
        private RexPrefix rex = RexPrefix.Magic;
        private Instruction instruction = Instruction.Unknown;
        private Operand operand1;
        private Operand operand2;
        private int operandCount;


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
                    return operand1.Type;

                case 1:
                    return operand2.Type;
            }
        }

        /// <summary>
        /// Gets the value of an 8-bit immediate operand.
        /// </summary>
        /// <param name="index">The index of the operand.</param>
        /// <returns>
        /// The value of the operand as a <see cref="byte"/>.
        /// </returns>
        public byte GetOperandByte(int index)
        {
            if (index < 0 || index >= this.operandCount)
                throw new ArgumentException("The index is not valid for this instruction", "index");

            switch (index)
            {
                default:
                    return this.GetOperandByte(ref operand1);

                case 1:
                    return this.GetOperandByte(ref operand2);
            }
        }

        /// <summary>
        /// Gets the value of a 16-bit immediate operand.
        /// </summary>
        /// <param name="index">The index of the operand.</param>
        /// <returns>
        /// The value of the operand as a <see cref="short"/>.
        /// </returns>
        public short GetOperandWord(int index)
        {
            if (index < 0 || index >= this.operandCount)
                throw new ArgumentException("The index is not valid for this instruction", "index");

            switch (index)
            {
                default:
                    return this.GetOperandWord(ref operand1);

                case 1:
                    return this.GetOperandWord(ref operand2);
            }
        }

        /// <summary>
        /// Gets the value of a 32-bit immediate operand.
        /// </summary>
        /// <param name="index">The index of the operand.</param>
        /// <returns>
        /// The value of the operand as an <see cref="int"/>.
        /// </returns>
        public int GetOperandDword(int index)
        {
            if (index < 0 || index >= this.operandCount)
                throw new ArgumentException("The index is not valid for this instruction", "index");

            switch (index)
            {
                default:
                    return this.GetOperandDword(ref operand1);

                case 1:
                    return this.GetOperandDword(ref operand2);
            }
        }

        /// <summary>
        /// Gets the value of a register operand.
        /// </summary>
        /// <param name="index">The index of the operand.</param>
        /// <returns>
        /// The value of the operand as a <see cref="Register"/>.
        /// </returns>
        public Register GetOperandRegister(int index)
        {
            if (index < 0 || index >= this.operandCount)
            {
                throw new ArgumentException("The index is not valid for this instruction", "index");
            }

            if (this.operand1.Type != OperandType.Register)
            {
                throw new InvalidOperationException("The specified operand is not a register");
            }

            return this.operand1.BaseRegister;
        }

        /// <summary>
        /// Gets the base register of a memory access operand.
        /// </summary>
        /// <param name="index">The index of the operand.</param>
        /// <returns>
        /// A member of the <see cref="Register"/> enumeration.
        /// </returns>
        /// <remarks>
        /// This is <c>BaseRegister</c> in the formula <c>[BaseRegister + IndexRegister*Scale + Displacement]</c>.
        /// </remarks>
        public Register GetOperandBaseRegister(int index)
        {
            if (index < 0 || index >= this.operandCount)
            {
                throw new ArgumentException("The index is not valid for this instruction", "index");
            }

            if (index != 0 || this.operand1.Type != OperandType.Memory)
            {
                throw new InvalidOperationException("The specified operand is not a memory access");
            }

            return this.operand1.BaseRegister;
        }

        /// <summary>
        /// Gets the index register of a memory access operand.
        /// </summary>
        /// <param name="index">The index of the operand.</param>
        /// <returns>
        /// A member of the <see cref="Register"/> enumeration.
        /// </returns>
        /// <remarks>
        /// This is <c>IndexRegister</c> in the formula <c>[BaseRegister + IndexRegister*Scale + Displacement]</c>.
        /// </remarks>
        public Register GetOperandIndexRegister(int index)
        {
            if (index < 0 || index >= this.operandCount)
            {
                throw new ArgumentException("The index is not valid for this instruction", "index");
            }

            if (index != 0 || this.operand1.Type != OperandType.Memory)
            {
                throw new InvalidOperationException("The specified operand is not a memory access");
            }

            return operand1.IndexRegister;
        }

        /// <summary>
        /// Gets the scale parameter of a memory access operand.
        /// </summary>
        /// <param name="index">The index of the operand.</param>
        /// <returns>
        /// Either 1, 2, 4 or 8.
        /// </returns>
        /// <remarks>
        /// This is <c>Scale</c> in the formula <c>[BaseRegister + IndexRegister*Scale + Displacement]</c>.
        /// </remarks>
        public int GetOperandScale(int index)
        {
            if (index < 0 || index >= this.operandCount)
            {
                throw new ArgumentException("The index is not valid for this instruction", "index");
            }

            if (index != 0 || this.operand1.Type != OperandType.Memory)
            {
                throw new InvalidOperationException("The specified operand is not a memory access");
            }

            return operand1.Scale;
        }

        /// <summary>
        /// Gets the displacement parameter of a memory access operand.
        /// </summary>
        /// <param name="index">The index of the operand.</param>
        /// <returns>
        /// The displacement, in bytes.
        /// </returns>
        /// <remarks>
        /// This is <c>Displacement</c> in the formula <c>[BaseRegister + IndexRegister*Scale + Displacement]</c>.
        /// </remarks>
        public int GetOperandDisplacement(int index)
        {
            if (index < 0 || index >= this.operandCount)
            {
                throw new ArgumentException("The index is not valid for this instruction", "index");
            }

            if (index != 0 || this.operand1.Type != OperandType.Memory)
            {
                throw new InvalidOperationException("The specified operand is not a memory access");
            }

            return this.operand1.Displacement;
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
                    case 0x04:
                        this.ReadInstruction(Instruction.Add, ExecutionModes.All);
                        this.operandCount = 2;
                        this.operand1.Type = OperandType.Register;
                        this.operand1.BaseRegister = Register.Al;
                        this.ReadImmediateByte(ref operand2);
                        return true;

                    case 0x05:
                        this.ReadInstruction(Instruction.Add, ExecutionModes.All);
                        this.operandCount = 2;
                        this.operand1.Type = OperandType.Register;
                        switch (this.GetOperandSize())
                        {
                            case Size.Qword:
                                this.operand1.BaseRegister = Register.Rax;
                                this.ReadImmediateDword(ref operand2);
                                break;

                            case Size.Dword:
                                this.operand1.BaseRegister = Register.Eax;
                                this.ReadImmediateDword(ref operand2);
                                break;

                            case Size.Word:
                                this.operand1.BaseRegister = Register.Ax;
                                this.ReadImmediateWord(ref operand2);
                                break;
                        }
                        return true;
                        
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
                        return this.ReadGroup1();

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
                        this.instruction = Instruction.Unknown;
                        this.operandCount = 0;
                        throw new NotImplementedException();
                }
            }
        }

        private bool ReadGroup1()
        {
            var modrm = this.stream.ReadByte();
            if (modrm < 0)
            {
                throw InvalidInstructionBytes();
            }

            var opCode = (modrm & 0x38) >> 3;
            switch (opCode)
            {
                case 0:
                    this.instruction = Instruction.Add;
                    break;

                default:
                    throw new NotImplementedException();
            }

            this.operandCount = 2;
            this.ReadModRMOperand(modrm);
            this.ReadImmediateByte(ref operand2);

            if (this.operand1.Type != OperandType.Memory && (this.prefixes & InstructionPrefixes.Lock) != 0)
            {
                throw InvalidInstructionBytes();
            }

            return true;
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
            this.ReadImmediateByte(ref this.operand1);
            // special case: AAD is a synonym for AAD 0AH
            if (this.operand1.Displacement == 0x0A)
            {
                this.operandCount = 0;
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

        private void ReadImmediateByte(ref Operand operand)
        {
            var value = this.stream.ReadByte();
            if (value < 0)
            {
                throw InvalidInstructionBytes();
            }

            operand.Type = OperandType.ImmediateByte;
            operand.Displacement = value;
        }

        private void ReadImmediateWord(ref Operand operand)
        {
            var bytesRead = this.stream.Read(this.buffer, 0, 2);
            if (bytesRead < 2)
            {
                throw InvalidInstructionBytes();
            }

            operand.Type = OperandType.ImmediateWord;
            operand.Displacement = BitConverter.ToInt16(this.buffer, 0);
        }

        private void ReadImmediateDword(ref Operand operand)
        {
            var bytesRead = this.stream.Read(this.buffer, 0, 4);
            if (bytesRead < 4)
            {
                throw InvalidInstructionBytes();
            }

            operand.Type = OperandType.ImmediateDword;
            operand.Displacement = BitConverter.ToInt32(this.buffer, 0);
        }

        private byte GetOperandByte(ref Operand operand)
        {
            if (operand.Type != OperandType.ImmediateByte)
            {
                throw new InvalidOperationException("The specified operand is not a byte");
            }

            return (byte)operand.Displacement;
        }

        private short GetOperandWord(ref Operand operand)
        {
            if (operand.Type != OperandType.ImmediateWord)
            {
                throw new InvalidOperationException("The specified operand is not a word");
            }

            return (short)operand.Displacement;
        }

        private int GetOperandDword(ref Operand operand)
        {
            if (operand.Type != OperandType.ImmediateDword)
            {
                throw new InvalidOperationException("The specified operand is not a dword");
            }

            return operand.Displacement;
        }

        private static Exception InvalidInstructionBytes()
        {
            return new FormatException("The data does not represent a valid instruction.");
        }
    }
}
