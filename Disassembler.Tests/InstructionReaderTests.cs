using System;

using NUnit.Framework;

namespace Fantasm.Disassembler.Tests
{
    [TestFixture]
    class InstructionReaderTests : InstructionReaderTestBase
    {
        private const byte Nop = 0x90;

        [Test]
        public void Read_WithEmptyStream_ReturnsFalse()
        {
            var reader = ReadBytes32();

            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_WithSingleInstruction_ReturnsTrueThenFalse()
        {
            var reader = ReadBytes32(Nop);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_ReadsGroup1PrefixBeforeInstruction()
        {
            var reader = ReadBytes32(0xF3, Nop);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_ReadsGroup2PrefixBeforeInstruction()
        {
            var reader = ReadBytes32(0x2E, Nop);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_ReadsGroup3PrefixBeforeInstruction()
        {
            var reader = ReadBytes32(0x66, Nop);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_ReadsGroup4PrefixBeforeInstruction()
        {
            var reader = ReadBytes32(0x67, Nop);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_CanReadTwoPrefixes()
        {
            var reader = ReadBytes32(0xF3, 0x2E, Nop);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_CanReadThreePrefixes()
        {
            var reader = ReadBytes32(
                0xF3,
                0x2E,
                0x66,
                Nop);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_CanReadFourPrefixes()
        {
            var reader = ReadBytes32(
                0xF3,
                0x2E,
                0x66,
                0x67,
                Nop);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_WithTwoGroup1Prefixes_ThrowsFormatException()
        {
            var reader = ReadBytes32(0xF0, 0xF3, Nop);

            Assert.IsTrue(reader.Read());
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_WithTwoGroup2Prefixes_ThrowsFormatException()
        {
            var reader = ReadBytes32(0x2E, 0x3E, Nop);

            Assert.IsTrue(reader.Read());
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_WithTwoGroup3Prefixes_ThrowsFormatException()
        {
            var reader = ReadBytes32(
                0x66,
                0x66,
                Nop);

            Assert.IsTrue(reader.Read());
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_WithTwoGroup4Prefixes_ThrowsFormatException()
        {
            var reader = ReadBytes32(
                0x67,
                0x67,
                Nop);

            Assert.IsTrue(reader.Read());
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_WithPrefixButNoOpcode_ThrowsFormatException()
        {
            var reader = ReadBytes32(0x66);
            reader.Read();
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_WithPrefixAfterRexPrefix_ThrowsFormatException()
        {
            var reader = ReadBytes64(0x40, 0x66, 0x90);
            reader.Read();
        }

        [Test]
        public void Read_WhenPreviousInstructionUsesRexPrefix_IgnoresPreviousRex()
        {
            // ADD RAX 01234567H
            // ADD AX 01234567H
            var reader = ReadBytes64(0x48, 0x05, 0x67, 0x45, 0x23, 0x01, 0x05, 0x67, 0x45, 0x23, 0x01);
            
            Assert.IsTrue(reader.Read());
            Assert.AreEqual(Register.Rax, reader.Operand1.GetRegister());

            Assert.IsTrue(reader.Read());
            Assert.AreEqual(Register.Eax, reader.Operand1.GetRegister());
            
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_WithRexWAndOperandSizeOverride_IgnoresOperandSizeOverride()
        {
            // ADD RAX 01234567H
            var reader = ReadBytes64(0x66, 0x48, 0x05, 0x67, 0x45, 0x23, 0x01);

            Assert.IsTrue(reader.Read());
            Assert.AreEqual(Register.Rax, reader.Operand1.GetRegister());

            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_WithRexNoWAndOperandSizeOverride_UsesOperandSizeOverride()
        {
            // ADD AX 0123H
            var reader = ReadBytes64(0x66, 0x40, 0x05, 0x23, 0x01);

            Assert.IsTrue(reader.Read());
            Assert.AreEqual(Register.Ax, reader.Operand1.GetRegister());

            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_ClearsPrefixes()
        {
            // ADD AX 1234h
            // ADD EAX 12345678h
            var reader = ReadBytes32(0x66, 0x05, 0x34, 0x12, 0x05, 0x78, 0x56, 0x34, 0x12);

            Assert.IsTrue(reader.Read());
            Assert.AreEqual(Register.Ax, reader.Operand1.GetRegister());

            Assert.IsTrue(reader.Read());
            Assert.AreEqual(Register.Eax, reader.Operand1.GetRegister());

            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_In16BitMode_Reads16BitOperands()
        {
            // ADD AX 1234h
            var reader = ReadBytes16(0x05, 0x34, 0x12);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_In32BitMode_Reads32BitOperands()
        {
            // ADD EAX 12345678h
            var reader = ReadBytes32(0x05, 0x78, 0x56, 0x34, 0x12);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_In32BitModeWithOperandSizeOverridePrefix_Reads16BitOperands()
        {
            // ADD AX 1234h
            var reader = ReadBytes32(0x66, 0x05, 0x34, 0x12);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_In16BitModeWithOperandSizeOverridePrefix_Reads32BitOperands()
        {
            // ADD EAX 12345678h
            var reader = ReadBytes16(0x66, 0x05, 0x78, 0x56, 0x34, 0x12);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_ForInstructionWithNoOperands_ResetsOperands()
        {
            // AAD 23H
            // NOP
            var reader = ReadBytes32(0xD5, 0x23, 0x90);
            reader.Read();
            reader.Read();

            Assert.AreEqual(OperandType.None, reader.Operand1.Type);
            Assert.AreEqual(OperandType.None, reader.Operand2.Type);
        }
    }
}
