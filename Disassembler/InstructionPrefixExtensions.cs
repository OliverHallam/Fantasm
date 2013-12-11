namespace Fantasm.Disassembler
{
    /// <summary>
    /// Extension methods for the <see cref="InstructionPrefix"/> enumeration.
    /// </summary>
    internal static class InstructionPrefixExtensions
    {
        /// <summary>
        /// Returns a boolean value indicating whether the specified instruction prefix is a Group 1 prefix.  Group 1
        /// contains the lock and repeat prefixes.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="prefix"/> is a Group 1 prefix; otherwise <see langword="false" />
        /// </returns>
        public static bool IsGroup1(this InstructionPrefix prefix)
        {
            return prefix == InstructionPrefix.Lock || prefix == InstructionPrefix.Rep
                   || prefix == InstructionPrefix.RepNZ;
        }

        /// <summary>
        /// Returns a boolean value indicating whether the specified instruction prefix is a Group 2 prefix.  Group 2
        /// contains the segement override prefixes.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="prefix"/> is a Group 2 prefix; otherwise <see langword="false" />
        /// </returns>
        public static bool IsGroup2(this InstructionPrefix prefix)
        {
            return prefix == InstructionPrefix.SegmentCS || prefix == InstructionPrefix.SegmentSS
                   || prefix == InstructionPrefix.SegmentDS || prefix == InstructionPrefix.SegmentES
                   || prefix == InstructionPrefix.SegmentFS || prefix == InstructionPrefix.SegmentGS;
        }

        /// <summary>
        /// Returns a boolean value indicating whether the specified instruction prefix is a Group 3 prefix.  Group 3
        /// contains the operand-size override prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="prefix"/> is a Group 3 prefix; otherwise <see langword="false" />
        /// </returns>
        public static bool IsGroup3(this InstructionPrefix prefix)
        {
            return prefix == InstructionPrefix.OperandSizeOverride;
        }

        /// <summary>
        /// Returns a boolean value indicating whether the specified instruction prefix is a Group 4 prefix.  Group 4
        /// contains the address-size override prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="prefix"/> is a Group 3 prefix; otherwise <see langword="false" />
        /// </returns>
        public static bool IsGroup4(this InstructionPrefix prefix)
        {
            return prefix == InstructionPrefix.AddressSizeOverride;
        }
    }
}
