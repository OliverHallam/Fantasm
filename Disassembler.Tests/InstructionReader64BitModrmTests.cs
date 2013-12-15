﻿using NUnit.Framework;

namespace Fantasm.Disassembler.Tests
{
    class InstructionReader64BitModrmTests : InstructionReaderTestBase
    {
        [Test]
        [TestCase(0, Register.Rax)]
        [TestCase(1, Register.Rcx)]
        [TestCase(2, Register.Rdx)]
        [TestCase(3, Register.Rbx)]
        [TestCase(6, Register.Rsi)]
        [TestCase(7, Register.Rdi)]
        public void ModRM_ForBaseRegisterWithNoDisplacement_DecodesCorrectRegister(byte modrm, Register register)
        {
            // ADD [REG] 0
            var reader = ReadBytes64(0x80, modrm, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(register, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(0, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, Register.R8)]
        [TestCase(1, Register.R9)]
        [TestCase(2, Register.R10)]
        [TestCase(3, Register.R11)]
        [TestCase(6, Register.R14)]
        [TestCase(7, Register.R15)]
        public void ModRM_ForBaseRegisterWithNoDisplacementWithRexB_DecodesCorrectRegister(byte modrm, Register register)
        {
            // ADD [REG] 0
            var reader = ReadBytes64(0x41, 0x80, modrm, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(register, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(0, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, Register.Rax)]
        [TestCase(1, Register.Rcx)]
        [TestCase(2, Register.Rdx)]
        [TestCase(3, Register.Rbx)]
        [TestCase(5, Register.Rbp)]
        [TestCase(6, Register.Rsi)]
        [TestCase(7, Register.Rdi)]
        public void ModRM_ForBaseRegisterWithByteDisplacement_DecodesCorrectRegister(byte modrm, Register register)
        {
            // ADD [REG + 0x23] 0
            var reader = ReadBytes64(0x80, (byte)(0x40 | modrm), 0x23, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(register, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(0x23, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, Register.R8)]
        [TestCase(1, Register.R9)]
        [TestCase(2, Register.R10)]
        [TestCase(3, Register.R11)]
        [TestCase(5, Register.R13)]
        [TestCase(6, Register.R14)]
        [TestCase(7, Register.R15)]
        public void ModRM_ForBaseRegisterWithByteDisplacementWithRexB_DecodesCorrectRegister(byte modrm, Register register)
        {
            // ADD [REG + 0x23] 0
            var reader = ReadBytes64(0x41, 0x80, (byte)(0x40 | modrm), 0x23, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(register, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(0x23, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, Register.Rax)]
        [TestCase(1, Register.Rcx)]
        [TestCase(2, Register.Rdx)]
        [TestCase(3, Register.Rbx)]
        [TestCase(5, Register.Rbp)]
        [TestCase(6, Register.Rsi)]
        [TestCase(7, Register.Rdi)]
        public void ModRM_ForBaseRegisterWithDwordDisplacement_DecodesCorrectRegister(byte modrm, Register register)
        {
            // ADD [REG + 0x01234567] 0
            var reader = ReadBytes64(0x80, (byte)(0x80 | modrm), 0x67, 0x45, 0x23, 0x01, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(register, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(0x01234567, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, Register.R8)]
        [TestCase(1, Register.R9)]
        [TestCase(2, Register.R10)]
        [TestCase(3, Register.R11)]
        [TestCase(5, Register.R13)]
        [TestCase(6, Register.R14)]
        [TestCase(7, Register.R15)]
        public void ModRM_ForBaseRegisterWithDwordDisplacementWithRexB_DecodesCorrectRegister(byte modrm, Register register)
        {
            // ADD [REG + 0x01234567] 0
            var reader = ReadBytes64(0x41, 0x80, (byte)(0x80 | modrm), 0x67, 0x45, 0x23, 0x01, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(register, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(0x01234567, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, Register.Eax)]
        [TestCase(1, Register.Ecx)]
        [TestCase(2, Register.Edx)]
        [TestCase(3, Register.Ebx)]
        [TestCase(6, Register.Esi)]
        [TestCase(7, Register.Edi)]
        public void ModRM_ForBaseRegisterWithNoDisplacement_DecodesCorrect32BitRegister(byte modrm, Register register)
        {
            // ADD [REG] 0
            var reader = ReadBytes64(0x67, 0x80, modrm, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(register, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(0, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, Register.R8D)]
        [TestCase(1, Register.R9D)]
        [TestCase(2, Register.R10D)]
        [TestCase(3, Register.R11D)]
        [TestCase(6, Register.R14D)]
        [TestCase(7, Register.R15D)]
        public void ModRM_ForBaseRegisterWithNoDisplacementWithRexB_DecodesCorrect32BitRegister(byte modrm, Register register)
        {
            // ADD [REG] 0
            var reader = ReadBytes64(0x67, 0x41, 0x80, modrm, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(register, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(0, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, Register.Eax)]
        [TestCase(1, Register.Ecx)]
        [TestCase(2, Register.Edx)]
        [TestCase(3, Register.Ebx)]
        [TestCase(5, Register.Ebp)]
        [TestCase(6, Register.Esi)]
        [TestCase(7, Register.Edi)]
        public void ModRM_ForBaseRegisterWithByteDisplacement_DecodesCorrect32BitRegister(byte modrm, Register register)
        {
            // ADD [REG + 0x23] 0
            var reader = ReadBytes64(0x67, 0x80, (byte)(0x40 | modrm), 0x23, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(register, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(0x23, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, Register.R8D)]
        [TestCase(1, Register.R9D)]
        [TestCase(2, Register.R10D)]
        [TestCase(3, Register.R11D)]
        [TestCase(5, Register.R13D)]
        [TestCase(6, Register.R14D)]
        [TestCase(7, Register.R15D)]
        public void ModRM_ForBaseRegisterWithByteDisplacementWithRexB_DecodesCorrect32BitRegister(byte modrm, Register register)
        {
            // ADD [REG + 0x23] 0
            var reader = ReadBytes64(0x67, 0x41, 0x80, (byte)(0x40 | modrm), 0x23, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(register, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(0x23, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, Register.Eax)]
        [TestCase(1, Register.Ecx)]
        [TestCase(2, Register.Edx)]
        [TestCase(3, Register.Ebx)]
        [TestCase(5, Register.Ebp)]
        [TestCase(6, Register.Esi)]
        [TestCase(7, Register.Edi)]
        public void ModRM_ForBaseRegisterWithDwordDisplacement_DecodesCorrect32BitRegister(byte modrm, Register register)
        {
            // ADD [REG + 0x01234567] 0
            var reader = ReadBytes64(0x67, 0x80, (byte)(0x80 | modrm), 0x67, 0x45, 0x23, 0x01, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(register, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(0x01234567, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, Register.R8D)]
        [TestCase(1, Register.R9D)]
        [TestCase(2, Register.R10D)]
        [TestCase(3, Register.R11D)]
        [TestCase(5, Register.R13D)]
        [TestCase(6, Register.R14D)]
        [TestCase(7, Register.R15D)]
        public void ModRM_ForBaseRegisterWithDwordDisplacementWithRexB_DecodesCorrect32BitRegister(byte modrm, Register register)
        {
            // ADD [REG + 0x01234567] 0
            var reader = ReadBytes64(0x67, 0x41, 0x80, (byte)(0x80 | modrm), 0x67, 0x45, 0x23, 0x01, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(register, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(0x01234567, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, Register.Al)]
        [TestCase(1, Register.Cl)]
        [TestCase(2, Register.Dl)]
        [TestCase(3, Register.Bl)]
        [TestCase(4, Register.Ah)]
        [TestCase(5, Register.Ch)]
        [TestCase(6, Register.Dh)]
        [TestCase(7, Register.Bh)]
        public void ModRM_WithoutRexPrefix_DecodesCorrectByteRegister(byte modrm, Register register)
        {
            // ADD REG 0
            var reader = ReadBytes64(0x80, (byte)(0xc0 | modrm), 0x00);
            reader.Read();

            Assert.AreEqual(register, reader.GetOperandRegister(0));
        }

        [Test]
        [TestCase(0, Register.Al)]
        [TestCase(1, Register.Cl)]
        [TestCase(2, Register.Dl)]
        [TestCase(3, Register.Bl)]
        [TestCase(4, Register.Spl)]
        [TestCase(5, Register.Bpl)]
        [TestCase(6, Register.Sil)]
        [TestCase(7, Register.Dil)]
        public void ModRM_WithRexPrefix_DecodesCorrectByteRegister(byte modrm, Register register)
        {
            // ADD REG 0
            var reader = ReadBytes64(0x40, 0x80, (byte)(0xc0 | modrm), 0x00);
            reader.Read();

            Assert.AreEqual(register, reader.GetOperandRegister(0));
        }


        [Test]
        [TestCase(0, Register.R8L)]
        [TestCase(1, Register.R9L)]
        [TestCase(2, Register.R10L)]
        [TestCase(3, Register.R11L)]
        [TestCase(4, Register.R12L)]
        [TestCase(5, Register.R13L)]
        [TestCase(6, Register.R14L)]
        [TestCase(7, Register.R15L)]
        public void ModRM_WithRexB_DecodesCorrectByteRegister(byte modrm, Register register)
        {
            // ADD REG 0
            var reader = ReadBytes64(0x41, 0x80, (byte)(0xc0 | modrm), 0x00);
            reader.Read();

            Assert.AreEqual(register, reader.GetOperandRegister(0));
        }

        [Test]
        public void ModRM_InsteadOfRbpWithNoDisplacement_UsesRipAddressingWithDwordDisplacement()
        {
            // ADD [RIP + 0x01234567] 0
            var reader = ReadBytes64(0x80, 0x05, 0x67, 0x45, 0x23, 0x01, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(Register.Rip, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(0x01234567, reader.GetOperandDisplacement(0));
        }

        [Test]
        public void ModRM_WithRexB_StillResolvesRipAddressing()
        {
            // ADD [RIP + 0x01234567] 0
            var reader = ReadBytes64(0x41, 0x80, 0x05, 0x67, 0x45, 0x23, 0x01, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(Register.Rip, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(0x01234567, reader.GetOperandDisplacement(0));
        }

        [Test]
        public void ModRM_WithAddressSizeOverride_CanDecodeEipAddressing()
        {
            // ADD [RIP + 0x01234567] 0
            var reader = ReadBytes64(0x67, 0x80, 0x05, 0x67, 0x45, 0x23, 0x01, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(Register.Eip, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(0x01234567, reader.GetOperandDisplacement(0));
        }

        [Test]
        public void ModRM_ForSibAddressWithNoDisplacement_ReadsSibAndDisplacement()
        {
            // ADD ([RAX] + [RAX] * 2) 0
            var reader = ReadBytes64(0x80, 0x04, 0x40, 0x00);
            reader.Read();

            Assert.AreEqual(2, reader.GetOperandScale(0));
            Assert.AreEqual(0, reader.GetOperandDisplacement(0));
        }

        [Test]
        public void ModRM_ForSibAddressWithByteDisplacement_ReadsSibAndDisplacement()
        {
            // ADD ([RAX] + [RAX] * 2 + 0x12) 0
            var reader = ReadBytes64(0x80, 0x44, 0x40, 0x12, 0x00);
            reader.Read();

            Assert.AreEqual(2, reader.GetOperandScale(0));
            Assert.AreEqual(0x12, reader.GetOperandDisplacement(0));
        }

        [Test]
        public void ModRM_ForSibAddressWithDwordDisplacement_ReadsSibAndDisplacement()
        {
            // ADD ([RAX] + [RAX] * 2 + 0x12345678) 0
            var reader = ReadBytes32(0x80, 0x84, 0x40, 0x78, 0x56, 0x34, 0x12, 0x00);
            reader.Read();

            Assert.AreEqual(2, reader.GetOperandScale(0));
            Assert.AreEqual(0x12345678, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, Register.Rax)]
        [TestCase(1, Register.Rcx)]
        [TestCase(2, Register.Rdx)]
        [TestCase(3, Register.Rbx)]
        [TestCase(4, Register.Rsp)]
        [TestCase(6, Register.Rsi)]
        [TestCase(7, Register.Rdi)]
        public void ModRM_ForSibAddress_ReadsCorrectBaseRegister(byte sibBase, Register register)
        {
            // ADD ([BASE + EAX]) 0
            var reader = ReadBytes64(0x80, 0x04, sibBase, 0x00);
            reader.Read();

            Assert.AreEqual(register, reader.GetOperandBaseRegister(0));
        }

        [Test]
        [TestCase(0, Register.R8)]
        [TestCase(1, Register.R9)]
        [TestCase(2, Register.R10)]
        [TestCase(3, Register.R11)]
        [TestCase(4, Register.R12)]
        [TestCase(6, Register.R14)]
        [TestCase(7, Register.R15)]
        public void ModRM_ForSibAddressWithRexB_ReadsCorrectBaseRegister(byte sibBase, Register register)
        {
            // ADD ([BASE + EAX]) 0
            var reader = ReadBytes64(0x41, 0x80, 0x04, sibBase, 0x00);
            reader.Read();

            Assert.AreEqual(register, reader.GetOperandBaseRegister(0));
        }

        [Test]
        [TestCase(0, Register.Eax)]
        [TestCase(1, Register.Ecx)]
        [TestCase(2, Register.Edx)]
        [TestCase(3, Register.Ebx)]
        [TestCase(4, Register.Esp)]
        [TestCase(6, Register.Esi)]
        [TestCase(7, Register.Edi)]
        public void ModRM_ForSibAddressWithAddressSizeOverride_ReadsCorrectBaseRegister(byte sibBase, Register register)
        {
            // ADD ([BASE + EAX]) 0
            var reader = ReadBytes64(0x67, 0x80, 0x04, sibBase, 0x00);
            reader.Read();

            Assert.AreEqual(register, reader.GetOperandBaseRegister(0));
        }

        [Test]
        [TestCase(0, Register.R8D)]
        [TestCase(1, Register.R9D)]
        [TestCase(2, Register.R10D)]
        [TestCase(3, Register.R11D)]
        [TestCase(4, Register.R12D)]
        [TestCase(6, Register.R14D)]
        [TestCase(7, Register.R15D)]
        public void ModRM_ForSibAddressWithAddressSizeOverrideAndRexB_ReadsCorrectBaseRegister(byte sibBase, Register register)
        {
            // ADD ([BASE + EAX]) 0
            var reader = ReadBytes64(0x67, 0x41, 0x80, 0x04, sibBase, 0x00);
            reader.Read();

            Assert.AreEqual(register, reader.GetOperandBaseRegister(0));
        }

        [Test]
        [TestCase(0, Register.Rax)]
        [TestCase(1, Register.Rcx)]
        [TestCase(2, Register.Rdx)]
        [TestCase(3, Register.Rbx)]
        [TestCase(4, Register.None)]
        [TestCase(5, Register.Rbp)]
        [TestCase(6, Register.Rsi)]
        [TestCase(7, Register.Rdi)]
        public void ModRM_ForSibAddress_ReadsCorrectIndexRegister(byte sibIndex, Register register)
        {
            // ADD ([RAX + index]) 0
            var reader = ReadBytes64(0x80, 0x04, (byte)(sibIndex<<3), 0x00);
            reader.Read();

            Assert.AreEqual(register, reader.GetOperandIndexRegister(0));
        }

        [Test]
        [TestCase(0, Register.R8)]
        [TestCase(1, Register.R9)]
        [TestCase(2, Register.R10)]
        [TestCase(3, Register.R11)]
        [TestCase(4, Register.R12)]
        [TestCase(5, Register.R13)]
        [TestCase(6, Register.R14)]
        [TestCase(7, Register.R15)]
        public void ModRM_ForSibAddressWithRexB_ReadsCorrectIndexRegister(byte sibIndex, Register register)
        {
            // ADD ([RAX + index]) 0
            var reader = ReadBytes64(0x41, 0x80, 0x04, (byte)(sibIndex << 3), 0x00);
            reader.Read();

            Assert.AreEqual(register, reader.GetOperandIndexRegister(0));
        }

        [Test]
        [TestCase(0, Register.Eax)]
        [TestCase(1, Register.Ecx)]
        [TestCase(2, Register.Edx)]
        [TestCase(3, Register.Ebx)]
        [TestCase(4, Register.None)]
        [TestCase(5, Register.Ebp)]
        [TestCase(6, Register.Esi)]
        [TestCase(7, Register.Edi)]
        public void ModRM_ForSibAddressWithAddressSizeOverride_ReadsCorrectIndexRegister(byte sibIndex, Register register)
        {
            // ADD ([EAX + index]) 0
            var reader = ReadBytes64(0x67, 0x80, 0x04, (byte)(sibIndex << 3), 0x00);
            reader.Read();

            Assert.AreEqual(register, reader.GetOperandIndexRegister(0));
        }

        [Test]
        [TestCase(0, Register.R8D)]
        [TestCase(1, Register.R9D)]
        [TestCase(2, Register.R10D)]
        [TestCase(3, Register.R11D)]
        [TestCase(4, Register.R12D)]
        [TestCase(5, Register.R13D)]
        [TestCase(6, Register.R14D)]
        [TestCase(7, Register.R15D)]
        public void ModRM_ForSibAddressWithAddressSizeOverrideAndRexB_ReadsCorrectIndexRegister(byte sibIndex, Register register)
        {
            // ADD ([EAX + index]) 0
            var reader = ReadBytes64(0x67, 0x41, 0x80, 0x04, (byte)(sibIndex << 3), 0x00);
            reader.Read();

            Assert.AreEqual(register, reader.GetOperandIndexRegister(0));
        }
    }
}
