using System;
using System.IO;

using NUnit.Framework;

namespace Fantasm.Disassembler.Tests
{
    [TestFixture]
    class InstructionReaderTests
    {
        private const byte Nop = 0x90;

        [Test]
        public void Read_WithEmptyStream_ReturnsFalse()
        {
            var reader = ReadBytes();

            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_WithSingleInstruction_ReturnsTrueThenFalse()
        {
            var reader = ReadBytes(Nop);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        private static InstructionReader ReadBytes(params byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            return new InstructionReader(stream, ExecutionMode.CompatibilityMode);
        }

        private static InstructionReader ReadBytes16(params byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            return new InstructionReader(stream, ExecutionMode.CompatibilityMode, false);
        }

        private static InstructionReader ReadBytes64(params byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            return new InstructionReader(stream, ExecutionMode.Allow64Bit, true);
        }

        [Test]
        [Ignore] // No implemented instructions support the lock prefix
        public void Read_ReadsGroup1PrefixBeforeInstruction()
        {
            var reader = ReadBytes((byte)InstructionPrefix.Lock, Nop);

            Assert.IsTrue(reader.Read());
            Assert.AreEqual(InstructionPrefix.Lock, reader.Group1Prefix);
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_ReadsGroup2PrefixBeforeInstruction()
        {
            var reader = ReadBytes((byte)InstructionPrefix.SegmentCS, Nop);

            Assert.IsTrue(reader.Read());
            Assert.AreEqual(InstructionPrefix.SegmentCS, reader.Group2Prefix);
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_ReadsGroup3PrefixBeforeInstruction()
        {
            var reader = ReadBytes((byte)InstructionPrefix.OperandSizeOverride, Nop);

            Assert.IsTrue(reader.Read());
            Assert.AreEqual(InstructionPrefix.OperandSizeOverride, reader.Group3Prefix);
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_ReadsGroup4PrefixBeforeInstruction()
        {
            var reader = ReadBytes((byte)InstructionPrefix.AddressSizeOverride, Nop);

            Assert.IsTrue(reader.Read());
            Assert.AreEqual(InstructionPrefix.AddressSizeOverride, reader.Group4Prefix);
            Assert.IsFalse(reader.Read());
        }

        [Test]
        [Ignore] // No implemented instructions support the lock prefix
        public void Read_CanReadTwoPrefixes()
        {
            var reader = ReadBytes((byte)InstructionPrefix.Lock, (byte)InstructionPrefix.SegmentCS, Nop);

            Assert.IsTrue(reader.Read());
            Assert.AreEqual(InstructionPrefix.Lock, reader.Group1Prefix);
            Assert.AreEqual(InstructionPrefix.SegmentCS, reader.Group2Prefix);
            Assert.IsFalse(reader.Read());
        }

        [Test]
        [Ignore] // No implemented instructions support the lock prefix
        public void Read_CanReadThreePrefixes()
        {
            var reader = ReadBytes(
                (byte)InstructionPrefix.Lock,
                (byte)InstructionPrefix.SegmentCS,
                (byte)InstructionPrefix.OperandSizeOverride,
                Nop);

            Assert.IsTrue(reader.Read());
            Assert.AreEqual(InstructionPrefix.Lock, reader.Group1Prefix);
            Assert.AreEqual(InstructionPrefix.SegmentCS, reader.Group2Prefix);
            Assert.AreEqual(InstructionPrefix.OperandSizeOverride, reader.Group3Prefix);
            Assert.IsFalse(reader.Read());
        }

        [Test]
        [Ignore] // No implemented instructions support the lock prefix
        public void Read_CanReadFourPrefixes()
        {
            var reader = ReadBytes(
                (byte)InstructionPrefix.Lock,
                (byte)InstructionPrefix.SegmentCS,
                (byte)InstructionPrefix.OperandSizeOverride,
                (byte)InstructionPrefix.AddressSizeOverride,
                Nop);

            Assert.IsTrue(reader.Read());
            Assert.AreEqual(InstructionPrefix.Lock, reader.Group1Prefix);
            Assert.AreEqual(InstructionPrefix.SegmentCS, reader.Group2Prefix);
            Assert.AreEqual(InstructionPrefix.OperandSizeOverride, reader.Group3Prefix);
            Assert.AreEqual(InstructionPrefix.AddressSizeOverride, reader.Group4Prefix);
            Assert.IsFalse(reader.Read());
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_WithTwoGroup1Prefixes_ThrowsFormatException()
        {
            var reader = ReadBytes((byte)InstructionPrefix.Lock, (byte)InstructionPrefix.Rep, Nop);

            Assert.IsTrue(reader.Read());
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_WithTwoGroup2Prefixes_ThrowsFormatException()
        {
            var reader = ReadBytes((byte)InstructionPrefix.SegmentCS, (byte)InstructionPrefix.SegmentDS, Nop);

            Assert.IsTrue(reader.Read());
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_WithTwoGroup3Prefixes_ThrowsFormatException()
        {
            var reader = ReadBytes(
                (byte)InstructionPrefix.OperandSizeOverride,
                (byte)InstructionPrefix.OperandSizeOverride,
                Nop);

            Assert.IsTrue(reader.Read());
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_WithTwoGroup4Prefixes_ThrowsFormatException()
        {
            var reader = ReadBytes(
                (byte)InstructionPrefix.AddressSizeOverride,
                (byte)InstructionPrefix.AddressSizeOverride,
                Nop);

            Assert.IsTrue(reader.Read());
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_WithPrefixButNoOpcode_ThrowsFormatException()
        {
            var reader = ReadBytes(0x66);
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
            Assert.AreEqual(Register.Rax, reader.GetOperandRegister(0));

            Assert.IsTrue(reader.Read());
            Assert.AreEqual(Register.Eax, reader.GetOperandRegister(0));
            
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_WithRexWAndOperandSizeOverride_IgnoresOperandSizeOverride()
        {
            // ADD RAX 01234567H
            var reader = ReadBytes64(0x66, 0x48, 0x05, 0x67, 0x45, 0x23, 0x01);

            Assert.IsTrue(reader.Read());
            Assert.AreEqual(Register.Rax, reader.GetOperandRegister(0));

            Assert.IsFalse(reader.Read());   
        }

        [Test]
        public void Read_WithRexNoWAndOperandSizeOverride_UsesOperandSizeOverride()
        {
            // ADD AX 0123H
            var reader = ReadBytes64(0x66, 0x40, 0x05, 0x23, 0x01);

            Assert.IsTrue(reader.Read());
            Assert.AreEqual(Register.Ax, reader.GetOperandRegister(0));

            Assert.IsFalse(reader.Read());
        }


        [Test]
        [Ignore] // No implemented instructions support the lock prefix
        public void Read_ClearsPrefixes()
        {
            var reader = ReadBytes(
                (byte)InstructionPrefix.Lock,
                (byte)InstructionPrefix.SegmentCS,
                (byte)InstructionPrefix.OperandSizeOverride,
                (byte)InstructionPrefix.AddressSizeOverride,
                Nop,
                Nop);

            reader.Read();
            reader.Read();

            Assert.AreEqual(InstructionPrefix.None, reader.Group1Prefix);
            Assert.AreEqual(InstructionPrefix.None, reader.Group2Prefix);
            Assert.AreEqual(InstructionPrefix.None, reader.Group3Prefix);
            Assert.AreEqual(InstructionPrefix.None, reader.Group4Prefix);
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
            var reader = ReadBytes(0x05, 0x78, 0x56, 0x34, 0x12);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_In32BitModeWithOperandSizeOverridePrefix_Reads16BitOperands()
        {
            // ADD AX 1234h
            var reader = ReadBytes(0x66, 0x05, 0x34, 0x12);

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
        [ExpectedException(typeof(ArgumentException))]
        public void GetOperandByte_WithNoOperands_ThrowsArgumentException()
        {
            var reader = ReadBytes();
            reader.GetOperandByte(0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetOperandWord_WithNoOperands_ThrowsArgumentException()
        {
            var reader = ReadBytes();
            reader.GetOperandWord(0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetOperandDword_WithNoOperands_ThrowsArgumentException()
        {
            var reader = ReadBytes();
            reader.GetOperandDword(0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetOperandRegister_WithNoOperands_ThrowsArgumentException()
        {
            var reader = ReadBytes();
            reader.GetOperandRegister(0);
        }

        [Test]
        public void GetOperandByte_CanRetrieveFirstOperand()
        {
            // AAD 23H
            var reader = ReadBytes(0xD5, 0x23);
            reader.Read();

            Assert.AreEqual(0x23, reader.GetOperandByte(0));
        }

        [Test]
        public void GetOperandByte_CanRetrieveSecondOperand()
        {
            // ADD AL 23H
            var reader = ReadBytes(0x04, 0x23);
            reader.Read();

            Assert.AreEqual(0x23, reader.GetOperandByte(1));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetOperandByte_ForNonByteArgument_ThrowsInvalidOperationException()
        {
            // ADD AL 23H
            var reader = ReadBytes(0x04, 0x23);
            reader.Read();

            reader.GetOperandByte(0);
        }



        [Test]
        [Ignore] // we don't support any instructions yet with a word first operand
        public void GetOperandWord_CanRetrieveFirstOperand()
        {
        }

        [Test]
        public void GetOperandWord_CanRetrieveSecondOperand()
        {
            // ADD AX 2345H
            var reader = ReadBytes16(0x05, 0x45, 0x23);
            reader.Read();

            Assert.AreEqual(0x2345, reader.GetOperandWord(1));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetOperandWord_ForNonWordArgument_ThrowsInvalidOperationException()
        {
            // ADD AL 23H
            var reader = ReadBytes(0x04, 0x23);
            reader.Read();

            reader.GetOperandWord(0);
        }


        [Test]
        [Ignore] // we don't support any instructions yet with a word first operand
        public void GetOperandDword_CanRetrieveFirstOperand()
        {
        }

        [Test]
        public void GetOperandDword_CanRetrieveSecondOperand()
        {
            // ADD EAX 23456789H
            var reader = ReadBytes(0x05, 0x89, 0x67, 0x45, 0x23);
            reader.Read();

            Assert.AreEqual(0x23456789, reader.GetOperandDword(1));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetOperandDword_ForNonDwordArgument_ThrowsInvalidOperationException()
        {
            // ADD AL 23H
            var reader = ReadBytes(0x04, 0x23);
            reader.Read();

            reader.GetOperandDword(0);
        }


        [Test]
        public void GetOperandRegister_CanRetrieveFirstOperand()
        {
            // ADD AL 23H
            var reader = ReadBytes(0x04, 0x23);
            reader.Read();

            Assert.AreEqual(Register.Al, reader.GetOperandRegister(0));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetOperandRegister_ForNonRegisterArgument_ThrowsInvalidOperationException()
        {
            // AAD 23H
            var reader = ReadBytes(0xD5, 0x23);
            reader.Read();

            reader.GetOperandRegister(0);
        }

        [Test]
        public void Read_ForInstructionWithNoOperands_ResetsOperandCount()
        {
            // AAD 23H
            // NOP
            var reader = ReadBytes(0xD5, 0x23, 0x90);
            reader.Read();
            reader.Read();

            Assert.AreEqual(0, reader.OperandCount);
        }
    }
}
