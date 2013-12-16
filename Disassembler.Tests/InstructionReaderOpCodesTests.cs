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
        public static OpCodeProperties[] OpCodes =
        {
            new OpCodeProperties(0x37, Instruction.Aaa, OperandFormat.None, Compatibility64.Invalid),
            new OpCodeProperties(0xD5, Instruction.Aad, OperandFormat.Ib, Compatibility64.Invalid),
            new OpCodeProperties(0xD4, Instruction.Aam, OperandFormat.Ib, Compatibility64.Invalid),
            new OpCodeProperties(0x3F, Instruction.Aas, OperandFormat.None, Compatibility64.Invalid),

            new OpCodeProperties(0x14, Instruction.Adc, OperandFormat.AL_Ib),
            new OpCodeProperties(0x15, Instruction.Adc, OperandSize.Size16, OperandFormat.AX_Iw),
            new OpCodeProperties(0x15, Instruction.Adc, OperandSize.Size32, OperandFormat.EAX_Id),
            new OpCodeProperties(RexPrefix.W, 0x15, Instruction.Adc, OperandFormat.RAX_Id), 
            new OpCodeProperties(0x80, 2, Instruction.Adc, OperandFormat.Eb_Ib, InstructionPrefixes.Lock),
            new OpCodeProperties(RexPrefix.W, 0x80, 2, Instruction.Adc, OperandFormat.Eb_Ib, InstructionPrefixes.Lock),
            new OpCodeProperties(0x81, 2, Instruction.Adc, OperandSize.Size16, OperandFormat.Ew_Iw, InstructionPrefixes.Lock),
            new OpCodeProperties(0x81, 2, Instruction.Adc, OperandSize.Size32, OperandFormat.Ed_Id, InstructionPrefixes.Lock),
            new OpCodeProperties(RexPrefix.W, 0x81, 2, Instruction.Adc, OperandFormat.Eq_Id, InstructionPrefixes.Lock),
            new OpCodeProperties(0x83, 2, Instruction.Adc, OperandSize.Size16, OperandFormat.Ew_Ib, InstructionPrefixes.Lock),
            new OpCodeProperties(0x83, 2, Instruction.Adc, OperandSize.Size32, OperandFormat.Ed_Ib, InstructionPrefixes.Lock),
            new OpCodeProperties(RexPrefix.W, 0x83, 2, Instruction.Adc, OperandFormat.Eq_Ib, InstructionPrefixes.Lock),
            new OpCodeProperties(0x10, Instruction.Adc, OperandFormat.Eb_Gb, InstructionPrefixes.Lock),
            new OpCodeProperties(RexPrefix.W, 0x10, Instruction.Adc, OperandFormat.Eb_Gb, InstructionPrefixes.Lock),
            new OpCodeProperties(0x11, Instruction.Adc, OperandSize.Size16, OperandFormat.Ew_Gw, InstructionPrefixes.Lock),
            new OpCodeProperties(0x11, Instruction.Adc, OperandSize.Size32, OperandFormat.Ed_Gd, InstructionPrefixes.Lock),
            new OpCodeProperties(RexPrefix.W, 0x11, Instruction.Adc, OperandFormat.Eq_Gq, InstructionPrefixes.Lock),
            new OpCodeProperties(0x12, Instruction.Adc, OperandFormat.Gb_Eb),
            new OpCodeProperties(RexPrefix.W, 0x12, Instruction.Adc, OperandFormat.Gb_Eb),
            new OpCodeProperties(0x13, Instruction.Adc, OperandSize.Size16, OperandFormat.Gw_Ew),
            new OpCodeProperties(0x13, Instruction.Adc, OperandSize.Size32, OperandFormat.Gd_Ed),
            new OpCodeProperties(RexPrefix.W, 0x13, Instruction.Adc, OperandFormat.Gq_Eq),

            new OpCodeProperties(0x04, Instruction.Add, OperandFormat.AL_Ib),
            new OpCodeProperties(0x05, Instruction.Add, OperandSize.Size16, OperandFormat.AX_Iw),
            new OpCodeProperties(0x05, Instruction.Add, OperandSize.Size32, OperandFormat.EAX_Id),
            new OpCodeProperties(RexPrefix.W, 0x05, Instruction.Add, OperandFormat.RAX_Id), 
            new OpCodeProperties(0x80, 0, Instruction.Add, OperandFormat.Eb_Ib, InstructionPrefixes.Lock),
            new OpCodeProperties(RexPrefix.W, 0x80, 0, Instruction.Add, OperandFormat.Eb_Ib, InstructionPrefixes.Lock),
            new OpCodeProperties(0x81, 0, Instruction.Add, OperandSize.Size16, OperandFormat.Ew_Iw, InstructionPrefixes.Lock),
            new OpCodeProperties(0x81, 0, Instruction.Add, OperandSize.Size32, OperandFormat.Ed_Id, InstructionPrefixes.Lock),
            new OpCodeProperties(RexPrefix.W, 0x81, 0, Instruction.Add, OperandFormat.Eq_Id, InstructionPrefixes.Lock),
            new OpCodeProperties(0x83, 0, Instruction.Add, OperandSize.Size16, OperandFormat.Ew_Ib, InstructionPrefixes.Lock),
            new OpCodeProperties(0x83, 0, Instruction.Add, OperandSize.Size32, OperandFormat.Ed_Ib, InstructionPrefixes.Lock),
            new OpCodeProperties(RexPrefix.W, 0x83, 0, Instruction.Add, OperandFormat.Eq_Ib, InstructionPrefixes.Lock),
            new OpCodeProperties(0x00, Instruction.Add, OperandFormat.Eb_Gb, InstructionPrefixes.Lock),
            new OpCodeProperties(RexPrefix.W, 0x0, Instruction.Add, OperandFormat.Eb_Gb, InstructionPrefixes.Lock),
            new OpCodeProperties(0x01, Instruction.Add, OperandSize.Size16, OperandFormat.Ew_Gw, InstructionPrefixes.Lock),
            new OpCodeProperties(0x01, Instruction.Add, OperandSize.Size32, OperandFormat.Ed_Gd, InstructionPrefixes.Lock),
            new OpCodeProperties(RexPrefix.W, 0x01, Instruction.Add, OperandFormat.Eq_Gq, InstructionPrefixes.Lock),
            new OpCodeProperties(0x02, Instruction.Add, OperandFormat.Gb_Eb),
            new OpCodeProperties(RexPrefix.W, 0x02, Instruction.Add, OperandFormat.Gb_Eb),
            new OpCodeProperties(0x03, Instruction.Add, OperandSize.Size16, OperandFormat.Gw_Ew),
            new OpCodeProperties(0x03, Instruction.Add, OperandSize.Size32, OperandFormat.Gd_Ed),
            new OpCodeProperties(RexPrefix.W, 0x03, Instruction.Add, OperandFormat.Gq_Eq),

            new OpCodeProperties(0x24, Instruction.And, OperandFormat.AL_Ib),
            new OpCodeProperties(0x25, Instruction.And, OperandSize.Size16, OperandFormat.AX_Iw),
            new OpCodeProperties(0x25, Instruction.And, OperandSize.Size32, OperandFormat.EAX_Id),
            new OpCodeProperties(RexPrefix.W, 0x25, Instruction.And, OperandFormat.RAX_Id), 
            new OpCodeProperties(0x80, 4, Instruction.And, OperandFormat.Eb_Ib, InstructionPrefixes.Lock),
            new OpCodeProperties(RexPrefix.W, 0x80, 4, Instruction.And, OperandFormat.Eb_Ib, InstructionPrefixes.Lock),
            new OpCodeProperties(0x81, 4, Instruction.And, OperandSize.Size16, OperandFormat.Ew_Iw, InstructionPrefixes.Lock),
            new OpCodeProperties(0x81, 4, Instruction.And, OperandSize.Size32, OperandFormat.Ed_Id, InstructionPrefixes.Lock),
            new OpCodeProperties(RexPrefix.W, 0x81, 4, Instruction.And, OperandFormat.Eq_Id, InstructionPrefixes.Lock),
            new OpCodeProperties(0x83, 4, Instruction.And, OperandSize.Size16, OperandFormat.Ew_Ib, InstructionPrefixes.Lock),
            new OpCodeProperties(0x83, 4, Instruction.And, OperandSize.Size32, OperandFormat.Ed_Ib, InstructionPrefixes.Lock),
            new OpCodeProperties(RexPrefix.W, 0x83, 4, Instruction.And, OperandFormat.Eq_Ib, InstructionPrefixes.Lock),
            new OpCodeProperties(0x20, Instruction.And, OperandFormat.Eb_Gb, InstructionPrefixes.Lock),
            new OpCodeProperties(RexPrefix.W, 0x20, Instruction.And, OperandFormat.Eb_Gb, InstructionPrefixes.Lock),
            new OpCodeProperties(0x21, Instruction.And, OperandSize.Size16, OperandFormat.Ew_Gw, InstructionPrefixes.Lock),
            new OpCodeProperties(0x21, Instruction.And, OperandSize.Size32, OperandFormat.Ed_Gd, InstructionPrefixes.Lock),
            new OpCodeProperties(RexPrefix.W, 0x21, Instruction.And, OperandFormat.Eq_Gq, InstructionPrefixes.Lock),
            new OpCodeProperties(0x22, Instruction.And, OperandFormat.Gb_Eb),
            new OpCodeProperties(RexPrefix.W, 0x22, Instruction.And, OperandFormat.Gb_Eb),
            new OpCodeProperties(0x23, Instruction.And, OperandSize.Size16, OperandFormat.Gw_Ew),
            new OpCodeProperties(0x23, Instruction.And, OperandSize.Size32, OperandFormat.Gd_Ed),
            new OpCodeProperties(RexPrefix.W, 0x23, Instruction.And, OperandFormat.Gq_Eq),

            new OpCodeProperties(0x63, Instruction.Arpl, OperandFormat.Ew_Gw, Compatibility64.NotEncodable) 
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
                case OperandFormat.None:
                    Assert.AreEqual(0, reader.OperandCount);
                    break;

                case OperandFormat.Ib:
                    Assert.AreEqual(1, reader.OperandCount);
                    Assert.AreEqual(OperandType.ImmediateByte, reader.GetOperandType(0));
                    Assert.AreEqual(0x11, reader.GetImmediateValue());
                    break;

                case OperandFormat.AL_Ib:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(OperandType.Register, reader.GetOperandType(0));
                    Assert.AreEqual(Register.Al, reader.GetRegister());
                    Assert.AreEqual(OperandType.ImmediateByte, reader.GetOperandType(1));
                    Assert.AreEqual(0x22, reader.GetImmediateValue());
                    break;

                case OperandFormat.AX_Iw:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(OperandType.Register, reader.GetOperandType(0));
                    Assert.AreEqual(Register.Ax, reader.GetRegister());
                    Assert.AreEqual(OperandType.ImmediateWord, reader.GetOperandType(1));
                    Assert.AreEqual(0x2222, reader.GetImmediateValue());
                    break;

                case OperandFormat.EAX_Id:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(OperandType.Register, reader.GetOperandType(0));
                    Assert.AreEqual(Register.Eax, reader.GetRegister());
                    Assert.AreEqual(OperandType.ImmediateDword, reader.GetOperandType(1));
                    Assert.AreEqual(0x22222222, reader.GetImmediateValue());
                    break;

                case OperandFormat.RAX_Id:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(OperandType.Register, reader.GetOperandType(0));
                    Assert.AreEqual(Register.Rax, reader.GetRegister());
                    Assert.AreEqual(OperandType.ImmediateDword, reader.GetOperandType(1));
                    Assert.AreEqual(0x22222222, reader.GetImmediateValue());
                    break;

                case OperandFormat.Eb_Ib:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(OperandType.DirectRegister, reader.GetOperandType(0));
                    Assert.AreEqual(opCode.RexPrefix != 0 ? Register.Spl : Register.Ah, reader.GetBaseRegister());
                    Assert.AreEqual(OperandType.ImmediateByte, reader.GetOperandType(1));
                    Assert.AreEqual(0x22, reader.GetImmediateValue());
                    break;

                case OperandFormat.Eb_Gb:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(OperandType.DirectRegister, reader.GetOperandType(0));
                    Assert.AreEqual(opCode.RexPrefix != 0 ? Register.Spl : Register.Ah, reader.GetBaseRegister());
                    Assert.AreEqual(OperandType.Register, reader.GetOperandType(1));
                    Assert.AreEqual(opCode.RexPrefix != 0 ? Register.Bpl : Register.Ch, reader.GetRegister());
                    break;

                case OperandFormat.Gb_Eb:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(OperandType.Register, reader.GetOperandType(0));
                    Assert.AreEqual(opCode.RexPrefix != 0 ? Register.Bpl : Register.Ch, reader.GetRegister());
                    Assert.AreEqual(OperandType.DirectRegister, reader.GetOperandType(1));
                    Assert.AreEqual(opCode.RexPrefix != 0 ? Register.Spl : Register.Ah, reader.GetBaseRegister());
                    break;

                case OperandFormat.Ew_Iw:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(OperandType.DirectRegister, reader.GetOperandType(0));
                    Assert.AreEqual(Register.Sp, reader.GetBaseRegister());
                    Assert.AreEqual(OperandType.ImmediateWord, reader.GetOperandType(1));
                    Assert.AreEqual(0x2222, reader.GetImmediateValue());
                    break;

                case OperandFormat.Ew_Ib:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(OperandType.DirectRegister, reader.GetOperandType(0));
                    Assert.AreEqual(Register.Sp, reader.GetBaseRegister());
                    Assert.AreEqual(OperandType.ImmediateByte, reader.GetOperandType(1));
                    Assert.AreEqual(0x22, reader.GetImmediateValue());
                    break;

                case OperandFormat.Ew_Gw:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(OperandType.DirectRegister, reader.GetOperandType(0));
                    Assert.AreEqual(Register.Sp, reader.GetBaseRegister());
                    Assert.AreEqual(OperandType.Register, reader.GetOperandType(1));
                    Assert.AreEqual(Register.Bp, reader.GetRegister());
                    break;

                case OperandFormat.Gw_Ew:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(OperandType.Register, reader.GetOperandType(0));
                    Assert.AreEqual(Register.Bp, reader.GetRegister());
                    Assert.AreEqual(OperandType.DirectRegister, reader.GetOperandType(1));
                    Assert.AreEqual(Register.Sp, reader.GetBaseRegister());
                    break;

                case OperandFormat.Ed_Id:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(OperandType.DirectRegister, reader.GetOperandType(0));
                    Assert.AreEqual(Register.Esp, reader.GetBaseRegister());
                    Assert.AreEqual(OperandType.ImmediateDword, reader.GetOperandType(1));
                    Assert.AreEqual(0x22222222, reader.GetImmediateValue());
                    break;

                case OperandFormat.Ed_Ib:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(OperandType.DirectRegister, reader.GetOperandType(0));
                    Assert.AreEqual(Register.Esp, reader.GetBaseRegister());
                    Assert.AreEqual(OperandType.ImmediateByte, reader.GetOperandType(1));
                    Assert.AreEqual(0x22, reader.GetImmediateValue());
                    break;

                case OperandFormat.Ed_Gd:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(OperandType.DirectRegister, reader.GetOperandType(0));
                    Assert.AreEqual(Register.Esp, reader.GetBaseRegister());
                    Assert.AreEqual(OperandType.Register, reader.GetOperandType(1));
                    Assert.AreEqual(Register.Ebp, reader.GetRegister());
                    break;

                case OperandFormat.Gd_Ed:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(OperandType.Register, reader.GetOperandType(0));
                    Assert.AreEqual(Register.Ebp, reader.GetRegister());
                    Assert.AreEqual(OperandType.DirectRegister, reader.GetOperandType(1));
                    Assert.AreEqual(Register.Esp, reader.GetBaseRegister());
                    break;

                case OperandFormat.Eq_Id:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(OperandType.DirectRegister, reader.GetOperandType(0));
                    Assert.AreEqual(Register.Rsp, reader.GetBaseRegister());
                    Assert.AreEqual(OperandType.ImmediateDword, reader.GetOperandType(1));
                    Assert.AreEqual(0x22222222, reader.GetImmediateValue());
                    break;

                case OperandFormat.Eq_Ib:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(OperandType.DirectRegister, reader.GetOperandType(0));
                    Assert.AreEqual(Register.Rsp, reader.GetBaseRegister());
                    Assert.AreEqual(OperandType.ImmediateByte, reader.GetOperandType(1));
                    Assert.AreEqual(0x22, reader.GetImmediateValue());
                    break;

                case OperandFormat.Eq_Gq:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(OperandType.DirectRegister, reader.GetOperandType(0));
                    Assert.AreEqual(Register.Rsp, reader.GetBaseRegister());
                    Assert.AreEqual(OperandType.Register, reader.GetOperandType(1));
                    Assert.AreEqual(Register.Rbp, reader.GetRegister());
                    break;

                case OperandFormat.Gq_Eq:
                    Assert.AreEqual(2, reader.OperandCount);
                    Assert.AreEqual(OperandType.Register, reader.GetOperandType(0));
                    Assert.AreEqual(Register.Rbp, reader.GetRegister());
                    Assert.AreEqual(OperandType.DirectRegister, reader.GetOperandType(1));
                    Assert.AreEqual(Register.Rsp, reader.GetBaseRegister());
                    break;

                default:
                    throw new NotImplementedException();
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
            var reader = new InstructionReader(new MemoryStream(bytes), mode, opCode.OperandSize == OperandSize.Size32);

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
            var reader = new InstructionReader(new MemoryStream(bytes), mode, opCode.OperandSize == OperandSize.Size32);

            reader.Read();
        }

        private static ExecutionMode GetExecutionMode(OpCodeProperties opCode)
        {
            return opCode.Compatibility64 != Compatibility64.Valid
                || opCode.OperandSize == OperandSize.Size16
                ? ExecutionMode.CompatibilityMode
                : ExecutionMode.Long64Bit;
        }

        public IEnumerable<OpCodeProperties> InstructionsInvalidIn64Bit()
        {
            return OpCodes.Where(o => o.Compatibility64 == Compatibility64.Invalid);
        }
            
        [Test]
        [TestCaseSource("InstructionsInvalidIn64Bit")]
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
                bytes.Add((byte)(opCode.RexPrefix));
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

                case OperandFormat.Gb_Eb:
                case OperandFormat.Gw_Ew:
                case OperandFormat.Gd_Ed:
                case OperandFormat.Gq_Eq:
                case OperandFormat.Eb_Gb:
                case OperandFormat.Ew_Gw:
                case OperandFormat.Ed_Gd:
                case OperandFormat.Eq_Gq:
                    // CH/BPL
                    bytes.Add((byte)(modrm | 0x28));
                    break;                    

                case OperandFormat.Eb_Ib:
                case OperandFormat.Ew_Ib:
                case OperandFormat.Ed_Ib:
                case OperandFormat.Eq_Ib:
                    bytes.Add((byte)(modrm | (opCode.OpCodeReg << 3)));
                    bytes.Add(0x22);
                    break;

                case OperandFormat.Ew_Iw:
                    bytes.Add((byte)(modrm | (opCode.OpCodeReg << 3)));
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    break;

                case OperandFormat.Eq_Id:
                case OperandFormat.Ed_Id:
                    bytes.Add((byte)(modrm | (opCode.OpCodeReg << 3)));
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    break;
            }

            return bytes.ToArray();
        }
    }
}
