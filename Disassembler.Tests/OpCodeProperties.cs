﻿using System.Linq;
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
        internal OperandFormat[] Operands;
        internal InstructionPrefixes Prefixes;
        internal Compatibility Compatibility;
        internal Register Register;

        internal OpCodeProperties(byte opCode, Instruction mnemonic, params OperandFormat[] operands)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                255,
                mnemonic,
                OperandSize.Size32,
                operands,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            Instruction mnemonic,
            Compatibility compatibility,
            params OperandFormat[] operands)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                255,
                mnemonic,
                OperandSize.Size32,
                operands,
                compatibility,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            byte opCode,
            Instruction mnemonic,
            params OperandFormat[] operands)
            : this(
                prefixes,
                0,
                new[] { opCode },
                255,
                mnemonic,
                OperandSize.Size32,
                operands,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(byte opCode, Instruction mnemonic, OperandSize operandSize, params OperandFormat[] operands)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                255,
                mnemonic,
                operandSize,
                operands,
                Compatibility.Valid,
                Register.None)
        {
        }
        
        internal OpCodeProperties(
            byte opCode,
            Instruction mnemonic,
            OperandSize operandSize,
            Compatibility compatibility,
            params OperandFormat[] operands)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                255,
                mnemonic,
                operandSize,
                operands,
                compatibility,
                Register.None)
        {
        }
        
        internal OpCodeProperties(
            byte opCode,
            Instruction mnemonic,
            OperandSize operandSize,
            Compatibility compatibility,
            Register register)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                255,
                mnemonic,
                operandSize,
                new [] { OperandFormat.Register },
                compatibility,
                register)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            byte opCode,
            Instruction mnemonic,
            OperandSize operandSize,
            params OperandFormat[] operands)
            : this(
                prefixes,
                0,
                new[] { opCode },
                255,
                mnemonic,
                operandSize,
                operands,
                Compatibility.Valid,
                Register.None)
        {
        }
        
        internal OpCodeProperties(byte opCode, byte opCodeReg, Instruction mnemonic, params OperandFormat[] operands)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operands,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            Compatibility compatibility,
            params OperandFormat[] operands)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operands,
                compatibility,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            params OperandFormat[] operands)
            : this(
                prefixes,
                0,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operands,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandSize operandSize,
            params OperandFormat[] operands)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                operandSize,
                operands,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandSize operandSize,
            Compatibility compatibility,
            params OperandFormat[] operands)
            : this(
                InstructionPrefixes.None,
                0,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                operandSize,
                operands,
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
            params OperandFormat[] operands)
            : this(
                prefixes,
                0,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                operandSize,
                operands,
                Compatibility.Valid,
                Register.None)
        {
        }
        

        internal OpCodeProperties(byte[] opCode, Instruction mnemonic, params OperandFormat[] operands)
            : this(
                InstructionPrefixes.None,
                0,
                opCode,
                255,
                mnemonic,
                OperandSize.Size32,
                operands,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            byte[] opCode,
            Instruction mnemonic,
            params OperandFormat[] operands)
            : this(
                prefixes,
                0,
                opCode,
                255,
                mnemonic,
                OperandSize.Size32,
                operands,
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
                new [] { OperandFormat.Register },
                Compatibility.Valid,
                register)
        {
        }

        internal OpCodeProperties(
            byte[] opCode,
            Instruction mnemonic,
            OperandSize operandSize,
            params OperandFormat[] operands)
            : this(
                InstructionPrefixes.None,
                0,
                opCode,
                255,
                mnemonic,
                operandSize,
                operands,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            byte[] opCode,
            Instruction mnemonic,
            OperandSize operandSize,
            params OperandFormat[] operands)
            : this(
                prefixes,
                0,
                opCode,
                255,
                mnemonic,
                operandSize,
                operands,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte[] opCode,
            byte opCodeReg,
            Instruction mnemonic,
            params OperandFormat[] operands)
            : this(
                InstructionPrefixes.None,
                0,
                opCode,
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operands,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            byte[] opCode,
            byte opCodeReg,
            Instruction mnemonic,
            params OperandFormat[] operands)
            : this(
                prefixes,
                0,
                opCode,
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operands,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte[] opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandSize operandSize,
            params OperandFormat[] operands)
            : this(
                InstructionPrefixes.None,
                0,
                opCode,
                opCodeReg,
                mnemonic,
                operandSize,
                operands,
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
            params OperandFormat[] operands)
            : this(
                prefixes,
                0,
                opCode,
                opCodeReg,
                mnemonic,
                operandSize,
                operands,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(RexPrefix rex, byte opCode, Instruction mnemonic, params OperandFormat[] operands)
            : this(
                InstructionPrefixes.None,
                RexPrefix.Magic | rex,
                new[] { opCode },
                255,
                mnemonic,
                OperandSize.Size32,
                operands,
                Compatibility.Valid,
                Register.None)
        {
        }
        
        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            RexPrefix rex,
            byte opCode,
            Instruction mnemonic,
            params OperandFormat[] operands)
            : this(
                prefixes,
                RexPrefix.Magic | rex,
                new[] { opCode },
                255,
                mnemonic,
                OperandSize.Size32,
                operands,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            RexPrefix rex,
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            params OperandFormat[] operands)
            : this(
                InstructionPrefixes.None,
                RexPrefix.Magic | rex,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operands,
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
            params OperandFormat[] operands)
            : this(
                prefixes,
                RexPrefix.Magic | rex,
                new[] { opCode },
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operands,
                Compatibility.NotEncodable32,
                Register.None)
        {
        }

        internal OpCodeProperties(RexPrefix rex, byte[] opCode, Instruction mnemonic, params OperandFormat[] operands)
            : this(
                InstructionPrefixes.None,
                RexPrefix.Magic | rex,
                opCode,
                255,
                mnemonic,
                OperandSize.Size32,
                operands,
                Compatibility.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            InstructionPrefixes prefixes,
            RexPrefix rex,
            byte[] opCode,
            Instruction mnemonic,
            params OperandFormat[] operands)
            : this(
                prefixes,
                RexPrefix.Magic | rex,
                opCode,
                255,
                mnemonic,
                OperandSize.Size32,
                operands,
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
                new [] { OperandFormat.Register },
                Compatibility.Valid,
                register)
        {
        }

        internal OpCodeProperties(
            RexPrefix rex,
            byte[] opCode,
            byte opCodeReg,
            Instruction mnemonic,
            params OperandFormat[] operands)
            : this(
                InstructionPrefixes.None,
                RexPrefix.Magic | rex,
                opCode,
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operands,
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
            params OperandFormat[] operands)
            : this(
                prefixes,
                RexPrefix.Magic | rex,
                opCode,
                opCodeReg,
                mnemonic,
                OperandSize.Size32,
                operands,
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
            OperandFormat[] operands,
            Compatibility compatibility,
            Register register)
        {
            this.RexPrefix = rex;
            this.OperandSize = operandSize;
            this.OpCode = opCode;
            this.OpCodeReg = opCodeReg;
            this.Mnemonic = mnemonic;
            this.Operands = operands;
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

            bool first = true;
            foreach (var operand in this.Operands)
            {
                if (!first)
                {
                    builder.Append(", ");
                }
                first = false;

                if (operand == OperandFormat.Register)
                {
                    builder.Append(this.Register);
                }
                else
                {
                    builder.Append(operand);
                }
            }

            return builder.ToString();
        }
    }
}