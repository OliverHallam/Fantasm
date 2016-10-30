namespace Fantasm.Disassembler
{
    /// <summary>
    /// A structure used to decode <c>SIB</c> bytes.
    /// </summary>
    internal struct SibDecoder
    {
        #region Constructors and Destructors

        public SibDecoder(RexPrefix rex, int mod, byte sib, Register baseRegister, Size displacementOverride)
        {
            var hasRex = rex != 0;

            var sibBits = new SibBits(rex, sib);

            // ESP is overloaded to mean no index
            if (sibBits.Index == 4)
            {
                this.Scale = 1;
                this.IndexRegister = Register.None;
            }
            else
            {
                this.Scale = 1 << sibBits.Scale;
                this.IndexRegister = RegDecoder.GetRegister(hasRex, sibBits.Index, baseRegister);
            }

            // EBP is overridden in the mod 0 case to mean no base, with displacement.
            if (mod == 0 && (sibBits.Base & 7) == 5)
            {
                this.BaseRegister = Register.None;
                this.DisplacementSize = Size.Dword;
            }
            else
            {
                this.DisplacementSize = displacementOverride;
                this.BaseRegister = RegDecoder.GetRegister(hasRex, sibBits.Base, baseRegister);
            }
        }

        #endregion

        #region Public Properties

        public Register BaseRegister { get; }

        public Register IndexRegister { get; }

        public Size DisplacementSize { get; }

        public int Scale { get; }

        #endregion
    }
}