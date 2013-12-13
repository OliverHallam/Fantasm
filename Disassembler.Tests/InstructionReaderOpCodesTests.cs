using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using NUnit.Framework;

namespace Fantasm.Disassembler.Tests
{
    [TestFixture(Description = "Tests of the InstructionReader class for specific OpCodes")]
    public class InstructionReaderOpCodesTests
    {
        public class OpCodeProperties
        {
            public byte OpCode;
            public Instruction Mnemonic;
            public InstructionPrefixes SupportedPrefixes;
            public OperandType[] Operands;

            internal OpCodeProperties(
                byte opCode,
                Instruction mnemonic,
                InstructionPrefixes supportedPrefixes,
                params OperandType[] operandTypes)

            {
                this.OpCode = opCode;
                this.Mnemonic = mnemonic;
                this.SupportedPrefixes = supportedPrefixes;
                this.Operands = operandTypes;
            }

            public override string ToString()
            {
                return string.Format("{0} ({1:X2})", this.Mnemonic, this.OpCode);
            }
        }

        public static OpCodeProperties[] OpCodes =
        {
            new OpCodeProperties(0x37, Instruction.Aaa, InstructionPrefixes.None),
            new OpCodeProperties(0xD4, Instruction.Aam, InstructionPrefixes.None, OperandType.ImmediateByte),
            new OpCodeProperties(0xD5, Instruction.Aad, InstructionPrefixes.None, OperandType.ImmediateByte),
        };
        
        [Test]
        [TestCaseSource("OpCodes")]
        public void InstructionReader_WithCorrectOperands_SuccessfullyDecodesInstruction(OpCodeProperties opCode)
        {
            var bytes = GetBytes(opCode);
            var reader = new InstructionReader(new MemoryStream(bytes), ExecutionMode.CompatibilityMode);

            reader.Read();

            Assert.AreEqual(opCode.Mnemonic, reader.Instruction);

            // check operands
            for (var index = 0; index < opCode.Operands.Length; index++)
            {
                switch (opCode.Operands[index])
                {
                    case OperandType.ImmediateByte:
                        Assert.AreEqual(this.GetImmediateByte(index), reader.GetOperandByte(index));
                        break;
                }
            }
        }

        public static IEnumerable<OpCodeProperties> OpCodesWithLockUnsupported()
        {
            return OpCodes.Where(o => !o.SupportedPrefixes.HasFlag(InstructionPrefixes.Lock));
        }

        [Test]
        [TestCaseSource("OpCodesWithLockUnsupported")]
        [ExpectedException(typeof(FormatException))]
        public void InstructionReader_ForLockPrefix_ThrowsFormatException(OpCodeProperties opCode)
        {
            var byteList = new List<byte> { 0xF0 };
            byteList.AddRange(this.GetBytes(opCode));
            var bytes = byteList.ToArray();

            var reader = new InstructionReader(new MemoryStream(bytes), ExecutionMode.CompatibilityMode);

            reader.Read();
        }

        [Test]
        [TestCaseSource("OpCodes")]
        [ExpectedException(typeof(FormatException))]
        public void InstructionReader_For64BitMode_ThrowsFormatException(OpCodeProperties opCode)
        {
            var bytes = this.GetBytes(opCode);
            var reader = new InstructionReader(new MemoryStream(bytes), ExecutionMode.Allow64Bit);

            reader.Read();
        }

        [Test]
        public void InstructionReader_ForAadWithBase10_HidesOperand()
        {
            // This is a special case - conventionally AAD 0AH is represented as AAD
            var bytes = new byte[] { 0xD5, 0x0A };
            var reader = new InstructionReader(new MemoryStream(bytes), ExecutionMode.CompatibilityMode);

            reader.Read();

            Assert.AreEqual(0, reader.OperandCount);
        }

        [Test]
        public void InstructionReader_ForAamWithBase10_HidesOperand()
        {
            // This is a special case - conventionally AAM 0AH is represented as AAM
            var bytes = new byte[] { 0xD4, 0x0A };
            var reader = new InstructionReader(new MemoryStream(bytes), ExecutionMode.CompatibilityMode);

            reader.Read();

            Assert.AreEqual(0, reader.OperandCount);
        }


        private byte[] GetBytes(OpCodeProperties opCode)
        {
            var bytes = new List<byte> { opCode.OpCode };

            for (var index = 0; index < opCode.Operands.Length; index++)
            {
                switch (opCode.Operands[index])
                {
                    case OperandType.ImmediateByte:
                        bytes.Add(this.GetImmediateByte(index));
                        break;
                }
            }

            return bytes.ToArray();
        }

        private byte GetImmediateByte(int index)
        {
            return (byte)((index + 1) * 0x11);
        }
    }
}
