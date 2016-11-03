using System;

namespace Fantasm.Disassembler
{
    internal enum OperandEncoding : byte
    {
        None,
        RAx,
        M,
        RM,
        Reg,
        OpCodeReg,
        Immediate,
        ImmediateByte,
        FarPointer,
        RelativeAddress
    }

    [Flags]
    internal enum OpCodeFlags
    {
        None = 0,

        OperandSizeDefault = 0x0,
        OperandSizeByte = 0x1,
        OperandSizeWord = 0x2,
        OperandSizeForce64 = 0x4,
        OperandSizeMask = 0x7,

        ModRM = 0x8,

        CompatibilityMode = 0x10,

        Lockable = 0x20,

        Ignore10 = 0x40,

        Operand1RAx = OperandEncoding.RAx << Operand1Shift,
        Operand1M = (OperandEncoding.M << Operand1Shift) | ModRM,
        Operand1RM = (OperandEncoding.RM << Operand1Shift) | ModRM,
        Operand1Reg = (OperandEncoding.Reg << Operand1Shift) | ModRM,
        Operand1OpCodeReg = (OperandEncoding.OpCodeReg << Operand1Shift),
        Operand1Immediate = OperandEncoding.Immediate << Operand1Shift,
        Operand1ImmediateByte = OperandEncoding.ImmediateByte << Operand1Shift,
        Operand1FarPointer = OperandEncoding.FarPointer << Operand1Shift,
        Operand1RelativeAddress = OperandEncoding.RelativeAddress << Operand1Shift,
        Operand1Mask = 0xFF00,
        Operand1Shift = 8,

        Operand2RAx = OperandEncoding.RAx << Operand2Shift,
        Operand2RM = (OperandEncoding.RM << Operand2Shift) | ModRM,
        Operand2M = (OperandEncoding.M << Operand2Shift) | ModRM,
        Operand2Reg = (OperandEncoding.Reg << Operand2Shift) | ModRM,
        Operand2OpCodeReg = (OperandEncoding.OpCodeReg << Operand2Shift),
        Operand2Immediate = OperandEncoding.Immediate << Operand2Shift,
        Operand2ImmediateByte = OperandEncoding.ImmediateByte << Operand2Shift,
        Operand2FarPointer = OperandEncoding.FarPointer << Operand2Shift,
        Operand2RelativeAddress = OperandEncoding.RelativeAddress << Operand2Shift,
        Operand2Mask = 0xFF0000,
        Operand2Shift = 16
    }

    internal struct OpCodeProperties
    {
        public OpCodeProperties(Instruction instruction, OpCodeFlags flags)
        {
            this.Instruction = instruction;
            this.Flags = flags;
        }

        public Instruction Instruction { get; }
        public OpCodeFlags Flags { get; }
    }

    internal static class OpCodeTables
    {
        public static OpCodeProperties[] OneByteOpCodeMap =
        {
            new OpCodeProperties(Instruction.Add, OpCodeFlags.Lockable | OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Add, OpCodeFlags.Lockable | OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Add, OpCodeFlags.Lockable | OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM),
            new OpCodeProperties(Instruction.Add, OpCodeFlags.Lockable | OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM),
            new OpCodeProperties(Instruction.Add, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1RAx | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Add, OpCodeFlags.Operand1RAx | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // 2 byte escape

            new OpCodeProperties(Instruction.Adc, OpCodeFlags.Lockable | OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Adc, OpCodeFlags.Lockable | OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Adc, OpCodeFlags.Lockable | OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM),
            new OpCodeProperties(Instruction.Adc, OpCodeFlags.Lockable | OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM),
            new OpCodeProperties(Instruction.Adc, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1RAx | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Adc, OpCodeFlags.Operand1RAx | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.And, OpCodeFlags.Lockable | OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.And, OpCodeFlags.Lockable | OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.And, OpCodeFlags.Lockable | OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM),
            new OpCodeProperties(Instruction.And, OpCodeFlags.Lockable | OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM),
            new OpCodeProperties(Instruction.And, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1RAx | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.And, OpCodeFlags.Operand1RAx | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // ES segment prefix
            new OpCodeProperties(Instruction.Daa, OpCodeFlags.CompatibilityMode),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // CS segment prefix
            new OpCodeProperties(Instruction.Das, OpCodeFlags.CompatibilityMode),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // SS segment prefix
            new OpCodeProperties(Instruction.Aaa, OpCodeFlags.CompatibilityMode),
            new OpCodeProperties(Instruction.Cmp, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Cmp, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Cmp, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM),
            new OpCodeProperties(Instruction.Cmp, OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM),
            new OpCodeProperties(Instruction.Cmp, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1RAx | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Cmp, OpCodeFlags.Operand1RAx | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // DS segment prefix
            new OpCodeProperties(Instruction.Aas, OpCodeFlags.CompatibilityMode),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), 
            new OpCodeProperties(Instruction.Dec, OpCodeFlags.Operand1OpCodeReg),
            new OpCodeProperties(Instruction.Dec, OpCodeFlags.Operand1OpCodeReg),
            new OpCodeProperties(Instruction.Dec, OpCodeFlags.Operand1OpCodeReg),
            new OpCodeProperties(Instruction.Dec, OpCodeFlags.Operand1OpCodeReg),
            new OpCodeProperties(Instruction.Dec, OpCodeFlags.Operand1OpCodeReg),
            new OpCodeProperties(Instruction.Dec, OpCodeFlags.Operand1OpCodeReg),
            new OpCodeProperties(Instruction.Dec, OpCodeFlags.Operand1OpCodeReg),
            new OpCodeProperties(Instruction.Dec, OpCodeFlags.Operand1OpCodeReg),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Bound, OpCodeFlags.CompatibilityMode | OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM),
            new OpCodeProperties(Instruction.Arpl, OpCodeFlags.CompatibilityMode | OpCodeFlags.OperandSizeWord | OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // FS Segment
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // GS Segment
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // Operand size override
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // Address size override
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1RM | OpCodeFlags.Operand2ImmediateByte), // Group 1
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Immediate), // Group 1
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.OperandSizeByte| OpCodeFlags.Operand1RM | OpCodeFlags.Operand2ImmediateByte), // Group 1
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2ImmediateByte), // Group 1
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Nop, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // CBW, CWDE, CDQE
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // CWD, CDQ, CQO
            new OpCodeProperties(Instruction.CallFar, OpCodeFlags.CompatibilityMode | OpCodeFlags.Operand1FarPointer),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Cmpsb, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // CMPSW, CMPSD, CMPSQ
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Enter, OpCodeFlags.OperandSizeWord | OpCodeFlags.Operand1Immediate | OpCodeFlags.Operand2ImmediateByte),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Aam, OpCodeFlags.CompatibilityMode | OpCodeFlags.Ignore10 | OpCodeFlags.Operand1ImmediateByte),
            new OpCodeProperties(Instruction.Aad, OpCodeFlags.CompatibilityMode | OpCodeFlags.Ignore10 | OpCodeFlags.Operand1ImmediateByte),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Call, OpCodeFlags.Operand1RelativeAddress),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // Lock prefix
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // RepNE prefix
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // Rep prefix
            new OpCodeProperties(Instruction.Hlt, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Cmc, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1RM), // Group 3
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.Operand1RM), // Group 3
            new OpCodeProperties(Instruction.Clc, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Cli, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Cld, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1RM), // Group 4
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.ModRM), // Group 5
        };

        public static OpCodeProperties[] TwoByteOpCodeMap =
        {
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Clts, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Ud2, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // Three byte instruction 0F 38 xx
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Cmovo, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Cmovno, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Cmovb, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg), // also CMOVNAE, CMOVC
            new OpCodeProperties(Instruction.Cmovae, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg), // also CMOVNB, CMOVNC
            new OpCodeProperties(Instruction.Cmove, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg), // also CMOVZ
            new OpCodeProperties(Instruction.Cmovne, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg), // also CMOVNZ
            new OpCodeProperties(Instruction.Cmovbe, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg), // also CMOVNA
            new OpCodeProperties(Instruction.Cmova, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg), // also CMOVNBE
            new OpCodeProperties(Instruction.Cmovs, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Cmovns, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Cmovp, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg), // also CMOVPE
            new OpCodeProperties(Instruction.Cmovnp, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg), // also CMOVPO
            new OpCodeProperties(Instruction.Cmovl, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg), // also CMOVNGE
            new OpCodeProperties(Instruction.Cmovge, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg), // also CMOVNL
            new OpCodeProperties(Instruction.Cmovle, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg), // also CMOVNG
            new OpCodeProperties(Instruction.Cmovg, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg), // also CMOVNLE

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Emms, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Cpuid, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Bt, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Bts, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Cmpxchg, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Cmpxchg, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Btr, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Ud1, OpCodeFlags.None), // Group 10
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2ImmediateByte), // Group 8
            new OpCodeProperties(Instruction.Btc, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Bsf, OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM),
            new OpCodeProperties(Instruction.Bsr, OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.ModRM), // Group 9
            new OpCodeProperties(Instruction.Bswap, OpCodeFlags.Operand1OpCodeReg),
            new OpCodeProperties(Instruction.Bswap, OpCodeFlags.Operand1OpCodeReg),
            new OpCodeProperties(Instruction.Bswap, OpCodeFlags.Operand1OpCodeReg),
            new OpCodeProperties(Instruction.Bswap, OpCodeFlags.Operand1OpCodeReg),
            new OpCodeProperties(Instruction.Bswap, OpCodeFlags.Operand1OpCodeReg),
            new OpCodeProperties(Instruction.Bswap, OpCodeFlags.Operand1OpCodeReg),
            new OpCodeProperties(Instruction.Bswap, OpCodeFlags.Operand1OpCodeReg),
            new OpCodeProperties(Instruction.Bswap, OpCodeFlags.Operand1OpCodeReg),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None)
        };

        public static Instruction[] Group1Instructions =
        {
            Instruction.Add,
            Instruction.Unknown,
            Instruction.Adc,
            Instruction.Unknown,
            Instruction.And,
            Instruction.Unknown,
            Instruction.Unknown,
            Instruction.Cmp,
        };

        public static Instruction[] Group3Instructions =
        {
            Instruction.Unknown,
            Instruction.Unknown,
            Instruction.Unknown,
            Instruction.Unknown,
            Instruction.Unknown,
            Instruction.Unknown,
            Instruction.Div,
            Instruction.Unknown,
        };

        public static Instruction[] Group4Instructions =
        {
            Instruction.Unknown,
            Instruction.Dec,
            Instruction.Unknown,
            Instruction.Unknown,
            Instruction.Unknown,
            Instruction.Unknown,
            Instruction.Unknown,
            Instruction.Unknown,
        };

        public static OpCodeProperties[] Group5OpCodes =
        {
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Dec, OpCodeFlags.Operand1RM),
            new OpCodeProperties(Instruction.Call, OpCodeFlags.OperandSizeForce64 | OpCodeFlags.Operand1RM),
            new OpCodeProperties(Instruction.CallFar, OpCodeFlags.OperandSizeForce64 | OpCodeFlags.Operand1M), // TODO: should have an extra 16 bits in the ptr
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None)
        };

        public static Instruction[] Group8Instructions =
        {
            Instruction.Unknown,
            Instruction.Unknown,
            Instruction.Unknown,
            Instruction.Unknown,
            Instruction.Bt,
            Instruction.Bts,
            Instruction.Btr,
            Instruction.Btc,
        };

    }
}

