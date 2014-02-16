namespace Fantasm.Disassembler
{
    /// <summary>
    /// The component bits of a <c>modrm</c> byte.
    /// </summary>
    internal struct ModRMBits
    {
        private readonly int mod;
        private readonly int reg;
        private readonly int rm;

        public ModRMBits(RexPrefix rex, byte modrm)
        {
            this.mod = (modrm & 0xc0) >> 6;

            this.reg = (modrm & 0x38) >> 3;
            if ((rex & RexPrefix.R) != 0)
            {
                this.reg |= 8;
            }

            this.rm = modrm & 0x07;
            if ((rex & RexPrefix.B) != 0)
            {
                this.rm = this.rm | 8;
            }
        }

        public int Mod
        {
            get
            {
                return this.mod;
            }
        }

        public int OpCode
        {
            get
            {
                // strip the rex bit
                return this.reg & 7;
            }
        }

        public int Reg
        {
            get
            {
                return this.reg;
            }
        }

        public int RM
        {
            get
            {
                return this.rm;
            }
        }
    }
}
