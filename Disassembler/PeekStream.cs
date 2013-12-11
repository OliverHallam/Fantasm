using System;
using System.IO;

namespace Fantasm.Disassembler
{
    /// <summary>
    /// A stream which reads bytes one at a time with a one-byte lookahead.
    /// </summary>
    internal class PeekStream
    {
        private readonly Stream stream;
        private int peekByte = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeekStream"/> class.
        /// </summary>
        /// <param name="stream">The underlying stream.</param>
        public PeekStream(Stream stream)
        {
            this.stream = stream;
        }

        /// <summary>
        /// Reads a byte from the stream and advances the position within the stream by one byte; or returns -1 if at
        /// the end of the stream.
        /// </summary>
        /// <returns>
        /// The byte cast to an <see cref="int"/> or -1 if at the end of the stream.
        /// </returns>
        public int Read()
        {
            if (this.peekByte >= 0)
            {
                var value = this.peekByte;
                this.peekByte = -1;
                return value;
            }

            return this.stream.ReadByte();
        }

        /// <summary>
        /// Reads a byte from the stream without advancing the position within the stream; or returns -1 if at
        /// the end of the stream.
        /// </summary>
        /// <returns>
        /// The byte cast to an <see cref="int"/> or -1 if at the end of the stream.
        /// </returns>
        public int Peek()
        {
            if (this.peekByte >= 0)
            {
                return this.peekByte;
            }

            return this.peekByte = this.stream.ReadByte();
        }

        /// <summary>
        /// Advances the position within the stream by one byte
        /// </summary>
        /// <exception cref="InvalidOperationException">The stream is at the end.</exception>
        public void Consume()
        {
            if (this.peekByte >= 0)
            {
                this.peekByte = -1;
                return;
            }

            if (this.stream.ReadByte() < 0)
            {
                throw new InvalidOperationException();
            }
        }
    }
}