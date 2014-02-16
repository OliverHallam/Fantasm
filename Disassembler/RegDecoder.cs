namespace Fantasm.Disassembler
{
    /// <summary>
    /// Methods that decode the reg bits in a <c>modrm</c> or opcode byte to a specific Register.
    /// </summary>
    internal static class RegDecoder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the register associated with the specified reg bits.
        /// </summary>
        /// <param name="hasRexPrefix">
        /// A boolean value indicating whether the instruction has a rex prefix, regardless of the value of the prefix.
        /// </param>
        /// <param name="reg">
        /// The 3 reg bits, optionally extended by a 4th bit from the rex prefix.  This is a value between 0 and 15.
        /// </param>
        /// <param name="baseRegister">
        /// The base register.  This is the equivalent of the EAX register with the correct width.
        /// </param>
        /// <returns>
        /// A member of the <see cref="Register"/> enumeration.
        /// </returns>
        public static Register GetRegister(bool hasRexPrefix, int reg, Register baseRegister)
        {
            if (reg >= 8)
            {
                return baseRegister + reg;
            }
            if (reg < 4)
            {
                return baseRegister + GetABCDRegisterOffset(reg);
            }
            if (baseRegister == Register.Al && !hasRexPrefix)
            {
                return Register.Ah + GetABCDRegisterOffset(reg - 4);
            }
            return baseRegister + GetDiSiBpSpRegisterOffset(reg);
        }

        #endregion

        #region Methods

        private static int GetABCDRegisterOffset(int reg)
        {
            switch (reg)
            {
                default:
                    // AX
                    return 0;
                case 1:
                    // CX
                    return 2;
                case 2:
                    // DX
                    return 3;
                case 3:
                    // BX
                    return 1;
            }
        }

        private static int GetDiSiBpSpRegisterOffset(int reg)
        {
            // [4,5,6,7] => [7,6,5,4] (SP, BP, SI, DI)
            return 11 - reg;
        }

        #endregion
    }
}