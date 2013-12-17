using System;
using System.Linq;

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
        public Compatibility64 Compatibility64;
        public Register Register;

        internal OpCodeProperties(
            RexPrefix rex,
            OperandSize operandSize,
            byte[] opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandFormat operands,
            InstructionPrefixes supportedPrefixes,
            Compatibility64 compatibility64,
            Register register)
        {
            this.RexPrefix = rex;
            this.OperandSize = operandSize;
            this.OpCode = opCode;
            this.OpCodeReg = opCodeReg;
            this.Mnemonic = mnemonic;
            this.Operands = operands;
            this.SupportedPrefixes = supportedPrefixes;
            this.Compatibility64 = compatibility64;
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
                Compatibility64.Valid,
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
                Compatibility64.Valid,
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
                Compatibility64.Valid,
                register)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            Instruction mnemonic,
            OperandFormat operands,
            Compatibility64 compatibility64)
            : this(
                0,
                OperandSize.Size32,
                new[] { opCode },
                255,
                mnemonic,
                operands,
                InstructionPrefixes.None,
                compatibility64,
                Register.None)
        {
        }

        internal OpCodeProperties(byte opCode, Instruction mnemonic, OperandFormat operands, InstructionPrefixes prefixes)
            : this(
                0,
                OperandSize.Size32,
                new[] { opCode },
                255,
                mnemonic,
                operands,
                prefixes,
                Compatibility64.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(byte opCode, Instruction mnemonic, OperandSize operandSize, OperandFormat operands, InstructionPrefixes prefixes)
            : this(
                0,
                operandSize,
                new[] { opCode },
                255,
                mnemonic,
                operands,
                prefixes,
                Compatibility64.Valid,
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
                Compatibility64.Valid,
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
                Compatibility64.Valid,
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
                Compatibility64.Valid,
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
                Compatibility64.Valid,
                Register.None)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operands,
            Compatibility64 compatibility64)
            : this(
                0,
                operandSize,
                new[] { opCode },
                255,
                mnemonic,
                operands,
                InstructionPrefixes.None,
                compatibility64,
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
                Compatibility64.Valid,
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
                Compatibility64.Valid,
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
                Compatibility64.Valid,
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
                Compatibility64.Valid,
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
                Compatibility64.Valid,
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
                Compatibility64.Valid,
                Register.None)
        {
        }

        public override string ToString()
        {
            var opCode = string.Join(" ", this.OpCode.Select(o => o.ToString("X2")));
            if (this.OpCodeReg != 255)
            {
                opCode += "/" + this.OpCodeReg;
            }

            var operands = this.Operands == OperandFormat.Register ? this.Register.ToString() : this.Operands.ToString();
            return String.Format("{0} ({1}) {2}", this.Mnemonic, opCode, operands);
        }
    }
}