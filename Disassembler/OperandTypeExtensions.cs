namespace Fantasm.Disassembler
{
    /// <summary>
    /// Extension methods for the <see cref="OperandType"/> enumeration.
    /// </summary>
    internal static class OperandTypeExtensions
    {
        /// <summary>
        /// Returns a boolean value indicating whether the specified operand type is a memory access.
        /// </summary>
        /// <param name="operandType">
        /// The operand type.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the operand is a memory access; otherwise <see langword="false" />
        /// </returns>
        public static bool IsMemoryAccess(this OperandType operandType)
        {
            switch (operandType)
            {
                case OperandType.BytePointer:
                case OperandType.WordPointer:
                case OperandType.DwordPointer:
                case OperandType.QwordPointer:
                case OperandType.DqwordPointer:
                    return true;

                default:
                    return false;
            }
        }
    }
}
