using System;

namespace Fantasm.Disassembler
{
    [Flags]
    internal enum OperandEncoding : byte
    {
        None,
        Three,
        Rax,
        Dx,
        R,
        M,
        RM,
        Reg,
        Sreg,
        Creg,
        Dreg,
        OpCodeReg,
        Immediate,
        Offset,
        FarPointer,
        RelativeAddress,
        TypeMask = 0xF,

        SizeAuto = 0,
        SizeByte = 0x10,
        SizeWord = 0x20,
        SizeFar = 0x30,
        SizeNone = 0x40,
        SizePseudoDescriptor = 0x50,
        SizeTruncate32 = 0x60,
        SizeMask = 0xf0
    }

    [Flags]
    internal enum OpCodeFlags
    {
        None = 0,

        OperandSizeDefault = 0x0,
        OperandSizeByte = 0x1,
        OperandSizeWord = 0x2,
        OperandSizeFixed = 0x3,
        OperandSizeFixed64 = 0x4,
        OperandSizeDefault64 = 0x5,
        OperandSizeExtendableDword = 0x6,
        OperandSizeMask = 0x7,

        ModRM = 0x8,

        CompatibilityMode = 0x10,

        Lockable = 0x20,

        Ignore10 = 0x40,

        Operand1Three = OperandEncoding.Three << Operand1Shift,
        Operand1Rax = OperandEncoding.Rax << Operand1Shift,
        Operand1Dx = OperandEncoding.Dx << Operand1Shift,
        Operand1R = (OperandEncoding.R << Operand1Shift) | ModRM,
        Operand1M = (OperandEncoding.M << Operand1Shift) | ModRM,
        Operand1RM = (OperandEncoding.RM << Operand1Shift) | ModRM,
        Operand1Reg = (OperandEncoding.Reg << Operand1Shift) | ModRM,
        Operand1Sreg = (OperandEncoding.Sreg << Operand1Shift) | ModRM,
        Operand1Creg = (OperandEncoding.Creg << Operand1Shift) | ModRM,
        Operand1Dreg = (OperandEncoding.Dreg << Operand1Shift) | ModRM,
        Operand1OpCodeReg = (OperandEncoding.OpCodeReg << Operand1Shift),
        Operand1Immediate = OperandEncoding.Immediate << Operand1Shift,
        Operand1Offset = OperandEncoding.Offset << Operand1Shift,
        Operand1FarPointer = OperandEncoding.FarPointer << Operand1Shift,
        Operand1RelativeAddress = OperandEncoding.RelativeAddress << Operand1Shift,

        Operand1Byte = OperandEncoding.SizeByte << Operand1Shift,
        Operand1Word = OperandEncoding.SizeWord << Operand1Shift,
        Operand1Far = OperandEncoding.SizeFar << Operand1Shift,
        Operand1Untyped = OperandEncoding.SizeNone << Operand1Shift,
        Operand1Pseudo = OperandEncoding.SizePseudoDescriptor << Operand1Shift,
        Operand1Truncate32 = OperandEncoding.SizeTruncate32 << Operand1Shift,

        Operand1Mask = 0xFF00,
        Operand1Shift = 8,

        Operand2Three = OperandEncoding.Three << Operand2Shift,
        Operand2Rax = OperandEncoding.Rax << Operand2Shift,
        Operand2Dx = OperandEncoding.Dx << Operand2Shift,
        Operand2R = (OperandEncoding.R << Operand2Shift) | ModRM,
        Operand2M = (OperandEncoding.M << Operand2Shift) | ModRM,
        Operand2RM = (OperandEncoding.RM << Operand2Shift) | ModRM,
        Operand2Reg = (OperandEncoding.Reg << Operand2Shift) | ModRM,
        Operand2Sreg = (OperandEncoding.Sreg << Operand2Shift) | ModRM,
        Operand2Creg = (OperandEncoding.Creg << Operand2Shift) | ModRM,
        Operand2Dreg = (OperandEncoding.Dreg << Operand2Shift) | ModRM,
        Operand2OpCodeReg = OperandEncoding.OpCodeReg << Operand2Shift,
        Operand2Immediate = OperandEncoding.Immediate << Operand2Shift,
        Operand2Offset = OperandEncoding.Offset << Operand2Shift,
        Operand2RelativeAddress = OperandEncoding.RelativeAddress << Operand2Shift,

        Operand2Byte = OperandEncoding.SizeByte << Operand2Shift,
        Operand2Word = OperandEncoding.SizeWord << Operand2Shift,
        Operand2Far = OperandEncoding.SizeFar << Operand2Shift,
        Operand2Untyped = OperandEncoding.SizeNone << Operand2Shift,
        Operand2Pseudo = OperandEncoding.SizePseudoDescriptor << Operand2Shift,
        Operand2Truncate32 = OperandEncoding.SizeTruncate32 << Operand2Shift,

        Operand2Mask = 0xFF0000,
        Operand2Shift = 16,

        Operand3Three = OperandEncoding.Three << Operand3Shift,
        Operand3Rax = OperandEncoding.Rax << Operand3Shift,
        Operand3Dx = OperandEncoding.Dx << Operand3Shift,
        Operand3R = (OperandEncoding.R << Operand3Shift) | ModRM,
        Operand3M = (OperandEncoding.M << Operand3Shift) | ModRM,
        Operand3RM = (OperandEncoding.RM << Operand3Shift) | ModRM,
        Operand3Reg = (OperandEncoding.Reg << Operand3Shift) | ModRM,
        Operand3SReg = (OperandEncoding.Sreg << Operand3Shift) | ModRM,
        Operand3CReg = (OperandEncoding.Sreg << Operand3Shift) | ModRM,
        Operand3DReg = (OperandEncoding.Sreg << Operand3Shift) | ModRM,
        Operand3OpCodeReg = OperandEncoding.OpCodeReg << Operand3Shift,
        Operand3Immediate = OperandEncoding.Immediate << Operand3Shift,
        Operand3Offset = OperandEncoding.Offset << Operand3Shift,
        Operand3FarPointer = OperandEncoding.FarPointer << Operand3Shift,
        Operand3RelativeAddress = OperandEncoding.RelativeAddress << Operand3Shift,

        Operand3Byte = OperandEncoding.SizeByte << Operand3Shift,
        Operand3Word = OperandEncoding.SizeWord << Operand3Shift,
        Operand3Far = OperandEncoding.SizeFar << Operand3Shift,
        Operand3Untyped = OperandEncoding.SizeNone << Operand3Shift,
        Operand3Pseudo = OperandEncoding.SizePseudoDescriptor << Operand3Shift,
        Operand3Truncate32 = OperandEncoding.SizeTruncate32 << Operand3Shift,

        Operand3Mask = unchecked((int)0xFF000000),
        Operand3Shift = 24
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
            new OpCodeProperties(Instruction.Add, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1Rax | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Add, OpCodeFlags.Operand1Rax | OpCodeFlags.Operand2Immediate | OpCodeFlags.Operand2Truncate32),
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
            new OpCodeProperties(Instruction.Adc, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1Rax | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Adc, OpCodeFlags.Operand1Rax | OpCodeFlags.Operand2Immediate | OpCodeFlags.Operand2Truncate32),
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
            new OpCodeProperties(Instruction.And, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1Rax | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.And, OpCodeFlags.Operand1Rax | OpCodeFlags.Operand2Immediate | OpCodeFlags.Operand2Truncate32),
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
            new OpCodeProperties(Instruction.Cmp, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1Rax | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Cmp, OpCodeFlags.Operand1Rax | OpCodeFlags.Operand2Immediate | OpCodeFlags.Operand2Truncate32),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // DS segment prefix
            new OpCodeProperties(Instruction.Aas, OpCodeFlags.CompatibilityMode),

            new OpCodeProperties(Instruction.Inc, OpCodeFlags.Operand1OpCodeReg),
            new OpCodeProperties(Instruction.Inc, OpCodeFlags.Operand1OpCodeReg),
            new OpCodeProperties(Instruction.Inc, OpCodeFlags.Operand1OpCodeReg),
            new OpCodeProperties(Instruction.Inc, OpCodeFlags.Operand1OpCodeReg),
            new OpCodeProperties(Instruction.Inc, OpCodeFlags.Operand1OpCodeReg),
            new OpCodeProperties(Instruction.Inc, OpCodeFlags.Operand1OpCodeReg),
            new OpCodeProperties(Instruction.Inc, OpCodeFlags.Operand1OpCodeReg),
            new OpCodeProperties(Instruction.Inc, OpCodeFlags.Operand1OpCodeReg), 
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
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.Operand1Reg| OpCodeFlags.Operand2RM | OpCodeFlags.Operand2Truncate32), // ARPL or MOVSXD
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // FS Segment
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // GS Segment
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // Operand size override
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // Address size override
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Imul, OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM | OpCodeFlags.Operand3Immediate | OpCodeFlags.Operand3Truncate32),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Imul, OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM | OpCodeFlags.Operand3Immediate | OpCodeFlags.Operand3Byte),
            new OpCodeProperties(Instruction.Insb, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // INSD/INSW
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Jo, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Byte),
            new OpCodeProperties(Instruction.Jno, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Byte),
            new OpCodeProperties(Instruction.Jb, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Byte), // also JNAE, JC
            new OpCodeProperties(Instruction.Jae, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Byte), // also JNB, JAE, JNC
            new OpCodeProperties(Instruction.Je, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Byte), // also JZ
            new OpCodeProperties(Instruction.Jne, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Byte), // also JNZ
            new OpCodeProperties(Instruction.Jbe, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Byte), // also JNA
            new OpCodeProperties(Instruction.Ja, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Byte), // also JNBE
            new OpCodeProperties(Instruction.Js, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Byte),
            new OpCodeProperties(Instruction.Jns, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Byte),
            new OpCodeProperties(Instruction.Jpe, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Byte), // also JP
            new OpCodeProperties(Instruction.Jpo, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Byte), // also JNP
            new OpCodeProperties(Instruction.Jl, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Byte), // also JNGE
            new OpCodeProperties(Instruction.Jge, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Byte), // also JNL
            new OpCodeProperties(Instruction.Jle, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Byte), // also JNG
            new OpCodeProperties(Instruction.Jg, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Byte), // also JNLE

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Immediate), // Group 1
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Immediate | OpCodeFlags.Operand2Truncate32), // Group 1
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.OperandSizeByte| OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Immediate), // Group 1
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Immediate | OpCodeFlags.Operand2Byte), // Group 1
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Sreg),
            new OpCodeProperties(Instruction.Lea, OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2M | OpCodeFlags.Operand2Untyped),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.Operand1Sreg | OpCodeFlags.Operand2RM),
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
            new OpCodeProperties(Instruction.Call, OpCodeFlags.CompatibilityMode | OpCodeFlags.Operand1FarPointer),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Lahf, OpCodeFlags.CompatibilityMode),

            new OpCodeProperties(Instruction.Mov, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1Rax | OpCodeFlags.Operand2Offset),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.Operand1Rax | OpCodeFlags.Operand2Offset),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1Offset | OpCodeFlags.Operand2Rax),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.Operand1Offset | OpCodeFlags.Operand2Rax),
            new OpCodeProperties(Instruction.Movsb, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // MOVSW, MOVSD, MOVSQ
            new OpCodeProperties(Instruction.Cmpsb, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // CMPSW, CMPSD, CMPSQ
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Lodsb, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // LODSW/LODSD/LODSQ
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),

            new OpCodeProperties(Instruction.Mov, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1OpCodeReg | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1OpCodeReg | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1OpCodeReg | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1OpCodeReg | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1OpCodeReg | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1OpCodeReg | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1OpCodeReg | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1OpCodeReg | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.Operand1OpCodeReg | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.Operand1OpCodeReg | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.Operand1OpCodeReg | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.Operand1OpCodeReg | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.Operand1OpCodeReg | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.Operand1OpCodeReg | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.Operand1OpCodeReg | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.Operand1OpCodeReg | OpCodeFlags.Operand2Immediate),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Les, OpCodeFlags.CompatibilityMode | OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2M | OpCodeFlags.Operand2Far), // VEX +2 bytes
            new OpCodeProperties(Instruction.Lds, OpCodeFlags.CompatibilityMode | OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2M | OpCodeFlags.Operand2Far), // VEX +1 byte
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Immediate), // Group 11
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Immediate | OpCodeFlags.Operand2Truncate32), // Group 11
            new OpCodeProperties(Instruction.Enter, OpCodeFlags.OperandSizeWord | OpCodeFlags.Operand1Immediate | OpCodeFlags.Operand2Immediate | OpCodeFlags.Operand2Byte),
            new OpCodeProperties(Instruction.Leave, OpCodeFlags.OperandSizeDefault64),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Int, OpCodeFlags.Operand1Three),
            new OpCodeProperties(Instruction.Int, OpCodeFlags.Operand1Immediate | OpCodeFlags.Operand1Byte),
            new OpCodeProperties(Instruction.Into, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None), // IRET/IRETD/IRETQ

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Aam, OpCodeFlags.CompatibilityMode | OpCodeFlags.Ignore10 | OpCodeFlags.Operand1Immediate | OpCodeFlags.Operand1Byte),
            new OpCodeProperties(Instruction.Aad, OpCodeFlags.CompatibilityMode | OpCodeFlags.Ignore10 | OpCodeFlags.Operand1Immediate | OpCodeFlags.Operand1Byte),
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

            new OpCodeProperties(Instruction.Loopne, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1Byte | OpCodeFlags.Operand1RelativeAddress),
            new OpCodeProperties(Instruction.Loope, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1Byte | OpCodeFlags.Operand1RelativeAddress),
            new OpCodeProperties(Instruction.Loop, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1Byte | OpCodeFlags.Operand1RelativeAddress),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1Byte | OpCodeFlags.Operand1RelativeAddress), // JCXZ/JECXZ/JRCXZ
            new OpCodeProperties(Instruction.In, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1Rax | OpCodeFlags.Operand2Immediate),
            new OpCodeProperties(Instruction.In, OpCodeFlags.Operand1Rax | OpCodeFlags.Operand2Immediate | OpCodeFlags.Operand2Byte),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Call, OpCodeFlags.Operand1RelativeAddress),
            new OpCodeProperties(Instruction.Jmp, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Truncate32),
            new OpCodeProperties(Instruction.Jmp, OpCodeFlags.CompatibilityMode | OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1FarPointer),
            new OpCodeProperties(Instruction.Jmp, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Byte),
            new OpCodeProperties(Instruction.In, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1Rax | OpCodeFlags.Operand2Dx),
            new OpCodeProperties(Instruction.In, OpCodeFlags.Operand1Rax | OpCodeFlags.Operand2Dx),
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
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.ModRM), // Group 6
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.ModRM), // Group 7
            new OpCodeProperties(Instruction.Lar, OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM | OpCodeFlags.Operand2Word),
            new OpCodeProperties(Instruction.Lsl, OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM | OpCodeFlags.Operand2Word),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Clts, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Invd, OpCodeFlags.None),
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

            new OpCodeProperties(Instruction.Mov, OpCodeFlags.OperandSizeFixed | OpCodeFlags.Operand1R | OpCodeFlags.Operand2Creg),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.OperandSizeFixed | OpCodeFlags.Operand1R | OpCodeFlags.Operand2Dreg),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.OperandSizeFixed | OpCodeFlags.Operand1Creg | OpCodeFlags.Operand2R),
            new OpCodeProperties(Instruction.Mov, OpCodeFlags.OperandSizeFixed | OpCodeFlags.Operand1Dreg | OpCodeFlags.Operand2R),
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

            new OpCodeProperties(Instruction.Jo, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Truncate32),
            new OpCodeProperties(Instruction.Jno, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Truncate32),
            new OpCodeProperties(Instruction.Jb, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Truncate32), // also JNAE, JC
            new OpCodeProperties(Instruction.Jae, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Truncate32), // also JNB, JAE, JNC
            new OpCodeProperties(Instruction.Je, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Truncate32), // also JZ
            new OpCodeProperties(Instruction.Jne, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Truncate32), // also JNZ
            new OpCodeProperties(Instruction.Jbe, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Truncate32), // also JNA
            new OpCodeProperties(Instruction.Ja, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Truncate32), // also JNBE
            new OpCodeProperties(Instruction.Js, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Truncate32),
            new OpCodeProperties(Instruction.Jns, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Truncate32),
            new OpCodeProperties(Instruction.Jpe, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Truncate32), // also JP
            new OpCodeProperties(Instruction.Jpo, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Truncate32), // also JNP
            new OpCodeProperties(Instruction.Jl, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Truncate32), // also JNGE
            new OpCodeProperties(Instruction.Jge, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Truncate32), // also JNL
            new OpCodeProperties(Instruction.Jle, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Truncate32), // also JNG
            new OpCodeProperties(Instruction.Jg, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RelativeAddress | OpCodeFlags.Operand1Truncate32), // also JNLE

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
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.ModRM), // Group 15
            new OpCodeProperties(Instruction.Imul, OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM),

            new OpCodeProperties(Instruction.Cmpxchg, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Cmpxchg, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Lss, OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2M | OpCodeFlags.Operand2Far),
            new OpCodeProperties(Instruction.Btr, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Lfs, OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2M | OpCodeFlags.Operand2Far),
            new OpCodeProperties(Instruction.Lgs, OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2M | OpCodeFlags.Operand2Far),
            new OpCodeProperties(Instruction.Movzx, OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM | OpCodeFlags.Operand2Byte),
            new OpCodeProperties(Instruction.Movzx, OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM | OpCodeFlags.Operand2Word),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Ud1, OpCodeFlags.None), // Group 10
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Immediate | OpCodeFlags.Operand2Byte), // Group 8
            new OpCodeProperties(Instruction.Btc, OpCodeFlags.Operand1RM | OpCodeFlags.Operand2Reg),
            new OpCodeProperties(Instruction.Bsf, OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM),
            new OpCodeProperties(Instruction.Bsr, OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM),
            new OpCodeProperties(Instruction.Movsx, OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM | OpCodeFlags.Operand2Byte),
            new OpCodeProperties(Instruction.Movsx, OpCodeFlags.Operand1Reg | OpCodeFlags.Operand2RM | OpCodeFlags.Operand2Word),

            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Movnti, OpCodeFlags.OperandSizeExtendableDword | OpCodeFlags.Operand1M | OpCodeFlags.Operand2Reg),
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
            Instruction.Imul,
            Instruction.Div,
            Instruction.Idiv,
        };

        public static Instruction[] Group4Instructions =
        {
            Instruction.Inc,
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
            new OpCodeProperties(Instruction.Inc, OpCodeFlags.Operand1RM),
            new OpCodeProperties(Instruction.Dec, OpCodeFlags.Operand1RM),
            new OpCodeProperties(Instruction.Call, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RM),
            new OpCodeProperties(Instruction.Call, OpCodeFlags.Operand1Far | OpCodeFlags.Operand1M),
            new OpCodeProperties(Instruction.Jmp, OpCodeFlags.OperandSizeFixed64 | OpCodeFlags.Operand1RM),
            new OpCodeProperties(Instruction.Jmp, OpCodeFlags.Operand1Far | OpCodeFlags.Operand1M),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None)
        };

        public static OpCodeProperties[] Group6OpCodes =
        {
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Lldt, OpCodeFlags.Operand1Word | OpCodeFlags.Operand1RM),
            new OpCodeProperties(Instruction.Ltr, OpCodeFlags.Operand1Word | OpCodeFlags.Operand1RM),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None)
        };

        public static OpCodeProperties[] Group7OpCodes =
        {
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Lgdt, OpCodeFlags.Operand1Pseudo | OpCodeFlags.Operand1M),
            new OpCodeProperties(Instruction.Lidt, OpCodeFlags.Operand1Pseudo | OpCodeFlags.Operand1M),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Unknown, OpCodeFlags.None),
            new OpCodeProperties(Instruction.Lmsw, OpCodeFlags.Operand1Word | OpCodeFlags.Operand1RM),
            new OpCodeProperties(Instruction.Invlpg, OpCodeFlags.OperandSizeByte | OpCodeFlags.Operand1M)
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

        public static Instruction[] Group11Instructions =
        {
            Instruction.Mov,
            Instruction.Unknown,
            Instruction.Unknown,
            Instruction.Unknown,
            Instruction.Unknown,
            Instruction.Unknown,
            Instruction.Unknown,
            Instruction.Unknown,
        };
    }
}



