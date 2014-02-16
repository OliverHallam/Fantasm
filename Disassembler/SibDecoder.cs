namespace Fantasm.Disassembler
{
    /// <summary>
    /// A structure used to decode <c>SIB</c> bytes.
    /// </summary>
    internal struct SibDecoder
    {
        #region Fields

        private readonly Register baseRegister;
        private readonly Register indexRegister;
        private readonly bool requiresDisplacement;
        private readonly int scale;

        #endregion

        #region Constructors and Destructors

        public SibDecoder(RexPrefix rex, int mod, byte sib, Register baseRegister)
        {
            var hasRex = rex != 0;

            var sibBits = new SibBits(rex, sib);

            // ESP is overloaded to mean no index
            if (sibBits.Index == 4)
            {
                this.scale = 1;
                this.indexRegister = Register.None;
            }
            else
            {
                this.scale = 1 << sibBits.Scale;
                this.indexRegister = RegDecoder.GetRegister(hasRex, sibBits.Index, baseRegister);
            }

            // EBP is overridden in the mod 0 case to mean no base, with displacement.
            this.requiresDisplacement = mod == 0 && (sibBits.Base & 7) == 5;
            this.baseRegister = this.requiresDisplacement
                ? Register.None
                : RegDecoder.GetRegister(hasRex, sibBits.Base, baseRegister);
        }

        #endregion

        #region Public Properties

        public Register BaseRegister
        {
            get
            {
                return this.baseRegister;
            }
        }

        public Register IndexRegister
        {
            get
            {
                return this.indexRegister;
            }
        }

        public bool RequiresDisplacement
        {
            get
            {
                return this.requiresDisplacement;
            }
        }

        public int Scale
        {
            get
            {
                return this.scale;
            }
        }

        #endregion
    }
}