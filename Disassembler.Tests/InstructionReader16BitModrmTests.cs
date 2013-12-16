using NUnit.Framework;

namespace Fantasm.Disassembler.Tests
{
    [TestFixture]
    class InstructionReader16BitModrmTests : InstructionReaderTestBase
    {
        [Test]
        public void ModRM_WithNoAddressSizeOverride_Uses16BitAddresses()
        {
            // ADD [0x12345678] 0
            var reader = ReadBytes16(0x80, 0x06, 0x34, 0x12, 0x00);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void ModRM_WithAddressSizeOverride_Uses32BitAddresses()
        {
            // ADD [0x1234] 0
            var reader = ReadBytes16(0x67, 0x80, 0x05, 0x78, 0x56, 0x34, 0x12, 0x00);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }
        [Test]
        public void ModRM_ForInlineAddress_DecodesCorrectAddress()
        {
            // ADD [0x1234] 0
            var reader = ReadBytes16(0x80, 0x06, 0x34, 0x12, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(Register.None, reader.GetBaseRegister());
            Assert.AreEqual(Register.None, reader.GetIndexRegister());
            Assert.AreEqual(1, reader.GetScale());
            Assert.AreEqual(0x1234, reader.GetDisplacement());
        }

        [Test]
        [TestCase(0, Register.Bx, Register.Si)]
        [TestCase(1, Register.Bx, Register.Di)]
        [TestCase(2, Register.Bp, Register.Si)]
        [TestCase(3, Register.Bp, Register.Di)]
        [TestCase(4, Register.Si, Register.None)]
        [TestCase(5, Register.Di, Register.None)]
        [TestCase(7, Register.Bx, Register.None)]
        public void ModRM_ForAddressWithNoDisplacement_DecodesCorrectRegisters(byte rm, Register baseRegister, Register indexRegister)
        {
            // ADD [Base + Index] 0
            var reader = ReadBytes16(0x80, rm, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(baseRegister, reader.GetBaseRegister());
            Assert.AreEqual(indexRegister, reader.GetIndexRegister());
            Assert.AreEqual(1, reader.GetScale());
            Assert.AreEqual(0, reader.GetDisplacement());
        }
        
        [Test]
        [TestCase(0, Register.Bx, Register.Si)]
        [TestCase(1, Register.Bx, Register.Di)]
        [TestCase(2, Register.Bp, Register.Si)]
        [TestCase(3, Register.Bp, Register.Di)]
        [TestCase(4, Register.Si, Register.None)]
        [TestCase(5, Register.Di, Register.None)]
        [TestCase(6, Register.Bp, Register.None)]
        [TestCase(7, Register.Bx, Register.None)]
        public void ModRM_ForAddressWithByteDisplacement_DecodesCorrectRegisters(
            byte rm,
            Register baseRegister,
            Register indexRegister)
        {
            // ADD [Base + Index + 0x23] 0
            var reader = ReadBytes16(0x80, (byte)(0x40 | rm), 0x23, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(baseRegister, reader.GetBaseRegister());
            Assert.AreEqual(indexRegister, reader.GetIndexRegister());
            Assert.AreEqual(1, reader.GetScale());
            Assert.AreEqual(0x23, reader.GetDisplacement());
        }

        [Test]
        [TestCase(0, Register.Bx, Register.Si)]
        [TestCase(1, Register.Bx, Register.Di)]
        [TestCase(2, Register.Bp, Register.Si)]
        [TestCase(3, Register.Bp, Register.Di)]
        [TestCase(4, Register.Si, Register.None)]
        [TestCase(5, Register.Di, Register.None)]
        [TestCase(6, Register.Bp, Register.None)]
        [TestCase(7, Register.Bx, Register.None)]
        public void ModRM_ForAddressWithWordDisplacement_DecodesCorrectRegisters(
            byte rm,
            Register baseRegister,
            Register indexRegister)
        {
            // ADD [Base + Index + 0x0123] 0
            var reader = ReadBytes16(0x80, (byte)(0x80 | rm), 0x23, 0x01, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(baseRegister, reader.GetBaseRegister());
            Assert.AreEqual(indexRegister, reader.GetIndexRegister());
            Assert.AreEqual(1, reader.GetScale());
            Assert.AreEqual(0x0123, reader.GetDisplacement());
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
        public void ModRM_ForDirect8BitRegister_DecodesCorrectRegister(byte modrmReg, Register register)
        {
            // ADD [REG] 0
            var reader = ReadBytes16(0x80, (byte)(0xc0 | modrmReg), 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.DirectRegister, reader.GetOperandType(0));
            Assert.AreEqual(register, reader.GetBaseRegister());
        }
    }
}
