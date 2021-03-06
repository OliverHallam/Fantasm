﻿using System;

using NUnit.Framework;

namespace Fantasm.Disassembler.Tests
{
    class InstructionReader32BitModrmTests : InstructionReaderTestBase
    {
        [Test]
        public void ModRM_WithNoAddressSizeOverride_Uses32BitAddresses()
        {
            // ADD [0x12345678] 0
            var reader = ReadBytes32(0x80, 0x05, 0x78, 0x56, 0x34, 0x12, 0x00);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void ModRM_WithAddressSizeOverride_Uses16BitAddresses()
        {
            // ADD [0x1234] 0
            var reader = ReadBytes32(0x67, 0x80, 0x06, 0x34, 0x12, 0x00);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        [TestCase(0, Register.Eax)]
        [TestCase(1, Register.Ecx)]
        [TestCase(2, Register.Edx)]
        [TestCase(3, Register.Ebx)]
        [TestCase(6, Register.Esi)]
        [TestCase(7, Register.Edi)]
        public void ModRM_ForAddressWithNoDisplacement_DecodesCorrectRegister(byte modrm, Register register)
        {
            // ADD [REG] 0
            var reader = ReadBytes32(0x80, modrm, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.BytePointer, reader.Operand1.Type);
            Assert.AreEqual(register, reader.Operand1.GetBaseRegister());
            Assert.AreEqual(0, reader.Operand1.GetDisplacement());
        }

        [Test]
        public void ModRM_WithJustDisplacement_DecodesCorrectly()
        {
            // ADD ([0x01234567]) 0
            var reader = ReadBytes32(0x80, 0x05, 0x67, 0x45, 0x23, 0x01, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.BytePointer, reader.Operand1.Type);
            Assert.AreEqual(Register.None, reader.Operand1.GetBaseRegister());
            Assert.AreEqual(0x01234567, reader.Operand1.GetDisplacement());
        }

        [Test]
        [TestCase(0, Register.Eax)]
        [TestCase(1, Register.Ecx)]
        [TestCase(2, Register.Edx)]
        [TestCase(3, Register.Ebx)]
        [TestCase(5, Register.Ebp)]
        [TestCase(6, Register.Esi)]
        [TestCase(7, Register.Edi)]
        public void ModRM_ForAddressWithByteDiplacement_ReadsCorrectRegister(byte modrmReg, Register register)
        {
            // ADD ([EAX + 0x23]) 0
            var reader = ReadBytes32(0x80, (byte)(0x40 | modrmReg), 0x23, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.BytePointer, reader.Operand1.Type);
            Assert.AreEqual(register, reader.Operand1.GetBaseRegister());
            Assert.AreEqual(0x23, reader.Operand1.GetDisplacement());
        }

        [Test]
        [TestCase(0, Register.Eax)]
        [TestCase(1, Register.Ecx)]
        [TestCase(2, Register.Edx)]
        [TestCase(3, Register.Ebx)]
        [TestCase(5, Register.Ebp)]
        [TestCase(6, Register.Esi)]
        [TestCase(7, Register.Edi)]
        public void ModRM_ForAddressWithDwordDiplacement_ReadsCorrectRegister(byte modrmReg, Register register)
        {
            // ADD ([EAX + 0x01234567]) 0
            var reader = ReadBytes32(0x80, (byte)(0x80 | modrmReg), 0x67, 0x45, 0x23, 0x01, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.BytePointer, reader.Operand1.Type);
            Assert.AreEqual(register, reader.Operand1.GetBaseRegister());
            Assert.AreEqual(0x01234567, reader.Operand1.GetDisplacement());
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
        public void ModRM_ForDirect8BitRegister_EncodesCorrectRegister(byte modrmReg, Register register)
        {
            // ADD AL 0
            var reader = ReadBytes32(0x80, (byte)(0xc0 | modrmReg), 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
            Assert.AreEqual(register, reader.Operand1.GetBaseRegister());
        }

        [Test]
        [TestCase(0, Register.Ax)]
        [TestCase(1, Register.Cx)]
        [TestCase(2, Register.Dx)]
        [TestCase(3, Register.Bx)]
        [TestCase(4, Register.Sp)]
        [TestCase(5, Register.Bp)]
        [TestCase(6, Register.Si)]
        [TestCase(7, Register.Di)]
        public void ModRM_ForDirect16BitRegister_EncodesCorrectRegister(byte modrmReg, Register register)
        {
            // ADD AX 0
            var reader = ReadBytes32(0x66, 0x81, (byte)(0xc0 | modrmReg), 0x00, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
            Assert.AreEqual(register, reader.Operand1.GetBaseRegister());
        }

        [Test]
        [TestCase(0, Register.Eax)]
        [TestCase(1, Register.Ecx)]
        [TestCase(2, Register.Edx)]
        [TestCase(3, Register.Ebx)]
        [TestCase(4, Register.Esp)]
        [TestCase(5, Register.Ebp)]
        [TestCase(6, Register.Esi)]
        [TestCase(7, Register.Edi)]
        public void ModRM_ForDirect32BitRegister_EncodesCorrectRegister(byte modrmReg, Register register)
        {
            // ADD AX 0
            var reader = ReadBytes32(0x81, (byte)(0xc0 | modrmReg), 0x00, 0x00, 0x00, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
            Assert.AreEqual(register, reader.Operand1.GetBaseRegister());
        }

        [Test]
        public void ModRM_WithRegisterOperand_DecodesRegister()
        {
            // ADD AL AH
            var reader = ReadBytes32(0x00, 0xe0);
            reader.Read();

            Assert.AreEqual(Register.Ah, reader.Operand2.GetRegister());
        }

        [Test]
        public void ModRM_ForSibAddressWithNoDisplacement_ReadsSibAndDisplacement()
        {
            // ADD ([EAX] + [EAX] * 2) 0
            var reader = ReadBytes32(0x80, 0x04, 0x40, 0x00);
            reader.Read();

            Assert.AreEqual(2, reader.Operand1.GetScale());
            Assert.AreEqual(0, reader.Operand1.GetDisplacement());
        }

        [Test]
        public void ModRM_ForSibAddressWithByteDisplacement_ReadsSibAndDisplacement()
        {
            // ADD ([EAX] + [EAX] * 2 + 0x12) 0
            var reader = ReadBytes32(0x80, 0x44, 0x40, 0x12, 0x00);
            reader.Read();

            Assert.AreEqual(2, reader.Operand1.GetScale());
            Assert.AreEqual(0x12, reader.Operand1.GetDisplacement());
        }

        [Test]
        public void ModRM_ForSibAddressWithDwordDisplacement_ReadsSibAndDisplacement()
        {
            // ADD ([EAX] + [EAX] * 2 + 0x12345678) 0
            var reader = ReadBytes32(0x80, 0x84, 0x40, 0x78, 0x56, 0x34, 0x12, 0x00);
            reader.Read();

            Assert.AreEqual(2, reader.Operand1.GetScale());
            Assert.AreEqual(0x12345678, reader.Operand1.GetDisplacement());
        }

        [Test]
        [TestCase(0, Register.Eax)]
        [TestCase(1, Register.Ecx)]
        [TestCase(2, Register.Edx)]
        [TestCase(3, Register.Ebx)]
        [TestCase(4, Register.Esp)]
        [TestCase(6, Register.Esi)]
        [TestCase(7, Register.Edi)]
        public void ModRM_ForSibAddress_ReadsCorrectBaseRegister(byte sibBase, Register register)
        {
            // ADD ([BASE + EAX]) 0
            var reader = ReadBytes32(0x80, 0x04, sibBase, 0x00);
            reader.Read();

            Assert.AreEqual(register, reader.Operand1.GetBaseRegister());
        }

        [Test]
        [TestCase(0, Register.Eax)]
        [TestCase(1, Register.Ecx)]
        [TestCase(2, Register.Edx)]
        [TestCase(3, Register.Ebx)]
        [TestCase(5, Register.Ebp)]
        [TestCase(6, Register.Esi)]
        [TestCase(7, Register.Edi)]
        public void ModRM_ForSibAddress_ReadsCorrectIndexRegister(byte sibIndex, Register register)
        {
            // ADD ([EAX + REG]) 0
            var reader = ReadBytes32(0x80, 0x04, (byte)(sibIndex << 3), 0x00);
            reader.Read();

            Assert.AreEqual(register, reader.Operand1.GetIndexRegister());
        }


        [Test]
        [TestCase(0, 1)]
        [TestCase(1, 2)]
        [TestCase(2, 4)]
        [TestCase(3, 8)]
        public void ModRM_ForSibAddress_ReadsCorrectScale(byte sibScale, int scale)
        {
            // ADD ([EAX + EAX * Scale]) 0
            var reader = ReadBytes32(0x80, 0x04, (byte)(sibScale << 6), 0x00);
            reader.Read();

            Assert.AreEqual(scale, reader.Operand1.GetScale());
        }

        [Test]
        public void ModRM_WithSibAddressWithNoBase_ReadsCorrectBaseAndOffset()
        {
            // ADD ([EAX*2 + 0x01234567]) 0
            var reader = ReadBytes32(0x80, 0x04, 0x45, 0x67, 0x45, 0x23, 0x01, 0x00);
            reader.Read();

            Assert.AreEqual(Register.None, reader.Operand1.GetBaseRegister());
            Assert.AreEqual(0x01234567, reader.Operand1.GetDisplacement());
        }


        [Test]
        public void ModRM_WithSib_CanAddByteDisplacementToEbp()
        {
            // ADD ([EBP] + 0x23) 0
            var reader = ReadBytes32(0x80, 0x44, 0x25, 0x23, 0x00);
            reader.Read();

            Assert.AreEqual(Register.Ebp, reader.Operand1.GetBaseRegister());
            Assert.AreEqual(0x23, reader.Operand1.GetDisplacement());
        }

        [Test]
        public void ModRM_WithSib_CanAddDwordDisplacementToEbp()
        {
            // ADD ([EBP] + 0x01234567) 0
            var reader = ReadBytes32(0x80, 0x84, 0x25, 0x67, 0x45, 0x23, 0x01, 0x00);
            reader.Read();

            Assert.AreEqual(Register.Ebp, reader.Operand1.GetBaseRegister());
            Assert.AreEqual(0x01234567, reader.Operand1.GetDisplacement());
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void ModRM_WithSibWithNoIndexRegister_IgnoresScale(int scale)
        {
            // ADD ([EAX]) 0
            var reader = ReadBytes32(0x80, 0x04, (byte)((scale << 6) | 0x20), 0x00);
            reader.Read();

            Assert.AreEqual(Register.None, reader.Operand1.GetIndexRegister());
            Assert.AreEqual(1, reader.Operand1.GetScale());
        }


        [Test]
        [TestCase(0, Register.Es)]
        [TestCase(1, Register.Cs)]
        [TestCase(2, Register.Ss)]
        [TestCase(3, Register.Ds)]
        [TestCase(4, Register.Fs)]
        [TestCase(5, Register.Gs)]
        public void ModRM_ForSegmentRegister_DecodesCorrectRegister(byte modrmReg, Register register)
        {
            var reader = ReadBytes32(0x8C, (byte)(0xc0 | (modrmReg << 3)), 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Register, reader.Operand2.Type);
            Assert.AreEqual(register, reader.Operand2.GetBaseRegister());
        }

        [Test]
        [TestCase(0, Register.Cr0)]
        [TestCase(2, Register.Cr2)]
        [TestCase(3, Register.Cr3)]
        [TestCase(4, Register.Cr4)]
        public void ModRM_ForControlRegister_DecodesCorrectRegister(byte modrmReg, Register register)
        {
            var reader = ReadBytes32(0x0F, 0x20, (byte)(0xc0 | (modrmReg << 3)), 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Register, reader.Operand2.Type);
            Assert.AreEqual(register, reader.Operand2.GetBaseRegister());
        }


        [Test]
        [ExpectedException(typeof(FormatException))]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        public void ModRM_ForInvalidControlRegister_Fails(byte modrmReg)
        {
            var reader = ReadBytes32(0x0F, 0x20, (byte)(0xc0 | (modrmReg << 3)), 0x00);
            reader.Read();
        }

        [Test]
        [TestCase(0, Register.Dr0)]
        [TestCase(1, Register.Dr1)]
        [TestCase(2, Register.Dr2)]
        [TestCase(3, Register.Dr3)]
        [TestCase(4, Register.Dr4)]
        [TestCase(5, Register.Dr5)]
        [TestCase(6, Register.Dr6)]
        [TestCase(7, Register.Dr7)]
        public void ModRM_ForDebugRegister_DecodesCorrectRegister(byte modrmReg, Register register)
        {
            var reader = ReadBytes32(0x0F, 0x21, (byte)(0xc0 | (modrmReg << 3)), 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Register, reader.Operand2.Type);
            Assert.AreEqual(register, reader.Operand2.GetBaseRegister());
        }
    }
}
