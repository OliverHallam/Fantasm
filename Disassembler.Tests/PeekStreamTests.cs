using System.IO;

using NUnit.Framework;

namespace Fantasm.Disassembler.Tests
{
    [TestFixture]
    class PeekStreamTests
    {
        [Test]
        public void Peek_WithNoMoreDataInStream_ReturnsMinusOne()
        {
            var stream = new MemoryStream(new byte[] { });
            var peek = new PeekStream(stream);

            Assert.AreEqual(-1, peek.Peek());
        }

        [Test]
        public void Peek_WithMoreDataInStream_ReturnsNextByteInStream()
        {
            var stream = new MemoryStream(new byte[] { 123 });
            var peek = new PeekStream(stream);

            Assert.AreEqual(123, peek.Peek());
        }

        [Test]
        public void Peek_WhenCalledMultipleTimesWithMoreDataInStream_ReturnsSameValue()
        {
            var stream = new MemoryStream(new byte[] { 123 });
            var peek = new PeekStream(stream);

            Assert.AreEqual(123, peek.Peek());
            Assert.AreEqual(123, peek.Peek());
        }

        [Test]
        public void Peek_WhenCalledMultipleTimesWithNoMoreDataInStream_ReturnsMinusOne()
        {
            var stream = new MemoryStream(new byte[] { 123 });
            var peek = new PeekStream(stream);

            Assert.AreEqual(123, peek.Peek());
            Assert.AreEqual(123, peek.Peek());
        }

        [Test]
        public void Read_WithNoPeekedValue_ReadsValueFromStream()
        {
            var stream = new MemoryStream(new byte[] { 123 });
            var peek = new PeekStream(stream);

            Assert.AreEqual(123, peek.Read());
        }

        [Test]
        public void Read_WithPeekedValue_ReadsPeekedValue()
        {
            var stream = new MemoryStream(new byte[] { 123 });
            var peek = new PeekStream(stream);

            peek.Peek();

            Assert.AreEqual(123, peek.Read());
        }

        [Test]
        public void Peek_AfterConsumingPeekedValue_ReturnsNextByte()
        {
            var stream = new MemoryStream(new byte[] { 123, 45 });
            var peek = new PeekStream(stream);

            peek.Peek();
            peek.Consume();

            Assert.AreEqual(45, peek.Peek());
        }

        [Test]
        public void Peek_AfterConsumingUnpeekedValue_ReturnsNextByte()
        {
            var stream = new MemoryStream(new byte[] { 123, 45 });
            var peek = new PeekStream(stream);

            peek.Consume();

            Assert.AreEqual(45, peek.Peek());
        }


        [Test]
        public void Read_WithPeekedValue_ConsumesPeekedValue()
        {
            var stream = new MemoryStream(new byte[] { 123 });
            var peek = new PeekStream(stream);

            peek.Peek();
            peek.Read();

            Assert.AreEqual(-1, peek.Peek());
        }
    }
}
