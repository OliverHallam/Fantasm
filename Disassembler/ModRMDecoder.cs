using System;
using System.Diagnostics;

namespace Fantasm.Disassembler
{
    /// <summary>
    /// A structure used to decode the r/m components of the <c>modr/m</c> bytes.
    /// </summary>
    internal struct ModRMDecoder
    {
        #region Fields

        private readonly Register baseRegister;
        private readonly Size displacementSize;
        private readonly Register indexRegister;
        private readonly bool needsSib;

        #endregion

        #region Constructors and Destructors

        public ModRMDecoder(
            ExecutionModes executionMode,
            RexPrefix rex,
            Size addressSize,
            Register addressSizeBaseRegister,
            ref ModRMBits modrmBits)
        {
            Debug.Assert(!DirectRegister(modrmBits.Mod));
            Debug.Assert(!UseSib(addressSize, ref modrmBits));

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

        public static bool DirectRegister(int mod)
        {
            return mod == 3;
        }

        public static Register GetRegister(RexPrefix rex, Register operandSizeBaseRegister, ref ModRMBits modrmBits)
        {
            return RegDecoder.GetRegister(rex != 0, modrmBits.RM, operandSizeBaseRegister);
        }

        public static bool UseSib(Size addressSize, ref ModRMBits modrmBits)
        {
            return addressSize != Size.Word && (modrmBits.RM & 7) == 4;
        }

        #region Public Properties

        public Register BaseRegister => this.baseRegister;

        public Size DisplacementSize => this.displacementSize;

        public Register IndexRegister => this.indexRegister;

        public bool NeedsSib => this.needsSib;

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
            switch (rm & 7)
            {
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
                    displacementSize = DefaultDisplacementSize(mod);
                    baseRegister = RegDecoder.GetRegister(rex != 0, rm, addressSizeBaseRegister);
                    needsSib = false;
                    break;
            }
        }

        public static Size DefaultDisplacementSize(int mod)
        {
            switch (mod)
            {
                case 1:
                    return Size.Byte;

                case 2:
                    return Size.Dword;

                default:
                    return Size.None;
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