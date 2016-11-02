using System.Linq;
using System.Text;

namespace Fantasm.Disassembler.Tests
{
    public class OpCodeProperties
    {
        internal OperandSize OperandSize;
        internal RexPrefix RexPrefix;
        internal byte[] OpCode;
        internal byte OpCodeReg;
        internal Instruction Mnemonic;
        internal OperandFormat Operand1;
        internal OperandFormat Operand2;
        internal InstructionPrefixes Prefixes;
        internal Compatibility Compatibility;
        internal Register Register;

        internal OpCodeProperties(byte opCode, Instruction mnemonic)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                255,
                mnemonic,
                OperandSize.Size32,
                OperandFormat.None,
                OperandFormat.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(byte opCode, Instruction mnemonic, OperandFormat operand1, OperandFormat operand2)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                255,
                mnemonic,
                OperandSize.Size32,
                operand1,
                operand2,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            Instruction mnemonic,
            Compatibility compatibility)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                255,
                mnemonic,
                OperandSize.Size32,
                OperandFormat.None,
                OperandFormat.None,
                compatibility,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            Instruction mnemonic,
            OperandFormat operand,
            Compatibility compatibility)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                255,
                mnemonic,
                OperandSize.Size32,
                operand,
                OperandFormat.None,
                compatibility,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            Instruction mnemonic,
            OperandFormat operand1,
            OperandFormat operand2,
            Compatibility compatibility)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                255,
                mnemonic,
                OperandSize.Size32,
                operand1,
                operand2,
                compatibility,
                Register.None)
        {
        }


        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            byte opCode,
            Instruction mnemonic,
            OperandFormat operand)
            : this(
                prefixes,
                0,
                new[] { opCode },
                255,
                mnemonic,
                OperandSize.Size32,
                operand,
                OperandFormat.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            byte opCode,
            Instruction mnemonic,
            OperandFormat operand1,
            OperandFormat operand2)
            : this(
                prefixes,
                0,
                new[] { opCode },
                255,
                mnemonic,
                OperandSize.Size32,
                operand1,
                operand2,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(byte opCode, Instruction mnemonic, OperandSize operandSize)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                255,
                mnemonic,
                operandSize,
                OperandFormat.None,
                OperandFormat.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(byte opCode, Instruction mnemonic, OperandSize operandSize, OperandFormat operand)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                255,
                mnemonic,
                operandSize,
                operand,
                OperandFormat.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(byte opCode, Instruction mnemonic, OperandSize operandSize, OperandFormat operand1, OperandFormat operand2)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                255,
                mnemonic,
                operandSize,
                operand1,
                operand2,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operand,
            Compatibility compatibility)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                255,
                mnemonic,
                operandSize,
                operand,
                OperandFormat.None,
                compatibility,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operand1,
            OperandFormat operand2,
            Compatibility compatibility)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                255,
                mnemonic,
                operandSize,
                operand1,
                operand2,
                compatibility,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            Instruction mnemonic,
            OperandSize operandSize,
            Register register,
            Compatibility compatibility)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                255,
                mnemonic,
                operandSize,
                OperandFormat.Register,
                OperandFormat.None,
                compatibility,
                register)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            byte opCode,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operand)
            : this(
                prefixes,
                0,
                new[] { opCode },
                255,
                mnemonic,
                operandSize,
                operand,
                OperandFormat.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            byte opCode,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operand1,
            OperandFormat operand2)
            : this(
                prefixes,
                0,
                new[] { opCode },
                255,
                mnemonic,
                operandSize,
                operand1,
                operand2,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(byte opCode, byte opCodeReg, Instruction mnemonic, OperandFormat operand)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operand,
                OperandFormat.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandFormat operand1,
            OperandFormat operand2)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operand1,
                operand2,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandFormat operand,
            Compatibility compatibility)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operand,
                OperandFormat.None,
                compatibility,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandFormat operand)
            : this(
                prefixes,
                0,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operand,
                OperandFormat.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandFormat operand1,
            OperandFormat operand2)
            : this(
                prefixes,
                0,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operand1,
                operand2,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operand)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                operandSize,
                operand,
                OperandFormat.None,
                Compatibility.Valid,
                Register.None)
        {
        }
        internal OpCodeProperties(
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operand1,
            OperandFormat operand2)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                operandSize,
                operand1,
                operand2,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operand,
            Compatibility compatibility)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                operandSize,
                operand,
                OperandFormat.None,
                compatibility,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operand)
            : this(
                prefixes,
                0,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                operandSize,
                operand,
                OperandFormat.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operand1,
            OperandFormat operand2)
            : this(
                prefixes,
                0,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                operandSize,
                operand1,
                operand2,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(byte[] opCode, Instruction mnemonic)
            : this(
                InstructionPrefixes.None,
                0,
                opCode,
                255,
                mnemonic,
                OperandSize.Size32,
                OperandFormat.None,
                OperandFormat.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(byte[] opCode, Instruction mnemonic, OperandFormat operand)
            : this(
                InstructionPrefixes.None,
                0,
                opCode,
                255,
                mnemonic,
                OperandSize.Size32,
                operand,
                OperandFormat.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            byte[] opCode,
            Instruction mnemonic,
            OperandFormat operand)
            : this(
                prefixes,
                0,
                opCode,
                255,
                mnemonic,
                OperandSize.Size32,
                operand,
                OperandFormat.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            byte[] opCode,
            Instruction mnemonic,
            OperandFormat operand1,
            OperandFormat operand2)
            : this(
                prefixes,
                0,
                opCode,
                255,
                mnemonic,
                OperandSize.Size32,
                operand1,
                operand2,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(byte[] opCode, Instruction mnemonic, Register register)
            : this(
                InstructionPrefixes.None,
                0,
                opCode,
                255,
                mnemonic,
                OperandSize.Size32,
                OperandFormat.Register,
                OperandFormat.None,
                Compatibility.Valid,
                register)
        {
        }

        internal OpCodeProperties(byte[] opCode, Instruction mnemonic, OperandSize operandSize, OperandFormat operand)
            : this(
                InstructionPrefixes.None,
                0,
                opCode,
                255,
                mnemonic,
                operandSize,
                operand,
                OperandFormat.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte[] opCode,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operand1,
            OperandFormat operand2)
            : this(
                InstructionPrefixes.None,
                0,
                opCode,
                255,
                mnemonic,
                operandSize,
                operand1,
                operand2,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            byte[] opCode,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operand1,
            OperandFormat operand2)
            : this(
                prefixes,
                0,
                opCode,
                255,
                mnemonic,
                operandSize,
                operand1,
                operand2,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte[] opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandFormat operand,
            InstructionPrefixes prefixes)
            : this(
                prefixes,
                0,
                opCode,
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operand,
                OperandFormat.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte[] opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandSize operandSize)
            : this(
                InstructionPrefixes.None,
                0,
                opCode,
                opCodeReg,
                mnemonic,
                operandSize,
                OperandFormat.None,
                OperandFormat.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte[] opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operand1,
            OperandFormat operand2)
            : this(
                InstructionPrefixes.None,
                0,
                opCode,
                opCodeReg,
                mnemonic,
                operandSize,
                operand1,
                operand2,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            byte[] opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operand1,
            OperandFormat operand2)
            : this(
                prefixes,
                0,
                opCode,
                opCodeReg,
                mnemonic,
                operandSize,
                operand1,
                operand2,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(RexPrefix rex, byte opCode, Instruction mnemonic)
            : this(
                InstructionPrefixes.None,
                RexPrefix.Magic | rex,
                new[] { opCode },
                255,
                mnemonic,
                OperandSize.Size32,
                OperandFormat.None,
                OperandFormat.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(RexPrefix rex, byte opCode, Instruction mnemonic, OperandFormat operand)
            : this(
                InstructionPrefixes.None,
                RexPrefix.Magic | rex,
                new[] { opCode },
                255,
                mnemonic,
                OperandSize.Size32,
                operand,
                OperandFormat.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            RexPrefix rex,
            byte opCode,
            Instruction mnemonic,
            OperandFormat operand1,
            OperandFormat operand2)
            : this(
                InstructionPrefixes.None,
                RexPrefix.Magic | rex,
                new[] { opCode },
                255,
                mnemonic,
                OperandSize.Size32,
                operand1,
                operand2,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            RexPrefix rex,
            byte opCode,
            Instruction mnemonic,
            OperandFormat operand)
            : this(
                prefixes,
                RexPrefix.Magic | rex,
                new[] { opCode },
                255,
                mnemonic,
                OperandSize.Size32,
                operand,
                OperandFormat.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            RexPrefix rex,
            byte opCode,
            Instruction mnemonic,
            OperandFormat operand1,
            OperandFormat operand2)
            : this(
                prefixes,
                RexPrefix.Magic | rex,
                new[] { opCode },
                255,
                mnemonic,
                OperandSize.Size32,
                operand1,
                operand2,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            RexPrefix rex,
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandFormat operand)
            : this(
                InstructionPrefixes.None,
                RexPrefix.Magic | rex,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operand,
                OperandFormat.None,
                Compatibility.NotEncodable32,
                Register.None)
        {
        }

        internal OpCodeProperties(
            RexPrefix rex,
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandFormat operand1,
            OperandFormat operand2)
            : this(
                InstructionPrefixes.None,
                RexPrefix.Magic | rex,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operand1,
                operand2,
                Compatibility.NotEncodable32,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            RexPrefix rex,
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandFormat operand)
            : this(
                prefixes,
                RexPrefix.Magic | rex,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operand,
                OperandFormat.None,
                Compatibility.NotEncodable32,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            RexPrefix rex,
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandFormat operand1,
            OperandFormat operand2)
            : this(
                prefixes,
                RexPrefix.Magic | rex,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operand1,
                operand2,
                Compatibility.NotEncodable32,
                Register.None)
        {
        }

        internal OpCodeProperties(RexPrefix rex, byte[] opCode, Instruction mnemonic, OperandFormat operand)
            : this(
                InstructionPrefixes.None,
                RexPrefix.Magic | rex,
                opCode,
                255,
                mnemonic,
                OperandSize.Size32,
                operand,
                OperandFormat.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(RexPrefix rex, byte[] opCode, Instruction mnemonic, OperandFormat operand1, OperandFormat operand2)
            : this(
                InstructionPrefixes.None,
                RexPrefix.Magic | rex,
                opCode,
                255,
                mnemonic,
                OperandSize.Size32,
                operand1,
                operand2,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            RexPrefix rex,
            byte[] opCode,
            Instruction mnemonic,
            OperandFormat operand1,
            OperandFormat operand2)
            : this(
                prefixes,
                RexPrefix.Magic | rex,
                opCode,
                255,
                mnemonic,
                OperandSize.Size32,
                operand1,
                operand2,
                Compatibility.NotEncodable32,
                Register.None)
        {
        }

        internal OpCodeProperties(RexPrefix rex, byte[] opCode, Instruction mnemonic, Register register)
            : this(
                InstructionPrefixes.None,
                RexPrefix.Magic | rex,
                opCode,
                255,
                mnemonic,
                OperandSize.Size32,
                OperandFormat.Register,
                OperandFormat.None,
                Compatibility.Valid,
                register)
        {
        }

        internal OpCodeProperties(
            RexPrefix rex,
            byte[] opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandFormat operand1,
            OperandFormat operand2)
            : this(
                InstructionPrefixes.None,
                RexPrefix.Magic | rex,
                opCode,
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operand1,
                operand2,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            RexPrefix rex,
            byte[] opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandFormat operand)
            : this(
                prefixes,
                RexPrefix.Magic | rex,
                opCode,
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operand,
                OperandFormat.None,
                Compatibility.NotEncodable32,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            RexPrefix rex,
            byte[] opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandFormat operand1,
            OperandFormat operand2)
            : this(
                prefixes,
                RexPrefix.Magic | rex,
                opCode,
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operand1,
                operand2,
                Compatibility.NotEncodable32,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            RexPrefix rex,
            byte[] opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operand1,
            OperandFormat operand2,
            Compatibility compatibility,
            Register register)
        {
            this.RexPrefix = rex;
            this.OperandSize = operandSize;
            this.OpCode = opCode;
            this.OpCodeReg = opCodeReg;
            this.Mnemonic = mnemonic;
            this.Operand1 = operand1;
            this.Operand2 = operand2;
            this.Prefixes = prefixes;
            this.Compatibility = compatibility;
            this.Register = register;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(this.Mnemonic);
            builder.Append(" (");

            if ((this.RexPrefix & RexPrefix.W) != 0)
            {
                builder.Append("Rex.W ");
            }

            var opCode = string.Join(" ", this.OpCode.Select(o => o.ToString("X2")));
            builder.Append(opCode);

            if (this.OpCodeReg != 255)
            {
                builder.Append("/" + this.OpCodeReg);
            }

            builder.Append(") ");

            if (this.Operand1 == OperandFormat.Register)
            {
                builder.Append(this.Register);
            }
            else if (this.Operand1 != OperandFormat.None)
            {
                builder.Append(this.Operand1);
            }

            if (this.Operand2 != OperandFormat.None)
            {
                builder.Append(", ");
                builder.Append(this.Operand2);
            }

            return builder.ToString();
        }
    }
}