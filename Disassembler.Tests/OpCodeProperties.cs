using System;

namespace Fantasm.Disassembler.Tests
{
    public class OpCodeProperties
    {
        public OperandSize OperandSize;
        internal RexPrefix RexPrefix;
        public byte OpCode;
        public byte OpCodeReg;
        public Instruction Mnemonic;
        public OperandFormat Operands;
        internal InstructionPrefixes SupportedPrefixes;
        public Compatibility64 Compatibility64;

        internal OpCodeProperties(
            RexPrefix rex,
            OperandSize operandSize,
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandFormat operands,
            InstructionPrefixes supportedPrefixes,
            Compatibility64 compatibility64)
        {
            this.RexPrefix = rex;
            this.OperandSize = operandSize;
            this.OpCode = opCode;
            this.OpCodeReg = opCodeReg;
            this.Mnemonic = mnemonic;
            this.Operands = operands;
            this.SupportedPrefixes = supportedPrefixes;
            this.Compatibility64 = compatibility64;
        }

        internal OpCodeProperties(
            byte opCode,
            Instruction mnemonic,
            OperandFormat operands)
            : this(0, OperandSize.Size32, opCode, 255, mnemonic, operands, InstructionPrefixes.None, Compatibility64.Valid)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            Instruction mnemonic,
            OperandFormat operands,
            Compatibility64 compatibility64)
            : this(0, OperandSize.Size32, opCode, 255, mnemonic, operands, InstructionPrefixes.None, compatibility64)
        {
        }

        internal OpCodeProperties(byte opCode, Instruction mnemonic, OperandFormat operands, InstructionPrefixes prefixes)
            : this(
                0,
                OperandSize.Size32,
                opCode,
                255,
                mnemonic,
                operands,
                prefixes,
                Compatibility64.Valid)
        {
        }

        internal OpCodeProperties(byte opCode, Instruction mnemonic, OperandSize operandSize, OperandFormat operands, InstructionPrefixes prefixes)
            : this(
                0,
                operandSize,
                opCode,
                255,
                mnemonic,
                operands,
                prefixes,
                Compatibility64.Valid)
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
                opCode,
                opCodeMod,
                mnemonic,
                operands,
                supportedPrefixes,
                Compatibility64.Valid)
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
                opCode,
                opCodeMod,
                mnemonic,
                operands,
                supportedPrefixes,
                Compatibility64.Valid)
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
                opCode,
                opCodeMod,
                mnemonic,
                operands,
                supportedPrefixes,
                Compatibility64.Valid)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operands)
            : this(0, operandSize, opCode, 255, mnemonic, operands, InstructionPrefixes.None, Compatibility64.Valid)
        {
        }

        internal OpCodeProperties(RexPrefix rex, byte opCode, Instruction mnemonic, OperandFormat operands)
            : this(
                RexPrefix.Magic | rex,
                OperandSize.Size32,
                opCode,
                255,
                mnemonic,
                operands,
                InstructionPrefixes.None,
                Compatibility64.Valid)
        {
        }

        internal OpCodeProperties(RexPrefix rex, byte opCode, Instruction mnemonic, OperandFormat operands, InstructionPrefixes prefixes)
            : this(
                RexPrefix.Magic | rex,
                OperandSize.Size32,
                opCode,
                255,
                mnemonic,
                operands,
                prefixes,
                Compatibility64.Valid)
        {
        }

        public override string ToString()
        {
            if (this.OpCodeReg != 255)
                return String.Format("{0} ({1:X2}/{2}) {3}", this.Mnemonic, this.OpCode, this.OpCodeReg, this.Operands);
            else
                return String.Format("{0} ({1:X2}) {2}", this.Mnemonic, this.OpCode, this.Operands);
        }
    }
}