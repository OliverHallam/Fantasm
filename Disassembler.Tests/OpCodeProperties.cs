using System;
using System.Linq;
using System.Text;

namespace Fantasm.Disassembler.Tests
{
    public class OpCodeProperties
    {
        public OperandSize OperandSize;
        internal RexPrefix RexPrefix;
        public byte[] OpCode;
        public byte OpCodeReg;
        public Instruction Mnemonic;
        public OperandFormat Operands;
        internal InstructionPrefixes SupportedPrefixes;
        public Compatibility Compatibility;
        public Register Register;

        internal OpCodeProperties(
            RexPrefix rex,
            OperandSize operandSize,
            byte[] opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandFormat operands,
            InstructionPrefixes supportedPrefixes,
            Compatibility compatibility,
            Register register)
        {
            this.RexPrefix = rex;
            this.OperandSize = operandSize;
            this.OpCode = opCode;
            this.OpCodeReg = opCodeReg;
            this.Mnemonic = mnemonic;
            this.Operands = operands;
            this.SupportedPrefixes = supportedPrefixes;
            this.Compatibility = compatibility;
            this.Register = register;
        }

        internal OpCodeProperties(byte opCode, Instruction mnemonic, OperandFormat operands)
            : this(
                0,
                OperandSize.Size32,
                new[] { opCode },
                255,
                mnemonic,
                operands,
                InstructionPrefixes.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(byte[] opCode, Instruction mnemonic, Register register)
            : this(
                0,
                OperandSize.Size32,
                opCode,
                255,
                mnemonic,
                OperandFormat.Register,
                InstructionPrefixes.None,
                Compatibility.Valid,
                register)
        {
        }

        internal OpCodeProperties(RexPrefix rex, byte[] opCode, Instruction mnemonic, Register register)
            : this(
                RexPrefix.Magic | rex,
                OperandSize.Size32,
                opCode,
                255,
                mnemonic,
                OperandFormat.Register,
                InstructionPrefixes.None,
                Compatibility.Valid,
                register)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            Instruction mnemonic,
            OperandFormat operands,
            Compatibility compatibility)
            : this(
                0,
                OperandSize.Size32,
                new[] { opCode },
                255,
                mnemonic,
                operands,
                InstructionPrefixes.None,
                compatibility,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandFormat operands,
            Compatibility compatibility)
            : this(
                0,
                OperandSize.Size32,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                operands,
                InstructionPrefixes.None,
                compatibility,
                Register.None)
        {
        }

        internal OpCodeProperties(
            RexPrefix rex,
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandFormat operands)
            : this(
                rex,
                OperandSize.Size32,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                operands,
                InstructionPrefixes.None,
                Compatibility.NotEncodable32,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            Instruction mnemonic,
            OperandFormat operands,
            InstructionPrefixes prefixes)
            : this(
                0,
                OperandSize.Size32,
                new[] { opCode },
                255,
                mnemonic,
                operands,
                prefixes,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operands,
            InstructionPrefixes prefixes)
            : this(
                0,
                operandSize,
                new[] { opCode },
                255,
                mnemonic,
                operands,
                prefixes,
                Compatibility.Valid,
                Register.None)
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
                new[] { opCode },
                opCodeMod,
                mnemonic,
                operands,
                supportedPrefixes,
                Compatibility.Valid,
                Register.None)
        {
        }


        internal OpCodeProperties(
            byte opCode,
            byte opCodeMod,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operands,
            InstructionPrefixes supportedPrefixes)
            : this(
                0,
                operandSize,
                new[] { opCode },
                opCodeMod,
                mnemonic,
                operands,
                supportedPrefixes,
                Compatibility.Valid,
                Register.None)
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
                RexPrefix.Magic | rex,
                OperandSize.Size32,
                new[] { opCode },
                opCodeMod,
                mnemonic,
                operands,
                supportedPrefixes,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(byte opCode, Instruction mnemonic, OperandSize operandSize, OperandFormat operands)
            : this(
                0,
                operandSize,
                new[] { opCode },
                255,
                mnemonic,
                operands,
                InstructionPrefixes.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            byte opCodeMod,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operands)
            : this(
                0,
                operandSize,
                new[] { opCode },
                opCodeMod,
                mnemonic,
                operands,
                InstructionPrefixes.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operands,
            Compatibility compatibility)
            : this(
                0,
                operandSize,
                new[] { opCode },
                255,
                mnemonic,
                operands,
                InstructionPrefixes.None,
                compatibility,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operands,
            Compatibility compatibility)
            : this(
                0,
                operandSize,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                operands,
                InstructionPrefixes.None,
                compatibility,
                Register.None)
        {
        }

        internal OpCodeProperties(byte[] opCode, Instruction mnemonic, OperandSize operandSize, OperandFormat operands)
            : this(
                0,
                operandSize,
                opCode,
                255,
                mnemonic,
                operands,
                InstructionPrefixes.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte[] opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operands)
            : this(
                0,
                operandSize,
                opCode,
                opCodeReg,
                mnemonic,
                operands,
                InstructionPrefixes.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(RexPrefix rex, byte opCode, Instruction mnemonic, OperandFormat operands)
            : this(
                RexPrefix.Magic | rex,
                OperandSize.Size32,
                new[] { opCode },
                255,
                mnemonic,
                operands,
                InstructionPrefixes.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(RexPrefix rex, byte[] opCode, Instruction mnemonic, OperandFormat operands)
            : this(
                RexPrefix.Magic | rex,
                OperandSize.Size32,
                opCode,
                255,
                mnemonic,
                operands,
                InstructionPrefixes.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(RexPrefix rex, byte[] opCode, byte opCodeReg, Instruction mnemonic, OperandFormat operands)
            : this(
                RexPrefix.Magic | rex,
                OperandSize.Size32,
                opCode,
                opCodeReg,
                mnemonic,
                operands,
                InstructionPrefixes.None,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(RexPrefix rex, byte opCode, Instruction mnemonic, OperandFormat operands, InstructionPrefixes prefixes)
            : this(
                RexPrefix.Magic | rex,
                OperandSize.Size32,
                new[] { opCode },
                255,
                mnemonic,
                operands,
                prefixes,
                Compatibility.Valid,
                Register.None)
        {
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

            var operands = this.Operands == OperandFormat.Register ? this.Register.ToString() : this.Operands.ToString();
            builder.Append(operands);

            return builder.ToString();
        }
    }
}