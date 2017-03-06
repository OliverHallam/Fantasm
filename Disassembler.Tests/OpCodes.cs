using System.Linq;

namespace Fantasm.Disassembler.Tests
{
    // Each row here corresponds to a line in the Intel manual
    internal static class OpCodes
    {
        public static InstructionRepresentation[] A =
        {
            new InstructionRepresentation(Compatibility.Invalid64, 0x37, Instruction.Aaa),
            new InstructionRepresentation(Compatibility.Invalid64, 0xD5, Instruction.Aad, OperandFormat.Ib),
            new InstructionRepresentation(Compatibility.Invalid64, 0xD4, Instruction.Aam, OperandFormat.Ib),
            new InstructionRepresentation(Compatibility.Invalid64, 0x3F, Instruction.Aas),

            new InstructionRepresentation(0x14, Instruction.Adc, OperandFormat.AL, OperandFormat.Ib),
            new InstructionRepresentation(OperandSize.Size16, 0x15, Instruction.Adc, OperandFormat.AX, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size32, 0x15, Instruction.Adc, OperandFormat.EAX, OperandFormat.Id),
            new InstructionRepresentation(RexPrefix.W, 0x15, Instruction.Adc, OperandFormat.RAX, OperandFormat.Id),
            new InstructionRepresentation(InstructionPrefixes.Lock, 0x80, 2, Instruction.Adc, OperandFormat.Eb, OperandFormat.Ib),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, 0x80, 2, Instruction.Adc, OperandFormat.Eb, OperandFormat.Ib),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size16, 0x81, 2, Instruction.Adc, OperandFormat.Ew, OperandFormat.Iw),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size32, 0x81, 2, Instruction.Adc, OperandFormat.Ed, OperandFormat.Id),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, 0x81, 2, Instruction.Adc, OperandFormat.Eq, OperandFormat.Id),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size16, 0x83, 2, Instruction.Adc, OperandFormat.Ew, OperandFormat.Ib),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size32, 0x83, 2, Instruction.Adc, OperandFormat.Ed, OperandFormat.Ib),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, 0x83, 2, Instruction.Adc, OperandFormat.Eq, OperandFormat.Ib),
            new InstructionRepresentation(InstructionPrefixes.Lock, 0x10, Instruction.Adc, OperandFormat.Eb, OperandFormat.Gb),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, 0x10, Instruction.Adc, OperandFormat.Eb, OperandFormat.Gb),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size16, 0x11, Instruction.Adc, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size32, 0x11, Instruction.Adc, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, 0x11, Instruction.Adc, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(0x12, Instruction.Adc, OperandFormat.Gb, OperandFormat.Eb),
            new InstructionRepresentation(RexPrefix.W, 0x12, Instruction.Adc, OperandFormat.Gb, OperandFormat.Eb),
            new InstructionRepresentation(OperandSize.Size16, 0x13, Instruction.Adc, OperandFormat.Gw, OperandFormat.Ew),
            new InstructionRepresentation(OperandSize.Size32, 0x13, Instruction.Adc, OperandFormat.Gd, OperandFormat.Ed),
            new InstructionRepresentation(RexPrefix.W, 0x13, Instruction.Adc, OperandFormat.Gq, OperandFormat.Eq),

            new InstructionRepresentation(0x04, Instruction.Add, OperandFormat.AL, OperandFormat.Ib),
            new InstructionRepresentation(OperandSize.Size16, 0x05, Instruction.Add, OperandFormat.AX, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size32, 0x05, Instruction.Add, OperandFormat.EAX, OperandFormat.Id),
            new InstructionRepresentation(RexPrefix.W, 0x05, Instruction.Add, OperandFormat.RAX, OperandFormat.Id),
            new InstructionRepresentation(InstructionPrefixes.Lock, 0x80, 0, Instruction.Add, OperandFormat.Eb, OperandFormat.Ib),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, 0x80, 0, Instruction.Add, OperandFormat.Eb, OperandFormat.Ib),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size16, 0x81, 0, Instruction.Add, OperandFormat.Ew, OperandFormat.Iw),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size32, 0x81, 0, Instruction.Add, OperandFormat.Ed, OperandFormat.Id),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, 0x81, 0, Instruction.Add, OperandFormat.Eq, OperandFormat.Id),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size16, 0x83, 0, Instruction.Add, OperandFormat.Ew, OperandFormat.Ib),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size32, 0x83, 0, Instruction.Add, OperandFormat.Ed, OperandFormat.Ib),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, 0x83, 0, Instruction.Add, OperandFormat.Eq, OperandFormat.Ib),
            new InstructionRepresentation(InstructionPrefixes.Lock, 0x00, Instruction.Add, OperandFormat.Eb, OperandFormat.Gb),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, 0x0, Instruction.Add, OperandFormat.Eb, OperandFormat.Gb),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size16, 0x01, Instruction.Add, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size32, 0x01, Instruction.Add, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, 0x01, Instruction.Add, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(0x02, Instruction.Add, OperandFormat.Gb, OperandFormat.Eb),
            new InstructionRepresentation(RexPrefix.W, 0x02, Instruction.Add, OperandFormat.Gb, OperandFormat.Eb),
            new InstructionRepresentation(OperandSize.Size16, 0x03, Instruction.Add, OperandFormat.Gw, OperandFormat.Ew),
            new InstructionRepresentation(OperandSize.Size32, 0x03, Instruction.Add, OperandFormat.Gd, OperandFormat.Ed),
            new InstructionRepresentation(RexPrefix.W, 0x03, Instruction.Add, OperandFormat.Gq, OperandFormat.Eq),

            new InstructionRepresentation(0x24, Instruction.And, OperandFormat.AL, OperandFormat.Ib),
            new InstructionRepresentation(OperandSize.Size16, 0x25, Instruction.And, OperandFormat.AX, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size32, 0x25, Instruction.And, OperandFormat.EAX, OperandFormat.Id),
            new InstructionRepresentation(RexPrefix.W, 0x25, Instruction.And, OperandFormat.RAX, OperandFormat.Id),
            new InstructionRepresentation(InstructionPrefixes.Lock, 0x80, 4, Instruction.And, OperandFormat.Eb, OperandFormat.Ib),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, 0x80, 4, Instruction.And, OperandFormat.Eb, OperandFormat.Ib),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size16, 0x81, 4, Instruction.And, OperandFormat.Ew, OperandFormat.Iw),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size32, 0x81, 4, Instruction.And, OperandFormat.Ed, OperandFormat.Id),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, 0x81, 4, Instruction.And, OperandFormat.Eq, OperandFormat.Id),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size16, 0x83, 4, Instruction.And, OperandFormat.Ew, OperandFormat.Ib),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size32, 0x83, 4, Instruction.And, OperandFormat.Ed, OperandFormat.Ib),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, 0x83, 4, Instruction.And, OperandFormat.Eq, OperandFormat.Ib),
            new InstructionRepresentation(InstructionPrefixes.Lock, 0x20, Instruction.And, OperandFormat.Eb, OperandFormat.Gb),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, 0x20, Instruction.And, OperandFormat.Eb, OperandFormat.Gb),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size16, 0x21, Instruction.And, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size32, 0x21, Instruction.And, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, 0x21, Instruction.And, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(0x22, Instruction.And, OperandFormat.Gb, OperandFormat.Eb),
            new InstructionRepresentation(RexPrefix.W, 0x22, Instruction.And, OperandFormat.Gb, OperandFormat.Eb),
            new InstructionRepresentation(OperandSize.Size16, 0x23, Instruction.And, OperandFormat.Gw, OperandFormat.Ew),
            new InstructionRepresentation(OperandSize.Size32, 0x23, Instruction.And, OperandFormat.Gd, OperandFormat.Ed),
            new InstructionRepresentation(RexPrefix.W, 0x23, Instruction.And, OperandFormat.Gq, OperandFormat.Eq),

            new InstructionRepresentation(Compatibility.NotEncodable64, 0x63, Instruction.Arpl, OperandFormat.Ew, OperandFormat.Gw)
        };

        public static InstructionRepresentation[] B =
        {
            new InstructionRepresentation(Compatibility.Invalid64, OperandSize.Size16, 0x62, Instruction.Bound, OperandFormat.Gw, OperandFormat.Ew),
            new InstructionRepresentation(Compatibility.Invalid64, OperandSize.Size32, 0x62, Instruction.Bound, OperandFormat.Gd, OperandFormat.Ed),

            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0xBC }, Instruction.Bsf, OperandFormat.Gw, OperandFormat.Ew),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0xBC }, Instruction.Bsf, OperandFormat.Gd, OperandFormat.Ed),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0xBC }, Instruction.Bsf, OperandFormat.Gq, OperandFormat.Eq),

            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0xBD }, Instruction.Bsr, OperandFormat.Gw, OperandFormat.Ew),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0xBD }, Instruction.Bsr, OperandFormat.Gd, OperandFormat.Ed),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0xBD }, Instruction.Bsr, OperandFormat.Gq, OperandFormat.Eq),

            // expanded from intel table
            new InstructionRepresentation(new byte[] { 0x0F, 0xC8 }, Instruction.Bswap, Register.Eax),
            new InstructionRepresentation(new byte[] { 0x0F, 0xC9 }, Instruction.Bswap, Register.Ecx),
            new InstructionRepresentation(new byte[] { 0x0F, 0xCA }, Instruction.Bswap, Register.Edx),
            new InstructionRepresentation(new byte[] { 0x0F, 0xCB }, Instruction.Bswap, Register.Ebx),
            new InstructionRepresentation(new byte[] { 0x0F, 0xCC }, Instruction.Bswap, Register.Esp),
            new InstructionRepresentation(new byte[] { 0x0F, 0xCD }, Instruction.Bswap, Register.Ebp),
            new InstructionRepresentation(new byte[] { 0x0F, 0xCE }, Instruction.Bswap, Register.Esi),
            new InstructionRepresentation(new byte[] { 0x0F, 0xCF }, Instruction.Bswap, Register.Edi),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0xC8 }, Instruction.Bswap, Register.Rax),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0xC9 }, Instruction.Bswap, Register.Rcx),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0xCA }, Instruction.Bswap, Register.Rdx),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0xCB }, Instruction.Bswap, Register.Rbx),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0xCC }, Instruction.Bswap, Register.Rsp),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0xCD }, Instruction.Bswap, Register.Rbp),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0xCE }, Instruction.Bswap, Register.Rsi),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0xCF }, Instruction.Bswap, Register.Rdi),
            new InstructionRepresentation(RexPrefix.B, new byte[] { 0x0F, 0xC8 }, Instruction.Bswap, Register.R8D),
            new InstructionRepresentation(RexPrefix.B, new byte[] { 0x0F, 0xC9 }, Instruction.Bswap, Register.R9D),
            new InstructionRepresentation(RexPrefix.B, new byte[] { 0x0F, 0xCA }, Instruction.Bswap, Register.R10D),
            new InstructionRepresentation(RexPrefix.B, new byte[] { 0x0F, 0xCB }, Instruction.Bswap, Register.R11D),
            new InstructionRepresentation(RexPrefix.B, new byte[] { 0x0F, 0xCC }, Instruction.Bswap, Register.R12D),
            new InstructionRepresentation(RexPrefix.B, new byte[] { 0x0F, 0xCD }, Instruction.Bswap, Register.R13D),
            new InstructionRepresentation(RexPrefix.B, new byte[] { 0x0F, 0xCE }, Instruction.Bswap, Register.R14D),
            new InstructionRepresentation(RexPrefix.B, new byte[] { 0x0F, 0xCF }, Instruction.Bswap, Register.R15D),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, new byte[] { 0x0F, 0xC8 }, Instruction.Bswap, Register.R8),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, new byte[] { 0x0F, 0xC9 }, Instruction.Bswap, Register.R9),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, new byte[] { 0x0F, 0xCA }, Instruction.Bswap, Register.R10),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, new byte[] { 0x0F, 0xCB }, Instruction.Bswap, Register.R11),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, new byte[] { 0x0F, 0xCC }, Instruction.Bswap, Register.R12),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, new byte[] { 0x0F, 0xCD }, Instruction.Bswap, Register.R13),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, new byte[] { 0x0F, 0xCE }, Instruction.Bswap, Register.R14),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, new byte[] { 0x0F, 0xCF }, Instruction.Bswap, Register.R15),

            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0xA3 }, Instruction.Bt, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0xA3 }, Instruction.Bt, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0xA3 }, Instruction.Bt, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0xBA }, 4, Instruction.Bt, OperandFormat.Ew, OperandFormat.Ib),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0xBA }, 4, Instruction.Bt, OperandFormat.Ed, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0xBA }, 4, Instruction.Bt, OperandFormat.Eq, OperandFormat.Ib),

            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size16, new byte[] { 0x0F, 0xBB }, Instruction.Btc, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size32, new byte[] { 0x0F, 0xBB }, Instruction.Btc, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, new byte[] { 0x0F, 0xBB }, Instruction.Btc, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size16, new byte[] { 0x0F, 0xBA }, 7, Instruction.Btc, OperandFormat.Ew, OperandFormat.Ib),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size32, new byte[] { 0x0F, 0xBA }, 7, Instruction.Btc, OperandFormat.Ed, OperandFormat.Ib),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, new byte[] { 0x0F, 0xBA }, 7, Instruction.Btc, OperandFormat.Eq, OperandFormat.Ib),

            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size16, new byte[] { 0x0F, 0xB3 }, Instruction.Btr, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size32, new byte[] { 0x0F, 0xB3 }, Instruction.Btr, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, new byte[] { 0x0F, 0xB3 }, Instruction.Btr, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size16, new byte[] { 0x0F, 0xBA }, 6, Instruction.Btr, OperandFormat.Ew, OperandFormat.Ib),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size32, new byte[] { 0x0F, 0xBA }, 6, Instruction.Btr, OperandFormat.Ed, OperandFormat.Ib),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, new byte[] { 0x0F, 0xBA }, 6, Instruction.Btr, OperandFormat.Eq, OperandFormat.Ib),

            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size16, new byte[] { 0x0F, 0xAB }, Instruction.Bts, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size32, new byte[] { 0x0F, 0xAB }, Instruction.Bts, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, new byte[] { 0x0F, 0xAB }, Instruction.Bts, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size16, new byte[] { 0x0F, 0xBA }, 5, Instruction.Bts, OperandFormat.Ew, OperandFormat.Ib),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size32, new byte[] { 0x0F, 0xBA }, 5, Instruction.Bts, OperandFormat.Ed, OperandFormat.Ib),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, new byte[] { 0x0F, 0xBA }, 5, Instruction.Bts, OperandFormat.Eq, OperandFormat.Ib)
        };

        public static InstructionRepresentation[] C =
        {
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size16, 0xE8, Instruction.Call, OperandFormat.Jw),
            new InstructionRepresentation(OperandSize.Size32, 0xE8, Instruction.Call, OperandFormat.Jd),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size16, 0xFF, 2, Instruction.Call, OperandFormat.Ew),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size32, 0xFF, 2, Instruction.Call, OperandFormat.Ed),
            new InstructionRepresentation(Compatibility.NotEncodable32, 0xFF, 2, Instruction.Call, OperandFormat.Eq),
            new InstructionRepresentation(Compatibility.Invalid64, OperandSize.Size16, 0x9A, Instruction.Call, OperandFormat.Aww),
            new InstructionRepresentation(Compatibility.Invalid64, OperandSize.Size32, 0x9A, Instruction.Call, OperandFormat.Awd),
            new InstructionRepresentation(OperandSize.Size16, 0xFF, 3, Instruction.Call, OperandFormat.Md),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size32, 0xFF, 3, Instruction.Call, OperandFormat.Mf),
            new InstructionRepresentation(RexPrefix.W, 0xFF, 3, Instruction.Call, OperandFormat.Mt),

            new InstructionRepresentation(OperandSize.Size16, 0x98, Instruction.Cbw),
            new InstructionRepresentation(OperandSize.Size32, 0x98, Instruction.Cwde),
            new InstructionRepresentation(RexPrefix.W, 0x98, Instruction.Cdqe),

            new InstructionRepresentation(0xF8, Instruction.Clc),

            new InstructionRepresentation(0xFC, Instruction.Cld),

            new InstructionRepresentation(0xFA, Instruction.Cli),

            new InstructionRepresentation(new byte[] { 0x0f, 0x06 }, Instruction.Clts),

            new InstructionRepresentation(0xF5, Instruction.Cmc),

            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x47 }, Instruction.Cmova, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x47 }, Instruction.Cmova, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x47 }, Instruction.Cmova, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x43 }, Instruction.Cmovae, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x43 }, Instruction.Cmovae, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x43 }, Instruction.Cmovae, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x42 }, Instruction.Cmovb, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x42 }, Instruction.Cmovb, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x42 }, Instruction.Cmovb, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x46 }, Instruction.Cmovbe, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x46 }, Instruction.Cmovbe, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x46 }, Instruction.Cmovbe, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x42 }, Instruction.Cmovc, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x42 }, Instruction.Cmovc, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x42 }, Instruction.Cmovc, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x44 }, Instruction.Cmove, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x44 }, Instruction.Cmove, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x44 }, Instruction.Cmove, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x4F }, Instruction.Cmovg, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x4F }, Instruction.Cmovg, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x4F }, Instruction.Cmovg, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x4D }, Instruction.Cmovge, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x4D }, Instruction.Cmovge, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x4D }, Instruction.Cmovge, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x4C }, Instruction.Cmovl, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x4C }, Instruction.Cmovl, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x4C }, Instruction.Cmovl, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x4E }, Instruction.Cmovle, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x4E }, Instruction.Cmovle, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x4E }, Instruction.Cmovle, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x46 }, Instruction.Cmovna, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x46 }, Instruction.Cmovna, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x46 }, Instruction.Cmovna, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x42 }, Instruction.Cmovnae, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x42 }, Instruction.Cmovnae, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x42 }, Instruction.Cmovnae, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x43 }, Instruction.Cmovnb, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x43 }, Instruction.Cmovnb, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x43 }, Instruction.Cmovnb, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x47 }, Instruction.Cmovnbe, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x47 }, Instruction.Cmovnbe, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x47 }, Instruction.Cmovnbe, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x43 }, Instruction.Cmovnc, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x43 }, Instruction.Cmovnc, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x43 }, Instruction.Cmovnc, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x45 }, Instruction.Cmovne, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x45 }, Instruction.Cmovne, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x45 }, Instruction.Cmovne, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x4E }, Instruction.Cmovng, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x4E }, Instruction.Cmovng, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x4E }, Instruction.Cmovng, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x4C }, Instruction.Cmovnge, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x4C }, Instruction.Cmovnge, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x4C }, Instruction.Cmovnge, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x4D }, Instruction.Cmovnl, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x4D }, Instruction.Cmovnl, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x4D }, Instruction.Cmovnl, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x4F }, Instruction.Cmovnle, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x4F }, Instruction.Cmovnle, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x4F }, Instruction.Cmovnle, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x41 }, Instruction.Cmovno, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x41 }, Instruction.Cmovno, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x41 }, Instruction.Cmovno, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x4B }, Instruction.Cmovnp, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x4B }, Instruction.Cmovnp, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x4B }, Instruction.Cmovnp, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x49 }, Instruction.Cmovns, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x49 }, Instruction.Cmovns, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x49 }, Instruction.Cmovns, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x45 }, Instruction.Cmovnz, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x45 }, Instruction.Cmovnz, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x45 }, Instruction.Cmovnz, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x40 }, Instruction.Cmovo, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x40 }, Instruction.Cmovo, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x40 }, Instruction.Cmovo, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x4A }, Instruction.Cmovp, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x4A }, Instruction.Cmovp, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x4A }, Instruction.Cmovp, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x4A }, Instruction.Cmovpe, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x4A }, Instruction.Cmovpe, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x4A }, Instruction.Cmovpe, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x4B }, Instruction.Cmovpo, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x4B }, Instruction.Cmovpo, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x4B }, Instruction.Cmovpo, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x48 }, Instruction.Cmovs, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x48 }, Instruction.Cmovs, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x48 }, Instruction.Cmovs, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x44 }, Instruction.Cmovz, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x44 }, Instruction.Cmovz, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x44 }, Instruction.Cmovz, OperandFormat.Eq, OperandFormat.Gq),

            new InstructionRepresentation(0x3C, Instruction.Cmp, OperandFormat.AL, OperandFormat.Ib),
            new InstructionRepresentation(OperandSize.Size16, 0x3D, Instruction.Cmp, OperandFormat.AX, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size32, 0x3D, Instruction.Cmp, OperandFormat.EAX, OperandFormat.Id),
            new InstructionRepresentation(RexPrefix.W, 0x3D, Instruction.Cmp, OperandFormat.RAX, OperandFormat.Id),
            new InstructionRepresentation(0x80, 7, Instruction.Cmp, OperandFormat.Eb, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.W, 0x80, 7, Instruction.Cmp, OperandFormat.Eb, OperandFormat.Ib),
            new InstructionRepresentation(OperandSize.Size16, 0x81, 7, Instruction.Cmp, OperandFormat.Ew, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size32, 0x81, 7, Instruction.Cmp, OperandFormat.Ed, OperandFormat.Id),
            new InstructionRepresentation(RexPrefix.W, 0x81, 7, Instruction.Cmp, OperandFormat.Eq, OperandFormat.Id),
            new InstructionRepresentation(OperandSize.Size16, 0x83, 7, Instruction.Cmp, OperandFormat.Ew, OperandFormat.Ib),
            new InstructionRepresentation(OperandSize.Size32, 0x83, 7, Instruction.Cmp, OperandFormat.Ed, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.W, 0x83, 7, Instruction.Cmp, OperandFormat.Eq, OperandFormat.Ib),
            new InstructionRepresentation(0x38, Instruction.Cmp, OperandFormat.Eb, OperandFormat.Gb),
            new InstructionRepresentation(RexPrefix.W, 0x38, Instruction.Cmp, OperandFormat.Eb, OperandFormat.Gb),
            new InstructionRepresentation(OperandSize.Size16, 0x39, Instruction.Cmp, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, 0x39, Instruction.Cmp, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, 0x39, Instruction.Cmp, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(0x3A, Instruction.Cmp, OperandFormat.Gb, OperandFormat.Eb),
            new InstructionRepresentation(RexPrefix.W, 0x3A, Instruction.Cmp, OperandFormat.Gb, OperandFormat.Eb),
            new InstructionRepresentation(OperandSize.Size16, 0x3B, Instruction.Cmp, OperandFormat.Gw, OperandFormat.Ew),
            new InstructionRepresentation(OperandSize.Size32, 0x3B, Instruction.Cmp, OperandFormat.Gd, OperandFormat.Ed),
            new InstructionRepresentation(RexPrefix.W, 0x3B, Instruction.Cmp, OperandFormat.Gq, OperandFormat.Eq),

            new InstructionRepresentation(0xA6, Instruction.Cmpsb),
            new InstructionRepresentation(OperandSize.Size16, 0xA7, Instruction.Cmpsw),
            new InstructionRepresentation(OperandSize.Size32, 0xA7, Instruction.Cmpsd),
            new InstructionRepresentation(RexPrefix.W, 0xA7, Instruction.Cmpsq),

            new InstructionRepresentation(InstructionPrefixes.Lock, new byte[] { 0x0F, 0xB0 }, Instruction.Cmpxchg, OperandFormat.Eb, OperandFormat.Gb),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, new byte[] { 0x0F, 0xB0 }, Instruction.Cmpxchg, OperandFormat.Eb, OperandFormat.Gb),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size16, new byte[] { 0x0F, 0xB1 }, Instruction.Cmpxchg, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size32, new byte[] { 0x0F, 0xB1 }, Instruction.Cmpxchg, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, new byte[] { 0x0F, 0xB1 }, Instruction.Cmpxchg, OperandFormat.Eq, OperandFormat.Gq),

            new InstructionRepresentation(InstructionPrefixes.Lock, new byte[] { 0x0F, 0xC7 }, 1, Instruction.Cmpxchg8b, OperandFormat.Mq),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, new byte[] { 0x0F, 0xC7 }, 1, Instruction.Cmpxchg16b, OperandFormat.Mdq),

            new InstructionRepresentation(new byte[] { 0x0F, 0xA2 }, Instruction.Cpuid),

            new InstructionRepresentation(InstructionPrefixes.RepNZ, new byte[] { 0x0F, 0x38, 0xF0 }, Instruction.Crc32, OperandFormat.Gd, OperandFormat.Eb),
            new InstructionRepresentation(InstructionPrefixes.RepNZ, (RexPrefix)0, new byte[] { 0x0F, 0x38, 0xF0 }, Instruction.Crc32, OperandFormat.Gd, OperandFormat.Eb),
            new InstructionRepresentation(InstructionPrefixes.RepNZ, OperandSize.Size16, new byte[] { 0x0F, 0x38, 0xF1 }, Instruction.Crc32, OperandFormat.Gd, OperandFormat.Ew),
            new InstructionRepresentation(InstructionPrefixes.RepNZ, OperandSize.Size32, new byte[] { 0x0F, 0x38, 0xF1 }, Instruction.Crc32, OperandFormat.Gd, OperandFormat.Ed),
            new InstructionRepresentation(InstructionPrefixes.RepNZ, RexPrefix.W, new byte[] { 0x0F, 0x38, 0xF0 }, Instruction.Crc32, OperandFormat.Gq, OperandFormat.Eb),
            new InstructionRepresentation(InstructionPrefixes.RepNZ, RexPrefix.W, new byte[] { 0x0F, 0x38, 0xF1 }, Instruction.Crc32, OperandFormat.Gq, OperandFormat.Eq),

            new InstructionRepresentation(OperandSize.Size16, 0x99, Instruction.Cwd),
            new InstructionRepresentation(OperandSize.Size32, 0x99, Instruction.Cdq),
            new InstructionRepresentation(RexPrefix.W, 0x99, Instruction.Cqo)
        };

        public static InstructionRepresentation[] D =
        {
            new InstructionRepresentation(Compatibility.Invalid64, 0x27, Instruction.Daa),

            new InstructionRepresentation(Compatibility.Invalid64, 0x2F, Instruction.Das),

            new InstructionRepresentation(InstructionPrefixes.Lock, 0xFE, 1, Instruction.Dec, OperandFormat.Eb),
            new InstructionRepresentation(InstructionPrefixes.Lock, (RexPrefix)0, 0xFE, 1, Instruction.Dec, OperandFormat.Eb),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size16, 0xFF, 1, Instruction.Dec, OperandFormat.Ew),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size32, 0xFF, 1, Instruction.Dec, OperandFormat.Ed),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, 0xFF, 1, Instruction.Dec, OperandFormat.Eq),
            // expanded from intel table
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size16, 0x48, Instruction.Dec, Register.Ax),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size16, 0x49, Instruction.Dec, Register.Cx),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size16, 0x4A, Instruction.Dec, Register.Dx),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size16, 0x4B, Instruction.Dec, Register.Bx),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size16, 0x4C, Instruction.Dec, Register.Sp),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size16, 0x4D, Instruction.Dec, Register.Bp),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size16, 0x4E, Instruction.Dec, Register.Si),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size16, 0x4F, Instruction.Dec, Register.Di),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size32, 0x48, Instruction.Dec, Register.Eax),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size32, 0x49, Instruction.Dec, Register.Ecx),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size32, 0x4A, Instruction.Dec, Register.Edx),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size32, 0x4B, Instruction.Dec, Register.Ebx),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size32, 0x4C, Instruction.Dec, Register.Esp),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size32, 0x4D, Instruction.Dec, Register.Ebp),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size32, 0x4E, Instruction.Dec, Register.Esi),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size32, 0x4F, Instruction.Dec, Register.Edi),

            new InstructionRepresentation(0xF6, 6, Instruction.Div, OperandFormat.Eb),
            new InstructionRepresentation((RexPrefix)0, 0xF6, 6, Instruction.Div, OperandFormat.Eb),
            new InstructionRepresentation(OperandSize.Size16, 0xF7, 6, Instruction.Div, OperandFormat.Ew),
            new InstructionRepresentation(OperandSize.Size32, 0xF7, 6, Instruction.Div, OperandFormat.Ed),
            new InstructionRepresentation(RexPrefix.W, 0xF7, 6, Instruction.Div, OperandFormat.Eq)
        };

        public static InstructionRepresentation[] E =
        {
            new InstructionRepresentation(new byte[] { 0x0f, 0x77 }, Instruction.Emms),

            new InstructionRepresentation(0xC8, Instruction.Enter, OperandFormat.Iw, OperandFormat.Ib)
        };

        public static InstructionRepresentation[] H =
        {
            new InstructionRepresentation(0xF4, Instruction.Hlt)
        };

        public static InstructionRepresentation[] I =
        {
            new InstructionRepresentation(0xF6, 7, Instruction.Idiv, OperandFormat.Eb),
            new InstructionRepresentation((RexPrefix)0, 0xF6, 7, Instruction.Idiv, OperandFormat.Eb),
            new InstructionRepresentation(OperandSize.Size16, 0xF7, 7, Instruction.Idiv, OperandFormat.Ew),
            new InstructionRepresentation(OperandSize.Size32, 0xF7, 7, Instruction.Idiv, OperandFormat.Ed),
            new InstructionRepresentation(RexPrefix.W, 0xF7, 7, Instruction.Idiv, OperandFormat.Eq),

            new InstructionRepresentation(0xF6, 5, Instruction.Imul, OperandFormat.Eb),
            new InstructionRepresentation(OperandSize.Size16, 0xF7, 5, Instruction.Imul, OperandFormat.Ew),
            new InstructionRepresentation(OperandSize.Size32, 0xF7, 5, Instruction.Imul, OperandFormat.Ed),
            new InstructionRepresentation(RexPrefix.W, 0xF7, 5, Instruction.Imul, OperandFormat.Eq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0xAF }, Instruction.Imul, OperandFormat.Gw, OperandFormat.Ew),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0xAF }, Instruction.Imul, OperandFormat.Gd, OperandFormat.Ed),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0xAF }, Instruction.Imul, OperandFormat.Gq, OperandFormat.Eq),
            new InstructionRepresentation(OperandSize.Size16, 0x6B, Instruction.Imul, OperandFormat.Gw, OperandFormat.Ew, OperandFormat.Ib),
            new InstructionRepresentation(OperandSize.Size32, 0x6B, Instruction.Imul, OperandFormat.Gd, OperandFormat.Ed, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.W, 0x6B, Instruction.Imul, OperandFormat.Gq, OperandFormat.Eq, OperandFormat.Ib),
            new InstructionRepresentation(OperandSize.Size16, 0x69, Instruction.Imul, OperandFormat.Gw, OperandFormat.Ew, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size32, 0x69, Instruction.Imul, OperandFormat.Gd, OperandFormat.Ed, OperandFormat.Id),
            new InstructionRepresentation(RexPrefix.W, 0x69, Instruction.Imul, OperandFormat.Gq, OperandFormat.Eq, OperandFormat.Id),

            new InstructionRepresentation(0xE4, Instruction.In, OperandFormat.AL, OperandFormat.Ib),
            new InstructionRepresentation(OperandSize.Size16, 0xE5, Instruction.In, OperandFormat.AX, OperandFormat.Ib),
            new InstructionRepresentation(OperandSize.Size32, 0xE5, Instruction.In, OperandFormat.EAX, OperandFormat.Ib),
            new InstructionRepresentation(0xEC, Instruction.In, OperandFormat.AL, OperandFormat.DX),
            new InstructionRepresentation(OperandSize.Size16, 0xED, Instruction.In, OperandFormat.AX, OperandFormat.DX),
            new InstructionRepresentation(OperandSize.Size32, 0xED, Instruction.In, OperandFormat.EAX, OperandFormat.DX),

            new InstructionRepresentation(InstructionPrefixes.Lock, 0xFE, 0, Instruction.Inc, OperandFormat.Eb),
            new InstructionRepresentation(InstructionPrefixes.Lock, (RexPrefix)0, 0xFE, 0, Instruction.Inc, OperandFormat.Eb),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size16, 0xFF, 0, Instruction.Inc, OperandFormat.Ew),
            new InstructionRepresentation(InstructionPrefixes.Lock, OperandSize.Size32, 0xFF, 0, Instruction.Inc, OperandFormat.Ed),
            new InstructionRepresentation(InstructionPrefixes.Lock, RexPrefix.W, 0xFF, 0, Instruction.Inc, OperandFormat.Eq),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size16, 0x40, Instruction.Inc, Register.Ax),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size16, 0x41, Instruction.Inc, Register.Cx),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size16, 0x42, Instruction.Inc, Register.Dx),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size16, 0x43, Instruction.Inc, Register.Bx),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size16, 0x44, Instruction.Inc, Register.Sp),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size16, 0x45, Instruction.Inc, Register.Bp),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size16, 0x46, Instruction.Inc, Register.Si),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size16, 0x47, Instruction.Inc, Register.Di),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size32, 0x40, Instruction.Inc, Register.Eax),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size32, 0x41, Instruction.Inc, Register.Ecx),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size32, 0x42, Instruction.Inc, Register.Edx),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size32, 0x43, Instruction.Inc, Register.Ebx),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size32, 0x44, Instruction.Inc, Register.Esp),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size32, 0x45, Instruction.Inc, Register.Ebp),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size32, 0x46, Instruction.Inc, Register.Esi),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size32, 0x47, Instruction.Inc, Register.Edi),

            new InstructionRepresentation(0x6C, Instruction.Insb),
            new InstructionRepresentation(OperandSize.Size16, 0x6D, Instruction.Insw),
            new InstructionRepresentation(OperandSize.Size32, 0x6D, Instruction.Insd),

            new InstructionRepresentation(0xCC, Instruction.Int, OperandFormat.Three),
            new InstructionRepresentation(0xCD, Instruction.Int, OperandFormat.Ib),
            new InstructionRepresentation(0xCE, Instruction.Into),

            new InstructionRepresentation(new byte[] { 0x0F, 0x08 }, Instruction.Invd),

            new InstructionRepresentation(new byte[] { 0x0F, 0x01 }, 7, Instruction.Invlpg, OperandFormat.Mb),

            new InstructionRepresentation(OperandSize.Size16, 0xCF, Instruction.Iret),
            new InstructionRepresentation(OperandSize.Size32, 0xCF, Instruction.Iretd),
            new InstructionRepresentation(RexPrefix.W, 0xCF, Instruction.Iretq)
        };

        public static InstructionRepresentation[] J =
        {
            new InstructionRepresentation(0x77, Instruction.Ja, OperandFormat.Jb), // JNBE
            new InstructionRepresentation(0x73, Instruction.Jae, OperandFormat.Jb), // JNB, JNC
            new InstructionRepresentation(0x72, Instruction.Jb, OperandFormat.Jb), // JC, JNAE
            new InstructionRepresentation(0x76, Instruction.Jbe, OperandFormat.Jb), // JNA
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size16, 0xE3, Instruction.Jcxz, OperandFormat.Jb),
            new InstructionRepresentation(OperandSize.Size32, 0xE3, Instruction.Jecxz, OperandFormat.Jb),
            new InstructionRepresentation((RexPrefix)0, 0xE3, Instruction.Jrcxz, OperandFormat.Jb),
            new InstructionRepresentation(0x74, Instruction.Je, OperandFormat.Jb), // JZ
            new InstructionRepresentation(0x7F, Instruction.Jg, OperandFormat.Jb), // JNLE
            new InstructionRepresentation(0x7D, Instruction.Jge, OperandFormat.Jb), // JNL
            new InstructionRepresentation(0x7C, Instruction.Jl, OperandFormat.Jb), // JNGE
            new InstructionRepresentation(0x7E, Instruction.Jle, OperandFormat.Jb), // JNG
            new InstructionRepresentation(0x75, Instruction.Jne, OperandFormat.Jb), // JNZ
            new InstructionRepresentation(0x71, Instruction.Jno, OperandFormat.Jb),
            new InstructionRepresentation(0x79, Instruction.Jns, OperandFormat.Jb),
            new InstructionRepresentation(0x70, Instruction.Jo, OperandFormat.Jb),
            new InstructionRepresentation(0x7A, Instruction.Jpe, OperandFormat.Jb), // JP
            new InstructionRepresentation(0x7B, Instruction.Jpo, OperandFormat.Jb), // JNP
            new InstructionRepresentation(0x78, Instruction.Js, OperandFormat.Jb),
            new InstructionRepresentation(Compatibility.NotSupported64, OperandSize.Size16, new byte[] { 0x0F, 0x87 }, Instruction.Ja, OperandFormat.Jw), // JNBE
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x87 }, Instruction.Ja, OperandFormat.Jd), // JNBE
            new InstructionRepresentation(Compatibility.NotSupported64, OperandSize.Size16, new byte[] { 0x0F, 0x83 }, Instruction.Jae, OperandFormat.Jw), // JNB, JNC
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x83 }, Instruction.Jae, OperandFormat.Jd), // JNB, JNC
            new InstructionRepresentation(Compatibility.NotSupported64, OperandSize.Size16, new byte[] { 0x0F, 0x82 }, Instruction.Jb, OperandFormat.Jw), // JC, JNAE
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x82 }, Instruction.Jb, OperandFormat.Jd), // JC, JNAE
            new InstructionRepresentation(Compatibility.NotSupported64, OperandSize.Size16, new byte[] { 0x0F, 0x86 }, Instruction.Jbe, OperandFormat.Jw), // JNA
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x86 }, Instruction.Jbe, OperandFormat.Jd), // JNA
            new InstructionRepresentation(Compatibility.NotSupported64, OperandSize.Size16, new byte[] { 0x0F, 0x84 }, Instruction.Je, OperandFormat.Jw), // JZ
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x84 }, Instruction.Je, OperandFormat.Jd), // JZ
            new InstructionRepresentation(Compatibility.NotSupported64, OperandSize.Size16, new byte[] { 0x0F, 0x8F }, Instruction.Jg, OperandFormat.Jw), // JNLE
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x8F }, Instruction.Jg, OperandFormat.Jd), // JNLE
            new InstructionRepresentation(Compatibility.NotSupported64, OperandSize.Size16, new byte[] { 0x0F, 0x8D }, Instruction.Jge, OperandFormat.Jw), // JNL
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x8D }, Instruction.Jge, OperandFormat.Jd), // JNL
            new InstructionRepresentation(Compatibility.NotSupported64, OperandSize.Size16, new byte[] { 0x0F, 0x8C }, Instruction.Jl, OperandFormat.Jw), // JNGE
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x8C }, Instruction.Jl, OperandFormat.Jd), // JNGE
            new InstructionRepresentation(Compatibility.NotSupported64, OperandSize.Size16, new byte[] { 0x0F, 0x8E }, Instruction.Jle, OperandFormat.Jw), // JNG
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x8E }, Instruction.Jle, OperandFormat.Jd), // JNG
            new InstructionRepresentation(Compatibility.NotSupported64, OperandSize.Size16, new byte[] { 0x0F, 0x85 }, Instruction.Jne, OperandFormat.Jw), // JNZ
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x85 }, Instruction.Jne, OperandFormat.Jd), // JNZ
            new InstructionRepresentation(Compatibility.NotSupported64, OperandSize.Size16, new byte[] { 0x0F, 0x81 }, Instruction.Jno, OperandFormat.Jw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x81}, Instruction.Jno, OperandFormat.Jd),
            new InstructionRepresentation(Compatibility.NotSupported64, OperandSize.Size16, new byte[] { 0x0F, 0x89 }, Instruction.Jns, OperandFormat.Jw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x89 }, Instruction.Jns, OperandFormat.Jd),
            new InstructionRepresentation(Compatibility.NotSupported64, OperandSize.Size16, new byte[] { 0x0F, 0x80 }, Instruction.Jo, OperandFormat.Jw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x80 }, Instruction.Jo, OperandFormat.Jd),
            new InstructionRepresentation(Compatibility.NotSupported64, OperandSize.Size16, new byte[] { 0x0F, 0x8A }, Instruction.Jpe, OperandFormat.Jw), // Jp
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x8A }, Instruction.Jpe, OperandFormat.Jd), // Jp
            new InstructionRepresentation(Compatibility.NotSupported64, OperandSize.Size16, new byte[] { 0x0F, 0x8B }, Instruction.Jpo, OperandFormat.Jw), // Jnp
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x8B }, Instruction.Jpo, OperandFormat.Jd), // Jnp
            new InstructionRepresentation(Compatibility.NotSupported64, OperandSize.Size16, new byte[] { 0x0F, 0x88 }, Instruction.Js, OperandFormat.Jw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x88 }, Instruction.Js, OperandFormat.Jd),

            new InstructionRepresentation(0xEB, Instruction.Jmp, OperandFormat.Jb),
            new InstructionRepresentation(Compatibility.NotSupported64, OperandSize.Size16, 0xE9, Instruction.Jmp, OperandFormat.Jw),
            new InstructionRepresentation(OperandSize.Size32, 0xE9, Instruction.Jmp, OperandFormat.Jd),
            new InstructionRepresentation(Compatibility.NotSupported64, OperandSize.Size16, 0xFF, 4, Instruction.Jmp, OperandFormat.Ew),
            new InstructionRepresentation(Compatibility.NotSupported64, OperandSize.Size32, 0xFF, 4, Instruction.Jmp, OperandFormat.Ed),
            new InstructionRepresentation((RexPrefix)0, 0xFF, 4, Instruction.Jmp, OperandFormat.Eq),
            new InstructionRepresentation(Compatibility.Invalid64, OperandSize.Size16, 0xEA, Instruction.Jmp, OperandFormat.Aww),
            new InstructionRepresentation(Compatibility.Invalid64, OperandSize.Size32, 0xEA, Instruction.Jmp, OperandFormat.Awd),
            new InstructionRepresentation(OperandSize.Size16, 0xFF, 5, Instruction.Jmp, OperandFormat.Md),
            new InstructionRepresentation(OperandSize.Size32, 0xFF, 5, Instruction.Jmp, OperandFormat.Mf),
            new InstructionRepresentation(RexPrefix.W, 0xFF, 5, Instruction.Jmp, OperandFormat.Mt)
        };

        public static InstructionRepresentation[] L =
        {
            new InstructionRepresentation(Compatibility.Invalid64, 0x9F, Instruction.Lahf),

            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x02 }, Instruction.Lar, OperandFormat.Gw, OperandFormat.Ew),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x02 }, Instruction.Lar, OperandFormat.Gd, OperandFormat.Ew), // TODO: last param is r32/m16

            new InstructionRepresentation(Compatibility.Invalid64, OperandSize.Size16, 0xc5, Instruction.Lds, OperandFormat.Gw, OperandFormat.Md),
            new InstructionRepresentation(Compatibility.Invalid64, OperandSize.Size32, 0xc5, Instruction.Lds, OperandFormat.Gd, OperandFormat.Mf),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0xB2 }, Instruction.Lss, OperandFormat.Gw, OperandFormat.Md),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0xB2 }, Instruction.Lss, OperandFormat.Gd, OperandFormat.Mf),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0xB2 }, Instruction.Lss, OperandFormat.Gq, OperandFormat.Mt),
            new InstructionRepresentation(Compatibility.Invalid64, OperandSize.Size16, 0xc4, Instruction.Les, OperandFormat.Gw, OperandFormat.Md),
            new InstructionRepresentation(Compatibility.Invalid64, OperandSize.Size32, 0xc4, Instruction.Les, OperandFormat.Gd, OperandFormat.Mf),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0xB4 }, Instruction.Lfs, OperandFormat.Gw, OperandFormat.Md),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0xB4 }, Instruction.Lfs, OperandFormat.Gd, OperandFormat.Mf),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0xB4 }, Instruction.Lfs, OperandFormat.Gq, OperandFormat.Mt),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0xB5 }, Instruction.Lgs, OperandFormat.Gw, OperandFormat.Md),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0xB5 }, Instruction.Lgs, OperandFormat.Gd, OperandFormat.Mf),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0xB5 }, Instruction.Lgs, OperandFormat.Gq, OperandFormat.Mt),

            new InstructionRepresentation(OperandSize.Size16, 0x8D, Instruction.Lea, OperandFormat.Gw, OperandFormat.M),
            new InstructionRepresentation(OperandSize.Size32, 0x8D, Instruction.Lea, OperandFormat.Gd, OperandFormat.M),
            new InstructionRepresentation(RexPrefix.W, 0x8D, Instruction.Lea, OperandFormat.Gq, OperandFormat.M),

            // TODO: the 16-bit version should be prefixed with addr16 to distinguish.
            new InstructionRepresentation(OperandSize.Size16, 0xC9, Instruction.Leave),
            new InstructionRepresentation(Compatibility.NotEncodable64, OperandSize.Size32, 0xC9, Instruction.Leave),
            new InstructionRepresentation(Compatibility.NotEncodable32, 0xC9, Instruction.Leave),

            new InstructionRepresentation(new byte[] { 0x0F, 0xAE, 0xE8 }, Instruction.Lfence),

            new InstructionRepresentation(Compatibility.NotEncodable64, new byte[] { 0x0F, 0x01 }, 2, Instruction.Lgdt, OperandFormat.Mf),
            new InstructionRepresentation(Compatibility.NotEncodable64, new byte[] { 0x0F, 0x01 }, 3, Instruction.Lidt, OperandFormat.Mf),
            new InstructionRepresentation(Compatibility.NotEncodable32, new byte[] { 0x0F, 0x01 }, 2, Instruction.Lgdt, OperandFormat.Mt),
            new InstructionRepresentation(Compatibility.NotEncodable32, new byte[] { 0x0F, 0x01 }, 3, Instruction.Lidt, OperandFormat.Mt),

            new InstructionRepresentation(new byte[] { 0x0F, 0x00 }, 2, Instruction.Lldt, OperandFormat.Ew),

            new InstructionRepresentation(new byte[] { 0x0F, 0x01 }, 6, Instruction.Lmsw, OperandFormat.Ew),

            new InstructionRepresentation(0xAC, Instruction.Lodsb),
            new InstructionRepresentation(OperandSize.Size16, 0xAD, Instruction.Lodsw),
            new InstructionRepresentation(OperandSize.Size32, 0xAD, Instruction.Lodsd),
            new InstructionRepresentation(RexPrefix.W, 0xAD, Instruction.Lodsq),

            new InstructionRepresentation(0xE2, Instruction.Loop, OperandFormat.Jb),
            new InstructionRepresentation(0xE1, Instruction.Loope, OperandFormat.Jb),
            new InstructionRepresentation(0xE0, Instruction.Loopne, OperandFormat.Jb),

            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x03 }, Instruction.Lsl, OperandFormat.Gw, OperandFormat.Ew),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x03 }, Instruction.Lsl, OperandFormat.Gd, OperandFormat.Ew), // TODO: last param is r32/m16
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x03 }, Instruction.Lsl, OperandFormat.Gq, OperandFormat.Ew), // TODO: last param is r32/m16

            new InstructionRepresentation(new byte[] { 0x0F, 0x00 }, 3, Instruction.Ltr, OperandFormat.Ew)
        };

        public static InstructionRepresentation[] M =
        {
            new InstructionRepresentation(0x88, Instruction.Mov, OperandFormat.Eb, OperandFormat.Gb),
            new InstructionRepresentation(RexPrefix.W, 0x88, Instruction.Mov, OperandFormat.Eb, OperandFormat.Gb),
            new InstructionRepresentation(OperandSize.Size16, 0x89, Instruction.Mov, OperandFormat.Ew, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, 0x89, Instruction.Mov, OperandFormat.Ed, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, 0x89, Instruction.Mov, OperandFormat.Eq, OperandFormat.Gq),
            new InstructionRepresentation(0x8A, Instruction.Mov, OperandFormat.Gb, OperandFormat.Eb),
            new InstructionRepresentation(RexPrefix.W, 0x8A, Instruction.Mov, OperandFormat.Gb, OperandFormat.Eb),
            new InstructionRepresentation(OperandSize.Size16, 0x8B, Instruction.Mov, OperandFormat.Gw, OperandFormat.Ew),
            new InstructionRepresentation(OperandSize.Size32, 0x8B, Instruction.Mov, OperandFormat.Gd, OperandFormat.Ed),
            new InstructionRepresentation(RexPrefix.W, 0x8B, Instruction.Mov, OperandFormat.Gq, OperandFormat.Eq),
            new InstructionRepresentation(OperandSize.Size16, 0x8C, Instruction.Mov, OperandFormat.Ew, OperandFormat.Sw),
            new InstructionRepresentation(OperandSize.Size32, 0x8C, Instruction.Mov, OperandFormat.Ed, OperandFormat.Sw),
            new InstructionRepresentation(RexPrefix.W, 0x8C, Instruction.Mov, OperandFormat.Eq, OperandFormat.Sw),
            new InstructionRepresentation(OperandSize.Size16, 0x8E, Instruction.Mov, OperandFormat.Sw, OperandFormat.Ew),
            new InstructionRepresentation(OperandSize.Size32, 0x8E, Instruction.Mov, OperandFormat.Sw, OperandFormat.Ed),
            new InstructionRepresentation(RexPrefix.W, 0x8E, Instruction.Mov, OperandFormat.Sw, OperandFormat.Eq),
            new InstructionRepresentation(0xA0, Instruction.Mov, OperandFormat.AL, OperandFormat.Ob),
            new InstructionRepresentation(RexPrefix.W, 0xA0, Instruction.Mov, OperandFormat.AL, OperandFormat.Ob),
            new InstructionRepresentation(OperandSize.Size16, 0xA1, Instruction.Mov, OperandFormat.AX, OperandFormat.Ow),
            new InstructionRepresentation(OperandSize.Size32, 0xA1, Instruction.Mov, OperandFormat.EAX, OperandFormat.Od),
            new InstructionRepresentation(RexPrefix.W, 0xA1, Instruction.Mov, OperandFormat.RAX, OperandFormat.Oq),
            new InstructionRepresentation(0xA2, Instruction.Mov, OperandFormat.Ob, OperandFormat.AL),
            new InstructionRepresentation(RexPrefix.W, 0xA2, Instruction.Mov, OperandFormat.Ob, OperandFormat.AL),
            new InstructionRepresentation(OperandSize.Size16, 0xA3, Instruction.Mov, OperandFormat.Ow, OperandFormat.AX),
            new InstructionRepresentation(OperandSize.Size32, 0xA3, Instruction.Mov, OperandFormat.Od, OperandFormat.EAX),
            new InstructionRepresentation(RexPrefix.W, 0xA3, Instruction.Mov, OperandFormat.Oq, OperandFormat.RAX),

            new InstructionRepresentation(0xB0, Instruction.Mov, Register.Al, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(0xB1, Instruction.Mov, Register.Cl, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(0xB2, Instruction.Mov, Register.Dl, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(0xB3, Instruction.Mov, Register.Bl, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(0xB4, Instruction.Mov, Register.Ah, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(0xB5, Instruction.Mov, Register.Ch, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(0xB6, Instruction.Mov, Register.Dh, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(0xB7, Instruction.Mov, Register.Bh, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.B, 0xB0, Instruction.Mov, Register.R8L, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.B, 0xB1, Instruction.Mov, Register.R9L, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.B, 0xB2, Instruction.Mov, Register.R10L, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.B, 0xB3, Instruction.Mov, Register.R11L, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.B, 0xB4, Instruction.Mov, Register.R12L, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.B, 0xB5, Instruction.Mov, Register.R13L, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.B, 0xB6, Instruction.Mov, Register.R14L, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.B, 0xB7, Instruction.Mov, Register.R15L, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.W, 0xB0, Instruction.Mov, Register.Al, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.W, 0xB1, Instruction.Mov, Register.Cl, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.W, 0xB2, Instruction.Mov, Register.Dl, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.W, 0xB3, Instruction.Mov, Register.Bl, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.W, 0xB4, Instruction.Mov, Register.Spl, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.W, 0xB5, Instruction.Mov, Register.Bpl, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.W, 0xB6, Instruction.Mov, Register.Sil, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.W, 0xB7, Instruction.Mov, Register.Dil, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, 0xB0, Instruction.Mov, Register.R8L, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, 0xB1, Instruction.Mov, Register.R9L, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, 0xB2, Instruction.Mov, Register.R10L, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, 0xB3, Instruction.Mov, Register.R11L, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, 0xB4, Instruction.Mov, Register.R12L, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, 0xB5, Instruction.Mov, Register.R13L, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, 0xB6, Instruction.Mov, Register.R14L, OperandFormat.Register, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, 0xB7, Instruction.Mov, Register.R15L, OperandFormat.Register, OperandFormat.Ib),

            new InstructionRepresentation(OperandSize.Size16, 0xB8, Instruction.Mov, Register.Ax, OperandFormat.Register, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size16, 0xB9, Instruction.Mov, Register.Cx, OperandFormat.Register, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size16, 0xBA, Instruction.Mov, Register.Dx, OperandFormat.Register, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size16, 0xBB, Instruction.Mov, Register.Bx, OperandFormat.Register, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size16, 0xBC, Instruction.Mov, Register.Sp, OperandFormat.Register, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size16, 0xBD, Instruction.Mov, Register.Bp, OperandFormat.Register, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size16, 0xBE, Instruction.Mov, Register.Si, OperandFormat.Register, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size16, 0xBF, Instruction.Mov, Register.Di, OperandFormat.Register, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size16, RexPrefix.B, 0xB8, Instruction.Mov, Register.R8W, OperandFormat.Register, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size16, RexPrefix.B, 0xB9, Instruction.Mov, Register.R9W, OperandFormat.Register, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size16, RexPrefix.B, 0xBA, Instruction.Mov, Register.R10W, OperandFormat.Register, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size16, RexPrefix.B, 0xBB, Instruction.Mov, Register.R11W, OperandFormat.Register, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size16, RexPrefix.B, 0xBC, Instruction.Mov, Register.R12W, OperandFormat.Register, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size16, RexPrefix.B, 0xBD, Instruction.Mov, Register.R13W, OperandFormat.Register, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size16, RexPrefix.B, 0xBE, Instruction.Mov, Register.R14W, OperandFormat.Register, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size16, RexPrefix.B, 0xBF, Instruction.Mov, Register.R15W, OperandFormat.Register, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size32, 0xB8, Instruction.Mov, Register.Eax, OperandFormat.Register, OperandFormat.Id),
            new InstructionRepresentation(OperandSize.Size32, 0xB9, Instruction.Mov, Register.Ecx, OperandFormat.Register, OperandFormat.Id),
            new InstructionRepresentation(OperandSize.Size32, 0xBA, Instruction.Mov, Register.Edx, OperandFormat.Register, OperandFormat.Id),
            new InstructionRepresentation(OperandSize.Size32, 0xBB, Instruction.Mov, Register.Ebx, OperandFormat.Register, OperandFormat.Id),
            new InstructionRepresentation(OperandSize.Size32, 0xBC, Instruction.Mov, Register.Esp, OperandFormat.Register, OperandFormat.Id),
            new InstructionRepresentation(OperandSize.Size32, 0xBD, Instruction.Mov, Register.Ebp, OperandFormat.Register, OperandFormat.Id),
            new InstructionRepresentation(OperandSize.Size32, 0xBE, Instruction.Mov, Register.Esi, OperandFormat.Register, OperandFormat.Id),
            new InstructionRepresentation(OperandSize.Size32, 0xBF, Instruction.Mov, Register.Edi, OperandFormat.Register, OperandFormat.Id),
            new InstructionRepresentation(OperandSize.Size32, RexPrefix.B, 0xB8, Instruction.Mov, Register.R8D, OperandFormat.Register, OperandFormat.Id),
            new InstructionRepresentation(OperandSize.Size32, RexPrefix.B, 0xB9, Instruction.Mov, Register.R9D, OperandFormat.Register, OperandFormat.Id),
            new InstructionRepresentation(OperandSize.Size32, RexPrefix.B, 0xBA, Instruction.Mov, Register.R10D, OperandFormat.Register, OperandFormat.Id),
            new InstructionRepresentation(OperandSize.Size32, RexPrefix.B, 0xBB, Instruction.Mov, Register.R11D, OperandFormat.Register, OperandFormat.Id),
            new InstructionRepresentation(OperandSize.Size32, RexPrefix.B, 0xBC, Instruction.Mov, Register.R12D, OperandFormat.Register, OperandFormat.Id),
            new InstructionRepresentation(OperandSize.Size32, RexPrefix.B, 0xBD, Instruction.Mov, Register.R13D, OperandFormat.Register, OperandFormat.Id),
            new InstructionRepresentation(OperandSize.Size32, RexPrefix.B, 0xBE, Instruction.Mov, Register.R14D, OperandFormat.Register, OperandFormat.Id),
            new InstructionRepresentation(OperandSize.Size32, RexPrefix.B, 0xBF, Instruction.Mov, Register.R15D, OperandFormat.Register, OperandFormat.Id),
            new InstructionRepresentation(RexPrefix.W, 0xB8, Instruction.Mov, Register.Rax, OperandFormat.Register, OperandFormat.Iq),
            new InstructionRepresentation(RexPrefix.W, 0xB9, Instruction.Mov, Register.Rcx, OperandFormat.Register, OperandFormat.Iq),
            new InstructionRepresentation(RexPrefix.W, 0xBA, Instruction.Mov, Register.Rdx, OperandFormat.Register, OperandFormat.Iq),
            new InstructionRepresentation(RexPrefix.W, 0xBB, Instruction.Mov, Register.Rbx, OperandFormat.Register, OperandFormat.Iq),
            new InstructionRepresentation(RexPrefix.W, 0xBC, Instruction.Mov, Register.Rsp, OperandFormat.Register, OperandFormat.Iq),
            new InstructionRepresentation(RexPrefix.W, 0xBD, Instruction.Mov, Register.Rbp, OperandFormat.Register, OperandFormat.Iq),
            new InstructionRepresentation(RexPrefix.W, 0xBE, Instruction.Mov, Register.Rsi, OperandFormat.Register, OperandFormat.Iq),
            new InstructionRepresentation(RexPrefix.W, 0xBF, Instruction.Mov, Register.Rdi, OperandFormat.Register, OperandFormat.Iq),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, 0xB8, Instruction.Mov, Register.R8, OperandFormat.Register, OperandFormat.Iq),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, 0xB9, Instruction.Mov, Register.R9, OperandFormat.Register, OperandFormat.Iq),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, 0xBA, Instruction.Mov, Register.R10, OperandFormat.Register, OperandFormat.Iq),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, 0xBB, Instruction.Mov, Register.R11, OperandFormat.Register, OperandFormat.Iq),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, 0xBC, Instruction.Mov, Register.R12, OperandFormat.Register, OperandFormat.Iq),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, 0xBD, Instruction.Mov, Register.R13, OperandFormat.Register, OperandFormat.Iq),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, 0xBE, Instruction.Mov, Register.R14, OperandFormat.Register, OperandFormat.Iq),
            new InstructionRepresentation(RexPrefix.W | RexPrefix.B, 0xBF, Instruction.Mov, Register.R15, OperandFormat.Register, OperandFormat.Iq),

            new InstructionRepresentation(0xC6, 0, Instruction.Mov, OperandFormat.Eb, OperandFormat.Ib),
            new InstructionRepresentation(RexPrefix.W, 0xC6, 0, Instruction.Mov, OperandFormat.Eb, OperandFormat.Ib),
            new InstructionRepresentation(OperandSize.Size16, 0xC7, 0, Instruction.Mov, OperandFormat.Ew, OperandFormat.Iw),
            new InstructionRepresentation(OperandSize.Size32, 0xC7, 0, Instruction.Mov, OperandFormat.Ed, OperandFormat.Id),
            new InstructionRepresentation(RexPrefix.W, 0xC7, 0, Instruction.Mov, OperandFormat.Eq, OperandFormat.Id),

            new InstructionRepresentation(Compatibility.NotEncodable64, new byte[] { 0x0f, 0x20 }, Instruction.Mov, OperandFormat.Rd, OperandFormat.Cd),
            new InstructionRepresentation(Compatibility.NotEncodable32, new byte[] { 0x0f, 0x20 }, Instruction.Mov, OperandFormat.Rq, OperandFormat.Cd),
            new InstructionRepresentation(RexPrefix.R, new byte[] { 0x0f, 0x20 }, Instruction.Mov, Register.Cr8, OperandFormat.Rq, OperandFormat.Register),
            new InstructionRepresentation(Compatibility.NotEncodable64, new byte[] { 0x0f, 0x22 }, Instruction.Mov, OperandFormat.Cd, OperandFormat.Rd),
            new InstructionRepresentation(Compatibility.NotEncodable32, new byte[] { 0x0f, 0x22 }, Instruction.Mov, OperandFormat.Cd, OperandFormat.Rq),
            new InstructionRepresentation(RexPrefix.R, new byte[] { 0x0f, 0x22 }, Instruction.Mov, Register.Cr8, OperandFormat.Register, OperandFormat.Rq),

            new InstructionRepresentation(Compatibility.NotEncodable64, new byte[] { 0x0f, 0x21 }, Instruction.Mov, OperandFormat.Rd, OperandFormat.Dd),
            new InstructionRepresentation(Compatibility.NotEncodable32, new byte[] { 0x0f, 0x21 }, Instruction.Mov, OperandFormat.Rq, OperandFormat.Dd),
            new InstructionRepresentation(Compatibility.NotEncodable64, new byte[] { 0x0f, 0x23 }, Instruction.Mov, OperandFormat.Dd, OperandFormat.Rd),
            new InstructionRepresentation(Compatibility.NotEncodable32, new byte[] { 0x0f, 0x23 }, Instruction.Mov, OperandFormat.Dd, OperandFormat.Rq),

            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x38, 0xF0 }, Instruction.Movbe, OperandFormat.Gw, OperandFormat.Mw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x38, 0xF0 }, Instruction.Movbe, OperandFormat.Gd, OperandFormat.Md),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x38, 0xF0 }, Instruction.Movbe, OperandFormat.Gq, OperandFormat.Mq),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0x38, 0xF1 }, Instruction.Movbe, OperandFormat.Mw, OperandFormat.Gw),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0x38, 0xF1 }, Instruction.Movbe, OperandFormat.Md, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0x38, 0xF1 }, Instruction.Movbe, OperandFormat.Mq, OperandFormat.Gq),

            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0xC3 }, Instruction.Movnti, OperandFormat.Md, OperandFormat.Gd),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0xC3 }, Instruction.Movnti, OperandFormat.Md, OperandFormat.Gd),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0xC3 }, Instruction.Movnti, OperandFormat.Mq, OperandFormat.Gq),

            new InstructionRepresentation(0xA4, Instruction.Movsb),
            new InstructionRepresentation(OperandSize.Size16, 0xA5, Instruction.Movsw),
            new InstructionRepresentation(OperandSize.Size32, 0xA5, Instruction.Movsd),
            new InstructionRepresentation(RexPrefix.W, 0xA5, Instruction.Movsq),

            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0xBE }, Instruction.Movsx, OperandFormat.Gw, OperandFormat.Eb),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0xBE }, Instruction.Movsx, OperandFormat.Gd, OperandFormat.Eb),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0xBE }, Instruction.Movsx, OperandFormat.Gq, OperandFormat.Eb),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0xBF }, Instruction.Movsx, OperandFormat.Gw, OperandFormat.Ew),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0xBF }, Instruction.Movsx, OperandFormat.Gd, OperandFormat.Ew),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0xBF }, Instruction.Movsx, OperandFormat.Gq, OperandFormat.Ew),
            new InstructionRepresentation(RexPrefix.W, 0x63, Instruction.Movsxd, OperandFormat.Gq, OperandFormat.Ed),

            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0xB6 }, Instruction.Movzx, OperandFormat.Gw, OperandFormat.Eb),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0xB6 }, Instruction.Movzx, OperandFormat.Gd, OperandFormat.Eb),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0xB6 }, Instruction.Movzx, OperandFormat.Gq, OperandFormat.Eb),
            new InstructionRepresentation(OperandSize.Size16, new byte[] { 0x0F, 0xB7 }, Instruction.Movzx, OperandFormat.Gw, OperandFormat.Ew),
            new InstructionRepresentation(OperandSize.Size32, new byte[] { 0x0F, 0xB7 }, Instruction.Movzx, OperandFormat.Gd, OperandFormat.Ew),
            new InstructionRepresentation(RexPrefix.W, new byte[] { 0x0F, 0xB7 }, Instruction.Movzx, OperandFormat.Gq, OperandFormat.Ew),
        };

        public static InstructionRepresentation[] All =
            A.Concat(B).Concat(C).Concat(D).Concat(E).Concat(H).Concat(I).Concat(J).Concat(L).Concat(M).ToArray();
    }
}
