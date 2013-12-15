using System;

namespace Fantasm.Disassembler
{
    public partial class InstructionReader
    {
        private void ReadRegisterOrMemoryOperand(int modrm, Register baseRegister)
        {
            var mod = (modrm & 0xc0) >> 6;
            var rm = modrm & 0x07;

            if (mod == 3)
            {
                // encodes a register directly.
                this.operand1.Type = OperandType.Register;
                this.operand1.BaseRegister = this.GetRegister(rm, baseRegister);
                return;
            }

            this.operand1.Type = OperandType.Memory;
            var addressSize = this.GetAddressSize();
            switch (addressSize)
            {
                case Size.Word:
                    this.ReadMemoryParameters16(rm, mod);
                    break;

                default:
                    this.ReadMemoryParameters32(rm, mod, addressSize);
                    break;
            }
        }

        private void ReadMemoryParameters16(int rm, int mod)
        {
            this.operand1.Scale = 1;
            this.ReadRMBaseIndex16(rm);

            switch (mod)
            {
                case 0:
                    if (this.operand1.BaseRegister == Register.Bp && this.operand1.IndexRegister == Register.None)
                    {
                        // instead of BP we use a 16-bit displacement
                        this.operand1.BaseRegister = Register.None;
                        this.ReadWordDisplacement();
                    }
                    else
                    {
                        this.operand1.Displacement = 0;
                    }
                    break;

                case 1:
                    this.ReadByteDisplacement();
                    break;

                case 2:
                    this.ReadWordDisplacement();
                    break;
            }
        }

        private void ReadMemoryParameters32(int rm, int mod, Size addressSize)
        {
            var addressSizeBaseRegister = GetAddressSizeBaseRegister(addressSize);

            // if the r/m bits in the modr/m byte are 5 then we use a sib byte
            int resolvedRM = rm;
            if (rm == 4)
            {
                resolvedRM = this.ReadSibBase(addressSizeBaseRegister);
            }
            else
            {
                this.operand1.IndexRegister = Register.None;
                this.operand1.Scale = 1;
            }
            this.operand1.BaseRegister = this.GetRegister(resolvedRM, addressSizeBaseRegister);

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
                            this.operand1.BaseRegister = addressSizeBaseRegister + (Register.Eip - Register.Eax);
                        }
                        else
                        {
                            // in all other cases, we ignore the register
                            this.operand1.BaseRegister = Register.None;
                        }
                        this.ReadDwordDisplacement();
                    }
                    else
                    {
                        this.operand1.Displacement = 0;
                    }
                    break;

                case 1:
                    this.ReadByteDisplacement();
                    break;

                case 2:
                    this.ReadDwordDisplacement();
                    break;
            }
        }

        private byte ReadSibBase(Register addressSizeBaseRegister)
        {
            var sib = this.stream.ReadByte();
            if (sib < 0)
            {
                throw InvalidInstructionBytes();
            }

            var index = (sib & 0x38) >> 3;
            if (index == 4 && (this.rex & RexPrefix.B) == 0)
            {
                this.operand1.IndexRegister = Register.None;
                this.operand1.Scale = 1;
            }
            else
            {
                this.operand1.IndexRegister = this.GetRegister(index, addressSizeBaseRegister);
                this.operand1.Scale = 1 << (sib >> 6);
            }

            return (byte)(sib & 0x07);
        }

        private void ReadRMBaseIndex16(int rm)
        {
            switch (rm)
            {
                case 0:
                    this.operand1.BaseRegister = Register.Bx;
                    this.operand1.IndexRegister = Register.Si;
                    break;
                case 1:
                    this.operand1.BaseRegister = Register.Bx;
                    this.operand1.IndexRegister = Register.Di;
                    break;
                case 2:
                    this.operand1.BaseRegister = Register.Bp;
                    this.operand1.IndexRegister = Register.Si;
                    break;
                case 3:
                    this.operand1.BaseRegister = Register.Bp;
                    this.operand1.IndexRegister = Register.Di;
                    break;
                case 4:
                    this.operand1.BaseRegister = Register.Si;
                    this.operand1.IndexRegister = Register.None;
                    break;
                case 5:
                    this.operand1.BaseRegister = Register.Di;
                    this.operand1.IndexRegister = Register.None;
                    break;
                case 6:
                    this.operand1.BaseRegister = Register.Bp;
                    this.operand1.IndexRegister = Register.None;
                    break;
                case 7:
                    this.operand1.BaseRegister = Register.Bx;
                    this.operand1.IndexRegister = Register.None;
                    break;
            }
        }

        private Register GetRegister(int reg, Register baseRegister)
        {
            if ((this.rex & RexPrefix.B) != 0)
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

        private void ReadByteDisplacement()
        {
            var value = this.stream.ReadByte();
            if (value < 0)
            {
                throw InvalidInstructionBytes();
            }

            this.operand1.Displacement = value;
        }

        private void ReadWordDisplacement()
        {
            var bytesRead = this.stream.Read(this.buffer, 0, 2);
            if (bytesRead < 2)
            {
                throw InvalidInstructionBytes();
            }

            this.operand1.Displacement = BitConverter.ToInt16(this.buffer, 0);
        }

        private void ReadDwordDisplacement()
        {
            var bytesRead = this.stream.Read(this.buffer, 0, 4);
            if (bytesRead < 4)
            {
                throw InvalidInstructionBytes();
            }

            this.operand1.Displacement = BitConverter.ToInt32(this.buffer, 0);
        }
    }
}
