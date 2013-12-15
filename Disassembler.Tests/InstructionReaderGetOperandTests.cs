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
            reader.GetOperandByte(0);
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
        [ExpectedException(typeof(ArgumentException))]
        public void GetOperandByte_WithNoOperands_ThrowsArgumentException()
        {
            var reader = ReadBytes32();
            reader.GetOperandByte(0);
        }

        [Test]
        public void GetOperandByte_CanRetrieveFirstOperand()
        {
            // AAD 23H
            var reader = ReadBytes32(0xD5, 0x23);
            reader.Read();

            Assert.AreEqual(0x23, reader.GetOperandByte(0));
        }

        [Test]
        public void GetOperandByte_CanRetrieveSecondOperand()
        {
            // ADD AL 23H
            var reader = ReadBytes32(0x04, 0x23);
            reader.Read();

            Assert.AreEqual(0x23, reader.GetOperandByte(1));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetOperandByte_ForNonByteArgument_ThrowsInvalidOperationException()
        {
            // ADD AL 23H
            var reader = ReadBytes32(0x04, 0x23);
            reader.Read();

            reader.GetOperandByte(0);
        }


        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetOperandWord_WithNoOperands_ThrowsArgumentException()
        {
            var reader = ReadBytes32();
            reader.GetOperandWord(0);
        }

        [Test]
        [Ignore] // we don't support any instructions yet with a word first operand
        public void GetOperandWord_CanRetrieveFirstOperand()
        {
        }

        [Test]
        public void GetOperandWord_CanRetrieveSecondOperand()
        {
            // ADD AX 2345H
            var reader = ReadBytes16(0x05, 0x45, 0x23);
            reader.Read();

            Assert.AreEqual(0x2345, reader.GetOperandWord(1));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetOperandWord_ForNonWordArgument_ThrowsInvalidOperationException()
        {
            // ADD AL 23H
            var reader = ReadBytes32(0x04, 0x23);
            reader.Read();

            reader.GetOperandWord(0);
        }


        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetOperandDword_WithNoOperands_ThrowsArgumentException()
        {
            var reader = ReadBytes32();
            reader.GetOperandDword(0);
        }

        [Test]
        [Ignore] // we don't support any instructions yet with a word first operand
        public void GetOperandDword_CanRetrieveFirstOperand()
        {
        }

        [Test]
        public void GetOperandDword_CanRetrieveSecondOperand()
        {
            // ADD EAX 23456789H
            var reader = ReadBytes32(0x05, 0x89, 0x67, 0x45, 0x23);
            reader.Read();

            Assert.AreEqual(0x23456789, reader.GetOperandDword(1));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetOperandDword_ForNonDwordArgument_ThrowsInvalidOperationException()
        {
            // ADD AL 23H
            var reader = ReadBytes32(0x04, 0x23);
            reader.Read();

            reader.GetOperandDword(0);
        }


        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetOperandRegister_WithNoOperands_ThrowsArgumentException()
        {
            var reader = ReadBytes32();
            reader.GetOperandRegister(0);
        }

        [Test]
        public void GetOperandRegister_CanRetrieveFirstOperand()
        {
            // ADD AL 23H
            var reader = ReadBytes32(0x04, 0x23);
            reader.Read();

            Assert.AreEqual(Register.Al, reader.GetOperandRegister(0));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetOperandRegister_ForNonRegisterArgument_ThrowsInvalidOperationException()
        {
            // AAD 23H
            var reader = ReadBytes32(0xD5, 0x23);
            reader.Read();

            reader.GetOperandRegister(0);
        }


        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetOperandBaseRegister_WithNoOperands_ThrowsArgumentException()
        {
            var reader = ReadBytes32();
            reader.GetOperandRegister(0);
        }

        [Test]
        public void GetOperandBaseRegister_CanRetrieveFirstOperand()
        {
            // ADD ([EAX]) 0
            var reader = ReadBytes32(0x80, 0x00, 0x00);
            reader.Read();

            Assert.AreEqual(Register.Eax, reader.GetOperandBaseRegister(0));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetOperandBaseRegister_ForNonFirstArgument_ThrowsInvalidOperationException()
        {
            // ADD ([EAX]) 0
            var reader = ReadBytes32(0x80, 0x00, 0x00);
            reader.Read();

            Assert.AreEqual(Register.Eax, reader.GetOperandBaseRegister(1));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetOperandBaseRegister_ForNonMemoryArgument_ThrowsInvalidOperationException()
        {
            // ADD AL 23H
            var reader = ReadBytes32(0x04, 0x23);
            reader.Read();

            reader.GetOperandBaseRegister(0);
        }


        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetOperandIndexRegister_WithNoOperands_ThrowsArgumentException()
        {
            var reader = ReadBytes32();
            reader.GetOperandIndexRegister(0);
        }

        [Test]
        public void GetOperandIndexRegister_CanRetrieveFirstOperand()
        {
            // TODO: use a better example

            // ADD ([EAX]) 0
            var reader = ReadBytes32(0x80, 0x00, 0x00);
            reader.Read();

            Assert.AreEqual(Register.None, reader.GetOperandIndexRegister(0));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetOperandIndexRegister_ForNonFirstArgument_ThrowsInvalidOperationException()
        {
            // ADD ([EAX]) 0
            var reader = ReadBytes32(0x80, 0x00, 0x00);
            reader.Read();

            reader.GetOperandIndexRegister(1);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetOperandIndexRegister_ForNonMemoryArgument_ThrowsInvalidOperationException()
        {
            // ADD AL 23H
            var reader = ReadBytes32(0x04, 0x23);
            reader.Read();

            reader.GetOperandIndexRegister(0);
        }


        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetOperandScale_WithNoOperands_ThrowsArgumentException()
        {
            var reader = ReadBytes32();
            reader.GetOperandScale(0);
        }

        [Test]
        public void GetOperandScale_CanRetrieveFirstOperand()
        {
            // TODO: use a better example

            // ADD ([EAX]) 0
            var reader = ReadBytes32(0x80, 0x00, 0x00);
            reader.Read();

            Assert.AreEqual(1, reader.GetOperandScale(0));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetOperandScale_ForNonFirstArgument_ThrowsInvalidOperationException()
        {
            // ADD ([EAX]) 0
            var reader = ReadBytes32(0x80, 0x00, 0x00);
            reader.Read();

            reader.GetOperandScale(1);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetOperandScale_ForNonMemoryArgument_ThrowsInvalidOperationException()
        {
            // ADD AL 23H
            var reader = ReadBytes32(0x04, 0x23);
            reader.Read();

            reader.GetOperandScale(0);
        }


        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetOperandDisplacement_WithNoOperands_ThrowsArgumentException()
        {
            var reader = ReadBytes32();
            reader.GetOperandDisplacement(0);
        }

        [Test]
        public void GetOperandDisplacement_CanRetrieveFirstOperand()
        {
            // TODO: use a better example

            // ADD ([EAX]) 0
            var reader = ReadBytes32(0x80, 0x00, 0x00);
            reader.Read();

            Assert.AreEqual(0, reader.GetOperandDisplacement(0));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetOperandDisplacement_ForNonFirstArgument_ThrowsInvalidOperationException()
        {
            // ADD ([EAX]) 0
            var reader = ReadBytes32(0x80, 0x00, 0x00);
            reader.Read();

            reader.GetOperandDisplacement(1);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetOperandDisplacement_ForNonMemoryArgument_ThrowsInvalidOperationException()
        {
            // ADD AL 23H
            var reader = ReadBytes32(0x04, 0x23);
            reader.Read();

            reader.GetOperandDisplacement(0);
        }

        // TODO: read clears all memory parameters
    }
}
