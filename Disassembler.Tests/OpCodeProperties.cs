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
        internal ExecutionModes SupportedModes;

        internal OpCodeProperties(
            RexPrefix rex,
            OperandSize operandSize,
            byte opCode,
            byte opCodeReg,
            Instruction mnemonic,
            OperandFormat operands,
            InstructionPrefixes supportedPrefixes,
            ExecutionModes supportedModes)
        {
            this.RexPrefix = rex;
            this.OperandSize = operandSize;
            this.OpCode = opCode;
            this.OpCodeReg = opCodeReg;
            this.Mnemonic = mnemonic;
            this.Operands = operands;
            this.SupportedPrefixes = supportedPrefixes;
            this.SupportedModes = supportedModes;
        }

        internal OpCodeProperties(
            byte opCode,
            Instruction mnemonic,
            OperandFormat operands,
            ExecutionModes supportedModes)
            : this(0, OperandSize.Size32, opCode, 255, mnemonic, operands, InstructionPrefixes.None, supportedModes)
        {
        }

        internal OpCodeProperties(byte opCode, Instruction mnemonic, OperandFormat operands)
            : this(
                0,
                OperandSize.Size32,
                opCode,
                255,
                mnemonic,
                operands,
                InstructionPrefixes.None,
                ExecutionModes.All)
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
                ExecutionModes.All)
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
                ExecutionModes.All)
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
                ExecutionModes.All)
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
                ExecutionModes.All)
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
                ExecutionModes.All)
        {
        }

        internal OpCodeProperties(
            byte opCode,
            Instruction mnemonic,
            OperandSize operandSize,
            OperandFormat operands)
            : this(0, operandSize, opCode, 255, mnemonic, operands, InstructionPrefixes.None, ExecutionModes.All)
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
                ExecutionModes.Long64Bit)
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
                ExecutionModes.Long64Bit)
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