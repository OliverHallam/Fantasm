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
            var stream = new MemoryStream(0);
            var reader = new InstructionReader(stream);

            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_WithSingleInstruction_ReturnsTrueThenFalse()
        {
            var stream = new MemoryStream(new[] { Nop });
            var reader = new InstructionReader(stream);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_ReadsGroup1PrefixBeforeInstruction()
        {
            var stream = new MemoryStream(new[] { (byte)InstructionPrefix.Lock, Nop });
            var reader = new InstructionReader(stream);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_ReadsGroup2PrefixBeforeInstruction()
        {
            var stream = new MemoryStream(new[] { (byte)InstructionPrefix.SegmentCS, Nop });
            var reader = new InstructionReader(stream);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_ReadsGroup3PrefixBeforeInstruction()
        {
            var stream = new MemoryStream(new[] { (byte)InstructionPrefix.OperandSizeOverride, Nop });
            var reader = new InstructionReader(stream);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_ReadsGroup4PrefixBeforeInstruction()
        {
            var stream = new MemoryStream(new[] { (byte)InstructionPrefix.AddressSizeOverride, Nop });
            var reader = new InstructionReader(stream);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_CanReadTwoPrefixes()
        {
            var stream = new MemoryStream(
                new[] { (byte)InstructionPrefix.Lock, (byte)InstructionPrefix.SegmentCS, Nop });
            var reader = new InstructionReader(stream);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_CanReadThreePrefixes()
        {
            var stream =
                new MemoryStream(
                    new[]
                    {
                        (byte)InstructionPrefix.Lock, (byte)InstructionPrefix.SegmentCS,
                        (byte)InstructionPrefix.OperandSizeOverride, Nop
                    });
            var reader = new InstructionReader(stream);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_CanReadFourPrefixes()
        {
            var stream =
                new MemoryStream(
                    new[]
                    {
                        (byte)InstructionPrefix.Lock, (byte)InstructionPrefix.SegmentCS,
                        (byte)InstructionPrefix.OperandSizeOverride, (byte)InstructionPrefix.AddressSizeOverride, Nop
                    });
            var reader = new InstructionReader(stream);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_WithTwoGroup1Prefixes_ThrowsFormatException()
        {
            var stream =
                new MemoryStream(
                    new[]
                    {
                        (byte)InstructionPrefix.Lock, (byte)InstructionPrefix.Rep, Nop
                    });
            var reader = new InstructionReader(stream);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_WithTwoGroup2Prefixes_ThrowsFormatException()
        {
            var stream =
                new MemoryStream(
                    new[]
                    {
                        (byte)InstructionPrefix.SegmentCS, (byte)InstructionPrefix.SegmentDS, Nop
                    });
            var reader = new InstructionReader(stream);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_WithTwoGroup3Prefixes_ThrowsFormatException()
        {
            var stream =
                new MemoryStream(
                    new[]
                    {
                        (byte)InstructionPrefix.OperandSizeOverride, (byte)InstructionPrefix.OperandSizeOverride, Nop
                    });
            var reader = new InstructionReader(stream);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_WithTwoGroup4Prefixes_ThrowsFormatException()
        {
            var stream =
                new MemoryStream(
                    new[]
                    {
                        (byte)InstructionPrefix.AddressSizeOverride, (byte)InstructionPrefix.AddressSizeOverride, Nop
                    });
            var reader = new InstructionReader(stream);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }
    }
}
