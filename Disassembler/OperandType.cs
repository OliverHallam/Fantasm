namespace Fantasm.Disassembler
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
        /// The operand is an address, relative to the start of the next instruction.  The value of operand can be
        /// retrieved from the <see cref="Operand.GetDisplacement"/> method.
        /// </summary>
        RelativeAddress,

        /// <summary>
        /// The operand is a far pointer.   This consists of a code segment selector and an offset.   The code segment
        /// selector can be retrieved from the <see cref="Operand.GetSegmentSelector" /> method, and the
        /// offset can be retrieved from the <see cref="Operand.GetDisplacement"/> method.
        /// </summary>
        FarPointer,

        /// <summary>
        /// The operand is the value of a register, retrievable from the <see cref="Operand.GetBaseRegister" />
        /// property.
        /// </summary>
        Register,

        /// <summary>
        /// The operand is a memory address.  In this case the memory address accessed is
        /// <c>[Register + IndexRegister*Scale + Displacement]</c>.  The values in the formula can be retrieved
        /// from the <see cref="Operand.GetBaseRegister"/>, <see cref="Operand.GetIndexRegister"/>,
        /// <see cref="Operand.GetScale"/> and <see cref="Operand.GetDisplacement"/> methods.
        /// </summary>
        Address,

        /// <summary>
        /// The operand is a memory access referencing a 8-bit value.  In this case the memory address accessed is
        /// <c>[Register + IndexRegister*Scale + Displacement]</c>.  The values in the formula can be retrieved
        /// from the <see cref="Operand.GetBaseRegister"/>, <see cref="Operand.GetIndexRegister"/>,
        /// <see cref="Operand.GetScale"/> and <see cref="Operand.GetDisplacement"/> methods.
        /// </summary>
        BytePointer,

        /// <summary>
        /// The operand is a memory access referencing a 16-bit value.  In this case the memory address accessed is
        /// <c>[Register + IndexRegister*Scale + Displacement]</c>.  The values in the formula can be retrieved
        /// from the <see cref="Operand.GetBaseRegister"/>, <see cref="Operand.GetIndexRegister"/>,
        /// <see cref="Operand.GetScale"/> and <see cref="Operand.GetDisplacement"/> methods.
        /// </summary>
        WordPointer,

        /// <summary>
        /// The operand is a memory access referencing a 32-bit value.  In this case the memory address accessed is
        /// <c>[Register + IndexRegister*Scale + Displacement]</c>.  The values in the formula can be retrieved
        /// from the <see cref="Operand.GetBaseRegister"/>, <see cref="Operand.GetIndexRegister"/>,
        /// <see cref="Operand.GetScale"/> and <see cref="Operand.GetDisplacement"/> methods.
        /// </summary>
        DwordPointer,

        /// <summary>
        /// The operand is a memory access referencing a 48-bit value.  In this case the memory address accessed is
        /// <c>[Register + IndexRegister*Scale + Displacement]</c>.  The values in the formula can be retrieved
        /// from the <see cref="Operand.GetBaseRegister"/>, <see cref="Operand.GetIndexRegister"/>,
        /// <see cref="Operand.GetScale"/> and <see cref="Operand.GetDisplacement"/> methods.
        /// </summary>
        FwordPointer,

        /// <summary>
        /// The operand is a memory access referencing a 64-bit value.  In this case the memory address accessed is
        /// <c>[Register + IndexRegister*Scale + Displacement]</c>.  The values in the formula can be retrieved
        /// from the <see cref="Operand.GetBaseRegister"/>, <see cref="Operand.GetIndexRegister"/>,
        /// <see cref="Operand.GetScale"/> and <see cref="Operand.GetDisplacement"/> methods.
        /// </summary>
        QwordPointer,

        /// <summary>
        /// The operand is a memory access referencing a 80-bit value.  In this case the memory address accessed is
        /// <c>[Register + IndexRegister*Scale + Displacement]</c>.  The values in the formula can be retrieved
        /// from the <see cref="Operand.GetBaseRegister"/>, <see cref="Operand.GetIndexRegister"/>,
        /// <see cref="Operand.GetScale"/> and <see cref="Operand.GetDisplacement"/> methods.
        /// </summary>
        TbytePointer,

        /// <summary>
        /// The operand is a memory access referencing a 128-bit value.  In this case the memory address accessed is
        /// <c>[Register + IndexRegister*Scale + Displacement]</c>.  The values in the formula can be retrieved
        /// from the <see cref="Operand.GetBaseRegister"/>, <see cref="Operand.GetIndexRegister"/>,
        /// <see cref="Operand.GetScale"/> and <see cref="Operand.GetDisplacement"/> methods.
        /// </summary>
        OwordPointer,
    }
}
