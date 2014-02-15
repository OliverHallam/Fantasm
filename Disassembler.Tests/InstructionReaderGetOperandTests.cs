using System;

using NUnit.Framework;

namespace Fantasm.Disassembler.Tests
{
    class InstructionReaderGetOperandTests : InstructionReaderTestBase
    {
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetOperandType_WithNoOperands_ThrowsArgumentException()
        {
            var reader = ReadBytes32();
            reader.GetOperandType(0);
        }

        [Test]
        public void GetOperandType_CanRetrieveTypeOfFirstOperand()
        {
            // ADD AL 23H
            var reader = ReadBytes32(0x04, 0x23);
            reader.Read();

            Assert.AreEqual(OperandType.Register, reader.GetOperandType(0));
        }

        [Test]
        public void GetOperandType_CanRetrieveTypeOfSecondOperand()
        {
            // ADD AL 23H
            var reader = ReadBytes32(0x04, 0x23);
            reader.Read();

            Assert.AreEqual(OperandType.ImmediateByte, reader.GetOperandType(1));
        }
        
        [Test]
        public void GetImmediate_CanRetrieveByteValue()
        {
            // AAD 23H
            var reader = ReadBytes32(0xD5, 0x23);
            reader.Read();

            Assert.AreEqual(0x23, reader.GetImmediateValue());
        }

        [Test]
        public void GetImmediate_CanRetrieveWordOperand()
        {
            // ADD AX 2345H
            var reader = ReadBytes16(0x05, 0x45, 0x23);
            reader.Read();

            Assert.AreEqual(0x2345, reader.GetImmediateValue());
        }

        [Test]
        public void GetImmediate_CanRetrieveDwordOperand()
        {
            // ADD EAX 23456789H
            var reader = ReadBytes32(0x05, 0x89, 0x67, 0x45, 0x23);
            reader.Read();

            Assert.AreEqual(0x23456789, reader.GetImmediateValue());
        }

        [Test]
        public void GetOperandRegister_CanRetrieveImplicitRegister()
        {
            // ADD AL 23H
            var reader = ReadBytes32(0x04, 0x23);
            reader.Read();

            Assert.AreEqual(Register.Al, reader.GetRegister());
        }

        [Test]
        public void GetOperandBaseRegister_CanRetrieveModRMRegister()
        {
            // ADD ([EAX]) 0
            var reader = ReadBytes32(0x80, 0x00, 0x00);
            reader.Read();

            Assert.AreEqual(Register.Eax, reader.GetBaseRegister());
        }

        [Test]
        public void GetOperandIndexRegister_CanRetrieveSibIndexRegister()
        {
            // ADD ([EAX] + [EAX]) 0
            var reader = ReadBytes32(0x80, 0x04, 0x00, 0x00);
            reader.Read();

            Assert.AreEqual(Register.Eax, reader.GetIndexRegister());
        }
        
        [Test]
        public void GetOperandScale_CanRetrieveSubScale()
        {
            // ADD ([EAX] + 4*[EAX]) 0
            var reader = ReadBytes32(0x80, 0x04, 0x80, 0x00);
            reader.Read();

            Assert.AreEqual(4, reader.GetScale());
        }
        
        [Test]
        public void GetOperandDisplacement_CanRetrieveModRMDisplacement()
        {
            // ADD [0x12345678] 0
            var reader = ReadBytes32(0x80, 0x05, 0x78, 0x56, 0x34, 0x12, 0x00);
            reader.Read();

            Assert.AreEqual(0x12345678, reader.GetDisplacement());
        }

        [Test]
        public void GetOperandDisplacement_CanReadRelativeAddress()
        {
            // call [EIP + 0x12345678]
            var reader = ReadBytes32(0xe8, 0x78, 0x56, 0x34, 0x12);
            reader.Read();

            Assert.AreEqual(OperandType.RelativeAddress, reader.GetOperandType(0));
            Assert.AreEqual(0x12345678, reader.GetDisplacement());
        }

        [Test]
        public void GetOperandDisplacement_CanReadPointerFromFarPointer()
        {
            // call [EIP + 0x12345678]
            var reader = ReadBytes32(0x9A, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12);
            reader.Read();

            Assert.AreEqual(0x12345678, reader.GetDisplacement());
        }

        [Test]
        public void GetOperandDisplacement_CanReadCodeSegmentPointerFromFarPointer()
        {
            // call [EIP + 0x12345678]
            var reader = ReadBytes32(0x9A, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12);
            reader.Read();

            Assert.AreEqual(unchecked ((short)0x9ABC), reader.GetSegmentSelector());
        }

        // TODO: read clears all memory parameters

        // TODO: Add method to get pointer format for memory arguments
    }
}
