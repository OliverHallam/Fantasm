using System;

namespace Fantasm.Disassembler
{
    /// <summary>
    /// The possible execution modes that affect instruction parsing.
    /// </summary>
    /// <remarks>
    /// For more information about execution modes, see Section 3.1.1 of Intel® 64 and IA-32 Architectures Software
    /// Developer’s Manual Volume 1: Basic Architecture
    /// </remarks>
    /// <seealso cref="http://www.intel.com/content/dam/www/public/us/en/documents/manuals/64-ia-32-architectures-software-developer-vol-1-manual.pdf">
    /// Intel® 64 and IA-32 Architectures Software Developer’s Manual Volume 1: Basic Architecture
    /// </seealso>
    [Flags]
    public enum ExecutionMode
    {
        /// <summary>
        /// Compatibility mode.
        /// </summary>
        CompatibilityMode,

        /// <summary>
        /// 64-bit mode.
        /// </summary>
        Long64Bit
    }
}
