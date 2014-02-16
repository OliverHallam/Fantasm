using System;
using System.IO;

namespace Fantasm.Disassembler
{
    public class InstructionByteStream
    {
        #region Fields

        private readonly byte[] buffer = new byte[4];
        private readonly Stream stream;

        #endregion

        #region Constructors and Destructors

        public InstructionByteStream(Stream stream)
        {
            this.stream = stream;
        }

        #endregion

        #region Public Methods and Operators

        public byte ReadByte()
        {
            var nextByte = this.stream.ReadByte();
            if (nextByte < 0)
            {
                throw InvalidInstructionBytes();
            }
            return (byte)nextByte;
        }

        public int ReadDword()
        {
            var bytesRead = this.stream.Read(this.buffer, 0, 4);
            if (bytesRead < 4)
            {
                throw InvalidInstructionBytes();
            }

            return BitConverter.ToInt32(this.buffer, 0);
        }

        public short ReadWord()
        {
            var bytesRead = this.stream.Read(this.buffer, 0, 2);
            if (bytesRead < 2)
            {
                throw InvalidInstructionBytes();
            }

            return BitConverter.ToInt16(this.buffer, 0);
        }

        public bool TryReadByte(out byte value)
        {
            var nextByte = this.stream.ReadByte();
            if (nextByte < 0)
            {
                value = 0;
                return false;
            }
            value = (byte)nextByte;
            return true;
        }

        #endregion

        #region Methods

        private static Exception InvalidInstructionBytes()
        {
            return new FormatException("The data does not represent a valid instruction.");
        }

        #endregion
    }
}