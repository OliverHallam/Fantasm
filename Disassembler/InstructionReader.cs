using System;
using System.IO;

namespace Fantasm.Disassembler
{
    /// <summary>
    /// A class used to read a stream of X86 assembly instructions.
    /// </summary>
    public class InstructionReader
    {
        #region Fields

        private readonly Size defaultAddressSize;
        private readonly Size defaultOperandSize;
        private readonly ExecutionModes executionMode;

        private readonly InstructionByteStream instructionByteStream;
        private Register baseRegister;
        private short codeSegment;
        private int displacement;
        private int immediate;
        private Register indexRegister;

        private Instruction instruction = Instruction.Unknown;
        private bool locked;

        private OperandType operand1;
        private OperandType operand2;
        private int operandCount;
        private InstructionPrefixes prefixes;
        private Register register;
        private RexPrefix rex = RexPrefix.Magic;
        private int scale;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="InstructionReader" /> class.
        /// </summary>
        /// <param name="stream">The stream to read the instruction data from.</param>
        /// <param name="executionMode">The execution mode.</param>
        /// <param name="default32BitOperands">
        ///     <see langword="true" /> if the default operand size is 32 bits; otherwise <see langword="false" />.  This
        ///     corresponds to the D flag in segment descriptor for the code segment block.
        /// </param>
        public InstructionReader(Stream stream, ExecutionMode executionMode, bool default32BitOperands)
        {
            this.instructionByteStream = new InstructionByteStream(stream);
            this.executionMode = executionMode.ToExecutionModes();
            this.defaultOperandSize = (default32BitOperands || executionMode == ExecutionMode.Long64Bit)
                ? Size.Dword
                : Size.Word;
            this.defaultAddressSize = default32BitOperands ? Size.Dword : Size.Word;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InstructionReader" /> class.
        /// </summary>
        /// <param name="stream">The stream to read the instruction data from.</param>
        /// <param name="executionMode">The execution mode.</param>
        public InstructionReader(Stream stream, ExecutionMode executionMode)
            : this(stream, executionMode, true)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the current instruction.
        /// </summary>
        public Instruction Instruction
        {
            get
            {
                return this.instruction;
            }
        }

        /// <summary>
        ///     Gets the number of operands for the current instruction.
        /// </summary>
        public int OperandCount
        {
            get
            {
                return this.operandCount;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the base register of a memory access operand, or the register in place of a memory access.
        /// </summary>
        /// <returns>
        ///     A member of the <see cref="register" /> enumeration.
        /// </returns>
        /// <remarks>
        ///     This is <c>baseOperandSizeRegister</c> in the formula <c>[baseOperandSizeRegister + indexRegister*Scale + Displacement]</c>.
        /// </remarks>
        public Register GetBaseRegister()
        {
            return this.baseRegister;
        }

        /// <summary>
        ///     Gets the displacement parameter of a memory access operand, the value of a relative address operand, or
        ///     the offset from a far pointer.
        /// </summary>
        /// <returns>
        ///     The displacement, in bytes.
        /// </returns>
        /// <remarks>
        ///     This is <c>Displacement</c> in the formula <c>[baseOperandSizeRegister + indexRegister*Scale + Displacement]</c>.
        /// </remarks>
        public int GetDisplacement()
        {
            return this.displacement;
        }

        /// <summary>
        ///     Gets the value of the immediate operand.
        /// </summary>
        /// <returns>
        ///     The value of the immediate operand, or <c>0</c> if there was none.
        /// </returns>
        /// <remarks>
        ///     This is the value of any operand with type <see cref="OperandType.ImmediateByte" />,
        ///     <see cref="OperandType.ImmediateWord" /> or <see cref="OperandType.ImmediateDword" />
        /// </remarks>
        public int GetImmediateValue()
        {
            return this.immediate;
        }

        /// <summary>
        ///     Gets the value of the second immediate operand.
        /// </summary>
        /// <returns>
        ///     The value of the immediate operand, or <c>0</c> if there was none.
        /// </returns>
        /// <remarks>
        ///     This is the value of any operand with type <see cref="OperandType.ImmediateByte" />,
        ///     <see cref="OperandType.ImmediateWord" /> or <see cref="OperandType.ImmediateDword" />
        /// </remarks>
        public int GetImmediateValue2()
        {
            // we re-use the displacement for this rare case
            return this.displacement;
        }

        /// <summary>
        ///     Gets the index register of a memory access operand.
        /// </summary>
        /// <returns>
        ///     A member of the <see cref="register" /> enumeration.
        /// </returns>
        /// <remarks>
        ///     This is <c>indexRegister</c> in the formula <c>[baseOperandSizeRegister + indexRegister*Scale + Displacement]</c>.
        /// </remarks>
        public Register GetIndexRegister()
        {
            return this.indexRegister;
        }

        /// <summary>
        ///     Gets the type of the specified operand.
        /// </summary>
        /// <param name="index">
        ///     The index of the operand.
        /// </param>
        /// <returns>
        ///     A member of the <see cref="OperandType" /> enumeration.
        /// </returns>
        public OperandType GetOperandType(int index)
        {
            if (index < 0 || index >= this.operandCount)
            {
                throw new ArgumentException("The index is not valid for this instruction", "index");
            }

            switch (index)
            {
                default:
                    return this.operand1;

                case 1:
                    return this.operand2;
            }
        }

        /// <summary>
        ///     Gets The value of the register operand.
        /// </summary>
        /// <returns>
        ///     The value of the register operand, or <see cref="Disassembler.Register.None" /> if there was none.
        /// </returns>
        /// <remarks>
        ///     This is the value of any operand with type <see cref="OperandType.Register" />.
        /// </remarks>
        public Register GetRegister()
        {
            return this.register;
        }

        /// <summary>
        ///     Gets the scale parameter of a memory access operand.
        /// </summary>
        /// <returns>
        ///     Either 1, 2, 4 or 8.
        /// </returns>
        /// <remarks>
        ///     This is <c>Scale</c> in the formula <c>[baseOperandSizeRegister + indexRegister*Scale + Displacement]</c>.
        /// </remarks>
        public int GetScale()
        {
            return this.scale;
        }

        /// <summary>
        ///     Gets the code segment selector from a far pointer operand.
        /// </summary>
        /// <returns>
        ///     The selector for the code segment that the pointer resides in.
        /// </returns>
        public short GetSegmentSelector()
        {
            return this.codeSegment;
        }

        /// <summary>
        ///     Attempts to read the next instruction from the stream.
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if there are more isntructions; <see langword="false" /> if the reader has reached
        ///     the end of the stream.
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
                throw this.InvalidInstruction();
            }
            return true;
        }

        #endregion

        #region Methods

        private static Register GetAddressSizeBaseRegister(Size addressSize)
        {
            switch (addressSize)
            {
                case Size.Qword:
                    return Register.Rax;
                case Size.Dword:
                    return Register.Eax;
                case Size.Word:
                    return Register.Ax;
                default:
                    throw new InvalidOperationException();
            }
        }

        private OperandType FarPointer(Size size)
        {
            this.codeSegment = this.instructionByteStream.ReadWord();
            this.ReadDisplacement(size);
            return OperandType.FarPointerLiteral;
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

        private Instruction GetGroup1Opcode(int opCode)
        {
            switch (opCode)
            {
                case 0:
                    return Instruction.Add;

                case 2:
                    return Instruction.Adc;

                case 4:
                    return Instruction.And;

                case 7:
                    return Instruction.Cmp;

                default:
                    throw new NotImplementedException();
            }
        }

        private Instruction GetGroup8Opcode(int opCode)
        {
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
                    throw this.InvalidInstruction();
            }
        }

        private int GetOpcodeReg(byte opcode)
        {
            var reg = opcode & 0x7;
            if ((this.rex & RexPrefix.B) != 0)
            {
                reg |= 8;
            }
            return reg;
        }

        private Size GetOperandSize()
        {
            if ((this.rex & RexPrefix.W) != 0)
            {
                return Size.Qword;
            }

            var operandSizeOverride = ((this.prefixes & InstructionPrefixes.OperandSizeOverride) != 0);
            return this.defaultOperandSize == Size.Word
                ? (operandSizeOverride ? Size.Dword : Size.Word)
                : (operandSizeOverride ? Size.Word : this.defaultOperandSize);
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

        private OperandType ImmediateByte()
        {
            this.immediate = this.instructionByteStream.ReadByte();
            return OperandType.ImmediateByte;
        }

        private OperandType ImmediateDword()
        {
            this.immediate = this.instructionByteStream.ReadDword();
            return OperandType.ImmediateDword;
        }

        private OperandType ImmediateWord()
        {
            this.immediate = this.instructionByteStream.ReadWord();
            return OperandType.ImmediateWord;
        }

        private OperandType ImplicitRegister(Register implicitRegister)
        {
            this.register = implicitRegister;
            return OperandType.Register;
        }

        private Exception InvalidInstruction()
        {
            return new FormatException("The data does not represent a valid instruction.");
        }

        private bool LockIfMemory()
        {
            if (this.operand1.IsMemoryAccess() && (this.prefixes & InstructionPrefixes.Lock) != 0)
            {
                this.locked = true;
            }
            return true;
        }

        private OperandType Memory(ref ModRMBits modrmBits, Size operandSize)
        {
            var operandType = this.RegisterOrMemory(ref modrmBits, this.GetBaseRegister(operandSize), operandSize);

            if (operandType == OperandType.DirectRegister)
            {
                throw this.InvalidInstruction();
            }

            return operandType;
        }

        private OperandType ModRMRegister(ref ModRMBits modrmBits, Register baseOperandSizeRegister)
        {
            this.register = RegDecoder.GetRegister(this.rex != 0, modrmBits.Reg, baseOperandSizeRegister);
            return OperandType.Register;
        }

        private bool ReadAsciiAdjustWithBase(Instruction instruction)
        {
            this.TestExecutionMode(ExecutionModes.CompatibilityMode);
            this.ReadInstruction(instruction);
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

        /// <summary>
        /// Decodes a block of 8 opcodes for the same instruction that behave like <c>ADD</c>.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="opCode">The opcode.</param>
        /// <returns><see langword="true" /></returns>
        private bool ReadBinaryOperation(Instruction instruction, int opCode)
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
            return true;
        }

        private void ReadDisplacement(Size size)
        {
            switch (size)
            {
                case Size.Byte:
                    this.displacement = this.instructionByteStream.ReadByte();
                    break;
                case Size.Word:
                    this.displacement = this.instructionByteStream.ReadWord();
                    break;
                case Size.Dword:
                    this.displacement = this.instructionByteStream.ReadDword();
                    break;
                case Size.Qword:
                    throw new NotImplementedException();
            }
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

            var readAny = false;

            while (true)
            {
                byte nextByte;
                if (readAny)
                {
                    nextByte = this.instructionByteStream.ReadByte();
                }
                else
                {
                    if (!this.instructionByteStream.TryReadByte(out nextByte))
                    {
                        return false;
                    }
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
                        return this.ReadBinaryOperation(Instruction.Add, nextByte) && this.LockIfMemory();

                    case 0x0F:
                        return this.ReadTwoByteInstruction();

                    case 0x10:
                    case 0x11:
                    case 0x12:
                    case 0x13:
                    case 0x14:
                    case 0x15:
                        return this.ReadBinaryOperation(Instruction.Adc, nextByte) && this.LockIfMemory();

                    case 0x20:
                    case 0x21:
                    case 0x22:
                    case 0x23:
                    case 0x24:
                    case 0x25:
                        return this.ReadBinaryOperation(Instruction.And, nextByte) && this.LockIfMemory();

                    case 0x26:
                        this.ReadPrefix(InstructionPrefixes.Group2Mask, InstructionPrefixes.SegmentES);
                        continue;
                    case 0x27:
                        if (this.executionMode == ExecutionModes.Long64Bit)
                        {
                            throw this.InvalidInstruction();
                        }

                        return this.ReadInstructionNoOperands(Instruction.Daa);

                    case 0x2E:
                        this.ReadPrefix(InstructionPrefixes.Group2Mask, InstructionPrefixes.SegmentCS);
                        continue;
                    case 0x2F:
                        if (this.executionMode == ExecutionModes.Long64Bit)
                        {
                            throw this.InvalidInstruction();
                        }

                        return this.ReadInstructionNoOperands(Instruction.Das);

                    case 0x36:
                        this.ReadPrefix(InstructionPrefixes.Group2Mask, InstructionPrefixes.SegmentSS);
                        continue;
                    case 0x37:
                        this.TestExecutionMode(ExecutionModes.CompatibilityMode);
                        return this.ReadInstructionNoOperands(Instruction.Aaa);

                    case 0x38:
                    case 0x39:
                    case 0x3A:
                    case 0x3B:
                    case 0x3C:
                    case 0x3D:
                        return this.ReadBinaryOperation(Instruction.Cmp, nextByte);

                    case 0x3E:
                        this.ReadPrefix(InstructionPrefixes.Group2Mask, InstructionPrefixes.SegmentDS);
                        continue;
                    case 0x3F:
                        this.TestExecutionMode(ExecutionModes.CompatibilityMode);
                        return this.ReadInstructionNoOperands(Instruction.Aas);

                    case 0x40:
                    case 0x41:
                    case 0x42:
                    case 0x43:
                    case 0x44:
                    case 0x45:
                    case 0x46:
                    case 0x47:
                        if (this.executionMode == ExecutionModes.Long64Bit)
                        {
                            this.ReadRexPrefix(nextByte);
                            continue;
                        }
                        goto default;

                    case 0x48:
                    case 0x49:
                    case 0x4A:
                    case 0x4B:
                    case 0x4C:
                    case 0x4D:
                    case 0x4E:
                    case 0x4F:
                        if (this.executionMode != ExecutionModes.Long64Bit)
                        {
                            return this.ReadWithRegisterFromOpcode(Instruction.Dec, nextByte, this.GetOperandSize());
                        }
                        this.ReadRexPrefix(nextByte);
                        continue;

                    case 0x62:
                        if (this.executionMode == ExecutionModes.Long64Bit)
                        {
                            throw this.InvalidInstruction();
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

                        var modrm = this.ReadModRM();
                        this.instruction = this.GetGroup1Opcode(modrm.OpCode);
                        this.operandCount = 2;
                        this.operand1 = this.RegisterOrMemory(ref modrm, registerSize);
                        this.operand2 = this.Immediate(immediateSize);
                        return this.instruction == Instruction.Cmp || this.LockIfMemory();
                    }

                    case 0x90:
                        return this.ReadInstructionNoOperands(Instruction.Nop);

                    case 0x98:
                    {
                        var alias = this.GetSizeExtendedAlias(Instruction.Cbw, Instruction.Cwde, Instruction.Cdqe);
                        return this.ReadInstructionNoOperands(alias);
                    }
                    case 0x99:
                    {
                        var alias = this.GetSizeExtendedAlias(Instruction.Cwd, Instruction.Cdq, Instruction.Cqo);
                        return this.ReadInstructionNoOperands(alias);
                    }


                    case 0x9A:
                        if (this.executionMode == ExecutionModes.Long64Bit)
                        {
                            throw this.InvalidInstruction();
                        }
                        this.instruction = Instruction.CallFar;
                        this.operandCount = 1;
                        this.operand1 = this.FarPointer(this.GetOperandSize());
                        return true;

                    case 0xA6:
                        return this.ReadInstructionNoOperands(Instruction.Cmpsb);

                    case 0xA7:
                    {
                        var alias = this.GetSizeExtendedAlias(Instruction.Cmpsw, Instruction.Cmpsd, Instruction.Cmpsq);
                        return this.ReadInstructionNoOperands(alias);
                    }

                    case 0xC8:
                        this.instruction = Instruction.Enter;
                        this.operandCount = 2;
                        this.operand1 = this.ImmediateWord();
                        this.operand2 = OperandType.ImmediateByte2;
                        this.displacement = this.instructionByteStream.ReadByte();
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
                        throw this.InvalidInstruction();
                    case 0xF2:
                        this.ReadPrefix(InstructionPrefixes.Group1Mask, InstructionPrefixes.RepNE);
                        continue;
                    case 0xF3:
                        this.ReadPrefix(InstructionPrefixes.Group1Mask, InstructionPrefixes.Rep);
                        continue;

                    case 0xF5:
                        return this.ReadInstructionNoOperands(Instruction.Cmc);
                    case 0xF6:
                        return this.ReadGroup3Instruction(Size.Byte);
                    case 0xF7:
                        return this.ReadGroup3Instruction(this.GetOperandSize());
                    case 0xF8:
                        return this.ReadInstructionNoOperands(Instruction.Clc);
                    case 0xFA:
                        return this.ReadInstructionNoOperands(Instruction.Cli);
                    case 0xFC:
                        return this.ReadInstructionNoOperands(Instruction.Cld);

                    case 0xFE:
                        return this.ReadGroup4Instruction();
                    case 0xFF:
                        return this.ReadGroup5Instruction();

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private bool ReadGroup3Instruction(Size size)
        {
            var modrm = this.ReadModRM();
            switch (modrm.Reg)
            {
                case 6:
                    this.instruction = Instruction.Div;
                    this.operandCount = 1;
                    this.operand1 = this.RegisterOrMemory(ref modrm, size);
                    return true;

                default:
                    throw new NotImplementedException();
            }
        }

        private bool ReadGroup4Instruction()
        {
            var modrm = this.ReadModRM();
            switch (modrm.Reg)
            {
                case 1:
                    this.instruction = Instruction.Dec;
                    this.operandCount = 1;
                    this.operand1 = this.RegisterOrMemory(ref modrm, Size.Byte);
                    return this.LockIfMemory();

                default:
                    throw new NotImplementedException();
            }
        }

        private bool ReadGroup5Instruction()
        {
            this.operandCount = 1;

            var modrm = this.ReadModRM();
            switch (modrm.Reg)
            {
                case 1:
                    this.instruction = Instruction.Dec;
                    this.operand1 = this.RegisterOrMemory(ref modrm, this.GetOperandSize());
                    return this.LockIfMemory();

                case 2:
                {
                    var operandSize = this.executionMode == ExecutionModes.Long64Bit ? Size.Qword : this.GetOperandSize();
                    this.instruction = Instruction.Call;
                    this.operand1 = this.RegisterOrMemory(ref modrm, operandSize);
                    return true;
                }

                case 3:
                {
                    var operandSize = this.executionMode == ExecutionModes.Long64Bit ? Size.Qword : this.GetOperandSize();
                    this.instruction = Instruction.CallFar;
                    this.operand1 = this.Memory(ref modrm, operandSize);
                    return true;
                }

                default:
                    throw new NotImplementedException();
            }
        }

        private bool ReadGroup9Instruction()
        {
            this.operandCount = 1;

            var modrm = this.ReadModRM();
            switch (modrm.Reg)
            {
                case 1:
                    var isExtended = ((this.rex & RexPrefix.W) != 0);
                    var operandSize = isExtended ? Size.Oword : Size.Qword;
                    this.instruction = isExtended ? Instruction.Cmpxchg16b : Instruction.Cmpxchg8b;
                    this.operand1 = this.Memory(ref modrm, operandSize);
                    if ((this.prefixes & InstructionPrefixes.Lock) != 0)
                    {
                        this.locked = true;
                    }
                    return true;

                default:
                    throw new NotImplementedException();
            }
        }

        private Instruction GetSizeExtendedAlias(Instruction wordInstruction, Instruction dwordInstruction, Instruction qwordInstruction)
        {
            switch (this.GetOperandSize())
            {
                case Size.Qword:
                    return qwordInstruction;

                case Size.Dword:
                    return dwordInstruction;

                default:
                    return wordInstruction;
            }
        }

        private void ReadInstruction(Instruction instruction)
        {
            if ((this.prefixes & InstructionPrefixes.Lock) != 0)
            {
                throw this.InvalidInstruction();
            }

            this.instruction = instruction;
        }

        private void TestExecutionMode(ExecutionModes executionModes)
        {
            if ((this.executionMode & executionModes) == 0)
            {
                throw this.InvalidInstruction();
            }
        }

        /// <summary>
        ///     Reads an instruction that takes no operands.
        /// </summary>
        /// <param name="instruction">The instruction mnemonic.</param>
        /// <returns>
        ///     Always returns <see langword="true" />
        /// </returns>
        private bool ReadInstructionNoOperands(Instruction instruction)
        {
            this.ReadInstruction(instruction);
            this.operandCount = 0;
            return true;
        }

        private ModRMBits ReadModRM()
        {
            return new ModRMBits(this.rex, this.instructionByteStream.ReadByte());
        }

        private void ReadPrefix(InstructionPrefixes group, InstructionPrefixes prefix)
        {
            if (this.rex != 0)
            {
                // REX prefix must appear after all other prefixes.
                throw this.InvalidInstruction();
            }

            if ((this.prefixes & group) != 0)
            {
                throw this.InvalidInstruction();
            }

            this.prefixes |= prefix;
        }

        private void ReadRexPrefix(int nextByte)
        {
            this.rex = (RexPrefix)nextByte;
        }

        private void ReadSibByte(int mod, Register addressSizeBaseRegister)
        {
            var sibByte = this.instructionByteStream.ReadByte();
            var sibDecoder = new SibDecoder(this.rex, mod, sibByte, addressSizeBaseRegister);
            this.baseRegister = sibDecoder.BaseRegister;
            this.scale = sibDecoder.Scale;
            this.indexRegister = sibDecoder.IndexRegister;
            if (sibDecoder.RequiresDisplacement)
            {
                this.displacement = this.instructionByteStream.ReadDword();
            }
        }

        private bool ReadTwoByteInstruction()
        {
            var opCodeByte = this.instructionByteStream.ReadByte();

            switch (opCodeByte)
            {
                case 0x06:
                    return this.ReadInstructionNoOperands(Instruction.Clts);
                case 0x0b:
                    return this.ReadInstructionNoOperands(Instruction.Ud2);

                case 0x38:
                    return this.ReadThreeByteInstructionA();
                case 0x40:
                    return this.Read_RM_Reg(Instruction.Cmovo, this.GetOperandSize());
                case 0x41:
                    return this.Read_RM_Reg(Instruction.Cmovno, this.GetOperandSize());
                case 0x42:
                    // CMOVB, CMOVNAE, CMOVC
                    return this.Read_RM_Reg(Instruction.Cmovb, this.GetOperandSize());
                case 0x43:
                    // CMOVAE, CMOVNB, CMOVNC
                    return this.Read_RM_Reg(Instruction.Cmovae, this.GetOperandSize());
                case 0x44:
                    // CMOVE, CMOVZ
                    return this.Read_RM_Reg(Instruction.Cmove, this.GetOperandSize());
                case 0x45:
                    // CMOVNE, CMOVNZ
                    return this.Read_RM_Reg(Instruction.Cmovne, this.GetOperandSize());
                case 0x46:
                    // CMOVBE, CMOVNA
                    return this.Read_RM_Reg(Instruction.Cmovbe, this.GetOperandSize());
                case 0x47:
                    // CMOVA, CMOVNBE
                    return this.Read_RM_Reg(Instruction.Cmova, this.GetOperandSize());
                case 0x48:
                    return this.Read_RM_Reg(Instruction.Cmovs, this.GetOperandSize());
                case 0x49:
                    return this.Read_RM_Reg(Instruction.Cmovns, this.GetOperandSize());
                case 0x4A:
                    // CMOVP, CMOVPE
                    return this.Read_RM_Reg(Instruction.Cmovp, this.GetOperandSize());
                case 0x4B:
                    // CMOVNP, CMOVPO
                    return this.Read_RM_Reg(Instruction.Cmovnp, this.GetOperandSize());
                case 0x4C:
                    // CMOVL, CMOVNGE
                    return this.Read_RM_Reg(Instruction.Cmovl, this.GetOperandSize());
                case 0x4D:
                    // CMOVGE, CMOVNL
                    return this.Read_RM_Reg(Instruction.Cmovge, this.GetOperandSize());
                case 0x4E:
                    // CMOVLE, CMOVNG
                    return this.Read_RM_Reg(Instruction.Cmovle, this.GetOperandSize());
                case 0x4F:
                    // CMOVG, CMOVNLE
                    return this.Read_RM_Reg(Instruction.Cmovg, this.GetOperandSize());

                case 0x77:
                    return this.ReadInstructionNoOperands(Instruction.Emms);

                case 0xA2:
                    return this.ReadInstructionNoOperands(Instruction.Cpuid);
                case 0xA3:
                    return this.Read_RM_Reg(Instruction.Bt, this.GetOperandSize());

                case 0xAB:
                    return this.Read_RM_Reg(Instruction.Bts, this.GetOperandSize()) && this.LockIfMemory();

                case 0xB0:
                    return this.Read_RM_Reg(Instruction.Cmpxchg, Size.Byte) && this.LockIfMemory();

                case 0xB1:
                    return this.Read_RM_Reg(Instruction.Cmpxchg, this.GetOperandSize()) && this.LockIfMemory();

                case 0xB3:
                    return this.Read_RM_Reg(Instruction.Btr, this.GetOperandSize()) && this.LockIfMemory();

                case 0xB9:
                {
                    // Group 10
                    return this.ReadInstructionNoOperands(Instruction.Ud1);
                }
                case 0xBA:
                {
                    var modrm = this.ReadModRM();
                    this.instruction = this.GetGroup8Opcode(modrm.OpCode);
                    this.operandCount = 2;
                    this.operand1 = this.RegisterOrMemory(ref modrm, this.GetOperandSize());
                    this.operand2 = this.ImmediateByte();
                    return this.instruction == Instruction.Bt || this.LockIfMemory();
                }
                case 0xBB:
                    return this.Read_RM_Reg(Instruction.Btc, this.GetOperandSize()) && this.LockIfMemory();
                case 0xBC:
                    return this.Read_Reg_RM(Instruction.Bsf, this.GetOperandSize());
                case 0xBD:
                    return this.Read_Reg_RM(Instruction.Bsr, this.GetOperandSize());

                case 0xC7:
                    return this.ReadGroup9Instruction();

                case 0xC8:
                case 0xC9:
                case 0xCA:
                case 0xCB:
                case 0xCC:
                case 0xCD:
                case 0xCE:
                case 0xCF:
                    var operandSize = this.GetOperandSize();
                    if (operandSize == Size.Word)
                    {
                        throw this.UndefinedBehaviour();
                    }

                    return this.ReadWithRegisterFromOpcode(Instruction.Bswap, opCodeByte, operandSize);

                default:
                    throw new NotImplementedException();
            }
        }

        private bool ReadWithRegisterFromOpcode(Instruction instruction, byte opCodeByte, Size operandSize)
        {
            this.instruction = instruction;
            this.operandCount = 1;
            this.operand1 = OperandType.Register;
            this.register = RegDecoder.GetRegister(
                this.rex != 0,
                this.GetOpcodeReg(opCodeByte),
                this.GetBaseRegister(operandSize));
            return true;
        }

        private bool ReadThreeByteInstructionA()
        {
            // instructions starting 0F 38
            var opCodeByte = this.instructionByteStream.ReadByte();

            switch (opCodeByte)
            {
                case 0xF0:
                    if ((this.prefixes & InstructionPrefixes.RepNZ) != 0)
                    {
                        return this.Read_Reg_RM(Instruction.Crc32, Size.Dword, Size.Byte);
                    }
                    goto default;
                case 0xF1:
                    if ((this.prefixes & InstructionPrefixes.RepNZ) != 0)
                    {
                        return this.Read_Reg_RM(Instruction.Crc32, Size.Dword, this.GetOperandSize());
                    }
                    goto default;

                default:
                    throw new NotImplementedException();
            }
        }

        private bool Read_Jz(Instruction instruction)
        {
            this.instruction = instruction;
            this.operandCount = 1;
            this.operand1 = OperandType.RelativeAddress;
            this.ReadDisplacement(this.GetOperandSize());
            return true;
        }

        private bool Read_RM_Reg(Instruction instruction, Size size)
        {
            this.instruction = instruction;
            this.operandCount = 2;
            var operandBaseRegister = this.GetBaseRegister(size);
            var modrm = this.ReadModRM();
            this.operand1 = this.RegisterOrMemory(ref modrm, operandBaseRegister, size);
            this.operand2 = this.ModRMRegister(ref modrm, operandBaseRegister);
            return true;
        }

        private bool Read_Reg_RM(Instruction instruction, Size size)
        {
            this.instruction = instruction;
            this.operandCount = 2;
            var modrm = this.ReadModRM();
            var baseRegister = this.GetBaseRegister(size);
            this.operand1 = this.ModRMRegister(ref modrm, baseRegister);
            this.operand2 = this.RegisterOrMemory(ref modrm, baseRegister, size);
            return true;
        }

        private bool Read_Reg_RM(Instruction instruction, Size regSize, Size rmSize)
        {
            this.instruction = instruction;
            this.operandCount = 2;
            var modrm = this.ReadModRM();
            this.operand1 = this.ModRMRegister(ref modrm, this.GetBaseRegister(regSize));
            this.operand2 = this.RegisterOrMemory(ref modrm, this.GetBaseRegister(rmSize), rmSize);
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

        private OperandType RegisterOrMemory(ref ModRMBits modrmBits, Size operandSize)
        {
            return this.RegisterOrMemory(ref modrmBits, this.GetBaseRegister(operandSize), operandSize);
        }

        private OperandType RegisterOrMemory(ref ModRMBits modrmBits, Register operandSizeBaseRegister, Size operandSize)
        {
            var addressSize = this.GetAddressSize();
            var addressSizeBaseRegister = GetAddressSizeBaseRegister(addressSize);
            var decoder = new ModRMDecoder(
                this.executionMode,
                this.rex,
                addressSize,
                operandSizeBaseRegister,
                addressSizeBaseRegister,
                ref modrmBits);

            this.baseRegister = decoder.BaseRegister;
            this.indexRegister = decoder.IndexRegister;

            if (decoder.IsDirectRegister)
            {
                return OperandType.DirectRegister;
            }

            if (decoder.NeedsSib)
            {
                this.ReadSibByte(decoder.Mod, addressSizeBaseRegister);
            }

            this.ReadDisplacement(decoder.DisplacementSize);

            switch (operandSize)
            {
                case Size.Byte:
                    return OperandType.BytePointer;
                case Size.Word:
                    return OperandType.WordPointer;
                case Size.Dword:
                    return OperandType.DwordPointer;
                case Size.Qword:
                    return OperandType.QwordPointer;
                default:
                    return OperandType.OwordPointer;
            }
        }

        private Exception UndefinedBehaviour()
        {
            return new FormatException("The behaviour of the specified instruction is undefined.");
        }

        #endregion
    }
}