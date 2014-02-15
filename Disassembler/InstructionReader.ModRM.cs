namespace Fantasm.Disassembler
{
    public partial class InstructionReader
    {
        private OperandType RegisterOrMemory(int modrm, Register baseRegister, Size operandSize)
        {
            var mod = (modrm & 0xc0) >> 6;
            var rm = modrm & 0x07;

            if (mod == 3)
            {
                // encodes a register directly.
                this.baseRegister = this.GetRegister(RexPrefix.B, rm, baseRegister);
                return OperandType.DirectRegister;
            }

            return this.ReadMemory(mod, rm, operandSize);
        }

        private OperandType Memory(int modrm, Size operandSize)
        {
            var mod = (modrm & 0xc0) >> 6;
            var rm = modrm & 0x07;

            if (mod == 3)
                throw InvalidInstructionBytes();

            return this.ReadMemory(mod, rm, operandSize);
        }

        private OperandType ReadMemory(int mod, int rm, Size operandSize)
        {
            var addressSize = this.GetAddressSize();
            switch (addressSize)
            {
                case Size.Word:
                    this.ReadMemory16(mod, rm);
                    break;

                default:
                    this.ReadMemory32(mod, rm, addressSize);
                    break;
            }

            switch (operandSize)
            {
                case Size.Byte:
                    return OperandType.BytePointer;
                case Size.Word:
                    return OperandType.WordPointer;
                case Size.Dword:
                    return OperandType.DwordPointer;
                default:
                    return OperandType.QwordPointer;
            }
        }

        private OperandType ModRMRegister(int modrm, Register baseRegister)
        {
            var reg = (modrm & 0x38) >> 3;
            this.register = this.GetRegister(RexPrefix.R, reg, baseRegister);
            return OperandType.Register;
        }

        private void ReadMemory16(int mod, int rm)
        {
            this.ReadRMBaseIndex16(rm);

            switch (mod)
            {
                case 0:
                    if (this.baseRegister == Register.Bp && this.indexRegister == Register.None)
                    {
                        // instead of BP we use a 16-bit displacement
                        this.baseRegister = Register.None;
                        this.displacement = this.ReadWord();
                    }
                    break;

                case 1:
                    this.displacement = this.ReadByte();
                    break;

                case 2:
                    this.displacement = this.ReadWord();
                    break;
            }
        }

        private void ReadMemory32(int mod, int rm, Size addressSize)
        {
            var addressSizeBaseRegister = GetAddressSizeBaseRegister(addressSize);

            // if the r/m bits in the modr/m byte are 5 then we use a sib byte
            int resolvedRM = rm;
            if (rm == 4)
            {
                resolvedRM = this.ReadSibBase(addressSizeBaseRegister);
            }
            this.baseRegister = this.GetRegister(RexPrefix.B, resolvedRM, addressSizeBaseRegister);

            switch (mod)
            {
                case 0:
                    if (resolvedRM == 5)
                    {
                        // in this case we always read a displacement (the base r/m, ignoring Rex.B from either modr/m
                        // or sib was 5).
                        if (this.executionMode == ExecutionModes.Long64Bit && rm == 5)
                        {
                            // in 64-bit mode without a sib byte we use RIP based addressing
                            this.baseRegister = addressSizeBaseRegister + (Register.Eip - Register.Eax);
                        }
                        else
                        {
                            // in all other cases, we ignore the register
                            this.baseRegister = Register.None;
                        }
                        this.displacement = this.ReadDword();
                    }
                    else
                    {
                        this.displacement = 0;
                    }
                    break;

                case 1:
                    this.displacement = this.ReadByte();
                    break;

                case 2:
                    this.displacement = this.ReadDword();
                    break;
            }
        }

        private byte ReadSibBase(Register addressSizeBaseRegister)
        {
            var sib = this.ReadByte();

            var index = (sib & 0x38) >> 3;
            if (index != 4 || (this.rex & RexPrefix.B) != 0)
            {
                this.indexRegister = this.GetRegister(RexPrefix.B,  index, addressSizeBaseRegister);
                this.scale = 1 << (sib >> 6);
            }

            return (byte)(sib & 0x07);
        }

        private void ReadRMBaseIndex16(int rm)
        {
            switch (rm)
            {
                case 0:
                    this.baseRegister = Register.Bx;
                    this.indexRegister = Register.Si;
                    break;
                case 1:
                    this.baseRegister = Register.Bx;
                    this.indexRegister = Register.Di;
                    break;
                case 2:
                    this.baseRegister = Register.Bp;
                    this.indexRegister = Register.Si;
                    break;
                case 3:
                    this.baseRegister = Register.Bp;
                    this.indexRegister = Register.Di;
                    break;
                case 4:
                    this.baseRegister = Register.Si;
                    break;
                case 5:
                    this.baseRegister = Register.Di;
                    break;
                case 6:
                    this.baseRegister = Register.Bp;
                    break;
                case 7:
                    this.baseRegister = Register.Bx;
                    break;
            }
        }

        private Register GetRegister(RexPrefix rexBit, int reg, Register baseRegister)
        {
            if ((this.rex & rexBit) != 0)
            {
                return baseRegister + 8 + reg;    
            }
            if (reg < 4)
            {
                return baseRegister + GetABCDRegisterOffset(reg);
            }
            if (baseRegister == Register.Al && this.rex == 0)
            {
                return Register.Ah + GetABCDRegisterOffset(reg - 4);
            }
            return baseRegister + GetDiSiBpSpRegisterOffset(reg);
        }

        private static Register GetAddressSizeBaseRegister(Size addressSize)
        {
            return addressSize == Size.Qword ? Register.Rax : Register.Eax;
        }

        private static int GetABCDRegisterOffset(int reg)
        {
            switch (reg)
            {
                default:
                    // AX
                    return 0;
                case 1:
                    // CX
                    return 2;
                case 2:
                    // DX
                    return 3;
                case 3:
                    // BX
                    return 1;
            }
        }

        private static int GetDiSiBpSpRegisterOffset(int reg)
        {
            // [4,5,6,7] => [7,6,5,4] (SP, BP, SI, DI)
            return 11 - reg;
        }
    }
}
