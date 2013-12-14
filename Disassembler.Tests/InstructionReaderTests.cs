using System;
using System.Runtime.InteropServices;

using NUnit.Framework;

namespace Fantasm.Disassembler.Tests
{
    [TestFixture]
    class InstructionReaderTests : InstructionReaderTestBase
    {
        private const byte Nop = 0x90;

        [Test]
        public void Read_WithEmptyStream_ReturnsFalse()
        {
            var reader = ReadBytes();

            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_WithSingleInstruction_ReturnsTrueThenFalse()
        {
            var reader = ReadBytes(Nop);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_ReadsGroup1PrefixBeforeInstruction()
        {
            var reader = ReadBytes(0xF3, Nop);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_ReadsGroup2PrefixBeforeInstruction()
        {
            var reader = ReadBytes(0x2E, Nop);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_ReadsGroup3PrefixBeforeInstruction()
        {
            var reader = ReadBytes(0x66, Nop);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_ReadsGroup4PrefixBeforeInstruction()
        {
            var reader = ReadBytes(0x67, Nop);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_CanReadTwoPrefixes()
        {
            var reader = ReadBytes(0xF3, 0x2E, Nop);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_CanReadThreePrefixes()
        {
            var reader = ReadBytes(
                0xF3,
                0x2E,
                0x66,
                Nop);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_CanReadFourPrefixes()
        {
            var reader = ReadBytes(
                0xF3,
                0x2E,
                0x66,
                0x67,
                Nop);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_WithTwoGroup1Prefixes_ThrowsFormatException()
        {
            var reader = ReadBytes(0xF0, 0xF3, Nop);

            Assert.IsTrue(reader.Read());
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_WithTwoGroup2Prefixes_ThrowsFormatException()
        {
            var reader = ReadBytes(0x2E, 0x3E, Nop);

            Assert.IsTrue(reader.Read());
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_WithTwoGroup3Prefixes_ThrowsFormatException()
        {
            var reader = ReadBytes(
                0x66,
                0x66,
                Nop);

            Assert.IsTrue(reader.Read());
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_WithTwoGroup4Prefixes_ThrowsFormatException()
        {
            var reader = ReadBytes(
                0x67,
                0x67,
                Nop);

            Assert.IsTrue(reader.Read());
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_WithPrefixButNoOpcode_ThrowsFormatException()
        {
            var reader = ReadBytes(0x66);
            reader.Read();
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_WithPrefixAfterRexPrefix_ThrowsFormatException()
        {
            var reader = ReadBytes64(0x40, 0x66, 0x90);
            reader.Read();
        }

        [Test]
        public void Read_WhenPreviousInstructionUsesRexPrefix_IgnoresPreviousRex()
        {
            // ADD RAX 01234567H
            // ADD AX 01234567H
            var reader = ReadBytes64(0x48, 0x05, 0x67, 0x45, 0x23, 0x01, 0x05, 0x67, 0x45, 0x23, 0x01);
            
            Assert.IsTrue(reader.Read());
            Assert.AreEqual(Register.Rax, reader.GetOperandRegister(0));

            Assert.IsTrue(reader.Read());
            Assert.AreEqual(Register.Eax, reader.GetOperandRegister(0));
            
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_WithRexWAndOperandSizeOverride_IgnoresOperandSizeOverride()
        {
            // ADD RAX 01234567H
            var reader = ReadBytes64(0x66, 0x48, 0x05, 0x67, 0x45, 0x23, 0x01);

            Assert.IsTrue(reader.Read());
            Assert.AreEqual(Register.Rax, reader.GetOperandRegister(0));

            Assert.IsFalse(reader.Read());   
        }

        [Test]
        public void Read_WithRexNoWAndOperandSizeOverride_UsesOperandSizeOverride()
        {
            // ADD AX 0123H
            var reader = ReadBytes64(0x66, 0x40, 0x05, 0x23, 0x01);

            Assert.IsTrue(reader.Read());
            Assert.AreEqual(Register.Ax, reader.GetOperandRegister(0));

            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_ClearsPrefixes()
        {
            // ADD AX 1234h
            // ADD EAX 12345678h
            var reader = ReadBytes(0x66, 0x05, 0x34, 0x12, 0x05, 0x78, 0x56, 0x34, 0x12);

            Assert.IsTrue(reader.Read());
            Assert.AreEqual(Register.Ax, reader.GetOperandRegister(0));

            Assert.IsTrue(reader.Read());
            Assert.AreEqual(Register.Eax, reader.GetOperandRegister(0));

            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_In16BitMode_Reads16BitOperands()
        {
            // ADD AX 1234h
            var reader = ReadBytes16(0x05, 0x34, 0x12);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_In32BitMode_Reads32BitOperands()
        {
            // ADD EAX 12345678h
            var reader = ReadBytes(0x05, 0x78, 0x56, 0x34, 0x12);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_In32BitModeWithOperandSizeOverridePrefix_Reads16BitOperands()
        {
            // ADD AX 1234h
            var reader = ReadBytes(0x66, 0x05, 0x34, 0x12);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_In16BitModeWithOperandSizeOverridePrefix_Reads32BitOperands()
        {
            // ADD EAX 12345678h
            var reader = ReadBytes16(0x66, 0x05, 0x78, 0x56, 0x34, 0x12);

            Assert.IsTrue(reader.Read());
            Assert.IsFalse(reader.Read());
        }

        [Test]
        public void Read_ForInstructionWithNoOperands_ResetsOperandCount()
        {
            // AAD 23H
            // NOP
            var reader = ReadBytes(0xD5, 0x23, 0x90);
            reader.Read();
            reader.Read();

            Assert.AreEqual(0, reader.OperandCount);
        }

        [Test]
        [TestCase(0, Register.Al)]
        [TestCase(1, Register.Cl)]
        [TestCase(2, Register.Dl)]
        [TestCase(3, Register.Bl)]
        [TestCase(4, Register.Ah)]
        [TestCase(5, Register.Ch)]
        [TestCase(6, Register.Dh)]
        [TestCase(7, Register.Bh)]
        public void Read_ForInstructionWith8bitModRM_CanReadRegisterOperand(byte modrmReg, Register register)
        {
            // ADD (AX) 0
            var reader = ReadBytes(0x80, (byte)(0xc0 | modrmReg), 0x00);
            reader.Read();
            
            Assert.AreEqual(OperandType.Register, reader.GetOperandType(0));
            Assert.AreEqual(register, reader.GetOperandRegister(0));
        }

        [Test]
        [TestCase(0, Register.Eax)]
        [TestCase(1, Register.Ecx)]
        [TestCase(2, Register.Edx)]
        [TestCase(3, Register.Ebx)]
        [TestCase(6, Register.Esi)]
        [TestCase(7, Register.Edi)]
        public void Read_ForInstructionWithModRM_ReadsCorrectBaseAddress(byte modrmReg, Register register)
        {
            // ADD ([EAX]) 0
            var reader = ReadBytes(0x80, modrmReg, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(register, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(Register.None, reader.GetOperandIndexRegister(0));
            Assert.AreEqual(1, reader.GetOperandScale(0));
            Assert.AreEqual(0, reader.GetOperandDisplacement(0));
        }

        [Test]
        public void Read_ForInstructionWithModRM_WithJustDisplacement_ReadsOperandCorrectly()
        {
            // ADD ([0x01234567]) 0
            var reader = ReadBytes(0x80, 0x05, 0x67, 0x45, 0x23, 0x01, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(Register.None, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(Register.None, reader.GetOperandIndexRegister(0));
            Assert.AreEqual(1, reader.GetOperandScale(0));
            Assert.AreEqual(0x01234567, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, Register.Eax)]
        [TestCase(1, Register.Ecx)]
        [TestCase(2, Register.Edx)]
        [TestCase(3, Register.Ebx)]
        [TestCase(5, Register.Ebp)]
        [TestCase(6, Register.Esi)]
        [TestCase(7, Register.Edi)]
        public void Read_ForInstructionWithModRMWith8BitDisplacement_ReadsOperandCorrectly(byte modrmReg, Register register)
        {
            // ADD ([EAX + 0x23]) 0
            var reader = ReadBytes(0x80, (byte)(0x40 | modrmReg), 0x23, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(register, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(Register.None, reader.GetOperandIndexRegister(0));
            Assert.AreEqual(1, reader.GetOperandScale(0));
            Assert.AreEqual(0x23, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, Register.Eax)]
        [TestCase(1, Register.Ecx)]
        [TestCase(2, Register.Edx)]
        [TestCase(3, Register.Ebx)]
        [TestCase(5, Register.Ebp)]
        [TestCase(6, Register.Esi)]
        [TestCase(7, Register.Edi)]
        public void Read_ForInstructionWithModRMWith32BitDisplacement_ReadsOperandCorrectly(byte modrmReg, Register register)
        {
            // ADD ([EAX + 0x01234567]) 0
            var reader = ReadBytes(0x80, (byte)(0x80 | modrmReg), 0x67, 0x45, 0x23, 0x01, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(register, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(Register.None, reader.GetOperandIndexRegister(0));
            Assert.AreEqual(1, reader.GetOperandScale(0));
            Assert.AreEqual(0x01234567, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, Register.Eax)]
        [TestCase(1, Register.Ecx)]
        [TestCase(2, Register.Edx)]
        [TestCase(3, Register.Ebx)]
        [TestCase(5, Register.Ebp)]
        [TestCase(6, Register.Esi)]
        [TestCase(7, Register.Edi)]
        public void Read_ForInstructionWithSib_ReadsIndexRegisterCorrectly(byte sibIndex, Register register)
        {
            // ADD ([EAX + REG]) 0
            var reader = ReadBytes(0x80, 0x04, (byte)(sibIndex << 3), 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(Register.Eax, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(register, reader.GetOperandIndexRegister(0));
            Assert.AreEqual(1, reader.GetOperandScale(0));
            Assert.AreEqual(0, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, 1)]
        [TestCase(1, 2)]
        [TestCase(2, 4)]
        [TestCase(3, 8)]
        public void Read_ForInstructionWithSib_ReadsScaleCorrectly(byte sibScale, int scale)
        {
            // ADD ([EAX + EAX * Scale]) 0
            var reader = ReadBytes(0x80, 0x04, (byte)(sibScale << 6), 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(Register.Eax, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(Register.Eax, reader.GetOperandIndexRegister(0));
            Assert.AreEqual(scale, reader.GetOperandScale(0));
            Assert.AreEqual(0, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, Register.Eax)]
        [TestCase(1, Register.Ecx)]
        [TestCase(2, Register.Edx)]
        [TestCase(3, Register.Ebx)]
        [TestCase(4, Register.Esp)]
        [TestCase(6, Register.Esi)]
        [TestCase(7, Register.Edi)]
        public void Read_ForInstructionWithSib_ReadsBaseCorrectly(byte sibBase, Register register)
        {
            // ADD ([BASE + EAX]) 0
            var reader = ReadBytes(0x80, 0x04, sibBase, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(register, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(Register.Eax, reader.GetOperandIndexRegister(0));
            Assert.AreEqual(1, reader.GetOperandScale(0));
            Assert.AreEqual(0, reader.GetOperandDisplacement(0));
        }

        [Test]
        public void Read_ForInstructionWithModRMSib_CanAddDwordDisplacementWithNoBase()
        {
            // ADD ([EAX*2 + 0x01234567]) 0
            var reader = ReadBytes(0x80, 0x04, 0x45, 0x67, 0x45, 0x23, 0x01, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(Register.None, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(Register.Eax, reader.GetOperandIndexRegister(0));
            Assert.AreEqual(2, reader.GetOperandScale(0));
            Assert.AreEqual(0x01234567, reader.GetOperandDisplacement(0));
        }

        [Test]
        public void Read_ForInstructionWithModRMSib_CanAddByteDisplacementToEbp()
        {
            // ADD ([EAX*2] + 0x23 + [EBP]) 0
            var reader = ReadBytes(0x80, 0x44, 0x45, 0x23, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(Register.Ebp, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(Register.Eax, reader.GetOperandIndexRegister(0));
            Assert.AreEqual(2, reader.GetOperandScale(0));
            Assert.AreEqual(0x23, reader.GetOperandDisplacement(0));
        }

        [Test]
        public void Read_ForInstructionWithModRMSib_CanAddDwordDisplacementToEbp()
        {
            // ADD ([EAX*2] + 0x01234567 + [EBP]) 0
            var reader = ReadBytes(0x80, 0x84, 0x45, 0x67, 0x45, 0x23, 0x01, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(Register.Ebp, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(Register.Eax, reader.GetOperandIndexRegister(0));
            Assert.AreEqual(2, reader.GetOperandScale(0));
            Assert.AreEqual(0x01234567, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, Register.Bx, Register.Si)]
        [TestCase(1, Register.Bx, Register.Di)]
        [TestCase(2, Register.Bp, Register.Si)]
        [TestCase(3, Register.Bp, Register.Di)]
        [TestCase(4, Register.Si, Register.None)]
        [TestCase(5, Register.Di, Register.None)]
        [TestCase(7, Register.Bx, Register.None)]
        public void Read_With16Bit_ModRMCanEncodeBaseAndIndexRegister(byte rm, Register baseRegister, Register indexRegister)
        {
            // ADD [Base + Index] 0
            var reader = ReadBytes16(0x80, rm, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(baseRegister, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(indexRegister, reader.GetOperandIndexRegister(0));
            Assert.AreEqual(1, reader.GetOperandScale(0));
            Assert.AreEqual(0, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, Register.Bx, Register.Si)]
        [TestCase(1, Register.Bx, Register.Di)]
        [TestCase(2, Register.Bp, Register.Si)]
        [TestCase(3, Register.Bp, Register.Di)]
        [TestCase(4, Register.Si, Register.None)]
        [TestCase(5, Register.Di, Register.None)]
        [TestCase(6, Register.Bp, Register.None)]
        [TestCase(7, Register.Bx, Register.None)]
        public void Read_With16Bit_ModRMCanEncodeBaseAndIndexRegisterWithByteOffset(
            byte rm,
            Register baseRegister,
            Register indexRegister)
        {
            // ADD [Base + Index + 0x23] 0
            var reader = ReadBytes16(0x80, (byte)(0x40 | rm), 0x23, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(baseRegister, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(indexRegister, reader.GetOperandIndexRegister(0));
            Assert.AreEqual(1, reader.GetOperandScale(0));
            Assert.AreEqual(0x23, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, Register.Bx, Register.Si)]
        [TestCase(1, Register.Bx, Register.Di)]
        [TestCase(2, Register.Bp, Register.Si)]
        [TestCase(3, Register.Bp, Register.Di)]
        [TestCase(4, Register.Si, Register.None)]
        [TestCase(5, Register.Di, Register.None)]
        [TestCase(6, Register.Bp, Register.None)]
        [TestCase(7, Register.Bx, Register.None)]
        public void Read_With16Bit_ModRMCanEncodeBaseAndIndexRegisterWithWordOffset(
            byte rm,
            Register baseRegister,
            Register indexRegister)
        {
            // ADD [Base + Index + 0x23] 0
            var reader = ReadBytes16(0x80, (byte)(0x80 | rm), 0x23, 0x01, 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Memory, reader.GetOperandType(0));
            Assert.AreEqual(baseRegister, reader.GetOperandBaseRegister(0));
            Assert.AreEqual(indexRegister, reader.GetOperandIndexRegister(0));
            Assert.AreEqual(1, reader.GetOperandScale(0));
            Assert.AreEqual(0x0123, reader.GetOperandDisplacement(0));
        }

        [Test]
        [TestCase(0, Register.Al)]
        [TestCase(1, Register.Cl)]
        [TestCase(2, Register.Dl)]
        [TestCase(3, Register.Bl)]
        [TestCase(4, Register.Ah)]
        [TestCase(5, Register.Ch)]
        [TestCase(6, Register.Dh)]
        [TestCase(7, Register.Bh)]
        public void Read_With16Bit_ModRMCanEncodeRegister(byte modrmReg, Register register)
        {
            // ADD [REG] 0
            var reader = ReadBytes(0x80, (byte)(0xc0 | modrmReg), 0x00);
            reader.Read();

            Assert.AreEqual(OperandType.Register, reader.GetOperandType(0));
            Assert.AreEqual(register, reader.GetOperandRegister(0));
        }
    }
}
