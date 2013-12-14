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
        /// The operand is the value of a register.
        /// </summary>
        Register,

        /// <summary>
        /// The operand is a memory access.  In this case the memory address accessed is
        /// <c>[BaseRegister + IndexRegister*Scale + Displacement]</c>.  The values in the formula can be retrieved
        /// from the
        /// </summary>
        Memory,
    }
}
