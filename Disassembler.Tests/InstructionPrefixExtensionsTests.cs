using System.Net;

using NUnit.Framework;

namespace Fantasm.Disassembler.Tests
{
    [TestFixture]
    class InstructionPrefixExtensionsTests
    {
        [Test]
        [TestCase(InstructionPrefix.Lock)]
        [TestCase((byte)InstructionPrefix.RepNE)] // casting to byte to stop multiple aliases confusing the test runner
        [TestCase((byte)InstructionPrefix.Rep)]
        public void IsGroup1_ForGroup1Prefix_ReturnsTrue(InstructionPrefix prefix)
        {
            Assert.IsTrue(prefix.IsGroup1());
        }

        [Test]
        [TestCase(InstructionPrefix.SegmentCS)]
        [TestCase(InstructionPrefix.OperandSizeOverride)]
        [TestCase(InstructionPrefix.AddressSizeOverride)]
        public void IsGroup1_ForNonGroup1Prefix_ReturnsFalse(InstructionPrefix prefix)
        {
            Assert.IsFalse(prefix.IsGroup1());
        }

        [Test]
        [TestCase(InstructionPrefix.SegmentCS)]
        [TestCase(InstructionPrefix.SegmentSS)]
        [TestCase(InstructionPrefix.SegmentDS)]
        [TestCase(InstructionPrefix.SegmentES)]
        [TestCase(InstructionPrefix.SegmentFS)]
        [TestCase(InstructionPrefix.SegmentGS)]
        public void IsGroup2_ForGroup2Prefix_ReturnsTrue(InstructionPrefix prefix)
        {
            Assert.IsTrue(prefix.IsGroup2());
        }

        [Test]
        [TestCase(InstructionPrefix.Lock)]
        [TestCase(InstructionPrefix.OperandSizeOverride)]
        [TestCase(InstructionPrefix.AddressSizeOverride)]
        public void IsGroup2_ForNonGroup2Prefix_ReturnsFalse(InstructionPrefix prefix)
        {
            Assert.IsFalse(prefix.IsGroup2());
        }

        [Test]
        [TestCase(InstructionPrefix.OperandSizeOverride)]
        public void IsGroup3_ForGroup3Prefix_ReturnsTrue(InstructionPrefix prefix)
        {
            Assert.IsTrue(prefix.IsGroup3());
        }

        [Test]
        [TestCase(InstructionPrefix.Lock)]
        [TestCase(InstructionPrefix.SegmentCS)]
        [TestCase(InstructionPrefix.AddressSizeOverride)]
        public void IsGroup3_ForNonGroup3Prefix_ReturnsFalse(InstructionPrefix prefix)
        {
            Assert.IsFalse(prefix.IsGroup3());
        }

        [Test]
        [TestCase(InstructionPrefix.AddressSizeOverride)]
        public void IsGroup4_ForGroup4Prefix_ReturnsTrue(InstructionPrefix prefix)
        {
            Assert.IsTrue(prefix.IsGroup4());
        }

        [Test]
        [TestCase(InstructionPrefix.Lock)]
        [TestCase(InstructionPrefix.SegmentCS)]
        [TestCase(InstructionPrefix.OperandSizeOverride)]
        public void IsGroup4_ForNonGroup4Prefix_ReturnsFalse(InstructionPrefix prefix)
        {
            Assert.IsFalse(prefix.IsGroup4());
        }
    }
}
