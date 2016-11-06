using NUnit.Framework;

namespace Fantasm.Disassembler.Tests
{
    class InstructionReaderGetOperandTests : InstructionReaderTestBase
    {
        [Test]
        public void Operand1_WithNoOperands_HasTypeNone()
        {
            var reader = ReadBytes32();
            Assert.AreEqual(OperandType.None, reader.Operand1.Type);
        }

        [Test]
        public void Operand1_HasCorrectType()
        {
            // ADD AL 23H
            var reader = ReadBytes32(0x04, 0x23);
            reader.Read();

            Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
        }

        [Test]
        public void Operand2_HasCorrectType()
        {
            // ADD AL 23H
            var reader = ReadBytes32(0x04, 0x23);
            reader.Read();

            Assert.AreEqual(OperandType.ImmediateByte, reader.Operand2.Type);
        }
        
        [Test]
        public void GetImmediate_CanRetrieveByteValue()
        {
            // AAD 23H
            var reader = ReadBytes32(0xD5, 0x23);
            reader.Read();

            Assert.AreEqual(0x23, reader.Operand1.GetImmediateValue());
        }

        [Test]
        public void GetImmediate_CanRetrieveWordOperand()
        {
            // ADD AX 2345H
            var reader = ReadBytes16(0x05, 0x45, 0x23);
            reader.Read();

            Assert.AreEqual(0x2345, reader.Operand2.GetImmediateValue());
        }

        [Test]
        public void GetImmediate_CanRetrieveDwordOperand()
        {
            // ADD EAX 23456789H
            var reader = ReadBytes32(0x05, 0x89, 0x67, 0x45, 0x23);
            reader.Read();

            Assert.AreEqual(0x23456789, reader.Operand2.GetImmediateValue());
        }

        [Test]
        public void GetOperandRegister_CanRetrieveImplicitRegister()
        {
            // ADD AL 23H
            var reader = ReadBytes32(0x04, 0x23);
            reader.Read();

            Assert.AreEqual(Register.Al, reader.Operand1.GetRegister());
        }

        [Test]
        public void GetOperandBaseRegister_CanRetrieveModRMRegister()
        {
            // ADD ([EAX]) 0
            var reader = ReadBytes32(0x80, 0x00, 0x00);
            reader.Read();

            Assert.AreEqual(Register.Eax, reader.Operand1.GetBaseRegister());
        }

        [Test]
        public void GetOperandIndexRegister_CanRetrieveSibIndexRegister()
        {
            // ADD ([EAX] + [EAX]) 0
            var reader = ReadBytes32(0x80, 0x04, 0x00, 0x00);
            reader.Read();

            Assert.AreEqual(Register.Eax, reader.Operand1.GetIndexRegister());
        }
        
        [Test]
        public void GetOperandScale_CanRetrieveSubScale()
        {
            // ADD ([EAX] + 4*[EAX]) 0
            var reader = ReadBytes32(0x80, 0x04, 0x80, 0x00);
            reader.Read();

            Assert.AreEqual(4, reader.Operand1.GetScale());
        }
        
        [Test]
        public void GetOperandDisplacement_CanRetrieveModRMDisplacement()
        {
            // ADD [0x12345678] 0
            var reader = ReadBytes32(0x80, 0x05, 0x78, 0x56, 0x34, 0x12, 0x00);
            reader.Read();

            Assert.AreEqual(0x12345678, reader.Operand1.GetDisplacement());
        }

        [Test]
        public void GetOperandDisplacement_CanReadRelativeAddress()
        {
            // call [EIP + 0x12345678]
            var reader = ReadBytes32(0xe8, 0x78, 0x56, 0x34, 0x12);
            reader.Read();

            Assert.AreEqual(OperandType.RelativeAddress, reader.Operand1.Type);
            Assert.AreEqual(0x12345678, reader.Operand1.GetDisplacement());
        }

        [Test]
        public void GetOperandDisplacement_CanReadPointerFromFarPointer()
        {
            // call [EIP + 0x12345678]
            var reader = ReadBytes32(0x9A, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12);
            reader.Read();

            Assert.AreEqual(0x12345678, reader.Operand1.GetDisplacement());
        }

        [Test]
        public void GetOperandDisplacement_CanReadCodeSegmentPointerFromFarPointer()
        {
            // call [EIP + 0x12345678]
            var reader = ReadBytes32(0x9A, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12);
            reader.Read();

            Assert.AreEqual(unchecked ((short)0x9ABC), reader.Operand1.GetSegmentSelector());
        }
    }
}
