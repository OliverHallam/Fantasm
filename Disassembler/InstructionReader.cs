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
        private Operand operand3;
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
            this.defaultOperandSize = default32BitOperands || executionMode == ExecutionMode.Long64Bit
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

        /// <summary>
        /// The first operand for the instruction.
        /// </summary>
        public Operand Operand1 => this.operand1;

        /// <summary>
        /// The second operand for the instruction.
        /// </summary>
        public Operand Operand2 => this.operand2;

        /// <summary>
        /// The third operand for the instruction
        /// </summary>
        public Operand Operand3 => this.operand3;

        #endregion

        #region Public Methods and Operators

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
                else if (!this.instructionByteStream.TryReadByte(out nextByte))
                {
                    return false;
                }

                readAny = true;

                switch (nextByte)
                {
                    case 0x0F:
                        this.ReadTwoByteInstruction();
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

                    case 0x3E:
                        this.ReadPrefix(InstructionPrefixes.Group2Mask, InstructionPrefixes.SegmentDS);
                        continue;

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
                }

                var opCode = OpCodeTables.OneByteOpCodeMap[nextByte];

                byte modrm = (opCode.Flags & OpCodeFlags.ModRM) != 0 ? this.instructionByteStream.ReadByte() : (byte)0;

                if (opCode.Instruction == Instruction.Unknown)
                {
                    switch (nextByte)
                    {
                        case 0x6D:
                            this.instruction = this.GetOperandSizeExtendedAlias(
                                Instruction.Insw,
                                Instruction.Insd,
                                Instruction.Insd);
                            break;

                        case 0x80:
                        case 0x81:
                        case 0x82:
                        case 0x83:
                            this.instruction = OpCodeTables.Group1Instructions[ModRMBits.GetReg(modrm)];
                            break;

                        case 0x98:
                            this.instruction = this.GetOperandSizeExtendedAlias(
                                Instruction.Cbw,
                                Instruction.Cwde,
                                Instruction.Cdqe);
                            break;
                        case 0x99:
                            this.instruction = this.GetOperandSizeExtendedAlias(
                                Instruction.Cwd,
                                Instruction.Cdq,
                                Instruction.Cqo);
                            break;

                        case 0xA7:
                            this.instruction = this.GetOperandSizeExtendedAlias(
                                Instruction.Cmpsw,
                                Instruction.Cmpsd,
                                Instruction.Cmpsq);
                            break;

                        case 0xCF:
                            this.instruction = this.GetOperandSizeExtendedAlias(
                                Instruction.Iret,
                                Instruction.Iretd,
                                Instruction.Iretq);
                            break;

                        case 0xE3:
                            this.instruction = this.GetAddressSizeExtendedAlias(
                                Instruction.Jcxz,
                                Instruction.Jecxz,
                                Instruction.Jrcxz);
                            break;

                        case 0xF6:
                        case 0xF7:
                            this.instruction = OpCodeTables.Group3Instructions[ModRMBits.GetReg(modrm)];
                            break;

                        case 0xFE:
                            this.instruction = OpCodeTables.Group4Instructions[ModRMBits.GetReg(modrm)];
                            break;

                        case 0xFF:
                            opCode = OpCodeTables.Group5OpCodes[ModRMBits.GetReg(modrm)];
                            this.instruction = opCode.Instruction;
                            break;

                        default:
                            throw this.InvalidInstruction();
                    }

                    if (this.instruction == Instruction.Unknown)
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    this.instruction = opCode.Instruction;
                }

                this.ReadInstruction(opCode.Flags, nextByte, modrm);

                return true;
            }
        }

        private Instruction GetOperandSizeExtendedAlias(
            Instruction wordInstruction,
            Instruction dwordInstruction,
            Instruction qwordInstruction)
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
        private Instruction GetAddressSizeExtendedAlias(
            Instruction wordInstruction,
            Instruction dwordInstruction,
            Instruction qwordInstruction)
        {
            switch (this.GetAddressSize())
            {
                case Size.Qword:
                    return qwordInstruction;

                case Size.Dword:
                    return dwordInstruction;

                default:
                    return wordInstruction;
            }
        }

        private void ReadTwoByteInstruction()
        {
            var opCodeByte = this.instructionByteStream.ReadByte();
            var opCode = OpCodeTables.TwoByteOpCodeMap[opCodeByte];

            byte modrm = (opCode.Flags & OpCodeFlags.ModRM) != 0 ? this.instructionByteStream.ReadByte() : (byte)0;
            
            if (opCode.Instruction == Instruction.Unknown)
            {
                switch (opCodeByte)
                {
                    case 0x01:
                        opCode = OpCodeTables.Group7OpCodes[ModRMBits.GetReg(modrm)];
                        this.instruction = opCode.Instruction;
                        break;

                    case 0x38:
                        this.ReadThreeByteInstructionA();
                        return;

                    case 0xBA:
                    {
                        this.instruction = OpCodeTables.Group8Instructions[ModRMBits.GetReg(modrm)];
                        break;
                    }

                    case 0xC7:
                        this.ReadGroup9Instruction(modrm);
                        return;
                }

                if (this.instruction == Instruction.Unknown)
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                this.instruction = opCode.Instruction;
            }

            this.ReadInstruction(opCode.Flags, opCodeByte, modrm);
        }

        private void ReadGroup9Instruction(byte modrm)
        {
            switch (ModRMBits.GetReg(modrm))
            {
                case 1:
                    var isExtended = (this.rex & RexPrefix.W) != 0;
                    var operandSize = isExtended ? Size.Dqword : Size.Qword;
                    this.instruction = isExtended ? Instruction.Cmpxchg16b : Instruction.Cmpxchg8b;
                    var bits = new ModRMBits(this.rex, modrm);
                    this.operand1 = this.MemoryOperand(ref bits, operandSize);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ReadThreeByteInstructionA()
        {
            // instructions starting 0F 38
            var opCodeByte = this.instructionByteStream.ReadByte();

            switch (opCodeByte)
            {
                case 0xF0:
                case 0xF1:
                    if ((this.prefixes & InstructionPrefixes.RepNZ) != 0)
                    {
                        this.instruction = Instruction.Crc32;
                        var operandSize = (this.rex & RexPrefix.W) != 0 ? Size.Qword : Size.Dword;
                        var rmSize = opCodeByte == 0xF0 ? Size.Byte : this.GetOperandSize();
                        var modrm = new ModRMBits(this.rex, this.instructionByteStream.ReadByte());
                        this.operand1 = this.ModRMRegister(ref modrm, GetBaseRegister(operandSize));
                        this.operand2 = this.RegisterOrMemoryOperand(ref modrm, rmSize);
                        break;
                    }
                    goto default;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ReadInstruction(OpCodeFlags flags, byte opCode, byte modrm)
        {
            if ((flags & OpCodeFlags.CompatibilityMode) != 0
                && (this.executionMode & ExecutionModes.CompatibilityMode) == 0)
            {
                throw this.InvalidInstruction();
            }

            var operandSize = this.GetOperandSize(flags);

            var operand1Encoding = (OperandEncoding)((int)(flags & OpCodeFlags.Operand1Mask) >> (int)OpCodeFlags.Operand1Shift);
            this.operand1 = this.ReadOperand(operand1Encoding, flags, operandSize, opCode, modrm);

            var operand2Encoding = (OperandEncoding)((int)(flags & OpCodeFlags.Operand2Mask) >> (int)OpCodeFlags.Operand2Shift);
            this.operand2 = this.ReadOperand(operand2Encoding, flags, operandSize, opCode, modrm);

            var operand3Encoding = (OperandEncoding)((int)(flags & OpCodeFlags.Operand3Mask) >> (int)OpCodeFlags.Operand3Shift);
            this.operand3 = this.ReadOperand(operand3Encoding, flags, operandSize, opCode, modrm);
        }

        private Size GetOperandSize(OpCodeFlags flags)
        {
            switch (flags & OpCodeFlags.OperandSizeMask)
            {
                case OpCodeFlags.OperandSizeByte:
                    return Size.Byte;

                case OpCodeFlags.OperandSizeWord:
                    return Size.Word;

                case OpCodeFlags.OperandSizeFixed64:
                    if (this.executionMode == ExecutionModes.Long64Bit)
                    {
                        return Size.Qword;
                    }
                    goto default;

                case OpCodeFlags.OperandSizeFar:
                    return (Size)((int)this.GetOperandSize() + 2);

                case OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.OperandSizeFar:
                    if (this.executionMode == ExecutionModes.Long64Bit)
                    {
                        return Size.Tbyte;
                    }
                    goto case OpCodeFlags.OperandSizeFar;

                default:
                    return this.GetOperandSize();
            }
        }

        private Operand ReadOperand(OperandEncoding encoding, OpCodeFlags flags, Size operandSize, byte opCode, byte modrm)
        {
            switch (encoding)
            {
                case OperandEncoding.None:
                    return default(Operand);

                case OperandEncoding.Three:
                    return Operand.ImmediateByte(3);

                case OperandEncoding.Dx:
                    return Operand.DirectRegister(Register.Dx);

                case OperandEncoding.Rax:
                    return Operand.DirectRegister(GetBaseRegister(operandSize));

                case OperandEncoding.M:
                {
                    var bits = new ModRMBits(this.rex, modrm);
                    return this.MemoryOperand(ref bits, operandSize);
                }

                case OperandEncoding.RM:
                {
                    var bits = new ModRMBits(this.rex, modrm);
                    return this.RegisterOrMemoryOperand(ref bits, operandSize);
                }

                case OperandEncoding.Reg:
                {
                    var bits = new ModRMBits(this.rex, modrm);
                    return this.ModRMRegister(ref bits, GetBaseRegister(operandSize));
                }

                case OperandEncoding.OpCodeReg:
                {
                    var register = RegDecoder.GetRegister(
                        this.rex != 0,
                        this.GetOpcodeReg(opCode),
                        GetBaseRegister(operandSize));
                    return Operand.DirectRegister(register);
                }

                case OperandEncoding.Immediate:
                    return this.ReadImmediateOperand(operandSize);

                case OperandEncoding.ImmediateByte:
                    var b = this.instructionByteStream.ReadByte();
                    if ((flags & OpCodeFlags.Ignore10) != 0 && b == 10)
                    {
                        // Special case for AAM and AAD - the base is omitted if it is 10.
                        return default(Operand);
                    }
                    else
                    {
                        return Operand.ImmediateByte(b);
                    }


                case OperandEncoding.FarPointer:
                    return this.ReadFarPointerOperand(operandSize);

                case OperandEncoding.RelativeAddress:
                {
                    var offset = this.ReadImmediateValue(operandSize);
                    return Operand.RelativeAddress(offset);
                }

                case OperandEncoding.RelativeAddressByte:
                {
                    var offset = this.instructionByteStream.ReadByte();
                    return Operand.RelativeAddress(offset);
                }

                default:
                    throw new NotSupportedException();
            }
        }

        private Operand MemoryOperand(ref ModRMBits modrmBits, Size operandSize)
        {
            var addressSize = this.GetAddressSize();

            if (ModRMDecoder.DirectRegister(modrmBits.Mod))
            {
                throw this.InvalidInstruction();
            }

            return this.ReadMemoryAccessOperand(ref modrmBits, operandSize, addressSize);
        }

        private Operand RegisterOrMemoryOperand(ref ModRMBits modrmBits, Size operandSize)
        {
            var addressSize = this.GetAddressSize();

            if (ModRMDecoder.DirectRegister(modrmBits.Mod))
            {
                var register = ModRMDecoder.GetRegister(this.rex, GetBaseRegister(operandSize), ref modrmBits);
                return Operand.DirectRegister(register);
            }

            return this.ReadMemoryAccessOperand(ref modrmBits, operandSize, addressSize);
        }

        private Operand ReadMemoryAccessOperand(
            ref ModRMBits modrmBits,
            Size operandSize,
            Size addressSize)
        {
            var addressSizeBaseRegister = GetBaseRegister(addressSize);
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

        private Operand ModRMRegister(ref ModRMBits modrmBits, Register baseOperandSizeRegister)
        {
            var register = RegDecoder.GetRegister(this.rex != 0, modrmBits.Reg, baseOperandSizeRegister);
            return Operand.DirectRegister(register);
        }

        private Operand ReadImmediateOperand(Size operandSize)
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

        private Operand ReadFarPointerOperand(Size size)
        {
            var segment = this.instructionByteStream.ReadWord();
            var displacement = this.ReadImmediateValue(size);
            return Operand.FarPointer(segment, displacement);
        }

        private Size GetAddressSize()
        {
            var addressSizeOverride = (this.prefixes & InstructionPrefixes.AddressSizeOverride) != 0;

            if ((this.executionMode & ExecutionModes.Long64Bit) != 0)
            {
                return addressSizeOverride ? Size.Dword : Size.Qword;
            }

            return (this.defaultAddressSize == Size.Dword) ^ addressSizeOverride ? Size.Dword : Size.Word;
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

        private static Register GetBaseRegister(Size operandSize)
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

        private int GetOpcodeReg(byte opcode)
        {
            var reg = opcode & 0x7;
            if ((this.rex & RexPrefix.B) != 0)
            {
                reg |= 8;
            }
            return reg;
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
                case Size.Qword: // "z" semantics
                    return this.instructionByteStream.ReadDword();

                default:
                    throw new InvalidOperationException();
            }
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

        private Exception InvalidInstruction()
        {
            return new FormatException("The data does not represent a valid instruction.");
        }

        #endregion
    }
}