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
            Byte,
            Word,
            Dword,
            Qword,
        }

        private readonly Stream stream;
        private readonly ExecutionModes executionMode;
        private readonly Size defaultOperandSize;
        private readonly Size defaultAddressSize;

        private readonly byte[] buffer = new byte[4];

        private InstructionPrefixes prefixes;
        private RexPrefix rex = RexPrefix.Magic;

        private bool locked;
        private Instruction instruction = Instruction.Unknown;

        private int operandCount;
        private OperandType operand1;
        private OperandType operand2;
        private Register register;
        private Register baseRegister;
        private Register indexRegister;
        private int scale;
        private int displacement;
        private int immediate;
        
        private short codeSegment;

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
            this.defaultOperandSize = (default32BitOperands || executionMode == ExecutionMode.Long64Bit)
                ? Size.Dword
                : Size.Word;
            this.defaultAddressSize = default32BitOperands ? Size.Dword : Size.Word;
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
        /// Gets the displacement parameter of a memory access operand, the value of a relative address operand, or
        /// the offset from a far pointer.
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
        /// Gets the code segment selector from a far pointer operand.
        /// </summary>
        /// <returns>
        /// The selector for the code segment that the pointer resides in.
        /// </returns>
        public short GetSegmentSelector()
        {
            return this.codeSegment;
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
            if (!this.ReadInstruction())
            {
                return false;
            }

            // if the lock prefix wasn't handled
            if ((this.prefixes & InstructionPrefixes.Lock) != 0 && !this.locked)
            {
                throw InvalidInstructionBytes();
            }
            return true;
        }

        private bool ReadInstruction()
        {
            this.prefixes = InstructionPrefixes.None;
            this.locked = false;
            this.rex = 0;
            this.instruction = Instruction.Unknown;
            this.operandCount = 0;
            this.immediate = this.displacement = 0;
            this.register = this.baseRegister = this.indexRegister = Register.None;
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
                        return this.ReadBinaryArithmetic(Instruction.Add, nextByte);
                    
                    case 0x0F:
                        return this.ReadTwoByteInstruction();

                    case 0x10:
                    case 0x11:
                    case 0x12:
                    case 0x13:
                    case 0x14:
                    case 0x15:
                        return this.ReadBinaryArithmetic(Instruction.Adc, nextByte);

                    case 0x20:
                    case 0x21:
                    case 0x22:
                    case 0x23:
                    case 0x24:
                    case 0x25:
                        return this.ReadBinaryArithmetic(Instruction.And, nextByte);

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

                    case 0x62:
                        if (this.executionMode == ExecutionModes.Long64Bit)
                        {
                            throw InvalidInstructionBytes();
                        }
                        return this.Read_Reg_RM(Instruction.Bound, this.GetOperandSize());
                    case 0x63:
                        if (this.executionMode == ExecutionModes.Long64Bit)
                        {
                            throw new NotImplementedException();
                        }
                        return this.Read_RM_Reg(Instruction.Arpl, Size.Word);
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
                        return this.LockIfMemory();
                    }

                    case 0x90:
                        return this.ReadInstructionNoOperands(Instruction.Nop, ExecutionModes.All);

                    case 0x9A:
                        if (this.executionMode == ExecutionModes.Long64Bit)
                        {
                            throw InvalidInstructionBytes();
                        }
                        this.instruction = Instruction.Call;
                        this.operandCount = 1;
                        this.operand1 = this.FarPointer(this.GetOperandSize());
                        return true;

                    case 0xD4:
                        return this.ReadAsciiAdjustWithBase(Instruction.Aam);
                    case 0xD5:
                        return this.ReadAsciiAdjustWithBase(Instruction.Aad);

                    case 0xE8:
                        return this.Read_Jz(Instruction.Call);

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

                    case 0xFF:
                    {
                        var operandSize = this.executionMode == ExecutionModes.Long64Bit
                            ? Size.Qword
                            : this.GetOperandSize();

                        var modrm = this.ReadByte();
                        bool isMemory;
                        this.instruction = this.GetGroup5Opcode(modrm, out isMemory);
                        this.operandCount = 1;
                        this.operand1 = this.RegisterOrMemory(modrm, operandSize);

                        if (isMemory && operand1 != OperandType.Memory)
                            throw InvalidInstructionBytes();

                        return true;
                    }

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private bool ReadTwoByteInstruction()
        {
            var opCodeByte = this.ReadByte();

            switch (opCodeByte)
            {
                case 0x0b:
                    return this.ReadInstructionNoOperands(Instruction.Ud2, ExecutionModes.All);
                case 0xA3:
                    return this.Read_RM_Reg(Instruction.Bt, this.GetOperandSize());

                case 0xAB:
                    return this.Read_RM_Reg(Instruction.Bts, this.GetOperandSize());

                case 0xB3:
                    return this.Read_RM_Reg(Instruction.Btr, this.GetOperandSize());

                case 0xB9:
                {
                    // Group 10
                    var modrm = this.ReadByte();
                    return this.ReadInstructionNoOperands(Instruction.Ud1, ExecutionModes.All);
                }
                case 0xBA:
                {
                    var modrm = this.ReadByte();
                    this.instruction = this.GetGroup8Opcode(modrm);
                    this.operandCount = 2;
                    this.operand1 = this.RegisterOrMemory(modrm, this.GetBaseRegister(this.GetOperandSize()));
                    this.operand2 = this.ImmediateByte();
                    return true;
                }
                case 0xBB:
                    return this.Read_RM_Reg(Instruction.Btc, this.GetOperandSize());
                case 0xBC:
                    return this.Read_Reg_RM(Instruction.Bsf, this.GetOperandSize());
                case 0xBD:
                    return this.Read_Reg_RM(Instruction.Bsr, this.GetOperandSize());

                case 0xC8:
                case 0xC9:
                case 0xCA:
                case 0xCB:
                case 0xCC:
                case 0xCD:
                case 0xCE:
                case 0xCF:
                    this.instruction = Instruction.Bswap;
                    this.operandCount = 1;
                    var operandSize = this.GetOperandSize();
                    if (operandSize == Size.Word)
                    {
                        throw this.UndefinedBehaviour();
                    }
                    this.operand1 = OperandType.Register;
                    this.register = this.GetRegister(RexPrefix.B, opCodeByte & 0x7, this.GetBaseRegister(operandSize));
                    return true;

                default:
                    throw new NotImplementedException();
            }
        }

        private bool ReadBinaryArithmetic(Instruction instruction, int opCode)
        {
            var size = (opCode & 1) == 0 ? Size.Byte : this.GetOperandSize();
            
            switch (opCode & 7)
            {
                case 0x00:
                case 0x01:
                    this.Read_RM_Reg(instruction, size);
                    break;

                case 0x02:
                case 0x03:
                    this.Read_Reg_RM(instruction, size);
                    break;
                
                default:
                    this.Read_rAx_Imm(instruction, size);
                    break;
            }

            return this.LockIfMemory();
        }

        private bool LockIfMemory()
        {
            if (this.operand1 == OperandType.Memory && (this.prefixes & InstructionPrefixes.Lock) != 0)
            {
                this.locked = true;
            }
            return true;
        }

        private bool Read_Jz(Instruction instruction)
        {
            this.instruction = instruction;
            this.operandCount = 1;
            this.operand1 = OperandType.RelativeAddress;
            this.ReadDisplacement(this.GetOperandSize());
            return true;
        }

        private bool Read_rAx_Imm(Instruction instruction, Size size)
        {
            this.instruction = instruction;
            this.operandCount = 2;
            var operandBaseRegister = this.GetBaseRegister(size);
            this.operand1 = this.ImplicitRegister(operandBaseRegister);
            this.operand2 = this.Immediate(size);
            return true;
        }

        private bool Read_Reg_RM(Instruction instruction, Size size)
        {
            this.instruction = instruction;
            this.operandCount = 2;
            var operandBaseRegister = this.GetBaseRegister(size); 
            var modrm = this.ReadByte();
            this.operand1 = this.ModRMRegister(modrm, operandBaseRegister);
            this.operand2 = this.RegisterOrMemory(modrm, operandBaseRegister);
            return true;
        }

        private bool Read_RM_Reg(Instruction instruction, Size size)
        {
            this.instruction = instruction;
            this.operandCount = 2;
            var operandBaseRegister = this.GetBaseRegister(size);
            var modrm = this.ReadByte();
            this.operand1 = this.RegisterOrMemory(modrm, operandBaseRegister);
            this.operand2 = this.ModRMRegister(modrm, operandBaseRegister);
            return true;
        }

        private bool Read_RM_Imm(Instruction instruction, Size registerSize, Size immediateSize)
        {
            this.instruction = instruction;
            this.operandCount = 2;
            var operandBaseRegister = this.GetBaseRegister(registerSize);
            var modrm = this.ReadByte();
            this.operand1 = this.RegisterOrMemory(modrm, operandBaseRegister);
            this.operand2 = this.Immediate(immediateSize);
            return true;
        }

        private Instruction GetGroup1Opcode(int modrm)
        {
            var opCode = (modrm & 0x38) >> 3;
            switch (opCode)
            {
                case 0:
                    return Instruction.Add;

                case 2:
                    return Instruction.Adc;

                case 4:
                    return Instruction.And;

                default:
                    throw new NotImplementedException();
            }
        }

        private Instruction GetGroup5Opcode(int modrm, out bool isMemory)
        {
            var opCode = (modrm & 0x38) >> 3;
            switch (opCode)
            {
                case 2:
                    isMemory = false;
                    return Instruction.Call;

                case 3:
                    isMemory = true;
                    return Instruction.Call;

                default:
                    throw new NotImplementedException();
            }
        }
        private Instruction GetGroup8Opcode(int modrm)
        {
            var opCode = (modrm & 0x38) >> 3;
            switch (opCode)
            {
                case 4:
                    return Instruction.Bt;

                case 5:
                    return Instruction.Bts;

                case 6:
                    return Instruction.Btr;

                case 7:
                    return Instruction.Btc;

                default:
                    throw InvalidInstructionBytes();
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
            if ((this.rex & (RexPrefix.W)) != 0)
            {
                return Size.Qword;
            }

            var operandSizeOverride = ((this.prefixes & InstructionPrefixes.OperandSizeOverride) != 0);
            return this.defaultOperandSize == Size.Word
                ? (operandSizeOverride ? Size.Dword : Size.Word)
                : (operandSizeOverride ? Size.Word : this.defaultOperandSize);
        }

        private Size GetAddressSize()
        {
            var addressSizeOverride = ((this.prefixes & InstructionPrefixes.AddressSizeOverride) != 0);

            if ((this.executionMode & ExecutionModes.Long64Bit) != 0)
            {
                return addressSizeOverride ? Size.Dword : Size.Qword;
            }
            
            return ((this.defaultAddressSize == Size.Dword) ^ addressSizeOverride) ? Size.Dword : Size.Word;
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

        private void ReadDisplacement(Size size)
        {
            if (size == Size.Word)
            {
                this.displacement = this.ReadWord();
            }
            else
            {
                this.displacement = this.ReadDword();
            }
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
                    return Register.Rax;
                case Size.Dword:
                    return Register.Eax;
                case Size.Word:
                    return Register.Ax;
                default:
                    return Register.Al;
            }
        }

        private OperandType RegisterOrMemory(byte modrm, Size operandSize)
        {
            return this.RegisterOrMemory(modrm, this.GetBaseRegister(operandSize));
        }

        private OperandType FarPointer(Size size)
        {
            this.codeSegment = this.ReadWord();
            this.ReadDisplacement(size);
            return OperandType.FarPointer;
        }

        private static Exception InvalidInstructionBytes()
        {
            return new FormatException("The data does not represent a valid instruction.");
        }

        private Exception UndefinedBehaviour()
        {
            return new FormatException("The behaviour of the specified instuction is undefined.");
        }
    }
}
