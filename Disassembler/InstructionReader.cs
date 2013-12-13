﻿using System;
using System.IO;

namespace Fantasm.Disassembler
{
    /// <summary>
    /// A class used to read a stream of X86 assembly instructions.
    /// </summary>
    public class InstructionReader
    {
        private struct Operand
        {
            public OperandType Type;
            public int Value;
        }

        private readonly PeekStream stream;
        private readonly ExecutionModes executionMode;

        private InstructionPrefix group1 = InstructionPrefix.None;
        private InstructionPrefix group2 = InstructionPrefix.None;
        private InstructionPrefix group3 = InstructionPrefix.None;
        private InstructionPrefix group4 = InstructionPrefix.None;
        private Instruction instruction = Instruction.Unknown;
        private Operand operand1;
        private int operandCount;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="InstructionReader"/> class.
        /// </summary>
        /// <param name="stream">The stream to read the instruction data from.</param>
        /// <param name="executionMode">The execution mode.</param>
        public InstructionReader(Stream stream, ExecutionMode executionMode)
        {
            this.stream = new PeekStream(stream);
            this.executionMode = executionMode.ToExecutionModes();
        }

        /// <summary>
        /// Gets the Group 1 instruction prefix (if any) associated with the current instruction.  Group 1 contains the
        /// lock and repeat prefixes.
        /// </summary>
        public InstructionPrefix Group1Prefix
        {
            get
            {
                return this.group1;
            }
        }

        /// <summary>
        /// Gets the Group 2 prefix (if any) associated with the current instruction.  Group 2 contains the segement
        /// override prefixes.
        /// </summary>
        public InstructionPrefix Group2Prefix
        {
            get
            {
                return this.group2;
            }
        }

        /// <summary>
        /// Gets the Group 3 prefix (if any) associated with the current instruction.  Group 3 contains the operand-
        /// size override prefix.
        /// </summary>
        public InstructionPrefix Group3Prefix
        {
            get
            {
                return this.group3;
            }
        }

        /// <summary>
        /// Gets the Group 4 prefix (if any) associated with the current instruction.  Group 4 contains the address-
        /// size override prefix.
        /// </summary>
        public InstructionPrefix Group4Prefix
        {
            get
            {
                return this.group4;
            }
        }

        /// <summary>
        /// Gets the current instruction.
        /// </summary>
        public Instruction Instruction
        {
            get
            {
                return this.instruction;
            }
        }

        /// <summary>
        /// Gets the number of operands for the current instruction.
        /// </summary>
        public int OperandCount
        {
            get
            {
                return this.operandCount;
            }
        }

        /// <summary>
        /// Gets the value of an immediate byte operand.
        /// </summary>
        /// <param name="index">The index of the operand</param>
        /// <returns></returns>
        public byte GetOperandByte(int index)
        {
            if (index < 0 || index >= this.operandCount)
                throw new ArgumentException("The index is not valid for this instruction", "index");

            return (byte)this.operand1.Value;
        }

        /// <summary>
        /// Attempts to read the next instruction from the stream.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if there are more isntructions; <see langword="false" /> if the reader has reached
        /// the end of the stream.
        /// </returns>
        public bool Read()
        {
            this.group1 = this.group2 = this.group3 = this.group4 = InstructionPrefix.None;

            while (true)
            {
                var nextByte = this.stream.Read();
                switch (nextByte)
                {
                    case -1:
                        return false;

                    case 0xF0:
                    case 0xF2:
                    case 0xF3:
                        SetPrefix(ref this.group1, (InstructionPrefix)nextByte);
                        continue;

                    case 0x26:
                    case 0x2E:
                    case 0x36:
                    case 0x3E:
                    case 0x64:
                    case 0x65:
                        SetPrefix(ref this.group2, (InstructionPrefix)nextByte);
                        continue;

                    case 0x66:
                        SetPrefix(ref this.group3, (InstructionPrefix)nextByte);
                        continue;

                    case 0x67:
                        SetPrefix(ref this.group4, (InstructionPrefix)nextByte);
                        continue;

                    case 0x37:
                        return this.ReadInstructionNoOperands(Instruction.Aaa, ExecutionModes.CompatibilityMode);

                    case 0x3F:
                        return this.ReadInstructionNoOperands(Instruction.Aas, ExecutionModes.CompatibilityMode);
                        
                    case 0x90:
                        return this.ReadInstructionNoOperands(Instruction.Nop, ExecutionModes.All);

                    case 0xD4:
                        return this.ReadAsciiAdjustWithBase(Instruction.Aam);

                    case 0xD5:
                        return this.ReadAsciiAdjustWithBase(Instruction.Aad);

                    default:
                        this.instruction = Instruction.Unknown;
                        this.operandCount = 0;
                        throw new NotImplementedException();
                }
            }
        }

        private void SetPrefix(ref InstructionPrefix currentPrefixForGroup, InstructionPrefix prefix)
        {
            if (currentPrefixForGroup != InstructionPrefix.None)
            {
                throw InvalidInstructionBytes();
            }

            currentPrefixForGroup = prefix;
        }

        /// <summary>
        /// Reads an instruction that takes no operands.
        /// </summary>
        /// <param name="instruction">The instruction mnemonic.</param>
        /// <param name="executionModes">The execution modes that are supported for the instruction.</param>
        /// <returns>
        /// Always returns <see langword="true" />
        /// </returns>
        private bool ReadInstructionNoOperands(Instruction instruction, ExecutionModes executionModes)
        {
            this.ReadInstruction(instruction, executionModes);

            this.operandCount = 0;
            return true;
        }

        private bool ReadAsciiAdjustWithBase(Instruction instruction)
        {
            this.ReadInstruction(instruction, ExecutionModes.CompatibilityMode);
            this.operandCount = 1;
            this.ReadImmediateByte(ref this.operand1);
            // special case: AAD is a synonym for AAD 0AH
            if (this.operand1.Value == 0x0A)
            {
                this.operandCount = 0;
            }
            return true;
        }

        private void ReadInstruction(Instruction instruction, ExecutionModes executionModes)
        {
            if (this.group1 == InstructionPrefix.Lock)
            {
                throw InvalidInstructionBytes();
            }

            if ((this.executionMode & executionModes) == 0)
            {
                throw InvalidInstructionBytes();
            }

            this.instruction = instruction;
        }

        private void ReadImmediateByte(ref Operand operand)
        {
            var value = this.stream.Read();
            if (value < 0)
            {
                throw InvalidInstructionBytes();
            }

            operand.Type = OperandType.ImmediateByte;
            operand.Value = value;
        }

        private static Exception InvalidInstructionBytes()
        {
            return new FormatException("The data does not represent a valid instruction.");
        }
    }
}
