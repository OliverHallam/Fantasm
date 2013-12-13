using System;
using System.IO;

using Microsoft.SqlServer.Server;

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
        [ExpectedException(typeof(ArgumentException))]
        public void GetOperandByte_WithNoOperands_ThrowsArgumentException()
        {
            var reader = ReadBytes();
            reader.GetOperandByte(0);
        }

        [Test]
        public void Read_CanReadImmediateByteArgument()
        {
            // AAD 23H
            var reader = ReadBytes(0xD5, 0x23);
            reader.Read();

            Assert.AreEqual(1, reader.OperandCount);
            Assert.AreEqual(0x23, reader.GetOperandByte(0));
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
