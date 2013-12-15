using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;

using NUnit.Framework;

namespace Fantasm.Disassembler.Tests
{
    [TestFixture(Description = "Tests of the InstructionReader class for specific OpCodes")]
    public class InstructionReaderOpCodesTests
    {
        public enum OperandFormat
        {
            /// <summary>
            /// The instruction takes no operands.
            /// </summary>
            None,

            /// <summary>
            /// The operand is a single byte.
            /// </summary>
            Ib,

            /// <summary>
            /// The first operand is the <c>AL</c> register and the second operand is an immediate byte.
            /// </summary>
            AL_Ib,

            /// <summary>
            /// The first operand is the <c>AX</c> register and the second operand is an immediate word.
            /// </summary>
            AX_Iw,

            /// <summary>
            /// The first operand is the <c>EAX</c> register and the second operand is an immediate dword.
            /// </summary>
            EAX_Id,

            /// <summary>
            /// The first operand is the <c>RAX</c> register and the second operand is an immediate dword.
            /// </summary>
            RAX_Id,
            
            /// <summary>
            /// The first operand is a memory address or register specified by a ModRM byte, and the second operand
            /// is an immediate byte
            /// </summary>
            Eb_Ib
        }

        public enum OperandSize
        {
            Size16,
            Size32,
        }

        public class OpCodeProperties
        {
            public OperandSize OperandSize;
            internal RexPrefix RexPrefix;
            public byte OpCode;
            public byte OpCodeReg;
            public Instruction Mnemonic;
            public OperandFormat Operands;
            internal InstructionPrefixes SupportedPrefixes;
            internal ExecutionModes SupportedModes;

            internal OpCodeProperties(
                RexPrefix rex,
                OperandSize operandSize,
                byte opCode,
                byte opCodeReg,
                Instruction mnemonic,
                OperandFormat operands,
                InstructionPrefixes supportedPrefixes,
                ExecutionModes supportedModes)
            {
                this.RexPrefix = rex;
                this.OperandSize = operandSize;
                this.OpCode = opCode;
                this.OpCodeReg = opCodeReg;
                this.Mnemonic = mnemonic;
                this.Operands = operands;
                this.SupportedPrefixes = supportedPrefixes;
                this.SupportedModes = supportedModes;
            }

            internal OpCodeProperties(
                byte opCode,
                Instruction mnemonic,
                OperandFormat operands,
                ExecutionModes supportedModes)
                : this(0, OperandSize.Size32, opCode, 255, mnemonic, operands, InstructionPrefixes.None, supportedModes)
            {
            }

            internal OpCodeProperties(byte opCode, Instruction mnemonic, OperandFormat operands)
                : this(
                    0,
                    OperandSize.Size32,
                    opCode,
                    255,
                    mnemonic,
                    operands,
                    InstructionPrefixes.None,
                    ExecutionModes.All)
            {
            }

            internal OpCodeProperties(
                byte opCode,
                byte opCodeMod,
                Instruction mnemonic,
                OperandFormat operands,
                InstructionPrefixes supportedPrefixes)
                : this(
                    0,
                    OperandSize.Size32,
                    opCode,
                    opCodeMod,
                    mnemonic,
                    operands,
                    supportedPrefixes,
                    ExecutionModes.All)
            {
            }

            internal OpCodeProperties(
                RexPrefix rex,
                byte opCode,
                byte opCodeMod,
                Instruction mnemonic,
                OperandFormat operands,
                InstructionPrefixes supportedPrefixes)
                : this(
                    rex,
                    OperandSize.Size32,
                    opCode,
                    opCodeMod,
                    mnemonic,
                    operands,
                    supportedPrefixes,
                    ExecutionModes.All)
            {
            }

            internal OpCodeProperties(
                byte opCode,
                Instruction mnemonic,
                OperandSize operandSize,
                OperandFormat operands)
                : this(0, operandSize, opCode, 255, mnemonic, operands, InstructionPrefixes.None, ExecutionModes.All)
            {
            }

            internal OpCodeProperties(RexPrefix rex, byte opCode, Instruction mnemonic, OperandFormat operands)
                : this(
                    RexPrefix.Magic | rex,
                    OperandSize.Size32,
                    opCode,
                    255,
                    mnemonic,
                    operands,
                    InstructionPrefixes.None,
                    ExecutionModes.Long64Bit)
            {
            }

            public override string ToString()
            {
                if (this.OpCodeReg != 255)
                    return string.Format("{0} ({1:X2}/{2})", this.Mnemonic, this.OpCode, this.OpCodeReg);
                else
                    return string.Format("{0} ({1:X2})", this.Mnemonic, this.OpCode);
            }
        }

        public static OpCodeProperties[] OpCodes =
        {
            new OpCodeProperties(0x04, Instruction.Add, OperandFormat.AL_Ib),
            new OpCodeProperties(0x05, Instruction.Add, OperandSize.Size16, OperandFormat.AX_Iw),
            new OpCodeProperties(0x05, Instruction.Add, OperandSize.Size32, OperandFormat.EAX_Id),
            new OpCodeProperties(RexPrefix.W, 0x05, Instruction.Add, OperandFormat.RAX_Id), 
            new OpCodeProperties(0x80, 0, Instruction.Add, OperandFormat.Eb_Ib, InstructionPrefixes.Lock),
            new OpCodeProperties(RexPrefix.W, 0x80, 0, Instruction.Add, OperandFormat.Eb_Ib, InstructionPrefixes.Lock),
            new OpCodeProperties(0x37, Instruction.Aaa, OperandFormat.None, ExecutionModes.CompatibilityMode),
            new OpCodeProperties(0x3F, Instruction.Aas, OperandFormat.None, ExecutionModes.CompatibilityMode),
            new OpCodeProperties(0xD4, Instruction.Aam, OperandFormat.Ib, ExecutionModes.CompatibilityMode),
            new OpCodeProperties(0xD5, Instruction.Aad, OperandFormat.Ib, ExecutionModes.CompatibilityMode),
        };

        [Test]
        [TestCaseSource("OpCodes")]
        public void InstructionReader_WithCorrectOperands_SuccessfullyDecodesInstruction(OpCodeProperties opCode)
        {
            var bytes = GetBytes(opCode);
            var mode = GetExecutionMode(opCode);
            var reader = new InstructionReader(
                new MemoryStream(bytes),
                mode,
                opCode.OperandSize == OperandSize.Size32);

            reader.Read();

            Assert.AreEqual(opCode.Mnemonic, reader.Instruction);

            // check operands
            switch (opCode.Operands)
            {
                case OperandFormat.Ib:
                    Assert.AreEqual(1, reader.OperandCount);
                    Assert.AreEqual(0x11, reader.GetOperandByte(0));
                    break;

                case OperandFormat.AL_Ib:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(Register.Al, reader.GetOperandRegister(0));
                    Assert.AreEqual(0x22, reader.GetOperandByte(1));
                    break;

                case OperandFormat.AX_Iw:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(Register.Ax, reader.GetOperandRegister(0));
                    Assert.AreEqual(0x2222, reader.GetOperandWord(1));
                    break;

                case OperandFormat.EAX_Id:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(Register.Eax, reader.GetOperandRegister(0));
                    Assert.AreEqual(0x22222222, reader.GetOperandDword(1));
                    break;

                case OperandFormat.RAX_Id:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(Register.Rax, reader.GetOperandRegister(0));
                    Assert.AreEqual(0x22222222, reader.GetOperandDword(1));
                    break;

                case OperandFormat.Eb_Ib:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(OperandType.Register, reader.GetOperandType(0));
                    Assert.AreEqual(opCode.RexPrefix != 0 ? Register.Spl : Register.Ah, reader.GetOperandRegister(0));
                    Assert.AreEqual(OperandType.ImmediateByte, reader.GetOperandType(1));
                    Assert.AreEqual(0x22, reader.GetOperandByte(1));
                    break;
            }

            Assert.IsFalse(reader.Read());
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
            
            var mode = GetExecutionMode(opCode);
            var reader = new InstructionReader(new MemoryStream(bytes), mode);

            reader.Read();
        }

        public static IEnumerable<OpCodeProperties> OpCodesWithLockSupported()
        {
            return OpCodes.Where(o => o.SupportedPrefixes.HasFlag(InstructionPrefixes.Lock));
        }

        [Test]
        [TestCaseSource("OpCodesWithLockSupported")]
        [ExpectedException(typeof(FormatException))]
        public void InstructionReader_ForLockPrefixWithNoMemoryAccess_ThrowsFormatException(OpCodeProperties opCode)
        {
            var byteList = new List<byte> { 0xF0 };
            byteList.AddRange(this.GetBytes(opCode, 0xC0)); // EAX
            var bytes = byteList.ToArray();

            var mode = GetExecutionMode(opCode);
            var reader = new InstructionReader(new MemoryStream(bytes), mode);

            reader.Read();
        }

        [Test]
        [TestCaseSource("OpCodesWithLockSupported")]
        public void InstructionReader_ForLockPrefixWithMemoryAccess_DoesNotThrow(OpCodeProperties opCode)
        {
            var byteList = new List<byte> { 0xF0 };
            byteList.AddRange(this.GetBytes(opCode, 0x00)); // [EAX]
            var bytes = byteList.ToArray();

            var mode = GetExecutionMode(opCode);
            var reader = new InstructionReader(new MemoryStream(bytes), mode);

            reader.Read();
        }

        private static ExecutionMode GetExecutionMode(OpCodeProperties opCode)
        {
            return opCode.SupportedModes == ExecutionModes.CompatibilityMode
                || opCode.OperandSize == OperandSize.Size16
                ? ExecutionMode.CompatibilityMode
                : ExecutionMode.Long64Bit;
        }

        public IEnumerable<OpCodeProperties> CompatibilityModeInstructions()
        {
            return OpCodes.Where(o => o.SupportedModes == ExecutionModes.CompatibilityMode);
        }
            
        [Test]
        [TestCaseSource("CompatibilityModeInstructions")]
        [ExpectedException(typeof(FormatException))]
        public void InstructionReader_For64BitMode_ThrowsFormatException(OpCodeProperties opCode)
        {
            var bytes = this.GetBytes(opCode);
            var reader = new InstructionReader(new MemoryStream(bytes), ExecutionMode.Long64Bit);

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
            return GetBytes(opCode, 0xc4);
        }

        private byte[] GetBytes(OpCodeProperties opCode, byte modrm)
        {
            var bytes = new List<byte>();

            if (opCode.RexPrefix != 0)
            {
                bytes.Add((byte)(RexPrefix.Magic | opCode.RexPrefix));
            }

            bytes.Add(opCode.OpCode);

            switch (opCode.Operands)
            {
                case OperandFormat.Ib:
                    bytes.Add(0x11);
                    break;

                case OperandFormat.AL_Ib:
                    bytes.Add(0x22);
                    break;

                case OperandFormat.AX_Iw:
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    break;

                case OperandFormat.RAX_Id:
                case OperandFormat.EAX_Id:
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    break;

                case OperandFormat.Eb_Ib:
                    bytes.Add((byte)(modrm | opCode.OpCodeReg));
                    bytes.Add(0x22);
                    break;
            }

            return bytes.ToArray();
        }
    }
}
