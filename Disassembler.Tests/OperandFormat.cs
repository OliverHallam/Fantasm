namespace Fantasm.Disassembler.Tests
{
    public enum OperandFormat
    {
        /// <summary>
        /// The operand is not present
        /// </summary>
        None,

        /// <summary>
        /// The operand is a single immedate byte.
        /// </summary>
        Ib,

        /// <summary>
        /// The operand is a 16-bit immediate value.
        /// </summary>
        Iw,

        /// <summary>
        /// The operand is a 32-bit immediate value.
        /// </summary>
        Id,

        /// <summary>
        /// The operand is an 8-bit relative address.
        /// </summary>
        Jb,

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
        /// The operand is a memory address or 8-bit register specified by a ModRM byte,
        /// </summary>
        Eb,

        /// <summary>
        /// The operand is a memory address or 16-bit register specified by a ModRM byte,
        /// </summary>
        Ew,

        /// <summary>
        /// The operand is a memory address or 32-bit register specified by a ModRM byte,
        /// </summary>
        Ed,

        /// <summary>
        /// The operand is a memory address or 64-bit register specified by a ModRM byte,
        /// </summary>
        Eq,

        /// <summary>
        /// The operand is an 8-bit memory address specified by a ModRM byte
        /// </summary>
        Mb,

        /// <summary>
        /// The operand is a 16-bit memory address specified by a ModRM byte
        /// </summary>
        Mw,

        /// <summary>
        /// The operand is a 32-bit memory address specified by a ModRM byte
        /// </summary>
        Md,

        /// <summary>
        /// The operand is a 48-bit memory address specified by a ModRM byte
        /// </summary>
        Mf,

        /// <summary>
        /// The operand is a 64-bit memory address specified by a ModRM byte
        /// </summary>
        Mq,

        /// <summary>
        /// The operand is a 80-bit memory address specified by a ModRM byte
        /// </summary>
        Mt,

        /// <summary>
        /// The operand is a 128-bit memory address specified by a ModRM byte
        /// </summary>
        Mdq,

        /// <summary>
        /// The operand is a register encoded in the instruction.
        /// </summary>
        Register,

        /// <summary>
        /// The is the <c>AL</c> register.
        /// </summary>
        AL,

        /// <summary>
        /// The operand is the <c>AX</c> register.
        /// </summary>
        AX,

        /// <summary>
        /// The operand is the <c>EAX</c> register.
        /// </summary>
        EAX,

        /// <summary>
        /// The operand is the <c>RAX</c> register.
        /// </summary>
        RAX,

        /// <summary>
        /// The operand is the <c>DX</c> register.
        /// </summary>
        DX,
        
        /// <summary>
        /// The operand is an 8 bit register from the modR/M reg field.
        /// </summary>
        Gb,

        /// <summary>
        /// The operand is a 16 bit register from the modR/M reg field.
        /// </summary>
        Gw,

        /// <summary>
        /// The operand is a 32 bit register from the modR/M reg field.
        /// </summary>
        Gd,

        /// <summary>
        /// The operand is a 64 bit register from the modR/M reg field.
        /// </summary>
        Gq,

        /// <summary>
        /// The operand is the integer 3 (for INT 3)
        /// </summary>
        Three
    }
}