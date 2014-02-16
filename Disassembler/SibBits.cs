namespace Fantasm.Disassembler
{
    /// <summary>
    /// The component bits of an <c>SIB</c> byte.
    /// </summary>
    struct SibBits
    {
        private int scaleBits;
        private int indexBits;
        private int baseBits;

        public SibBits(RexPrefix rex, byte sib)
        {
            scaleBits = sib >> 6;

            indexBits = (sib & 0x38) >> 3;
            if ((rex & RexPrefix.X) != 0)
            {
                indexBits |= 8;
            }

            baseBits = sib & 0x7;
            if ((rex & RexPrefix.B) != 0)
            {
                baseBits |= 8;
            }
        }

        public int Scale
        {
            get
            {
                return this.scaleBits;
            }
        }

        public int Index
        {
            get
            {
                return this.indexBits;
            }
        }

        public int Base
        {
            get
            {
                return this.baseBits;
            }
        }
    }
}
