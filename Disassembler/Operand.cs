using System;

namespace Fantasm.Disassembler
{
    /// <summary>
    /// An operand to an x86 instruction.
    /// </summary>
    public struct Operand
    {
        /// <summary>
        /// The type of the operand.
        /// </summary>
        private readonly OperandType type;

        private readonly Register baseRegister;
        private readonly Register indexRegister;
        private readonly int scale;
        private readonly int displacement;

        /// <summary>
        /// Initializes a new instance of the <see cref="Operand"/> struct.
        /// </summary>
        /// <param name="type">The type of the operand.</param>
        /// <param name="baseRegister">The target register, or the base register in an address calculation.</param>
        /// <param name="indexRegister">The index register for an address calculation.</param>
        /// <param name="scale">The scale value in an address calculation, or the code segment in a far pointer.</param>
        /// <param name="displacement">The immediate value, a pointer, or the displacement in an address calculation.</param>
        private Operand(OperandType type, Register baseRegister, Register indexRegister, int scale, int displacement)
        {
            this.type = type;
            this.baseRegister = baseRegister;
            this.indexRegister = indexRegister;
            this.scale = scale;
            this.displacement = displacement;
        }

        /// <summary>
        /// The type of the operand.
        /// </summary>
        public OperandType Type => this.type;

        /// <summary>
        /// Gets the code segment selector from a far pointer operand.
        /// </summary>
        /// <returns>
        /// The selector for the code segment that the pointer resides in.
        /// </returns>
        public short GetSegmentSelector()
        {
            // we re-use the scale operand.
            return (short)this.scale;
        }

        /// <summary>
        ///     Gets the value of the immediate operand.
        /// </summary>
        /// <returns>
        ///     The value of the immediate operand, or <c>0</c> if there was none.
        /// </returns>
        /// <remarks>
        ///     This is the value of any operand with type <see cref="OperandType.ImmediateByte" />,
        ///     <see cref="OperandType.ImmediateWord" /> or <see cref="OperandType.ImmediateDword" />
        /// </remarks>
        public int GetImmediateValue()
        {
            return this.displacement;
        }

        /// <summary>
        /// Gets a register that is accessed directly.
        /// </summary>
        /// <returns>
        /// A member of the <see cref="Register" /> enumeration.
        /// </returns>
        public Register GetRegister()
        {
            return this.baseRegister;
        }

        /// <summary>
        /// Gets the base register of a memory access operand.
        /// </summary>
        /// <returns>
        /// A member of the <see cref="Register" /> enumeration.
        /// </returns>
        /// <remarks>
        /// This is <c>baseOperandSizeRegister</c> in the formula <c>[baseOperandSizeRegister + indexRegister*Scale + Displacement]</c>.
        /// </remarks>
        public Register GetBaseRegister()
        {
            return this.baseRegister;
        }

        /// <summary>
        /// Gets the index register of a memory access operand.
        /// </summary>
        /// <returns>
        /// A member of the <see cref="Register" /> enumeration.
        /// </returns>
        /// <remarks>
        /// This is <c>indexRegister</c> in the formula <c>[baseOperandSizeRegister + indexRegister*Scale + Displacement]</c>.
        /// </remarks>
        public Register GetIndexRegister()
        {
            return this.indexRegister;
        }

        /// <summary>
        /// Gets the scale parameter of a memory access operand.
        /// </summary>
        /// <returns>
        /// Either 1, 2, 4 or 8.
        /// </returns>
        /// <remarks>
        /// This is <c>Scale</c> in the formula <c>[baseOperandSizeRegister + indexRegister*Scale + Displacement]</c>.
        /// </remarks>
        public int GetScale()
        {
            return this.scale;
        }

        /// <summary>
        /// Gets the displacement parameter of a memory access operand, the value of a relative address operand, or
        /// the offset from a far pointer.
        /// </summary>
        /// <returns>
        /// The displacement, in bytes.
        /// </returns>
        /// <remarks>
        /// This is <c>Displacement</c> in the formula <c>[baseOperandSizeRegister + indexRegister*Scale + Displacement]</c>.
        /// </remarks>
        public int GetDisplacement()
        {
            return this.displacement;
        }

        /// <summary>
        /// Creates a far pointer operand.
        /// </summary>
        /// <param name="segment">The code segment.</param>
        /// <param name="address">The address.</param>
        /// <returns>
        /// A far pointer operand.
        /// </returns>
        public static Operand FarPointer(short segment, int address)
        {
            return new Operand(OperandType.FarPointer, Register.None, Register.None, segment, address);
        }

        /// <summary>
        /// Creates an immediate byte operand.
        /// </summary>
        /// <param name="value">The immediate value.</param>
        /// <returns>
        /// An immediate byte operand.
        /// </returns>
        public static Operand ImmediateByte(byte value)
        {
            return new Operand(OperandType.ImmediateByte, Register.None, Register.None, 0, value);
        }

        /// <summary>
        /// Creates an immediate word operand.
        /// </summary>
        /// <param name="value">The immediate value.</param>
        /// <returns>
        /// An immediate byte operand.
        /// </returns>
        public static Operand ImmediateWord(short value)
        {
            return new Operand(OperandType.ImmediateWord, Register.None, Register.None, 0, value);
        }

        /// <summary>
        /// Creates an immediate dword operand.
        /// </summary>
        /// <param name="value">The immediate value.</param>
        /// <returns>
        /// An immediate byte operand.
        /// </returns>
        public static Operand ImmediateDword(int value)
        {
            return new Operand(OperandType.ImmediateDword, Register.None, Register.None, 0, value);
        }

        /// <summary>
        /// Creates a register operand.
        /// </summary>
        /// <param name="register">The register.</param>
        /// <returns>
        /// A register operand.
        /// </returns>
        public static Operand DirectRegister(Register register)
        {
            return new Operand(OperandType.Register, register, Register.None, 0, 0);
        }

        /// <summary>
        /// Creates a memory access operand.
        /// </summary>
        /// <param name="size">The size of the operand.</param>
        /// <param name="baseRegister">The base register.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="indexRegister">The index register.</param>
        /// <param name="displacement">The displacement.</param>
        /// <returns>
        /// A memory access operand.
        /// </returns>
        public static Operand MemoryAccess(
            int size,
            Register baseRegister,
            int scale,
            Register indexRegister,
            int displacement)
        {
            return new Operand(GetPointerOperandType(size), baseRegister, indexRegister, scale, displacement);
        }

        /// <summary>
        /// Creates a relative address operand.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>
        /// A new relative address operand.
        /// </returns>
        public static Operand RelativeAddress(int offset)
        {
            return new Operand(OperandType.RelativeAddress, Register.None, Register.None, 0, offset);
        }

        /// <summary>
        /// Gets the pointer operand type correponding to the specified operand size.
        /// </summary>
        /// <param name="operandSize">The size of the operand.</param>
        /// <returns>
        /// The operand type.
        /// </returns>
        private static OperandType GetPointerOperandType(int operandSize)
        {
            switch (operandSize)
            {
                case 1:
                    return OperandType.BytePointer;
                case 2:
                    return OperandType.WordPointer;
                case 4:
                    return OperandType.DwordPointer;
                case 6:
                    return OperandType.FwordPointer;
                case 8:
                    return OperandType.QwordPointer;
                case 10:
                    return OperandType.TbytePointer;
                case 16:
                    return OperandType.OwordPointer;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operandSize));
            }
        }
    }
}
