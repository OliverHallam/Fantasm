using System;

namespace Fantasm.Disassembler
{
    /// <summary>
    /// A bitfield version of the <see cref="ExecutionMode"/> enumeration.
    /// </summary>
    [Flags]
    internal enum ExecutionModes
    {
        /// <summary>
        /// Compatibility mode.
        /// </summary>
        CompatibilityMode = 1,

        /// <summary>
        /// 64-bit mode.
        /// </summary>
        Long64Bit = 2,
        
        /// <summary>
        /// All execution modes.
        /// </summary>
        All = CompatibilityMode | Long64Bit
    }
}
