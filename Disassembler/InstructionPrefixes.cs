using System;

namespace Fantasm.Disassembler
{
    [Flags]
    internal enum InstructionPrefixes
    {
        /// <summary>
        /// No instruction prefixes are set.
        /// </summary>
        None,

        /// <summary>
        /// The <c>LOCK</c> prefix.
        /// </summary>
        Lock = 0x0001,
        
        /// <summary>
        /// The <c>REPNE</c> prefix.
        /// </summary>
        /// <remarks>
        /// This is a synonym for <c>REPNZ</c>
        /// </remarks>
        RepNE = 0x0002,

        /// <summary>
        /// The <c>REP</c> prefix.
        /// </summary>
        /// <remarks>
        /// This is a synonym for <c>REPE</c> and <c>REPZ</c>.
        /// </remarks>
        Rep = 0x0004,

        /// <summary>
        /// The <c>REPNZ</c> prefix.
        /// </summary>
        /// <remarks>
        /// This is a synonym for <c>REPNE</c>
        /// </remarks>
        RepNZ = RepNE,

        /// <summary>
        /// The <c>REPE</c> prefix.
        /// </summary>
        /// <remarks>
        /// This is a synonym for <c>REP</c> and <c>REPZ</c>.
        /// </remarks>
        RepE = Rep,

        /// <summary>
        /// The <c>REPZ</c> prefix.
        /// </summary>
        /// <remarks>
        /// This is a synonym for <c>REP</c> and <c>REPE</c>.
        /// </remarks>
        RepZ = Rep,

        /// <summary>
        /// The CS segment override prefix.
        /// </summary>
        SegmentCS = 0x0008,

        /// <summary>
        /// The SS segment override prefix.
        /// </summary>
        SegmentSS = 0x0010,

        /// <summary>
        /// The DS segment override prefix.
        /// </summary>
        /// <remarks>
        /// This is a synonym for the "Branch taken" branch hint.
        /// </remarks>
        SegmentDS = 0x0020,

        /// <summary>
        /// The ES segment override prefix.
        /// </summary>
        /// <remarks>
        /// This is a synonym for the "Branch not taken" branch hint.
        /// </remarks>
        SegmentES = 0x0040,

        /// <summary>
        /// The FS segment override prefix.
        /// </summary>
        SegmentFS = 0x0080,

        /// <summary>
        /// The GS segment override prefix.
        /// </summary>
        SegmentGS = 0x0100,

        /// <summary>
        /// The "Branch not taken" branch hint.
        /// </summary>
        /// <remarks>
        /// This is a synonym for the ES segment override prefix
        /// </remarks>
        BranchNotTaken = SegmentES,

        /// <summary>
        /// The "Branch not taken" branch hint.
        /// </summary>
        BranchTaken = SegmentDS,

        /// <summary>
        /// The operand-size override prefix.
        /// </summary>
        OperandSizeOverride = 0x0200,

        /// <summary>
        /// The address-size override prefix.
        /// </summary>
        AddressSizeOverride = 0x0400,
        
        /// <summary>
        /// A bit mask for the Group 1 prefixes.  Only one Group 1 prefix may be specified for each instruction.
        /// Group 1 contains the lock and repeat prefixes.
        /// </summary>
        Group1Mask = 0x0007,

        /// <summary>
        /// A bit mask for the Group 2 prefixes.  Only one Group 2 prefix may be specified for each instruction.
        /// Group 2 contains the segment override prefixes.
        /// </summary>
        Group2Mask = 0x01F8,

        /// <summary>
        /// A bit mask for the Group 3 prefixes.  Only one Group 3 prefix may be specified for each instruction.
        /// Group 3 contains the operand-size override prefix.
        /// </summary>
        Group3Mask = 0x0200,

        /// <summary>
        /// A bit mask for the Group 4 prefixes.  Only one Group 4 prefix may be specified for each instruction.
        /// Group 4 contains the address-size override prefix.
        /// </summary>
        Group4Mask = 0x0400,
    }
}
