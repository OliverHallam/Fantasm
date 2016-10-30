using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using NUnit.Framework;

namespace Fantasm.Disassembler.Tests
{
    [TestFixture(Description = "Tests of the InstructionReader class for specific OpCodes")]
    public class InstructionReaderOpCodesTests
    {
        // Each row here corresponds to a line in the Intel manual
        public static OpCodeProperties[] OpCodes =
        {
            new OpCodeProperties(0x37, Instruction.Aaa, OperandFormat.None, Compatibility.Invalid64),
            new OpCodeProperties(0xD5, Instruction.Aad, OperandFormat.Ib, Compatibility.Invalid64),
            new OpCodeProperties(0xD4, Instruction.Aam, OperandFormat.Ib, Compatibility.Invalid64),
            new OpCodeProperties(0x3F, Instruction.Aas, OperandFormat.None, Compatibility.Invalid64),

            new OpCodeProperties(0x14, Instruction.Adc, OperandFormat.AL_Ib),
            new OpCodeProperties(0x15, Instruction.Adc, OperandSize.Size16, OperandFormat.AX_Iw),
            new OpCodeProperties(0x15, Instruction.Adc, OperandSize.Size32, OperandFormat.EAX_Id),
            new OpCodeProperties(RexPrefix.W, 0x15, Instruction.Adc, OperandFormat.RAX_Id), 
            new OpCodeProperties(InstructionPrefixes.Lock, 0x80, 2, Instruction.Adc, OperandFormat.Eb_Ib),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, 0x80, 2, Instruction.Adc, OperandFormat.Eb_Ib),
            new OpCodeProperties(InstructionPrefixes.Lock, 0x81, 2, Instruction.Adc, OperandSize.Size16, OperandFormat.Ew_Iw),
            new OpCodeProperties(InstructionPrefixes.Lock, 0x81, 2, Instruction.Adc, OperandSize.Size32, OperandFormat.Ed_Id),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, 0x81, 2, Instruction.Adc, OperandFormat.Eq_Id),
            new OpCodeProperties(InstructionPrefixes.Lock, 0x83, 2, Instruction.Adc, OperandSize.Size16, OperandFormat.Ew_Ib),
            new OpCodeProperties(InstructionPrefixes.Lock, 0x83, 2, Instruction.Adc, OperandSize.Size32, OperandFormat.Ed_Ib),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, 0x83, 2, Instruction.Adc, OperandFormat.Eq_Ib),
            new OpCodeProperties(InstructionPrefixes.Lock, 0x10, Instruction.Adc, OperandFormat.Eb_Gb),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, 0x10, Instruction.Adc, OperandFormat.Eb_Gb),
            new OpCodeProperties(InstructionPrefixes.Lock, 0x11, Instruction.Adc, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(InstructionPrefixes.Lock, 0x11, Instruction.Adc, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, 0x11, Instruction.Adc, OperandFormat.Eq_Gq),
            new OpCodeProperties(0x12, Instruction.Adc, OperandFormat.Gb_Eb),
            new OpCodeProperties(RexPrefix.W, 0x12, Instruction.Adc, OperandFormat.Gb_Eb),
            new OpCodeProperties(0x13, Instruction.Adc, OperandSize.Size16, OperandFormat.Gw_Ew),
            new OpCodeProperties(0x13, Instruction.Adc, OperandSize.Size32, OperandFormat.Gd_Ed),
            new OpCodeProperties(RexPrefix.W, 0x13, Instruction.Adc, OperandFormat.Gq_Eq),

            new OpCodeProperties(0x04, Instruction.Add, OperandFormat.AL_Ib),
            new OpCodeProperties(0x05, Instruction.Add, OperandSize.Size16, OperandFormat.AX_Iw),
            new OpCodeProperties(0x05, Instruction.Add, OperandSize.Size32, OperandFormat.EAX_Id),
            new OpCodeProperties(RexPrefix.W, 0x05, Instruction.Add, OperandFormat.RAX_Id), 
            new OpCodeProperties(InstructionPrefixes.Lock, 0x80, 0, Instruction.Add, OperandFormat.Eb_Ib),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, 0x80, 0, Instruction.Add, OperandFormat.Eb_Ib),
            new OpCodeProperties(InstructionPrefixes.Lock, 0x81, 0, Instruction.Add, OperandSize.Size16, OperandFormat.Ew_Iw),
            new OpCodeProperties(InstructionPrefixes.Lock, 0x81, 0, Instruction.Add, OperandSize.Size32, OperandFormat.Ed_Id),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, 0x81, 0, Instruction.Add, OperandFormat.Eq_Id),
            new OpCodeProperties(InstructionPrefixes.Lock, 0x83, 0, Instruction.Add, OperandSize.Size16, OperandFormat.Ew_Ib),
            new OpCodeProperties(InstructionPrefixes.Lock, 0x83, 0, Instruction.Add, OperandSize.Size32, OperandFormat.Ed_Ib),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, 0x83, 0, Instruction.Add, OperandFormat.Eq_Ib),
            new OpCodeProperties(InstructionPrefixes.Lock, 0x00, Instruction.Add, OperandFormat.Eb_Gb),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, 0x0, Instruction.Add, OperandFormat.Eb_Gb),
            new OpCodeProperties(InstructionPrefixes.Lock, 0x01, Instruction.Add, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(InstructionPrefixes.Lock, 0x01, Instruction.Add, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, 0x01, Instruction.Add, OperandFormat.Eq_Gq),
            new OpCodeProperties(0x02, Instruction.Add, OperandFormat.Gb_Eb),
            new OpCodeProperties(RexPrefix.W, 0x02, Instruction.Add, OperandFormat.Gb_Eb),
            new OpCodeProperties(0x03, Instruction.Add, OperandSize.Size16, OperandFormat.Gw_Ew),
            new OpCodeProperties(0x03, Instruction.Add, OperandSize.Size32, OperandFormat.Gd_Ed),
            new OpCodeProperties(RexPrefix.W, 0x03, Instruction.Add, OperandFormat.Gq_Eq),

            new OpCodeProperties(0x24, Instruction.And, OperandFormat.AL_Ib),
            new OpCodeProperties(0x25, Instruction.And, OperandSize.Size16, OperandFormat.AX_Iw),
            new OpCodeProperties(0x25, Instruction.And, OperandSize.Size32, OperandFormat.EAX_Id),
            new OpCodeProperties(RexPrefix.W, 0x25, Instruction.And, OperandFormat.RAX_Id), 
            new OpCodeProperties(InstructionPrefixes.Lock, 0x80, 4, Instruction.And, OperandFormat.Eb_Ib),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, 0x80, 4, Instruction.And, OperandFormat.Eb_Ib),
            new OpCodeProperties(InstructionPrefixes.Lock, 0x81, 4, Instruction.And, OperandSize.Size16, OperandFormat.Ew_Iw),
            new OpCodeProperties(InstructionPrefixes.Lock, 0x81, 4, Instruction.And, OperandSize.Size32, OperandFormat.Ed_Id),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, 0x81, 4, Instruction.And, OperandFormat.Eq_Id),
            new OpCodeProperties(InstructionPrefixes.Lock, 0x83, 4, Instruction.And, OperandSize.Size16, OperandFormat.Ew_Ib),
            new OpCodeProperties(InstructionPrefixes.Lock, 0x83, 4, Instruction.And, OperandSize.Size32, OperandFormat.Ed_Ib),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, 0x83, 4, Instruction.And, OperandFormat.Eq_Ib),
            new OpCodeProperties(InstructionPrefixes.Lock, 0x20, Instruction.And, OperandFormat.Eb_Gb),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, 0x20, Instruction.And, OperandFormat.Eb_Gb),
            new OpCodeProperties(InstructionPrefixes.Lock, 0x21, Instruction.And, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(InstructionPrefixes.Lock, 0x21, Instruction.And, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, 0x21, Instruction.And, OperandFormat.Eq_Gq),
            new OpCodeProperties(0x22, Instruction.And, OperandFormat.Gb_Eb),
            new OpCodeProperties(RexPrefix.W, 0x22, Instruction.And, OperandFormat.Gb_Eb),
            new OpCodeProperties(0x23, Instruction.And, OperandSize.Size16, OperandFormat.Gw_Ew),
            new OpCodeProperties(0x23, Instruction.And, OperandSize.Size32, OperandFormat.Gd_Ed),
            new OpCodeProperties(RexPrefix.W, 0x23, Instruction.And, OperandFormat.Gq_Eq),

            new OpCodeProperties(0x63, Instruction.Arpl, OperandFormat.Ew_Gw, Compatibility.NotEncodable64),

            new OpCodeProperties(0x62, Instruction.Bound, OperandSize.Size16, OperandFormat.Gw_Ew, Compatibility.Invalid64),
            new OpCodeProperties(0x62, Instruction.Bound, OperandSize.Size32, OperandFormat.Gd_Ed, Compatibility.Invalid64),

            new OpCodeProperties(new byte[] { 0x0F, 0xBC }, Instruction.Bsf, OperandSize.Size16, OperandFormat.Gw_Ew),
            new OpCodeProperties(new byte[] { 0x0F, 0xBC }, Instruction.Bsf, OperandSize.Size32, OperandFormat.Gd_Ed),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0xBC }, Instruction.Bsf, OperandFormat.Gq_Eq),

            new OpCodeProperties(new byte[] { 0x0F, 0xBD }, Instruction.Bsr, OperandSize.Size16, OperandFormat.Gw_Ew),
            new OpCodeProperties(new byte[] { 0x0F, 0xBD }, Instruction.Bsr, OperandSize.Size32, OperandFormat.Gd_Ed),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0xBD }, Instruction.Bsr, OperandFormat.Gq_Eq),

            // expanded from intel table
            new OpCodeProperties(new byte[] { 0x0F, 0xC8 }, Instruction.Bswap, Register.Eax), 
            new OpCodeProperties(new byte[] { 0x0F, 0xC9 }, Instruction.Bswap, Register.Ecx), 
            new OpCodeProperties(new byte[] { 0x0F, 0xCA }, Instruction.Bswap, Register.Edx), 
            new OpCodeProperties(new byte[] { 0x0F, 0xCB }, Instruction.Bswap, Register.Ebx), 
            new OpCodeProperties(new byte[] { 0x0F, 0xCC }, Instruction.Bswap, Register.Esp), 
            new OpCodeProperties(new byte[] { 0x0F, 0xCD }, Instruction.Bswap, Register.Ebp), 
            new OpCodeProperties(new byte[] { 0x0F, 0xCE }, Instruction.Bswap, Register.Esi), 
            new OpCodeProperties(new byte[] { 0x0F, 0xCF }, Instruction.Bswap, Register.Edi), 
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0xC8 }, Instruction.Bswap, Register.Rax), 
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0xC9 }, Instruction.Bswap, Register.Rcx), 
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0xCA }, Instruction.Bswap, Register.Rdx), 
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0xCB }, Instruction.Bswap, Register.Rbx), 
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0xCC }, Instruction.Bswap, Register.Rsp), 
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0xCD }, Instruction.Bswap, Register.Rbp), 
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0xCE }, Instruction.Bswap, Register.Rsi), 
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0xCF }, Instruction.Bswap, Register.Rdi), 
            new OpCodeProperties(RexPrefix.B, new byte[] { 0x0F, 0xC8 }, Instruction.Bswap, Register.R8D), 
            new OpCodeProperties(RexPrefix.B, new byte[] { 0x0F, 0xC9 }, Instruction.Bswap, Register.R9D), 
            new OpCodeProperties(RexPrefix.B, new byte[] { 0x0F, 0xCA }, Instruction.Bswap, Register.R10D), 
            new OpCodeProperties(RexPrefix.B, new byte[] { 0x0F, 0xCB }, Instruction.Bswap, Register.R11D), 
            new OpCodeProperties(RexPrefix.B, new byte[] { 0x0F, 0xCC }, Instruction.Bswap, Register.R12D), 
            new OpCodeProperties(RexPrefix.B, new byte[] { 0x0F, 0xCD }, Instruction.Bswap, Register.R13D), 
            new OpCodeProperties(RexPrefix.B, new byte[] { 0x0F, 0xCE }, Instruction.Bswap, Register.R14D), 
            new OpCodeProperties(RexPrefix.B, new byte[] { 0x0F, 0xCF }, Instruction.Bswap, Register.R15D), 
            new OpCodeProperties(RexPrefix.W | RexPrefix.B, new byte[] { 0x0F, 0xC8 }, Instruction.Bswap, Register.R8), 
            new OpCodeProperties(RexPrefix.W | RexPrefix.B, new byte[] { 0x0F, 0xC9 }, Instruction.Bswap, Register.R9), 
            new OpCodeProperties(RexPrefix.W | RexPrefix.B, new byte[] { 0x0F, 0xCA }, Instruction.Bswap, Register.R10), 
            new OpCodeProperties(RexPrefix.W | RexPrefix.B, new byte[] { 0x0F, 0xCB }, Instruction.Bswap, Register.R11), 
            new OpCodeProperties(RexPrefix.W | RexPrefix.B, new byte[] { 0x0F, 0xCC }, Instruction.Bswap, Register.R12), 
            new OpCodeProperties(RexPrefix.W | RexPrefix.B, new byte[] { 0x0F, 0xCD }, Instruction.Bswap, Register.R13), 
            new OpCodeProperties(RexPrefix.W | RexPrefix.B, new byte[] { 0x0F, 0xCE }, Instruction.Bswap, Register.R14), 
            new OpCodeProperties(RexPrefix.W | RexPrefix.B, new byte[] { 0x0F, 0xCF }, Instruction.Bswap, Register.R15), 

            new OpCodeProperties(new byte[] { 0x0F, 0xA3 }, Instruction.Bt, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0xA3 }, Instruction.Bt, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0xA3 }, Instruction.Bt, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0xBA }, 4, Instruction.Bt, OperandSize.Size16, OperandFormat.Ew_Ib),
            new OpCodeProperties(new byte[] { 0x0F, 0xBA }, 4, Instruction.Bt, OperandSize.Size32, OperandFormat.Ed_Ib),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0xBA }, 4, Instruction.Bt, OperandFormat.Eq_Ib),

            new OpCodeProperties(InstructionPrefixes.Lock, new byte[] { 0x0F, 0xBB }, Instruction.Btc, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(InstructionPrefixes.Lock, new byte[] { 0x0F, 0xBB }, Instruction.Btc, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, new byte[] { 0x0F, 0xBB }, Instruction.Btc, OperandFormat.Eq_Gq),
            new OpCodeProperties(InstructionPrefixes.Lock, new byte[] { 0x0F, 0xBA }, 7, Instruction.Btc, OperandSize.Size16, OperandFormat.Ew_Ib),
            new OpCodeProperties(InstructionPrefixes.Lock, new byte[] { 0x0F, 0xBA }, 7, Instruction.Btc, OperandSize.Size32, OperandFormat.Ed_Ib),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, new byte[] { 0x0F, 0xBA }, 7, Instruction.Btc, OperandFormat.Eq_Ib),

            new OpCodeProperties(InstructionPrefixes.Lock, new byte[] { 0x0F, 0xB3 }, Instruction.Btr, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(InstructionPrefixes.Lock, new byte[] { 0x0F, 0xB3 }, Instruction.Btr, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, new byte[] { 0x0F, 0xB3 }, Instruction.Btr, OperandFormat.Eq_Gq),
            new OpCodeProperties(InstructionPrefixes.Lock, new byte[] { 0x0F, 0xBA }, 6, Instruction.Btr, OperandSize.Size16, OperandFormat.Ew_Ib),
            new OpCodeProperties(InstructionPrefixes.Lock, new byte[] { 0x0F, 0xBA }, 6, Instruction.Btr, OperandSize.Size32, OperandFormat.Ed_Ib),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, new byte[] { 0x0F, 0xBA }, 6, Instruction.Btr, OperandFormat.Eq_Ib),

            new OpCodeProperties(InstructionPrefixes.Lock, new byte[] { 0x0F, 0xAB }, Instruction.Bts, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(InstructionPrefixes.Lock, new byte[] { 0x0F, 0xAB }, Instruction.Bts, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, new byte[] { 0x0F, 0xAB }, Instruction.Bts, OperandFormat.Eq_Gq),
            new OpCodeProperties(InstructionPrefixes.Lock, new byte[] { 0x0F, 0xBA }, 5, Instruction.Bts, OperandSize.Size16, OperandFormat.Ew_Ib),
            new OpCodeProperties(InstructionPrefixes.Lock, new byte[] { 0x0F, 0xBA }, 5, Instruction.Bts, OperandSize.Size32, OperandFormat.Ed_Ib),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, new byte[] { 0x0F, 0xBA }, 5, Instruction.Bts, OperandFormat.Eq_Ib),

            new OpCodeProperties(0xE8, Instruction.Call, OperandSize.Size16, OperandFormat.Jw, Compatibility.NotEncodable64), 
            new OpCodeProperties(0xE8, Instruction.Call, OperandSize.Size32, OperandFormat.Jd),
            new OpCodeProperties(0xFF, 2, Instruction.Call, OperandSize.Size16, OperandFormat.Ew, Compatibility.NotEncodable64), 
            new OpCodeProperties(0xFF, 2, Instruction.Call, OperandSize.Size32, OperandFormat.Ed, Compatibility.NotEncodable64), 
            new OpCodeProperties(0xFF, 2, Instruction.Call, OperandFormat.Eq, Compatibility.NotEncodable32), 
            new OpCodeProperties(0x9A, Instruction.CallFar, OperandSize.Size16, OperandFormat.Aww, Compatibility.Invalid64), 
            new OpCodeProperties(0x9A, Instruction.CallFar, OperandSize.Size32, OperandFormat.Awd, Compatibility.Invalid64),
            new OpCodeProperties(0xFF, 3, Instruction.CallFar, OperandSize.Size16, OperandFormat.Mw), 
            new OpCodeProperties(0xFF, 3, Instruction.CallFar, OperandSize.Size32, OperandFormat.Md, Compatibility.NotEncodable64), 
            new OpCodeProperties(0xFF, 3, Instruction.CallFar, OperandFormat.Mq, Compatibility.NotEncodable32), 
            new OpCodeProperties(RexPrefix.W, 0xFF, 3, Instruction.CallFar, OperandFormat.Mq),

            new OpCodeProperties(0x98, Instruction.Cbw, OperandSize.Size16, OperandFormat.None), 
            new OpCodeProperties(0x98, Instruction.Cwde, OperandSize.Size32, OperandFormat.None), 
            new OpCodeProperties(RexPrefix.W, 0x98, Instruction.Cdqe, OperandFormat.None), 

            new OpCodeProperties(0xF8, Instruction.Clc, OperandFormat.None), 

            new OpCodeProperties(0xFC, Instruction.Cld, OperandFormat.None), 
            
            new OpCodeProperties(0xFA, Instruction.Cli, OperandFormat.None), 

            new OpCodeProperties(new byte[] { 0x0f, 0x06 }, Instruction.Clts, OperandFormat.None), 

            new OpCodeProperties(0xF5, Instruction.Cmc, OperandFormat.None), 

            new OpCodeProperties(new byte[] { 0x0F, 0x47 }, Instruction.Cmova, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x47 }, Instruction.Cmova, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x47 }, Instruction.Cmova, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x43 }, Instruction.Cmovae, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x43 }, Instruction.Cmovae, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x43 }, Instruction.Cmovae, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x42 }, Instruction.Cmovb, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x42 }, Instruction.Cmovb, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x42 }, Instruction.Cmovb, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x46 }, Instruction.Cmovbe, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x46 }, Instruction.Cmovbe, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x46 }, Instruction.Cmovbe, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x42 }, Instruction.Cmovc, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x42 }, Instruction.Cmovc, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x42 }, Instruction.Cmovc, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x44 }, Instruction.Cmove, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x44 }, Instruction.Cmove, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x44 }, Instruction.Cmove, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x4F }, Instruction.Cmovg, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x4F }, Instruction.Cmovg, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x4F }, Instruction.Cmovg, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x4D }, Instruction.Cmovge, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x4D }, Instruction.Cmovge, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x4D }, Instruction.Cmovge, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x4C }, Instruction.Cmovl, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x4C }, Instruction.Cmovl, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x4C }, Instruction.Cmovl, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x4E }, Instruction.Cmovle, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x4E }, Instruction.Cmovle, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x4E }, Instruction.Cmovle, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x46 }, Instruction.Cmovna, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x46 }, Instruction.Cmovna, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x46 }, Instruction.Cmovna, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x42 }, Instruction.Cmovnae, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x42 }, Instruction.Cmovnae, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x42 }, Instruction.Cmovnae, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x43 }, Instruction.Cmovnb, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x43 }, Instruction.Cmovnb, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x43 }, Instruction.Cmovnb, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x47 }, Instruction.Cmovnbe, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x47 }, Instruction.Cmovnbe, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x47 }, Instruction.Cmovnbe, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x43 }, Instruction.Cmovnc, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x43 }, Instruction.Cmovnc, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x43 }, Instruction.Cmovnc, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x45 }, Instruction.Cmovne, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x45 }, Instruction.Cmovne, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x45 }, Instruction.Cmovne, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x4E }, Instruction.Cmovng, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x4E }, Instruction.Cmovng, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x4E }, Instruction.Cmovng, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x4C }, Instruction.Cmovnge, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x4C }, Instruction.Cmovnge, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x4C }, Instruction.Cmovnge, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x4D }, Instruction.Cmovnl, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x4D }, Instruction.Cmovnl, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x4D }, Instruction.Cmovnl, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x4F }, Instruction.Cmovnle, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x4F }, Instruction.Cmovnle, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x4F }, Instruction.Cmovnle, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x41 }, Instruction.Cmovno, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x41 }, Instruction.Cmovno, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x41 }, Instruction.Cmovno, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x4B }, Instruction.Cmovnp, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x4B }, Instruction.Cmovnp, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x4B }, Instruction.Cmovnp, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x49 }, Instruction.Cmovns, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x49 }, Instruction.Cmovns, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x49 }, Instruction.Cmovns, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x45 }, Instruction.Cmovnz, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x45 }, Instruction.Cmovnz, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x45 }, Instruction.Cmovnz, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x40 }, Instruction.Cmovo, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x40 }, Instruction.Cmovo, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x40 }, Instruction.Cmovo, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x4A }, Instruction.Cmovp, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x4A }, Instruction.Cmovp, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x4A }, Instruction.Cmovp, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x4A }, Instruction.Cmovpe, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x4A }, Instruction.Cmovpe, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x4A }, Instruction.Cmovpe, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x4B }, Instruction.Cmovpo, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x4B }, Instruction.Cmovpo, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x4B }, Instruction.Cmovpo, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x48 }, Instruction.Cmovs, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x48 }, Instruction.Cmovs, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x48 }, Instruction.Cmovs, OperandFormat.Eq_Gq),
            new OpCodeProperties(new byte[] { 0x0F, 0x44 }, Instruction.Cmovz, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(new byte[] { 0x0F, 0x44 }, Instruction.Cmovz, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, new byte[] { 0x0F, 0x44 }, Instruction.Cmovz, OperandFormat.Eq_Gq),

            new OpCodeProperties(0x3C, Instruction.Cmp, OperandFormat.AL_Ib),
            new OpCodeProperties(0x3D, Instruction.Cmp, OperandSize.Size16, OperandFormat.AX_Iw),
            new OpCodeProperties(0x3D, Instruction.Cmp, OperandSize.Size32, OperandFormat.EAX_Id),
            new OpCodeProperties(RexPrefix.W, 0x3D, Instruction.Cmp, OperandFormat.RAX_Id), 
            new OpCodeProperties(0x80, 7, Instruction.Cmp, OperandFormat.Eb_Ib),
            new OpCodeProperties(RexPrefix.W, 0x80, 7, Instruction.Cmp, OperandFormat.Eb_Ib),
            new OpCodeProperties(0x81, 7, Instruction.Cmp, OperandSize.Size16, OperandFormat.Ew_Iw),
            new OpCodeProperties(0x81, 7, Instruction.Cmp, OperandSize.Size32, OperandFormat.Ed_Id),
            new OpCodeProperties(RexPrefix.W, 0x81, 7, Instruction.Cmp, OperandFormat.Eq_Id),
            new OpCodeProperties(0x83, 7, Instruction.Cmp, OperandSize.Size16, OperandFormat.Ew_Ib),
            new OpCodeProperties(0x83, 7, Instruction.Cmp, OperandSize.Size32, OperandFormat.Ed_Ib),
            new OpCodeProperties(RexPrefix.W, 0x83, 7, Instruction.Cmp, OperandFormat.Eq_Ib),
            new OpCodeProperties(0x38, Instruction.Cmp, OperandFormat.Eb_Gb),
            new OpCodeProperties(RexPrefix.W, 0x38, Instruction.Cmp, OperandFormat.Eb_Gb),
            new OpCodeProperties(0x39, Instruction.Cmp, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(0x39, Instruction.Cmp, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(RexPrefix.W, 0x39, Instruction.Cmp, OperandFormat.Eq_Gq),
            new OpCodeProperties(0x3A, Instruction.Cmp, OperandFormat.Gb_Eb),
            new OpCodeProperties(RexPrefix.W, 0x3A, Instruction.Cmp, OperandFormat.Gb_Eb),
            new OpCodeProperties(0x3B, Instruction.Cmp, OperandSize.Size16, OperandFormat.Gw_Ew),
            new OpCodeProperties(0x3B, Instruction.Cmp, OperandSize.Size32, OperandFormat.Gd_Ed),
            new OpCodeProperties(RexPrefix.W, 0x3B, Instruction.Cmp, OperandFormat.Gq_Eq),

            new OpCodeProperties(0xA6, Instruction.Cmpsb, OperandFormat.None),
            new OpCodeProperties(0xA7, Instruction.Cmpsw, OperandSize.Size16, OperandFormat.None),
            new OpCodeProperties(0xA7, Instruction.Cmpsd, OperandSize.Size32, OperandFormat.None),
            new OpCodeProperties(RexPrefix.W, 0xA7, Instruction.Cmpsq, OperandFormat.None),

            new OpCodeProperties(InstructionPrefixes.Lock, new byte[] { 0x0F, 0xB0 }, Instruction.Cmpxchg, OperandFormat.Eb_Gb),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, new byte[] { 0x0F, 0xB0 }, Instruction.Cmpxchg, OperandFormat.Eb_Gb),
            new OpCodeProperties(InstructionPrefixes.Lock, new byte[] { 0x0F, 0xB1 }, Instruction.Cmpxchg, OperandSize.Size16, OperandFormat.Ew_Gw),
            new OpCodeProperties(InstructionPrefixes.Lock, new byte[] { 0x0F, 0xB1 }, Instruction.Cmpxchg, OperandSize.Size32, OperandFormat.Ed_Gd),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, new byte[] { 0x0F, 0xB1 }, Instruction.Cmpxchg, OperandFormat.Eq_Gq),

            new OpCodeProperties(new byte[] { 0x0F, 0xC7 }, 1, Instruction.Cmpxchg8b, OperandFormat.Mq, InstructionPrefixes.Lock),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, new byte[] { 0x0F, 0xC7 }, 1, Instruction.Cmpxchg16b, OperandFormat.Mdq),

            new OpCodeProperties(new byte[] { 0x0F, 0xA2 }, Instruction.Cpuid, OperandFormat.None),

            new OpCodeProperties(InstructionPrefixes.RepNZ, new byte[] { 0x0F, 0x38, 0xF0 }, Instruction.Crc32, OperandFormat.Gd_Eb),
            new OpCodeProperties(InstructionPrefixes.RepNZ, (RexPrefix)0, new byte[] { 0x0F, 0x38, 0xF0 }, Instruction.Crc32, OperandFormat.Gd_Eb),
            new OpCodeProperties(InstructionPrefixes.RepNZ, new byte[] { 0x0F, 0x38, 0xF1 }, Instruction.Crc32, OperandSize.Size16, OperandFormat.Gd_Ew),
            new OpCodeProperties(InstructionPrefixes.RepNZ, new byte[] { 0x0F, 0x38, 0xF1 }, Instruction.Crc32, OperandSize.Size32, OperandFormat.Gd_Ed),
            new OpCodeProperties(InstructionPrefixes.RepNZ, RexPrefix.W, new byte[] { 0x0F, 0x38, 0xF0 }, Instruction.Crc32, OperandFormat.Gd_Eb),
            new OpCodeProperties(InstructionPrefixes.RepNZ, RexPrefix.W, new byte[] { 0x0F, 0x38, 0xF1 }, Instruction.Crc32, OperandFormat.Gd_Eq),

            new OpCodeProperties(0x99, Instruction.Cwd, OperandSize.Size16, OperandFormat.None),
            new OpCodeProperties(0x99, Instruction.Cdq, OperandSize.Size32, OperandFormat.None),
            new OpCodeProperties(RexPrefix.W, 0x99, Instruction.Cqo, OperandFormat.None),

            new OpCodeProperties(0x27, Instruction.Daa, OperandFormat.None, Compatibility.Invalid64),

            new OpCodeProperties(0x2F, Instruction.Das, OperandFormat.None, Compatibility.Invalid64),

            new OpCodeProperties(InstructionPrefixes.Lock, 0xFE, 1, Instruction.Dec, OperandFormat.Eb),
            new OpCodeProperties(InstructionPrefixes.Lock, (RexPrefix)0, 0xFE, 1, Instruction.Dec, OperandFormat.Eb),
            new OpCodeProperties(InstructionPrefixes.Lock, 0xFF, 1, Instruction.Dec, OperandSize.Size16, OperandFormat.Ew),
            new OpCodeProperties(InstructionPrefixes.Lock, 0xFF, 1, Instruction.Dec, OperandSize.Size32, OperandFormat.Ed),
            new OpCodeProperties(InstructionPrefixes.Lock, RexPrefix.W, 0xFF, 1, Instruction.Dec, OperandFormat.Eq),
            // expanded from intel table
            new OpCodeProperties(0x48, Instruction.Dec, OperandSize.Size16, Register.Ax, Compatibility.NotEncodable64),
            new OpCodeProperties(0x49, Instruction.Dec, OperandSize.Size16, Register.Cx, Compatibility.NotEncodable64),
            new OpCodeProperties(0x4A, Instruction.Dec, OperandSize.Size16, Register.Dx, Compatibility.NotEncodable64),
            new OpCodeProperties(0x4B, Instruction.Dec, OperandSize.Size16, Register.Bx, Compatibility.NotEncodable64),
            new OpCodeProperties(0x4C, Instruction.Dec, OperandSize.Size16, Register.Sp, Compatibility.NotEncodable64),
            new OpCodeProperties(0x4D, Instruction.Dec, OperandSize.Size16, Register.Bp, Compatibility.NotEncodable64),
            new OpCodeProperties(0x4E, Instruction.Dec, OperandSize.Size16, Register.Si, Compatibility.NotEncodable64),
            new OpCodeProperties(0x4F, Instruction.Dec, OperandSize.Size16, Register.Di, Compatibility.NotEncodable64),
            new OpCodeProperties(0x48, Instruction.Dec, OperandSize.Size32, Register.Eax, Compatibility.NotEncodable64),
            new OpCodeProperties(0x49, Instruction.Dec, OperandSize.Size32, Register.Ecx, Compatibility.NotEncodable64),
            new OpCodeProperties(0x4A, Instruction.Dec, OperandSize.Size32, Register.Edx, Compatibility.NotEncodable64),
            new OpCodeProperties(0x4B, Instruction.Dec, OperandSize.Size32, Register.Ebx, Compatibility.NotEncodable64),
            new OpCodeProperties(0x4C, Instruction.Dec, OperandSize.Size32, Register.Esp, Compatibility.NotEncodable64),
            new OpCodeProperties(0x4D, Instruction.Dec, OperandSize.Size32, Register.Ebp, Compatibility.NotEncodable64),
            new OpCodeProperties(0x4E, Instruction.Dec, OperandSize.Size32, Register.Esi, Compatibility.NotEncodable64),
            new OpCodeProperties(0x4F, Instruction.Dec, OperandSize.Size32, Register.Edi, Compatibility.NotEncodable64),

            new OpCodeProperties(0xF6, 6, Instruction.Div, OperandFormat.Eb),
            new OpCodeProperties((RexPrefix)0, 0xF6, 6, Instruction.Div, OperandFormat.Eb),
            new OpCodeProperties(0xF7, 6, Instruction.Div, OperandSize.Size16, OperandFormat.Ew),
            new OpCodeProperties(0xF7, 6, Instruction.Div, OperandSize.Size32, OperandFormat.Ed),
            new OpCodeProperties(RexPrefix.W, 0xF7, 6, Instruction.Div, OperandFormat.Eq),

            new OpCodeProperties(new byte[] { 0x0f, 0x77 }, Instruction.Emms, OperandFormat.None),

            new OpCodeProperties(0xC8, Instruction.Enter, OperandFormat.Iw_Ib),

        };

        [Test]
        [TestCaseSource("OpCodes")]
        public void InstructionReader_WithCorrectOperands_SuccessfullyDecodesInstruction(OpCodeProperties opCode)
        {
            var bytes = GetBytes(opCode);
            var mode = GetExecutionMode(opCode);
            var reader = new InstructionReader(
                new MemoryStream(bytes),
                mode,
                opCode.OperandSize == OperandSize.Size32);

            reader.Read();

            Assert.AreEqual(opCode.Mnemonic, reader.Instruction);

            // check operands
            switch (opCode.Operands)
            {
                case OperandFormat.None:
                    Assert.AreEqual(OperandType.None, reader.Operand1.Type);
                    Assert.AreEqual(OperandType.None, reader.Operand2.Type);
                    break;

                case OperandFormat.Ib:
                    Assert.AreEqual(OperandType.ImmediateByte, reader.Operand1.Type);
                    Assert.AreEqual(0x11, reader.Operand1.GetImmediateValue());
                    Assert.AreEqual(OperandType.None, reader.Operand2.Type);
                    break;

                case OperandFormat.Iw_Ib:
                    Assert.AreEqual(OperandType.ImmediateWord, reader.Operand1.Type);
                    Assert.AreEqual(0x1111, reader.Operand1.GetImmediateValue());
                    Assert.AreEqual(OperandType.ImmediateByte, reader.Operand2.Type);
                    Assert.AreEqual(0x22, reader.Operand2.GetImmediateValue());
                    break;

                case OperandFormat.Register:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(opCode.Register, reader.Operand1.GetRegister());
                    Assert.AreEqual(OperandType.None, reader.Operand2.Type);
                    break;

                case OperandFormat.Eb:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(opCode.RexPrefix != 0 ? Register.Spl : Register.Ah, reader.Operand1.GetBaseRegister());
                    Assert.AreEqual(OperandType.None, reader.Operand2.Type);
                    break;

                case OperandFormat.Ew:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Sp, reader.Operand1.GetBaseRegister());
                    Assert.AreEqual(OperandType.None, reader.Operand2.Type);
                    break;

                case OperandFormat.Ed:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Esp, reader.Operand1.GetBaseRegister());
                    Assert.AreEqual(OperandType.None, reader.Operand2.Type);
                    break;

                case OperandFormat.Eq:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Rsp, reader.Operand1.GetBaseRegister());
                    Assert.AreEqual(OperandType.None, reader.Operand2.Type);
                    break;

                case OperandFormat.Mw:
                    Assert.AreEqual(OperandType.WordPointer, reader.Operand1.Type);
                    Assert.AreEqual(Register.Bx, reader.Operand1.GetBaseRegister());
                    Assert.AreEqual(OperandType.None, reader.Operand2.Type);
                    break;

                case OperandFormat.Md:
                    Assert.AreEqual(OperandType.DwordPointer, reader.Operand1.Type);
                    Assert.AreEqual(Register.Edi, reader.Operand1.GetBaseRegister());
                    Assert.AreEqual(OperandType.None, reader.Operand2.Type);
                    break;

                case OperandFormat.Mq:
                    Assert.AreEqual(OperandType.QwordPointer, reader.Operand1.Type);
                    Assert.AreEqual(Register.Rdi, reader.Operand1.GetBaseRegister());
                    Assert.AreEqual(OperandType.None, reader.Operand2.Type);
                    break;

                case OperandFormat.Mdq:
                    Assert.AreEqual(OperandType.OwordPointer, reader.Operand1.Type);
                    Assert.AreEqual(Register.Rdi, reader.Operand1.GetBaseRegister());
                    Assert.AreEqual(OperandType.None, reader.Operand2.Type);
                    break;

                case OperandFormat.Jw:
                    Assert.AreEqual(OperandType.RelativeAddress, reader.Operand1.Type);
                    Assert.AreEqual(0x1111, reader.Operand1.GetDisplacement());
                    Assert.AreEqual(OperandType.None, reader.Operand2.Type);
                    break;

                case OperandFormat.Jd:
                    Assert.AreEqual(OperandType.RelativeAddress, reader.Operand1.Type);
                    Assert.AreEqual(0x11111111, reader.Operand1.GetDisplacement());
                    Assert.AreEqual(OperandType.None, reader.Operand2.Type);
                    break;

                case OperandFormat.Aww:
                    Assert.AreEqual(OperandType.FarPointer, reader.Operand1.Type);
                    Assert.AreEqual(0x1111, reader.Operand1.GetSegmentSelector());
                    Assert.AreEqual(0x2222, reader.Operand1.GetDisplacement());
                    Assert.AreEqual(OperandType.None, reader.Operand2.Type);
                    break;

                case OperandFormat.Awd:
                    Assert.AreEqual(OperandType.FarPointer, reader.Operand1.Type);
                    Assert.AreEqual(0x1111, reader.Operand1.GetSegmentSelector());
                    Assert.AreEqual(0x22222222, reader.Operand1.GetDisplacement());
                    Assert.AreEqual(OperandType.None, reader.Operand2.Type);
                    break;

                case OperandFormat.AL_Ib:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Al, reader.Operand1.GetRegister());
                    Assert.AreEqual(OperandType.ImmediateByte, reader.Operand2.Type);
                    Assert.AreEqual(0x22, reader.Operand2.GetImmediateValue());
                    break;

                case OperandFormat.AX_Iw:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Ax, reader.Operand1.GetRegister());
                    Assert.AreEqual(OperandType.ImmediateWord, reader.Operand2.Type);
                    Assert.AreEqual(0x2222, reader.Operand2.GetImmediateValue());
                    break;

                case OperandFormat.EAX_Id:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Eax, reader.Operand1.GetRegister());
                    Assert.AreEqual(OperandType.ImmediateDword, reader.Operand2.Type);
                    Assert.AreEqual(0x22222222, reader.Operand2.GetImmediateValue());
                    break;

                case OperandFormat.RAX_Id:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Rax, reader.Operand1.GetRegister());
                    Assert.AreEqual(OperandType.ImmediateDword, reader.Operand2.Type);
                    Assert.AreEqual(0x22222222, reader.Operand2.GetImmediateValue());
                    break;

                case OperandFormat.Eb_Ib:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(opCode.RexPrefix != 0 ? Register.Spl : Register.Ah, reader.Operand1.GetBaseRegister());
                    Assert.AreEqual(OperandType.ImmediateByte, reader.Operand2.Type);
                    Assert.AreEqual(0x22, reader.Operand2.GetImmediateValue());
                    break;

                case OperandFormat.Eb_Gb:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(opCode.RexPrefix != 0 ? Register.Spl : Register.Ah, reader.Operand1.GetBaseRegister());
                    Assert.AreEqual(OperandType.Register, reader.Operand2.Type);
                    Assert.AreEqual(opCode.RexPrefix != 0 ? Register.Bpl : Register.Ch, reader.Operand2.GetRegister());
                    break;

                case OperandFormat.Gb_Eb:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(opCode.RexPrefix != 0 ? Register.Bpl : Register.Ch, reader.Operand1.GetRegister());
                    Assert.AreEqual(OperandType.Register, reader.Operand2.Type);
                    Assert.AreEqual(opCode.RexPrefix != 0 ? Register.Spl : Register.Ah, reader.Operand2.GetBaseRegister());
                    break;

                case OperandFormat.Ew_Iw:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Sp, reader.Operand1.GetBaseRegister());
                    Assert.AreEqual(OperandType.ImmediateWord, reader.Operand2.Type);
                    Assert.AreEqual(0x2222, reader.Operand2.GetImmediateValue());
                    break;

                case OperandFormat.Ew_Ib:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Sp, reader.Operand1.GetBaseRegister());
                    Assert.AreEqual(OperandType.ImmediateByte, reader.Operand2.Type);
                    Assert.AreEqual(0x22, reader.Operand2.GetImmediateValue());
                    break;

                case OperandFormat.Ew_Gw:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Sp, reader.Operand1.GetBaseRegister());
                    Assert.AreEqual(OperandType.Register, reader.Operand2.Type);
                    Assert.AreEqual(Register.Bp, reader.Operand2.GetRegister());
                    break;

                case OperandFormat.Gw_Ew:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Bp, reader.Operand1.GetRegister());
                    Assert.AreEqual(OperandType.Register, reader.Operand2.Type);
                    Assert.AreEqual(Register.Sp, reader.Operand2.GetBaseRegister());
                    break;

                case OperandFormat.Ed_Id:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Esp, reader.Operand1.GetBaseRegister());
                    Assert.AreEqual(OperandType.ImmediateDword, reader.Operand2.Type);
                    Assert.AreEqual(0x22222222, reader.Operand2.GetImmediateValue());
                    break;

                case OperandFormat.Ed_Ib:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Esp, reader.Operand1.GetBaseRegister());
                    Assert.AreEqual(OperandType.ImmediateByte, reader.Operand2.Type);
                    Assert.AreEqual(0x22, reader.Operand2.GetImmediateValue());
                    break;

                case OperandFormat.Ed_Gd:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Esp, reader.Operand1.GetBaseRegister());
                    Assert.AreEqual(OperandType.Register, reader.Operand2.Type);
                    Assert.AreEqual(Register.Ebp, reader.Operand2.GetRegister());
                    break;

                case OperandFormat.Eq_Id:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Rsp, reader.Operand1.GetBaseRegister());
                    Assert.AreEqual(OperandType.ImmediateDword, reader.Operand2.Type);
                    Assert.AreEqual(0x22222222, reader.Operand2.GetImmediateValue());
                    break;

                case OperandFormat.Eq_Ib:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Rsp, reader.Operand1.GetBaseRegister());
                    Assert.AreEqual(OperandType.ImmediateByte, reader.Operand2.Type);
                    Assert.AreEqual(0x22, reader.Operand2.GetImmediateValue());
                    break;

                case OperandFormat.Eq_Gq:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Rsp, reader.Operand1.GetBaseRegister());
                    Assert.AreEqual(OperandType.Register, reader.Operand2.Type);
                    Assert.AreEqual(Register.Rbp, reader.Operand2.GetRegister());
                    break;

                case OperandFormat.Gd_Eb:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Ebp, reader.Operand1.GetRegister());
                    Assert.AreEqual(OperandType.Register, reader.Operand2.Type);
                    Assert.AreEqual(opCode.RexPrefix != 0 ? Register.Spl : Register.Ah, reader.Operand2.GetBaseRegister());
                    break;

                case OperandFormat.Gd_Ew:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Ebp, reader.Operand1.GetRegister());
                    Assert.AreEqual(OperandType.Register, reader.Operand2.Type);
                    Assert.AreEqual(Register.Sp, reader.Operand2.GetBaseRegister());
                    break;

                case OperandFormat.Gd_Ed:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Ebp, reader.Operand1.GetRegister());
                    Assert.AreEqual(OperandType.Register, reader.Operand2.Type);
                    Assert.AreEqual(Register.Esp, reader.Operand2.GetBaseRegister());
                    break;

                case OperandFormat.Gd_Eq:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Ebp, reader.Operand1.GetRegister());
                    Assert.AreEqual(OperandType.Register, reader.Operand2.Type);
                    Assert.AreEqual(Register.Rsp, reader.Operand2.GetBaseRegister());
                    break;

                case OperandFormat.Gq_Eq:
                    Assert.AreEqual(OperandType.Register, reader.Operand1.Type);
                    Assert.AreEqual(Register.Rbp, reader.Operand1.GetRegister());
                    Assert.AreEqual(OperandType.Register, reader.Operand2.Type);
                    Assert.AreEqual(Register.Rsp, reader.Operand2.GetBaseRegister());
                    break;

                default:
                    throw new NotImplementedException();
            }

            Assert.IsFalse(reader.Read());
        }

        public static IEnumerable<OpCodeProperties> OpCodesWithLockUnsupported()
        {
            return OpCodes.Where(o => !o.Prefixes.HasFlag(InstructionPrefixes.Lock));
        }

        [Test]
        [TestCaseSource("OpCodesWithLockUnsupported")]
        [ExpectedException(typeof(FormatException))]
        public void InstructionReader_ForLockPrefix_ThrowsFormatException(OpCodeProperties opCode)
        {
            var byteList = new List<byte> { 0xF0 };
            // use a memory address to smoke out false negatives
            byteList.AddRange(this.GetBytes(opCode, 0x00));
            var bytes = byteList.ToArray();
            
            var mode = GetExecutionMode(opCode);
            var reader = new InstructionReader(new MemoryStream(bytes), mode);

            reader.Read();
        }

        public static IEnumerable<OpCodeProperties> OpCodesWithLockSupported()
        {
            return OpCodes.Where(o => o.Prefixes.HasFlag(InstructionPrefixes.Lock));
        }

        [Test]
        [TestCaseSource("OpCodesWithLockSupported")]
        [ExpectedException(typeof(FormatException))]
        public void InstructionReader_ForLockPrefixWithNoMemoryAccess_ThrowsFormatException(OpCodeProperties opCode)
        {
            var byteList = new List<byte> { 0xF0 };
            byteList.AddRange(this.GetBytes(opCode, 0xC0)); // EAX
            var bytes = byteList.ToArray();

            var mode = GetExecutionMode(opCode);
            var reader = new InstructionReader(new MemoryStream(bytes), mode, opCode.OperandSize == OperandSize.Size32);

            reader.Read();
        }

        [Test]
        [TestCaseSource("OpCodesWithLockSupported")]
        public void InstructionReader_ForLockPrefixWithMemoryAccess_DoesNotThrow(OpCodeProperties opCode)
        {
            var byteList = new List<byte> { 0xF0 };
            byteList.AddRange(this.GetBytes(opCode, 0x00)); // [EAX]
            var bytes = byteList.ToArray();

            var mode = GetExecutionMode(opCode);
            var reader = new InstructionReader(new MemoryStream(bytes), mode, opCode.OperandSize == OperandSize.Size32);

            reader.Read();
        }

        private static ExecutionMode GetExecutionMode(OpCodeProperties opCode)
        {
            return (opCode.Compatibility & Compatibility.Compatibility64) != Compatibility.Valid
                || opCode.OperandSize == OperandSize.Size16
                ? ExecutionMode.CompatibilityMode
                : ExecutionMode.Long64Bit;
        }

        public IEnumerable<OpCodeProperties> InstructionsInvalidIn64Bit()
        {
            return OpCodes.Where(o => o.Compatibility == Compatibility.Invalid64);
        }
            
        [Test]
        [TestCaseSource("InstructionsInvalidIn64Bit")]
        [ExpectedException(typeof(FormatException))]
        public void InstructionReader_For64BitMode_ThrowsFormatException(OpCodeProperties opCode)
        {
            var bytes = this.GetBytes(opCode);
            var reader = new InstructionReader(new MemoryStream(bytes), ExecutionMode.Long64Bit);

            reader.Read();
        }

        public IEnumerable<OpCodeProperties> InstructionsWithMemoryParameters()
        {
            return
                OpCodes.Where(
                    o =>
                        o.Operands == OperandFormat.Mw || o.Operands == OperandFormat.Md
                        || o.Operands == OperandFormat.Mq);
        }

        [Test]
        [TestCaseSource("InstructionsWithMemoryParameters")]
        [ExpectedException(typeof(FormatException))]
        public void InstructionReader_ForRegister_ThrowsFormatException(OpCodeProperties opCode)
        {
            var bytes = GetBytes(opCode, 0xc0); // EAX
            var reader = new InstructionReader(new MemoryStream(bytes), ExecutionMode.Long64Bit);

            reader.Read();
        }

        [Test]
        public void InstructionReader_ForAadWithBase10_HidesOperand()
        {
            // This is a special case - conventionally AAD 0AH is represented as AAD
            var bytes = new byte[] { 0xD5, 0x0A };
            var reader = new InstructionReader(new MemoryStream(bytes), ExecutionMode.CompatibilityMode);

            reader.Read();

            Assert.AreEqual(OperandType.None, reader.Operand1.Type);
            Assert.AreEqual(OperandType.None, reader.Operand2.Type);
        }

        [Test]
        public void InstructionReader_ForAamWithBase10_HidesOperand()
        {
            // This is a special case - conventionally AAM 0AH is represented as AAM
            var bytes = new byte[] { 0xD4, 0x0A };
            var reader = new InstructionReader(new MemoryStream(bytes), ExecutionMode.CompatibilityMode);

            reader.Read();

            Assert.AreEqual(OperandType.None, reader.Operand1.Type);
            Assert.AreEqual(OperandType.None, reader.Operand2.Type);
        }

        private byte[] GetBytes(OpCodeProperties opCode)
        {
            byte modrm;
            switch (opCode.Operands)
            {
                case OperandFormat.Mw:
                case OperandFormat.Md:
                case OperandFormat.Mq:
                case OperandFormat.Mdq:
                    modrm = 0x07;
                    break;

                default:
                    modrm = 0xc4;
                    break;
            }


            return GetBytes(opCode, modrm);
        }

        private byte[] GetBytes(OpCodeProperties opCode, byte modrm)
        {
            var bytes = new List<byte>();

            if ((opCode.Prefixes & InstructionPrefixes.RepNZ) != 0)
            {
                bytes.Add(0xF2);
            }

            if (opCode.RexPrefix != 0)
            {
                bytes.Add((byte)(opCode.RexPrefix | RexPrefix.Magic));
            }

            bytes.AddRange(opCode.OpCode);

            switch (opCode.Operands)
            {
                case OperandFormat.Ib:
                    bytes.Add(0x11);
                    break;

                case OperandFormat.Iw_Ib:
                    bytes.Add(0x11);
                    bytes.Add(0x11);
                    bytes.Add(0x22);
                    break;

                case OperandFormat.Jw:
                    bytes.Add(0x11);
                    bytes.Add(0x11);
                    break;

                case OperandFormat.Jd:
                    bytes.Add(0x11);
                    bytes.Add(0x11);
                    bytes.Add(0x11);
                    bytes.Add(0x11);
                    break;

                case OperandFormat.Aww:
                    bytes.Add(0x11);
                    bytes.Add(0x11);
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    break;

                case OperandFormat.Awd:
                    bytes.Add(0x11);
                    bytes.Add(0x11);
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    break;

                case OperandFormat.Eb:
                case OperandFormat.Ew:
                case OperandFormat.Ed:
                case OperandFormat.Eq:
                case OperandFormat.Mw:
                case OperandFormat.Md:
                case OperandFormat.Mdq:
                case OperandFormat.Mq:
                    bytes.Add((byte)(modrm | (opCode.OpCodeReg << 3)));
                    break;

                case OperandFormat.AL_Ib:
                    bytes.Add(0x22);
                    break;

                case OperandFormat.AX_Iw:
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    break;

                case OperandFormat.RAX_Id:
                case OperandFormat.EAX_Id:
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    break;

                case OperandFormat.Gb_Eb:
                case OperandFormat.Gw_Ew:
                case OperandFormat.Gd_Eb:
                case OperandFormat.Gd_Ew:
                case OperandFormat.Gd_Ed:
                case OperandFormat.Gd_Eq:
                case OperandFormat.Gq_Eq:
                case OperandFormat.Eb_Gb:
                case OperandFormat.Ew_Gw:
                case OperandFormat.Ed_Gd:
                case OperandFormat.Eq_Gq:
                    // CH/BPL
                    bytes.Add((byte)(modrm | 0x28));
                    break;                    

                case OperandFormat.Eb_Ib:
                case OperandFormat.Ew_Ib:
                case OperandFormat.Ed_Ib:
                case OperandFormat.Eq_Ib:
                    bytes.Add((byte)(modrm | (opCode.OpCodeReg << 3)));
                    bytes.Add(0x22);
                    break;

                case OperandFormat.Ew_Iw:
                    bytes.Add((byte)(modrm | (opCode.OpCodeReg << 3)));
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    break;

                case OperandFormat.Eq_Id:
                case OperandFormat.Ed_Id:
                    bytes.Add((byte)(modrm | (opCode.OpCodeReg << 3)));
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    bytes.Add(0x22);
                    break;
            }

            return bytes.ToArray();
        }
    }
}
