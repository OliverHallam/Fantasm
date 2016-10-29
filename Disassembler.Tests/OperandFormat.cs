namespace Fantasm.Disassembler.Tests
{
    public enum OperandFormat
    {
        /// <summary>
        /// The instruction takes no operands.
        /// </summary>
        None,

        /// <summary>
        /// The operand is a single byte.
        /// </summary>
        Ib,

        /// <summary>
        /// The operand is a 16-bit relative address.
        /// </summary>
        Jw,

        /// <summary>
        /// The operand is a 32-bit relative address.
        /// </summary>
        Jd,

        /// <summary>
        /// The operand is a 32 bit pointer, with 16 bit code segment selector and a 16 bit offset.
        /// </summary>
        Aww,

        /// <summary>
        /// The operand is a 48 bit pointer, with 16 bit code segment selector and a 32 bit offset.
        /// </summary>
        Awd,

        /// <summary>
        /// The first (and only) operand is a memory address or 16-bit register specified by a ModRM byte,
        /// </summary>
        Ew,

        /// <summary>
        /// The first (and only) operand is a memory address or 32-bit register specified by a ModRM byte,
        /// </summary>
        Ed,

        /// <summary>
        /// The first (and only) operand is a memory address or 64-bit register specified by a ModRM byte,
        /// </summary>
        Eq,

        /// <summary>
        /// The first (and only) operand is a 16-bit memory address specified by a ModRM byte
        /// </summary>
        Mw,

        /// <summary>
        /// The first (and only) operand is a 32-bit memory address specified by a ModRM byte
        /// </summary>
        Md,

        /// <summary>
        /// The first (and only) operand is a 64-bit memory address specified by a ModRM byte
        /// </summary>
        Mq,

        /// <summary>
        /// The first (and only) operand is a 128-bit memory address specified by a ModRM byte
        /// </summary>
        Mdq,

        /// <summary>
        /// The operand is a register encoded in the instruction.
        /// </summary>
        Register,

        /// <summary>
        /// The first operand is the <c>AL</c> register and the second operand is an immediate byte.
        /// </summary>
        AL_Ib,

        /// <summary>
        /// The first operand is the <c>AX</c> register and the second operand is an immediate word.
        /// </summary>
        AX_Iw,

        /// <summary>
        /// The first operand is the <c>EAX</c> register and the second operand is an immediate dword.
        /// </summary>
        EAX_Id,

        /// <summary>
        /// The first operand is the <c>RAX</c> register and the second operand is an immediate dword.
        /// </summary>
        RAX_Id,
            
        /// <summary>
        /// The first operand is a memory address or 8-bit register specified by a ModRM byte, and the second
        /// operand is an immediate byte
        /// </summary>
        Eb_Ib,

        /// <summary>
        /// The first operand is a memory address or 8-bit register specified by a ModRM byte, and the second
        /// operand is an 8 bit register from the modR/M reg.
        /// </summary>
        Eb_Gb,

        /// <summary>
        /// The first operand is an 8 bit register from the modR/M reg field, and the second operand is a
        /// memory address or 8-bit register specified by a modR/M byte.
        /// </summary>
        Gb_Eb,

        /// <summary>
        /// The first operand is a memory address or 16-bit register specified by a ModRM byte, and the second
        /// operand is an immediate word.
        /// </summary>
        Ew_Iw,

        /// <summary>
        /// The first operand is a memory address or 16-bit register specified by a ModRM byte, and the second
        /// operand is an immediate byte.
        /// </summary>
        Ew_Ib,

        /// <summary>
        /// The first operand is a memory address or 16-bit register specified by a ModRM byte, and the second
        /// operand is a 16 bit register from the modR/M reg.
        /// </summary>
        Ew_Gw,

        /// <summary>
        /// The first operand is a 16 bit register from the modR/M reg field, and the second operand is a
        /// memory address or 16-bit register specified by a modR/M byte.
        /// </summary>
        Gw_Ew,

        /// <summary>
        /// The first operand is a memory address or 32-bit register specified by a ModRM byte, and the second
        /// operand is an immediate dword.
        /// </summary>
        Ed_Id,

        /// <summary>
        /// The first operand is a memory address or 32-bit register specified by a ModRM byte, and the second
        /// operand is an immediate byte.
        /// </summary>
        Ed_Ib,

        /// <summary>
        /// The first operand is a memory address or 32-bit register specified by a ModRM byte, and the second
        /// operand is a 32 bit register from the modR/M reg.
        /// </summary>
        Ed_Gd,

        /// <summary>
        /// The first operand is a 32 bit register from the modR/M reg field, and the second operand is a
        /// memory address or 8-bit register specified by a modR/M byte.
        /// </summary>
        Gd_Eb,

        /// <summary>
        /// The first operand is a 32 bit register from the modR/M reg field, and the second operand is a
        /// memory address or 16-bit register specified by a modR/M byte.
        /// </summary>
        Gd_Ew,

        /// <summary>
        /// The first operand is a 32 bit register from the modR/M reg field, and the second operand is a
        /// memory address or 32-bit register specified by a modR/M byte.
        /// </summary>
        Gd_Ed,

        /// <summary>
        /// The first operand is a 32 bit register from the modR/M reg field, and the second operand is a
        /// memory address or 64-bit register specified by a modR/M byte.
        /// </summary>
        Gd_Eq,

        /// <summary>
        /// The first operand is a memory address or 64-bit register specified by a ModRM byte, and the second
        /// operand is an immediate dword.
        /// </summary>
        Eq_Id,

        /// <summary>
        /// The first operand is a memory address or 64-bit register specified by a ModRM byte, and the second
        /// operand is an immediate byte.
        /// </summary>
        Eq_Ib,

        /// <summary>
        /// The first operand is a memory address or 64-bit register specified by a ModRM byte, and the second
        /// operand is a 64 bit register from the modR/M reg.
        /// </summary>
        Eq_Gq,

        /// <summary>
        /// The first operand is a 64 bit register from the modR/M reg field, and the second operand is a
        /// memory address or 64-bit register specified by a modR/M byte.
        /// </summary>
        Gq_Eq
    }
}