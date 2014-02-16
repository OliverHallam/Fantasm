using System;

namespace Fantasm.Disassembler
{
    /// <summary>
    /// A structure used to decode the r/m components of the <c>modr/m</c> bytes.
    /// </summary>
    internal struct ModRMDecoder
    {
        #region Fields

        private readonly Register baseRegister;
        private readonly bool isDirectRegister;
        private readonly Size displacementSize;
        private readonly Register indexRegister;
        private readonly int mod;
        private readonly bool needsSib;

        #endregion

        #region Constructors and Destructors

        public ModRMDecoder(
            ExecutionModes executionMode,
            RexPrefix rex,
            Size addressSize,
            Register operandSizeBaseRegister,
            ref ModRMBits modrmBits, Register addressSizeBaseRegister)
        {
            this.mod = modrmBits.Mod;

            this.isDirectRegister = modrmBits.Mod == 3;
            if (this.isDirectRegister)
            {
                this.baseRegister = RegDecoder.GetRegister(rex != 0, modrmBits.RM, operandSizeBaseRegister);
                this.displacementSize = Size.None;
                this.indexRegister = Register.None;
                this.needsSib = false;
                return;
            }

            switch (addressSize)
            {
                case Size.Word:
                    ReadMemory16(
                        modrmBits.Mod,
                        modrmBits.RM,
                        out this.baseRegister,
                        out this.indexRegister,
                        out this.displacementSize);
                    this.needsSib = false;
                    break;

                default:
                    this.indexRegister = Register.None;
                    ReadMemory32(
                        executionMode,
                        addressSizeBaseRegister,
                        rex,
                        modrmBits.Mod,
                        modrmBits.RM,
                        out this.baseRegister,
                        out this.needsSib,
                        out this.displacementSize);
                    break;
            }
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

        public Size DisplacementSize
        {
            get
            {
                return this.displacementSize;
            }
        }

        public Register IndexRegister
        {
            get
            {
                return this.indexRegister;
            }
        }

        public bool IsDirectRegister
        {
            get
            {
                return this.isDirectRegister;
            }
        }

        public int Mod
        {
            get
            {
                return this.mod;
            }
        }

        public bool NeedsSib
        {
            get
            {
                return this.needsSib;
            }
        }

        #endregion

        #region Methods

        private static void ReadMemory16(
            int mod,
            int rm,
            out Register baseRegister,
            out Register indexRegister,
            out Size displacementSize)
        {
            ReadRMBaseIndex16(rm, out baseRegister, out indexRegister);

            switch (mod)
            {
                case 0:
                    if (baseRegister == Register.Bp && indexRegister == Register.None)
                    {
                        // instead of BP we use a 16-bit displacement
                        baseRegister = Register.None;
                        displacementSize = Size.Word;
                    }
                    else
                    {
                        displacementSize = Size.None;
                    }
                    break;

                case 1:
                    displacementSize = Size.Byte;
                    break;

                case 2:
                    displacementSize = Size.Word;
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        private static void ReadMemory32(
            ExecutionModes executionMode,
            Register addressSizeBaseRegister,
            RexPrefix rex,
            int mod,
            int rm,
            out Register baseRegister,
            out bool needsSib,
            out Size displacementSize)
        {
            switch (mod)
            {
                case 1:
                    displacementSize = Size.Byte;
                    break;

                case 2:
                    displacementSize = Size.Dword;
                    break;

                default:
                    displacementSize = Size.None;
                    break;
            }

            switch (rm & 7)
            {
                case 4:
                    // use sib byte;
                    baseRegister = Register.None;
                    needsSib = true;
                    break;

                case 5:
                    if (mod != 0)
                    {
                        goto default;
                    }

                    if (executionMode == ExecutionModes.Long64Bit)
                    {
                        // in 64-bit mode without a sib byte we use RIP based addressing
                        baseRegister = addressSizeBaseRegister + (Register.Eip - Register.Eax);
                    }
                    else
                    {
                        // in all other cases, we ignore the register
                        baseRegister = Register.None;
                    }

                    displacementSize = Size.Dword;
                    needsSib = false;
                    break;

                default:
                    baseRegister = RegDecoder.GetRegister(rex != 0, rm, addressSizeBaseRegister);
                    needsSib = false;
                    break;
            }
        }

        private static void ReadRMBaseIndex16(int rm, out Register baseRegister, out Register indexRegister)
        {
            switch (rm)
            {
                case 0:
                    baseRegister = Register.Bx;
                    indexRegister = Register.Si;
                    break;
                case 1:
                    baseRegister = Register.Bx;
                    indexRegister = Register.Di;
                    break;
                case 2:
                    baseRegister = Register.Bp;
                    indexRegister = Register.Si;
                    break;
                case 3:
                    baseRegister = Register.Bp;
                    indexRegister = Register.Di;
                    break;
                case 4:
                    baseRegister = Register.Si;
                    indexRegister = Register.None;
                    break;
                case 5:
                    baseRegister = Register.Di;
                    indexRegister = Register.None;
                    break;
                case 6:
                    baseRegister = Register.Bp;
                    indexRegister = Register.None;
                    break;
                case 7:
                    baseRegister = Register.Bx;
                    indexRegister = Register.None;
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        #endregion
    }
}