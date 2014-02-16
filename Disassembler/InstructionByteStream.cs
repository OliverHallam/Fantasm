using System;
using System.IO;

namespace Fantasm.Disassembler
{
    public class InstructionByteStream
    {
        private readonly Stream stream;
        private readonly byte[] buffer = new byte[4];

        public InstructionByteStream(Stream stream)
        {
            this.stream = stream;
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

        public byte ReadByte()
        {
            var nextByte = this.stream.ReadByte();
            if (nextByte < 0)
            {
                throw InvalidInstructionBytes();
            }
            return (byte)nextByte;
        }

        public short ReadWord()
        {
            var bytesRead = this.stream.Read(buffer, 0, 2);
            if (bytesRead < 2)
            {
                throw InvalidInstructionBytes();
            }

            return BitConverter.ToInt16(buffer, 0);
        }

        public int ReadDword()
        {
            var bytesRead = this.stream.Read(buffer, 0, 4);
            if (bytesRead < 4)
            {
                throw InvalidInstructionBytes();
            }

            return BitConverter.ToInt32(buffer, 0);
        }

        private static Exception InvalidInstructionBytes()
        {
            return new FormatException("The data does not represent a valid instruction.");
        }
    }
}