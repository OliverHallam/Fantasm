﻿namespace Fantasm.Disassembler
{
    /// <summary>
    /// The type of an operand.
    /// </summary>
    public enum OperandType
    {
        /// <summary>
        /// The type of the operand is unknown.
        /// </summary>
        None,

        /// <summary>
        /// The operand is an 8-bit immediate value.
        /// </summary>
        ImmediateByte,

        /// <summary>
        /// The operand is a 16-bit immediate value.
        /// </summary>
        ImmediateWord,

        /// <summary>
        /// The operand is a 32-bit immediate value.
        /// </summary>
        ImmediateDword,

        /// <summary>
        /// The operand is the value of a register in place of a memory access, retrievable from the
        /// <see cref="InstructionReader.GetBaseRegister" /> property.
        /// </summary>
        DirectRegister,

        /// <summary>
        /// The operand is the value of a register, retrievable from the <see cref="InstructionReader.GetRegister" /> property.
        /// </summary>
        Register,

        /// <summary>
        /// The operand is a memory access.  In this case the memory address accessed is
        /// <c>[DirectRegister + IndexRegister*Scale + Displacement]</c>.  The values in the formula can be retrieved
        /// from the <see cref="InstructionReader.GetBaseRegister"/>, <see cref="InstructionReader.GetIndexRegister"/>,
        /// <see cref="InstructionReader.GetScale"/> and <see cref="InstructionReader.GetDisplacement"/> methods.
        /// </summary>
        Memory,

        /// <summary>
        /// The operand is an address, relative to the start of the next instruction.  The value of operand can be
        /// retrieved from the <see cref="InstructionReader.GetDisplacement"/> method.
        /// </summary>
        RelativeAddress,

        /// <summary>
        /// The operand is a far pointer.   This consists of a code segment selector and an offset.   The code segment
        /// selector can be retrieved from the <see cref="InstructionReader.GetSegmentSelector" /> method, and the
        /// offset can be retrieved from the <see cref="InstructionReader.GetDisplacement"/> method.
        /// </summary>
        FarPointer
    }
}
