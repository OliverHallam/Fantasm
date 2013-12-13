using System;

namespace Fantasm.Disassembler
{
    /// <summary>
    /// An enumeration which defines the bits in a REX prefix
    /// </summary>
    [Flags]
    internal enum RexPrefix : byte
    {
        /// <summary>
        /// The bits that must always be set.
        /// </summary>
        Magic = 0x40,
        
        /// <summary>
        /// The REX.W flag which overrides the operand size to 64 bit.
        /// </summary>
        W = 0x08,

        /// <summary>
        /// The REX.R flag which extends the ModR/M reg field.
        /// </summary>
        R = 0x04,

        /// <summary>
        /// The REX.X flag which extends the SIB index field.
        /// </summary>
        X = 0x02,

        /// <summary>
        /// The REX.B flag which extend the ModR/M r/m field, SIB base field or Opcode reg field.
        /// </summary>
        B = 0x01
    }
}
