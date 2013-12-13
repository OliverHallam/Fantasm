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
        /// The operand is an immediate byte.
        /// </summary>
        ImmediateByte,
    }
}
