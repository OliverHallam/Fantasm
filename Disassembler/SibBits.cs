namespace Fantasm.Disassembler
{
    /// <summary>
    /// The component bits of an <c>SIB</c> byte.
    /// </summary>
    internal struct SibBits
    {
        #region Fields

        private readonly int baseBits;
        private readonly int indexBits;
        private readonly int scaleBits;

        #endregion

        #region Constructors and Destructors

        public SibBits(RexPrefix rex, byte sib)
        {
            this.scaleBits = sib >> 6;

            this.indexBits = (sib & 0x38) >> 3;
            if ((rex & RexPrefix.X) != 0)
            {
                this.indexBits |= 8;
            }

            this.baseBits = sib & 0x7;
            if ((rex & RexPrefix.B) != 0)
            {
                this.baseBits |= 8;
            }
        }

        #endregion

        #region Public Properties

        public int Base
        {
            get
            {
                return this.baseBits;
            }
        }

        public int Index
        {
            get
            {
                return this.indexBits;
            }
        }

        public int Scale
        {
            get
            {
                return this.scaleBits;
            }
        }

        #endregion
    }
}