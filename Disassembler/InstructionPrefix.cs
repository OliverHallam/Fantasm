namespace Fantasm.Disassembler
{
    /// <summary>
    /// The IA-32 instruction prefixes.
    /// </summary>
    /// <remarks>
    /// These are defined in section 2.1.1 of <c>Intel® 64 and IA-32 Architectures Software Developer's Manual Volume 2A: Instruction Set Reference, A-M</c>.
    /// </remarks>
    /// <seealso href="http://www.intel.com/content/dam/www/public/us/en/documents/manuals/64-ia-32-architectures-software-developer-vol-2a-manual.pdf">
    /// Intel® 64 and IA-32 Architectures Software Developer's Manual Volume 2A: Instruction Set Reference, A-M
    /// </seealso>
    internal enum InstructionPrefix : byte
    {
        /// <summary>
        /// The <c>LOCK</c> prefix.
        /// </summary>
        Lock = 0xF0,

        /// <summary>
        /// The <c>REPNE</c> prefix.
        /// </summary>
        RepNE = 0xF2,

        /// <summary>
        /// The <c>REPNZ</c> prefix.
        /// </summary>
        RepNZ = 0xF2,

        /// <summary>
        /// The <c>REP</c> prefix.
        /// </summary>
        Rep = 0xF3,

        /// <summary>
        /// The <c>REPE</c> prefix.
        /// </summary>
        RepE = 0xF3,

        /// <summary>
        /// The <c>REPZ</c> prefix.
        /// </summary>
        RepZ = 0xF3,

        /// <summary>
        /// The CS Segment override prefix.
        /// </summary>
        SegmentCS = 0x2E,

        /// <summary>
        /// The SS Segment override prefix.
        /// </summary>
        SegmentSS = 0x36,

        /// <summary>
        /// The DS Segment override prefix.
        /// </summary>
        SegmentDS = 0x3E,

        /// <summary>
        /// The ES Segment override prefix.
        /// </summary>
        SegmentES = 0x26,

        /// <summary>
        /// The FS Segment override prefix.
        /// </summary>
        SegmentFS = 0x64,

        /// <summary>
        /// The GS Segment override prefix.
        /// </summary>
        SegmentGS = 0x65,

        /// <summary>
        /// The "Branch not taken" branch hint.
        /// </summary>
        BranchNotTaken = 0x2E,

        /// <summary>
        /// The "Branch taken" branch hint.
        /// </summary>
        BranchTaken = 0x3E,

        /// <summary>
        /// The operand-size override prefix.
        /// </summary>
        OperandSizeOverride = 0x66,

        /// <summary>
        /// The address-size override prefix.
        /// </summary>
        AddressSizeOverride = 0x67
    }
}
