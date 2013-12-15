namespace Fantasm.Disassembler
{
    /// <summary>
    /// Extension methods for the <see cref="ExecutionMode"/> enumeration.
    /// </summary>
    internal static class ExecutionModeExtensions
    {
        /// <summary>
        /// Returns a boolean value indicating whether the specified execution mode is valid.
        /// </summary>
        /// <param name="mode">The execution mode.</param>
        /// <returns>
        /// <see langword="true" /> if the specified execution mode is valid; otherwise <see langword="false" />.
        /// </returns>
        public static bool IsValid(this ExecutionMode mode)
        {
            return mode == ExecutionMode.CompatibilityMode || mode == ExecutionMode.Long64Bit;
        }

        /// <summary>
        /// Converts the specified <see cref="ExecutionMode" /> value to a <see cref="ExecutionModes"/> value that
        /// can be used to test against other <see cref="ExecutionModes"/> values.
        /// </summary>
        /// <param name="mode">The exection mode.</param>
        /// <returns>
        /// A member of the <see cref="ExecutionModes"/> enumeration.
        /// </returns>
        public static ExecutionModes ToExecutionModes(this ExecutionMode mode)
        {
            return (ExecutionModes)(1 << (int)mode);
        }
    }
}
