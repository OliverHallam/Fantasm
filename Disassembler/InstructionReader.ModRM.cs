using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;

namespace Fantasm.Disassembler
{
    public partial class InstructionReader
    {
        private void ReadModRMOperand(int modrm)
        {
            var mod = (modrm & 0xc0) >> 6;
            var rm = modrm & 0x07;

            if (mod == 3)
            {
                // encodes a register directly.
                this.operand1.Type = OperandType.Register;
                this.operand1.BaseRegister = this.GetRegister8(rm);
                return;
            }

            this.operand1.Type = OperandType.Memory;
            if (this.Is32BitOperandSize())
            {
                this.ReadMemoryParameters32(rm, mod);
            }
            else
            {
                this.ReadMemoryParameters16(rm, mod);
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

        private void ReadMemoryParameters32(int rm, int mod)
        {
            this.operand1.BaseRegister = this.GetRegister32(rm);

            // instead of encoding ESP we use a SIB byte
            if (this.operand1.BaseRegister == Register.Esp)
            {
                this.ReadSib();
            }
            else
            {
                this.operand1.IndexRegister = Register.None;
                this.operand1.Scale = 1;
            }

            switch (mod)
            {
                case 0:
                    if (this.operand1.BaseRegister == Register.Ebp)
                    {
                        // instead of EBP we use a 32-bit displacement (whether from modrm or sib)
                        this.operand1.BaseRegister = Register.None;
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

        private void ReadSib()
        {
            var sib = this.stream.ReadByte();
            if (sib < 0)
            {
                throw InvalidInstructionBytes();
            }

            var index = (sib & 0x38) >> 3;
            if (index == 4)
            {
                this.operand1.IndexRegister = Register.None;
                this.operand1.Scale = 1;
            }
            else
            {
                this.operand1.IndexRegister = this.GetRegister32(index);
                this.operand1.Scale = 1 << (sib >> 6);
            }

            var baseValue = sib & 0x07;
            this.operand1.BaseRegister = this.GetRegister32(baseValue);
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

        private Register GetRegister8(int reg)
        {
            switch (reg)
            {
                default:
                    return Register.Al;
                case 1:
                    return Register.Cl;
                case 2:
                    return Register.Dl;
                case 3:
                    return Register.Bl;
                case 4:
                    return Register.Ah;
                case 5:
                    return Register.Ch;
                case 6:
                    return Register.Dh;
                case 7:
                    return Register.Bh;
            }
        }

        private Register GetRegister32(int rm)
        {
            switch (rm)
            {
                case 0:
                    return Register.Eax;
                case 1:
                    return Register.Ecx;
                case 2:
                    return Register.Edx;
                case 3:
                    return Register.Ebx;
                case 4:
                    return Register.Esp;
                case 5:
                    return Register.Ebp;
                case 6:
                    return Register.Esi;
                case 7:
                    return Register.Edi;
                default:
                    return Register.None;
            }
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
