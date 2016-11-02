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

        private Instruction instruction = Instruction.Unknown;

        private Operand operand1;
        private Operand operand2;
        private InstructionPrefixes prefixes;
        private RexPrefix rex = RexPrefix.Magic;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstructionReader" /> class.
        /// </summary>
        /// <param name="stream">The stream to read the instruction data from.</param>
        /// <param name="executionMode">The execution mode.</param>
        /// <param name="default32BitOperands">
        /// <see langword="true" /> if the default operand size is 32 bits; otherwise <see langword="false" />.  This
        /// corresponds to the D flag in segment descriptor for the code segment block.
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
        /// Initializes a new instance of the <see cref="InstructionReader" /> class.
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
        /// Gets the current instruction.
        /// </summary>
        public Instruction Instruction => this.instruction;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The first operand for the instruction.
        /// </summary>
        public Operand Operand1 => this.operand1;

        /// <summary>
        /// The first operand for the instruction.
        /// </summary>
        public Operand Operand2 => this.operand2;

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
            if ((this.prefixes & InstructionPrefixes.Lock) != 0)
            {
                if (!CanBeLocked(this.instruction) || !this.operand1.Type.IsMemoryAccess())
                {
                    throw this.InvalidInstruction();
                }
            }

            return true;
        }

        #endregion

        #region Methods

        private bool ReadInstruction()
        {
            this.prefixes = InstructionPrefixes.None;
            this.rex = 0;
            this.instruction = Instruction.Unknown;
            this.operand1 = default(Operand);
            this.operand2 = default(Operand);

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
                        this.ReadBinaryOperation(Instruction.Add, nextByte);
                        break;

                    case 0x0F:
                        this.ReadTwoByteInstruction();
                        break;

                    case 0x10:
                    case 0x11:
                    case 0x12:
                    case 0x13:
                    case 0x14:
                    case 0x15:
                        this.ReadBinaryOperation(Instruction.Adc, nextByte);
                        break;

                    case 0x20:
                    case 0x21:
                    case 0x22:
                    case 0x23:
                    case 0x24:
                    case 0x25:
                        this.ReadBinaryOperation(Instruction.And, nextByte);
                        break;

                    case 0x26:
                        this.ReadPrefix(InstructionPrefixes.Group2Mask, InstructionPrefixes.SegmentES);
                        continue;
                    case 0x27:
                        this.TestExecutionMode(ExecutionModes.CompatibilityMode);
                        Instruction instruction1 = Instruction.Daa;
                        this.ReadInstruction(instruction1);
                        break;

                    case 0x2E:
                        this.ReadPrefix(InstructionPrefixes.Group2Mask, InstructionPrefixes.SegmentCS);
                        continue;
                    case 0x2F:
                        this.TestExecutionMode(ExecutionModes.CompatibilityMode);
                        Instruction instruction2 = Instruction.Das;
                        this.ReadInstruction(instruction2);
                        break;

                    case 0x36:
                        this.ReadPrefix(InstructionPrefixes.Group2Mask, InstructionPrefixes.SegmentSS);
                        continue;
                    case 0x37:
                        this.TestExecutionMode(ExecutionModes.CompatibilityMode);
                        Instruction instruction3 = Instruction.Aaa;
                        this.ReadInstruction(instruction3);
                        break;
                    case 0x38:
                    case 0x39:
                    case 0x3A:
                    case 0x3B:
                    case 0x3C:
                    case 0x3D:
                        this.ReadBinaryOperation(Instruction.Cmp, nextByte);
                        break;
                    case 0x3E:
                        this.ReadPrefix(InstructionPrefixes.Group2Mask, InstructionPrefixes.SegmentDS);
                        continue;
                    case 0x3F:
                        this.TestExecutionMode(ExecutionModes.CompatibilityMode);
                        Instruction instruction4 = Instruction.Aas;
                        this.ReadInstruction(instruction4);
                        break;

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
                            this.ReadWithRegisterFromOpcode(Instruction.Dec, nextByte, this.GetOperandSize());
                            break;
                        }
                        this.ReadRexPrefix(nextByte);
                        continue;

                    case 0x62:
                        this.TestExecutionMode(ExecutionModes.CompatibilityMode);
                        this.Read_Reg_RM(Instruction.Bound, this.GetOperandSize());
                        break;
                    case 0x63:
                        this.TestExecutionMode(ExecutionModes.CompatibilityMode);
                        this.Read_RM_Reg(Instruction.Arpl, Size.Word);
                        break;
                    case 0x64:
                        this.ReadPrefix(InstructionPrefixes.Group2Mask, InstructionPrefixes.SegmentFS);
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
                        this.operand1 = this.RegisterOrMemory(ref modrm, registerSize);
                        this.operand2 = this.Immediate(immediateSize);
                        break;
                    }

                    case 0x90:
                        Instruction instruction5 = Instruction.Nop;
                        this.ReadInstruction(instruction5);
                        break;

                    case 0x98:
                    {
                        var alias = this.GetSizeExtendedAlias(Instruction.Cbw, Instruction.Cwde, Instruction.Cdqe);
                        this.ReadInstruction(alias);
                        break;
                    }
                    case 0x99:
                    {
                        var alias = this.GetSizeExtendedAlias(Instruction.Cwd, Instruction.Cdq, Instruction.Cqo);
                        this.ReadInstruction(alias);
                        break;
                    }


                    case 0x9A:
                        if (this.executionMode == ExecutionModes.Long64Bit)
                        {
                            throw this.InvalidInstruction();
                        }
                        this.instruction = Instruction.CallFar;
                        this.operand1 = this.ReadFarPointerOperand(this.GetOperandSize());
                        break;

                    case 0xA6:
                        Instruction instruction6 = Instruction.Cmpsb;
                        this.ReadInstruction(instruction6);
                        break;

                    case 0xA7:
                    {
                        var alias = this.GetSizeExtendedAlias(Instruction.Cmpsw, Instruction.Cmpsd, Instruction.Cmpsq);
                        this.ReadInstruction(alias);
                        break;
                    }

                    case 0xC8:
                        this.instruction = Instruction.Enter;
                        this.operand1 = this.ReadImmediateWordOperand();
                        this.operand2 = this.ReadImmediateByteOperand();
                        break;

                    case 0xD4:
                        this.ReadAsciiAdjustWithBase(Instruction.Aam);
                        break;
                    case 0xD5:
                        this.ReadAsciiAdjustWithBase(Instruction.Aad);
                        break;

                    case 0xE8:
                        this.Read_Jz(Instruction.Call);
                        break;

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
                    case 0xF4:
                        this.ReadInstruction(Instruction.Hlt);
                        break;
                    case 0xF5:
                        Instruction instruction7 = Instruction.Cmc;
                        this.ReadInstruction(instruction7);
                        break;
                    case 0xF6:
                        this.ReadGroup3Instruction(Size.Byte);
                        break;
                    case 0xF7:
                        this.ReadGroup3Instruction(this.GetOperandSize());
                        break;
                    case 0xF8:
                        Instruction instruction8 = Instruction.Clc;
                        this.ReadInstruction(instruction8);
                        break;
                    case 0xFA:
                        Instruction instruction9 = Instruction.Cli;
                        this.ReadInstruction(instruction9);
                        break;
                    case 0xFC:
                        Instruction instruction10 = Instruction.Cld;
                        this.ReadInstruction(instruction10);
                        break;

                    case 0xFE:
                        this.ReadGroup4Instruction();
                        break;
                    case 0xFF:
                        this.ReadGroup5Instruction();
                        break;

                    default:
                        throw new NotImplementedException();
                }

                return true;
            }
        }

        private Operand ReadFarPointerOperand(Size size)
        {
            var segment = this.instructionByteStream.ReadWord();
            var displacement = this.ReadImmediateValue(size);
            return Operand.FarPointer(segment, displacement);
        }

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
                case Size.Byte:
                    return Register.Al;
                default:
                    throw new InvalidOperationException();
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

        private Operand Immediate(Size operandSize)
        {
            switch (operandSize)
            {
                case Size.Byte:
                    return this.ReadImmediateByteOperand();
                case Size.Word:
                    return this.ReadImmediateWordOperand();
                default:
                    // even when our operands are QWords we still use dword immediate values.
                    return this.ReadImmediateDwordOperand();
            }
        }

        private Operand ReadImmediateByteOperand()
        {
            var value = this.instructionByteStream.ReadByte();
            return Operand.ImmediateByte(value);
        }

        private Operand ReadImmediateWordOperand()
        {
            var value = this.instructionByteStream.ReadWord();
            return Operand.ImmediateWord(value);
        }

        private Operand ReadImmediateDwordOperand()
        {
            var value = this.instructionByteStream.ReadDword();
            return Operand.ImmediateDword(value);
        }

        private Exception InvalidInstruction()
        {
            return new FormatException("The data does not represent a valid instruction.");
        }

        private static bool CanBeLocked(Instruction instruction)
        {
            switch (instruction)
            {
                case Instruction.Adc:
                case Instruction.Add:
                case Instruction.And:
                case Instruction.Btc:
                case Instruction.Btr:
                case Instruction.Bts:
                case Instruction.Cmpxchg:
                case Instruction.Cmpxchg8b:
                case Instruction.Cmpxchg16b:
                case Instruction.Dec:
                case Instruction.Inc:
                case Instruction.Neg:
                case Instruction.Not:
                case Instruction.Or:
                case Instruction.Sbb:
                case Instruction.Sub:
                case Instruction.Xor:
                case Instruction.Xadd:
                case Instruction.Xchg:
                    return true;

                default:
                    return false;
            }
        }

        private Operand ModRMRegister(ref ModRMBits modrmBits, Register baseOperandSizeRegister)
        {
            var register = RegDecoder.GetRegister(this.rex != 0, modrmBits.Reg, baseOperandSizeRegister);
            return Operand.DirectRegister(register);
        }

        private void ReadAsciiAdjustWithBase(Instruction instruction)
        {
            this.TestExecutionMode(ExecutionModes.CompatibilityMode);
            this.ReadInstruction(instruction);
            var imm = this.instructionByteStream.ReadByte();

            // special case: AAD is a synonym for AAD 0AH
            if (imm != 0x0A)
            {
                this.operand1 = Operand.ImmediateByte(imm);
            }
        }

        /// <summary>
        /// Decodes a block of 8 opcodes for the same instruction that behave like <c>ADD</c>.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="opCode">The opcode.</param>
        /// <returns><see langword="true" /></returns>
        private void ReadBinaryOperation(Instruction instruction, int opCode)
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
        }

        private int ReadImmediateValue(Size size)
        {
            switch (size)
            {
                case Size.None:
                    return 0;

                case Size.Byte:
                    return this.instructionByteStream.ReadByte();

                case Size.Word:
                    return this.instructionByteStream.ReadWord();

                case Size.Dword:
                    return this.instructionByteStream.ReadDword();

                default:
                    throw new NotImplementedException();
            }
        }

        private void ReadGroup3Instruction(Size size)
        {
            var modrm = this.ReadModRM();
            switch (modrm.Reg)
            {
                case 6:
                    this.instruction = Instruction.Div;
                    this.operand1 = this.RegisterOrMemory(ref modrm, size);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ReadGroup4Instruction()
        {
            var modrm = this.ReadModRM();
            switch (modrm.Reg)
            {
                case 1:
                    this.instruction = Instruction.Dec;
                    this.operand1 = this.RegisterOrMemory(ref modrm, Size.Byte);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ReadGroup5Instruction()
        {
            var modrm = this.ReadModRM();
            switch (modrm.Reg)
            {
                case 1:
                    this.instruction = Instruction.Dec;
                    this.operand1 = this.RegisterOrMemory(ref modrm, this.GetOperandSize());
                    break;

                case 2:
                {
                    var operandSize = this.executionMode == ExecutionModes.Long64Bit ? Size.Qword : this.GetOperandSize();
                    this.instruction = Instruction.Call;
                    this.operand1 = this.RegisterOrMemory(ref modrm, operandSize);
                    break;
                }

                case 3:
                {
                    var operandSize = this.executionMode == ExecutionModes.Long64Bit ? Size.Qword : this.GetOperandSize();
                    this.instruction = Instruction.CallFar;
                    this.operand1 = this.Memory(ref modrm, operandSize);
                    break;
                }

                default:
                    throw new NotImplementedException();
            }
        }

        private void ReadGroup9Instruction()
        {
            var modrm = this.ReadModRM();
            switch (modrm.Reg)
            {
                case 1:
                    var isExtended = ((this.rex & RexPrefix.W) != 0);
                    var operandSize = isExtended ? Size.Oword : Size.Qword;
                    this.instruction = isExtended ? Instruction.Cmpxchg16b : Instruction.Cmpxchg8b;
                    this.operand1 = this.Memory(ref modrm, operandSize);
                    break;

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

        private void ReadTwoByteInstruction()
        {
            var opCodeByte = this.instructionByteStream.ReadByte();

            switch (opCodeByte)
            {
                case 0x06:
                    Instruction instruction1 = Instruction.Clts;
                    this.ReadInstruction(instruction1);
                    break;
                case 0x0b:
                    Instruction instruction2 = Instruction.Ud2;
                    this.ReadInstruction(instruction2);
                    break;

                case 0x38:
                    this.ReadThreeByteInstructionA();
                    break;

                case 0x40:
                    this.Read_RM_Reg(Instruction.Cmovo, this.GetOperandSize());
                    break;
                case 0x41:
                    this.Read_RM_Reg(Instruction.Cmovno, this.GetOperandSize());
                    break;
                case 0x42:
                    // CMOVB, CMOVNAE, CMOVC
                    this.Read_RM_Reg(Instruction.Cmovb, this.GetOperandSize());
                    break;
                case 0x43:
                    // CMOVAE, CMOVNB, CMOVNC
                    this.Read_RM_Reg(Instruction.Cmovae, this.GetOperandSize());
                    break;
                case 0x44:
                    // CMOVE, CMOVZ
                    this.Read_RM_Reg(Instruction.Cmove, this.GetOperandSize());
                    break;
                case 0x45:
                    // CMOVNE, CMOVNZ
                    this.Read_RM_Reg(Instruction.Cmovne, this.GetOperandSize());
                    break;
                case 0x46:
                    // CMOVBE, CMOVNA
                    this.Read_RM_Reg(Instruction.Cmovbe, this.GetOperandSize());
                    break;
                case 0x47:
                    // CMOVA, CMOVNBE
                    this.Read_RM_Reg(Instruction.Cmova, this.GetOperandSize());
                    break;
                case 0x48:
                    this.Read_RM_Reg(Instruction.Cmovs, this.GetOperandSize());
                    break;
                case 0x49:
                    this.Read_RM_Reg(Instruction.Cmovns, this.GetOperandSize());
                    break;
                case 0x4A:
                    // CMOVP, CMOVPE
                    this.Read_RM_Reg(Instruction.Cmovp, this.GetOperandSize());
                    break;
                case 0x4B:
                    // CMOVNP, CMOVPO
                    this.Read_RM_Reg(Instruction.Cmovnp, this.GetOperandSize());
                    break;
                case 0x4C:
                    // CMOVL, CMOVNGE
                    this.Read_RM_Reg(Instruction.Cmovl, this.GetOperandSize());
                    break;
                case 0x4D:
                    // CMOVGE, CMOVNL
                    this.Read_RM_Reg(Instruction.Cmovge, this.GetOperandSize());
                    break;
                case 0x4E:
                    // CMOVLE, CMOVNG
                    this.Read_RM_Reg(Instruction.Cmovle, this.GetOperandSize());
                    break;
                case 0x4F:
                    // CMOVG, CMOVNLE
                    this.Read_RM_Reg(Instruction.Cmovg, this.GetOperandSize());
                    break;

                case 0x77:
                    Instruction instruction3 = Instruction.Emms;
                    this.ReadInstruction(instruction3);
                    break;

                case 0xA2:
                    Instruction instruction4 = Instruction.Cpuid;
                    this.ReadInstruction(instruction4);
                    break;
                case 0xA3:
                    this.Read_RM_Reg(Instruction.Bt, this.GetOperandSize());
                    break;

                case 0xAB:
                    this.Read_RM_Reg(Instruction.Bts, this.GetOperandSize());
                    break;

                case 0xB0:
                    this.Read_RM_Reg(Instruction.Cmpxchg, Size.Byte);
                    break;
                case 0xB1:
                    this.Read_RM_Reg(Instruction.Cmpxchg, this.GetOperandSize());
                    break;

                case 0xB3:
                    this.Read_RM_Reg(Instruction.Btr, this.GetOperandSize());
                    break;

                case 0xB9:
                {
                    // Group 10
                    Instruction instruction5 = Instruction.Ud1;
                    this.ReadInstruction(instruction5);
                    break;
                }
                case 0xBA:
                {
                    var modrm = this.ReadModRM();
                    this.instruction = this.GetGroup8Opcode(modrm.OpCode);
                    this.operand1 = this.RegisterOrMemory(ref modrm, this.GetOperandSize());
                    this.operand2 = this.ReadImmediateByteOperand();
                    break;
                }
                case 0xBB:
                    this.Read_RM_Reg(Instruction.Btc, this.GetOperandSize());
                    break;
                case 0xBC:
                    this.Read_Reg_RM(Instruction.Bsf, this.GetOperandSize());
                    break;
                case 0xBD:
                    this.Read_Reg_RM(Instruction.Bsr, this.GetOperandSize());
                    break;

                case 0xC7:
                    this.ReadGroup9Instruction();
                    break;

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

                    this.ReadWithRegisterFromOpcode(Instruction.Bswap, opCodeByte, operandSize);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ReadWithRegisterFromOpcode(Instruction instruction, byte opCodeByte, Size operandSize)
        {
            this.instruction = instruction;
            var register = RegDecoder.GetRegister(
                this.rex != 0,
                this.GetOpcodeReg(opCodeByte),
                this.GetBaseRegister(operandSize));
            this.operand1 = Operand.DirectRegister(register);
        }

        private void ReadThreeByteInstructionA()
        {
            // instructions starting 0F 38
            var opCodeByte = this.instructionByteStream.ReadByte();

            switch (opCodeByte)
            {
                case 0xF0:
                    if ((this.prefixes & InstructionPrefixes.RepNZ) != 0)
                    {
                        this.Read_Reg_RM(Instruction.Crc32, Size.Dword, Size.Byte);
                        break;
                    }
                    goto default;
                case 0xF1:
                    if ((this.prefixes & InstructionPrefixes.RepNZ) != 0)
                    {
                        this.Read_Reg_RM(Instruction.Crc32, Size.Dword, this.GetOperandSize());
                        break;
                    }
                    goto default;

                default:
                    throw new NotImplementedException();
            }
        }

        private void Read_Jz(Instruction instruction)
        {
            this.instruction = instruction;
            var offset = this.ReadImmediateValue(this.GetOperandSize());
            this.operand1 = Operand.RelativeAddress(offset);
        }

        private void Read_RM_Reg(Instruction instruction, Size size)
        {
            this.instruction = instruction;
            var operandBaseRegister = this.GetBaseRegister(size);
            var modrm = this.ReadModRM();
            this.operand1 = this.RegisterOrMemory(ref modrm, operandBaseRegister, size);
            this.operand2 = this.ModRMRegister(ref modrm, operandBaseRegister);
        }

        private void Read_Reg_RM(Instruction instruction, Size size)
        {
            this.instruction = instruction;
            var modrm = this.ReadModRM();
            var baseRegister = this.GetBaseRegister(size);
            this.operand1 = this.ModRMRegister(ref modrm, baseRegister);
            this.operand2 = this.RegisterOrMemory(ref modrm, baseRegister, size);
        }

        private void Read_Reg_RM(Instruction instruction, Size regSize, Size rmSize)
        {
            this.instruction = instruction;
            var modrm = this.ReadModRM();
            this.operand1 = this.ModRMRegister(ref modrm, this.GetBaseRegister(regSize));
            this.operand2 = this.RegisterOrMemory(ref modrm, this.GetBaseRegister(rmSize), rmSize);
        }

        private void Read_rAx_Imm(Instruction instruction, Size size)
        {
            this.instruction = instruction;
            this.operand1 = Operand.DirectRegister(this.GetBaseRegister(size));
            this.operand2 = this.Immediate(size);
        }

        private Operand RegisterOrMemory(ref ModRMBits modrmBits, Size operandSize)
        {
            return this.RegisterOrMemory(ref modrmBits, this.GetBaseRegister(operandSize), operandSize);
        }

        private Operand Memory(ref ModRMBits modrmBits, Size operandSize)
        {
            var addressSize = this.GetAddressSize();

            if (ModRMDecoder.DirectRegister(ref modrmBits))
            {
                throw this.InvalidInstruction();
            }

            return this.ReadMemoryAccessOperand(ref modrmBits, operandSize, addressSize);
        }

        private Operand RegisterOrMemory(ref ModRMBits modrmBits, Register operandSizeBaseRegister, Size operandSize)
        {
            var addressSize = this.GetAddressSize();

            if (ModRMDecoder.DirectRegister(ref modrmBits))
            {
                var register = ModRMDecoder.GetRegister(this.rex, operandSizeBaseRegister, ref modrmBits);
                return Operand.DirectRegister(register);
            }

            return this.ReadMemoryAccessOperand(ref modrmBits, operandSize, addressSize);
        }

        private Operand ReadMemoryAccessOperand(
            ref ModRMBits modrmBits,
            Size operandSize,
            Size addressSize)
        {
            var addressSizeBaseRegister = GetAddressSizeBaseRegister(addressSize);
            if (ModRMDecoder.UseSib(addressSize, ref modrmBits))
            {
                var sibByte = this.instructionByteStream.ReadByte();
                var sibDecoder = new SibDecoder(this.rex, modrmBits.Mod, sibByte, addressSizeBaseRegister);
                var displacement1 = this.ReadImmediateValue(sibDecoder.DisplacementSize);
                return Operand.MemoryAccess((int)operandSize, sibDecoder.BaseRegister, sibDecoder.Scale, sibDecoder.IndexRegister, displacement1);
            }

            var decoder = new ModRMDecoder(
                  this.executionMode,
                  this.rex,
                  addressSize,
                  addressSizeBaseRegister,
                  ref modrmBits);

            var displacement = this.ReadImmediateValue(decoder.DisplacementSize);
            return Operand.MemoryAccess((int)operandSize, decoder.BaseRegister, 1, decoder.IndexRegister, displacement);
        }

        private Exception UndefinedBehaviour()
        {
            return new FormatException("The behaviour of the specified instruction is undefined.");
        }

        #endregion
    }
}