﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using NUnit.Framework;

namespace Fantasm.Disassembler.Tests
{
    [TestFixture(Description = "Tests of the InstructionReader class for specific OpCodes")]
    public class InstructionReaderOpCodesTests
    {
        public InstructionRepresentation[] AllOpcodes()
        {
            return OpCodes.All;
        }

        [Test]
        [TestCaseSource(nameof(AllOpcodes))]
        public void InstructionReader_WithCorrectOperands_SuccessfullyDecodesInstruction(InstructionRepresentation opCode)
        {
            var modrm = GetModrm(opCode);
            var mode = GetExecutionMode(opCode);
            var bytes = GetBytes(mode, opCode, modrm);

            var reader = new InstructionReader(
                new MemoryStream(bytes),
                mode,
                true);

            reader.Read();

            Assert.AreEqual(opCode.Mnemonic, reader.Instruction);

            // check operands
            CheckOperand(opCode, opCode.Operands.Length > 0 ? opCode.Operands[0] : OperandFormat.None, reader.Operand1);
            CheckOperand(opCode, opCode.Operands.Length > 1 ? opCode.Operands[1] : OperandFormat.None, reader.Operand2);
            CheckOperand(opCode, opCode.Operands.Length > 2 ? opCode.Operands[2] : OperandFormat.None, reader.Operand3);

            Assert.IsFalse(reader.Read());
        }

        private static void CheckOperand(InstructionRepresentation opCode, OperandFormat operandType, Operand operand)
        {
            switch (operandType)
            {
                case OperandFormat.None:
                    Assert.AreEqual(OperandType.None, operand.Type);
                    break;

                case OperandFormat.Three:
                    Assert.AreEqual(OperandType.ImmediateByte, operand.Type);
                    Assert.AreEqual(3, operand.GetImmediateValue());
                    break;

                case OperandFormat.Ib:
                    Assert.AreEqual(OperandType.ImmediateByte, operand.Type);
                    Assert.AreEqual(0x11, operand.GetImmediateValue());
                    break;

                case OperandFormat.Iw:
                    Assert.AreEqual(OperandType.ImmediateWord, operand.Type);
                    Assert.AreEqual(0x2222, operand.GetImmediateValue());
                    break;

                case OperandFormat.Id:
                    Assert.AreEqual(OperandType.ImmediateDword, operand.Type);
                    Assert.AreEqual(0x33333333, operand.GetImmediateValue());
                    break;

                case OperandFormat.Iq:
                    Assert.AreEqual(OperandType.ImmediateQword, operand.Type);
                    Assert.AreEqual(0x4444444444444444, operand.GetImmediateValue());
                    break;

                case OperandFormat.Register:
                    Assert.AreEqual(OperandType.Register, operand.Type);
                    Assert.AreEqual(opCode.Register, operand.GetRegister());
                    break;

                case OperandFormat.Eb:
                    Assert.AreEqual(OperandType.Register, operand.Type);
                    Assert.AreEqual(opCode.RexPrefix != 0 ? Register.Spl : Register.Ah, operand.GetBaseRegister());
                    break;

                case OperandFormat.Ew:
                    Assert.AreEqual(OperandType.Register, operand.Type);
                    Assert.AreEqual(Register.Sp, operand.GetBaseRegister());
                    break;

                case OperandFormat.Ed:
                    Assert.AreEqual(OperandType.Register, operand.Type);
                    Assert.AreEqual(Register.Esp, operand.GetBaseRegister());
                    break;

                case OperandFormat.Eq:
                    Assert.AreEqual(OperandType.Register, operand.Type);
                    Assert.AreEqual(Register.Rsp, operand.GetBaseRegister());
                    break;

                case OperandFormat.M:
                {
                    Assert.AreEqual(OperandType.Address, operand.Type);
                    var valid32 = (opCode.Compatibility & Compatibility.Compatibility32) == Compatibility.Valid;
                    Assert.AreEqual(valid32 ? Register.Edi : Register.Rdi, operand.GetBaseRegister());
                    break;
                }

                case OperandFormat.Mb:
                    Assert.AreEqual(OperandType.BytePointer, operand.Type);
                    Assert.AreEqual(Register.Edi, operand.GetBaseRegister());
                    break;

                case OperandFormat.Mw:
                    Assert.AreEqual(OperandType.WordPointer, operand.Type);
                    Assert.AreEqual(Register.Edi, operand.GetBaseRegister());
                    break;

                case OperandFormat.Md:
                    Assert.AreEqual(OperandType.DwordPointer, operand.Type);
                    Assert.AreEqual(Register.Edi, operand.GetBaseRegister());
                    break;

                case OperandFormat.Mf:
                    Assert.AreEqual(OperandType.FwordPointer, operand.Type);
                    Assert.AreEqual(Register.Edi, operand.GetBaseRegister());
                    break;

                case OperandFormat.Mq:
                {
                    Assert.AreEqual(OperandType.QwordPointer, operand.Type);
                    // if the instruction is valid in 32 bit we use the 32 bit register size.
                    var valid32 = (opCode.Compatibility & Compatibility.Compatibility32) == Compatibility.Valid;
                    Assert.AreEqual(valid32 ? Register.Edi : Register.Rdi, operand.GetBaseRegister());
                    break;
                }

                case OperandFormat.Mt:
                    Assert.AreEqual(OperandType.TbytePointer, operand.Type);
                    Assert.AreEqual(Register.Rdi, operand.GetBaseRegister());
                    break;

                case OperandFormat.Mdq:
                    Assert.AreEqual(OperandType.OwordPointer, operand.Type);
                    Assert.AreEqual(Register.Rdi, operand.GetBaseRegister());
                    break;

                case OperandFormat.Jb:
                    Assert.AreEqual(OperandType.RelativeAddress, operand.Type);
                    Assert.AreEqual(0x11, operand.GetDisplacement());
                    break;

                case OperandFormat.Jw:
                    Assert.AreEqual(OperandType.RelativeAddress, operand.Type);
                    Assert.AreEqual(0x1111, operand.GetDisplacement());
                    break;

                case OperandFormat.Jd:
                    Assert.AreEqual(OperandType.RelativeAddress, operand.Type);
                    Assert.AreEqual(0x11111111, operand.GetDisplacement());
                    break;

                case OperandFormat.Aww:
                    Assert.AreEqual(OperandType.FarPointer, operand.Type);
                    Assert.AreEqual(0x1111, operand.GetSegmentSelector());
                    Assert.AreEqual(0x2222, operand.GetDisplacement());
                    break;

                case OperandFormat.Awd:
                    Assert.AreEqual(OperandType.FarPointer, operand.Type);
                    Assert.AreEqual(0x1111, operand.GetSegmentSelector());
                    Assert.AreEqual(0x22222222, operand.GetDisplacement());
                    break;

                case OperandFormat.Ob:
                {
                    Assert.AreEqual(OperandType.BytePointer, operand.Type);
                    Assert.AreEqual(Register.None, operand.GetBaseRegister());
                    Assert.AreEqual(Register.None, operand.GetIndexRegister());
                    Assert.AreEqual(1, operand.GetScale());
                    var valid32 = (opCode.Compatibility & Compatibility.Compatibility32) == Compatibility.Valid;
                    Assert.AreEqual(valid32 ? 0x11111111 : 0x1111111111111111, operand.GetDisplacement());
                    break;
                }

                case OperandFormat.Ow:
                {
                    Assert.AreEqual(OperandType.WordPointer, operand.Type);
                    Assert.AreEqual(Register.None, operand.GetBaseRegister());
                    Assert.AreEqual(Register.None, operand.GetIndexRegister());
                    Assert.AreEqual(1, operand.GetScale());
                    var valid32 = (opCode.Compatibility & Compatibility.Compatibility32) == Compatibility.Valid;
                    Assert.AreEqual(valid32 ? 0x11111111 : 0x1111111111111111, operand.GetDisplacement());
                    break;
                }

                case OperandFormat.Od:
                {
                    Assert.AreEqual(OperandType.DwordPointer, operand.Type);
                    Assert.AreEqual(Register.None, operand.GetBaseRegister());
                    Assert.AreEqual(Register.None, operand.GetIndexRegister());
                    Assert.AreEqual(1, operand.GetScale());
                    var valid32 = (opCode.Compatibility & Compatibility.Compatibility32) == Compatibility.Valid;
                    Assert.AreEqual(valid32 ? 0x11111111 : 0x1111111111111111, operand.GetDisplacement());
                    break;
                }

                case OperandFormat.Oq:
                {
                    Assert.AreEqual(OperandType.QwordPointer, operand.Type);
                    Assert.AreEqual(Register.None, operand.GetBaseRegister());
                    Assert.AreEqual(Register.None, operand.GetIndexRegister());
                    Assert.AreEqual(1, operand.GetScale());
                    var valid32 = (opCode.Compatibility & Compatibility.Compatibility32) == Compatibility.Valid;
                    Assert.AreEqual(valid32 ? 0x11111111 : 0x1111111111111111, operand.GetDisplacement());
                    break;
                }

                case OperandFormat.Sw:
                    Assert.AreEqual(OperandType.Register, operand.Type);
                    Assert.AreEqual(Register.Cs, operand.GetBaseRegister());
                    break;

                case OperandFormat.AL:
                    Assert.AreEqual(OperandType.Register, operand.Type);
                    Assert.AreEqual(Register.Al, operand.GetRegister());
                    break;

                case OperandFormat.AX:
                    Assert.AreEqual(OperandType.Register, operand.Type);
                    Assert.AreEqual(Register.Ax, operand.GetRegister());
                    break;

                case OperandFormat.EAX:
                    Assert.AreEqual(OperandType.Register, operand.Type);
                    Assert.AreEqual(Register.Eax, operand.GetRegister());
                    break;

                case OperandFormat.RAX:
                    Assert.AreEqual(OperandType.Register, operand.Type);
                    Assert.AreEqual(Register.Rax, operand.GetRegister());
                    break;

                case OperandFormat.DX:
                    Assert.AreEqual(OperandType.Register, operand.Type);
                    Assert.AreEqual(Register.Dx, operand.GetRegister());
                    break;

                case OperandFormat.Gb:
                    Assert.AreEqual(OperandType.Register, operand.Type);
                    Assert.AreEqual(opCode.RexPrefix != 0 ? Register.Bpl : Register.Ch, operand.GetRegister());
                    break;

                case OperandFormat.Gw:
                    Assert.AreEqual(OperandType.Register, operand.Type);
                    Assert.AreEqual(Register.Bp, operand.GetRegister());
                    break;

                case OperandFormat.Gd:
                    Assert.AreEqual(OperandType.Register, operand.Type);
                    Assert.AreEqual(Register.Ebp, operand.GetRegister());
                    break;

                case OperandFormat.Gq:
                    Assert.AreEqual(OperandType.Register, operand.Type);
                    Assert.AreEqual(Register.Rbp, operand.GetRegister());
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public static IEnumerable<InstructionRepresentation> OpCodesWithLockUnsupported()
        {
            return OpCodes.All.Where(o => !o.Prefixes.HasFlag(InstructionPrefixes.Lock));
        }

        [Test]
        [TestCaseSource(nameof(OpCodesWithLockUnsupported))]
        [ExpectedException(typeof(FormatException))]
        public void InstructionReader_ForLockPrefix_ThrowsFormatException(InstructionRepresentation opCode)
        {
            var byteList = new List<byte> { 0xF0 };
            // use a memory address to smoke out false negatives
            var mode = GetExecutionMode(opCode);
            byteList.AddRange(GetBytes(mode, opCode, Combine(GetOpcodeModrm(opCode), 3)));
            var bytes = byteList.ToArray();

            var reader = new InstructionReader(new MemoryStream(bytes), mode);
            
            reader.Read();
        }

        public static IEnumerable<InstructionRepresentation> OpCodesWithLockSupported()
        {
            return OpCodes.All.Where(o => o.Prefixes.HasFlag(InstructionPrefixes.Lock));
        }

        [Test]
        [TestCaseSource(nameof(OpCodesWithLockSupported))]
        [ExpectedException(typeof(FormatException))]
        public void InstructionReader_ForLockPrefixWithNoMemoryAccess_ThrowsFormatException(InstructionRepresentation opCode)
        {
            var byteList = new List<byte> { 0xF0 };
            var mode = GetExecutionMode(opCode);
            byteList.AddRange(GetBytes(mode, opCode, Combine(GetOpcodeModrm(opCode), 0xC0))); // EAX
            var bytes = byteList.ToArray();

            var reader = new InstructionReader(new MemoryStream(bytes), mode, opCode.OperandSize == OperandSize.Size32);

            reader.Read();
        }

        [Test]
        [TestCaseSource(nameof(OpCodesWithLockSupported))]
        public void InstructionReader_ForLockPrefixWithMemoryAccess_DoesNotThrow(InstructionRepresentation opCode)
        {
            var byteList = new List<byte> { 0xF0 };
            var mode = GetExecutionMode(opCode);
            byteList.AddRange(GetBytes(mode, opCode, GetOpcodeModrm(opCode) ?? 0)); // [EAX]
            var bytes = byteList.ToArray();

            var reader = new InstructionReader(new MemoryStream(bytes), mode, true);

            reader.Read();
        }

        private static ExecutionMode GetExecutionMode(InstructionRepresentation opCode)
        {
            return (opCode.Compatibility & Compatibility.Compatibility64) != Compatibility.Valid
                || (opCode.OperandSize == OperandSize.Size16 && (opCode.Compatibility & Compatibility.Compatibility32) == Compatibility.Valid)
                ? ExecutionMode.CompatibilityMode
                : ExecutionMode.Long64Bit;
        }

        public IEnumerable<InstructionRepresentation> InstructionsInvalidIn64Bit()
        {
            return OpCodes.All.Where(o => o.Compatibility == Compatibility.Invalid64);
        }
            
        [Test]
        [TestCaseSource(nameof(InstructionsInvalidIn64Bit))]
        [ExpectedException(typeof(FormatException))]
        public void InstructionReader_For64BitMode_ThrowsFormatException(InstructionRepresentation opCode)
        {
            var bytes = GetBytes(ExecutionMode.Long64Bit, opCode, GetModrm(opCode));
            var reader = new InstructionReader(new MemoryStream(bytes), ExecutionMode.Long64Bit);
            
            reader.Read();
        }

        public IEnumerable<InstructionRepresentation> InstructionsWithMemoryParameters()
        {
            return OpCodes.All.Where(
                    opCode => opCode.Operands.Any(
                        operand => operand == OperandFormat.Mw || operand == OperandFormat.Md
                        || operand == OperandFormat.Mq || operand == OperandFormat.Mdq));
        }

        [Test]
        [TestCaseSource(nameof(InstructionsWithMemoryParameters))]
        [ExpectedException(typeof(FormatException))]
        public void InstructionReader_ForRegister_ThrowsFormatException(InstructionRepresentation opCode)
        {
            var bytes = GetBytes(ExecutionMode.Long64Bit, opCode, Combine(GetOpcodeModrm(opCode), 0xC0)); // EAX
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

            Assert.AreEqual(OperandType.None, reader.Operand1.Type);
            Assert.AreEqual(OperandType.None, reader.Operand2.Type);
        }

        [Test]
        public void InstructionReader_ForAamWithBase10_HidesOperand()
        {
            // This is a special case - conventionally AAM 0AH is represented as AAM
            var bytes = new byte[] { 0xD4, 0x0A };
            var reader = new InstructionReader(new MemoryStream(bytes), ExecutionMode.CompatibilityMode);

            reader.Read();

            Assert.AreEqual(OperandType.None, reader.Operand1.Type);
            Assert.AreEqual(OperandType.None, reader.Operand2.Type);
        }

        private static byte[] GetBytes(ExecutionMode mode, InstructionRepresentation opCode, byte? modrm)
        {
            var bytes = new List<byte>();

            if ((opCode.Prefixes & InstructionPrefixes.RepNZ) != 0)
            {
                bytes.Add(0xF2);
            }

            if (opCode.OperandSize == OperandSize.Size16)
            {
                bytes.Add(0x66);
            }

            if (mode == ExecutionMode.Long64Bit && opCode.OperandSize == OperandSize.Size32
                && (opCode.Compatibility & Compatibility.Compatibility32) != Compatibility.NotEncodable32)
            {
                bytes.Add(0x67);
            }

            if ((opCode.Compatibility & Compatibility.Compatibility64) == Compatibility.NotEncodable64
                && opCode.OperandSize == OperandSize.Size16)
            {
                // also set the address size for good measure!
                bytes.Add(0x67);
            }

            if (opCode.RexPrefix != 0)
            {
                bytes.Add((byte)(opCode.RexPrefix | RexPrefix.Magic));
            }

            bytes.AddRange(opCode.OpCode);

            if (modrm.HasValue)
            {
                bytes.Add(modrm.Value);
            }

            foreach (var operand in opCode.Operands)
            {
                WriteImmediateBytes(opCode.Compatibility, operand, bytes);
            }

            return bytes.ToArray();
        }

        private static byte? GetOpcodeModrm(InstructionRepresentation opCode)
        {
            // add the op code reg bits if necessary
            if (opCode.OpCodeReg != 255)
            {
                return (byte)(opCode.OpCodeReg << 3);
            }

            return null;
        }

        private static byte? GetModrm(InstructionRepresentation opCode)
        {
            var modrm = GetOpcodeModrm(opCode);
            foreach (var operand in opCode.Operands)
            {
                modrm = Combine(modrm, GetModrm(operand));
            }
            return modrm;
        }

        public static byte? Combine(byte? b1, byte? b2)
        {
            return (byte?)(b1 | b2) ?? b1 ?? b2;
        }

        private static byte? GetModrm(OperandFormat operand)
        {
            switch (operand)
            {
                case OperandFormat.Eb:
                case OperandFormat.Ew:
                case OperandFormat.Ed:
                case OperandFormat.Eq:
                    // AH/SP/ESP/RSP
                    return 0xC4;

                case OperandFormat.M:
                case OperandFormat.Mb:
                case OperandFormat.Mw:
                case OperandFormat.Md:
                case OperandFormat.Mf:
                case OperandFormat.Mq:
                case OperandFormat.Mt:
                case OperandFormat.Mdq:
                    // [BX]/[EDI]/[RDI]
                    return 0x07;

                case OperandFormat.Gb:
                case OperandFormat.Gw:
                case OperandFormat.Gd:
                case OperandFormat.Gq:
                    // CH/BPL/BP/EBP/RBP
                    return 0x28;

                case OperandFormat.Sw:
                    // CS
                    return 0x08;
            }

            return null;
        }

        private static void WriteImmediateBytes(Compatibility compatibility, OperandFormat operand, List<byte> bytes)
        {
            switch (operand)
            {
                case OperandFormat.Ib:
                    bytes.Add(0x11);
                    break;

                case OperandFormat.Iw:
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    break;

                case OperandFormat.Id:
                    bytes.Add(0x33);
                    bytes.Add(0x33);
                    bytes.Add(0x33);
                    bytes.Add(0x33);
                    break;

                case OperandFormat.Iq:
                    bytes.Add(0x44);
                    bytes.Add(0x44);
                    bytes.Add(0x44);
                    bytes.Add(0x44);
                    bytes.Add(0x44);
                    bytes.Add(0x44);
                    bytes.Add(0x44);
                    bytes.Add(0x44);
                    break;

                case OperandFormat.Jb:
                    bytes.Add(0x11);
                    break;

                case OperandFormat.Jw:
                    bytes.Add(0x11);
                    bytes.Add(0x11);
                    break;

                case OperandFormat.Jd:
                    bytes.Add(0x11);
                    bytes.Add(0x11);
                    bytes.Add(0x11);
                    bytes.Add(0x11);
                    break;

                case OperandFormat.Aww:
                    bytes.Add(0x11);
                    bytes.Add(0x11);
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    break;

                case OperandFormat.Awd:
                    bytes.Add(0x11);
                    bytes.Add(0x11);
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    break;

                case OperandFormat.Ob:
                case OperandFormat.Ow:
                case OperandFormat.Od:
                case OperandFormat.Oq:
                    bytes.Add(0x11);
                    bytes.Add(0x11);
                    bytes.Add(0x11);
                    bytes.Add(0x11);
                    if ((compatibility & Compatibility.Compatibility32) != Compatibility.Valid)
                    {
                        bytes.Add(0x11);
                        bytes.Add(0x11);
                        bytes.Add(0x11);
                        bytes.Add(0x11);
                    }
                    break;
            }
        }
    }
}
