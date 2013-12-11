using System;
using System.IO;

namespace Fantasm.Disassembler
{
    /// <summary>
    /// A class used to read a stream of X86 assembly instructions.
    /// </summary>
    public class InstructionReader
    {
        private readonly PeekStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstructionReader"/> class.
        /// </summary>
        /// <param name="stream">The stream to read the instruction data from.</param>
        public InstructionReader(Stream stream)
        {
            this.stream = new PeekStream(stream);
        }

        /// <summary>
        /// Attempts to read the next instruction from the stream.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if there are more isntructions; <see langword="false" /> if the reader has reached
        /// the end of the stream.
        /// </returns>
        public bool Read()
        {
            return ReadPrefixes() && ReadOpCode();
        }

        private bool ReadPrefixes()
        {
            InstructionPrefix group1 = 0, group2 = 0, group3 = 0, group4 = 0;

            var nextByte = this.stream.Peek();
            if (nextByte < 0)
            {
                return false;
            }

            while (true)
            {
                var prefix = (InstructionPrefix)(byte)nextByte;
                if (prefix.IsGroup1())
                {
                    this.SetPrefix(ref group1, prefix);
                }
                else if (prefix.IsGroup2())
                {
                    this.SetPrefix(ref group2, prefix);
                }
                else if (prefix.IsGroup3())
                {
                    this.SetPrefix(ref group3, prefix);
                }
                else if (prefix.IsGroup4())
                {
                    this.SetPrefix(ref group4, prefix);
                }
                else
                {
                    return true;
                }

                nextByte = this.stream.Peek();
                if (nextByte < 0)
                {
                    throw InvalidInstructionBytes();
                }
            }
        }

        private void SetPrefix(ref InstructionPrefix group, InstructionPrefix prefix)
        {
            if (group != 0)
            {
                throw UndefinedBehaviour();
            }

            group = prefix;
            this.stream.Consume();
        }

        private bool ReadOpCode()
        {
            return this.stream.Read() > 0;
        }

        private static Exception InvalidInstructionBytes()
        {
            return new FormatException("The data is not valid assembly code.");
        }

        private static Exception UndefinedBehaviour()
        {
            return new FormatException("The data is invalid assembly code that would lead to undefined behaviour.");
        }
    }
}
