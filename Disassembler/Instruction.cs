namespace Fantasm.Disassembler
{
    /// <summary>
    /// The x86 instruction mnemonics.
    /// </summary>
    public enum Instruction
    {
        Unknown,

        // Data transfer instructions
        Mov,
        Cmove,
        Cmovne,
        Cmova,
        Cmovae,
        Cmovb,
        Cmovbe,
        Cmovg,
        Cmovge,
        Cmovl,
        Cmovle,
        Cmovc,
        Cmovnc,
        Cmovo,
        Cmovno,
        Cmovs,
        Cmovns,
        Cmovp,
        Cmovnp,
        Xchg,
        Bswap,
        Xadd,
        Cmpxchg,
        Cmpxchg8b,
        Push,
        Pop,
        Pusha,
        Popa,
        Cwd,
        Cbw,
        Movsx,
        Movzx,

        // Binary arithmetic instructions
        Add,
        Adc,
        Sub,
        Sbb,
        Imul,
        Mul,
        Idiv,
        Div,
        Inc,
        Dec,
        Neg,
        Cmp,

        // Decimal arithmetic instructions
        Daa,
        Das,
        Aaa,
        Aas,
        Aam,
        Aad,

        // Logical instructions
        And,
        Or,
        Xor,
        Not,

        // Shift and rotate instructions
        Sar,
        Shr,
        Sal,
        Shrd,
        Shld,
        Ror,
        Rol,
        Rcr,
        Rcl,

        // Bit and byte instructions
        Bt,
        Bts,
        Btr,
        Btc,
        Bsf,
        Bsr,
        Sete,
        Setne,
        Seta,
        Setae,
        Setb,
        Setbe,
        Setg,
        Setge,
        Setl,
        Setle,
        Sets,
        Setns,
        Seto,
        Setno,
        Setpe,
        Setpo,
        Test,

        // Control transfer instructions
        Jmp,
        Je,
        Jne,
        Ja,
        Jae,
        Jb,
        Jbe,
        Jg,
        Jge,
        Jl,
        Jle,
        Jc,
        Jnc,
        Jo,
        Jno,
        Js,
        Jns,
        Jpo,
        Jpe,
        Jcxz,
        Loop,
        Loopz,
        Loopnz,
        Call,
        Ret,
        Iret,
        Int,
        Into,
        Bound,
        Enter,
        Leave,

        // String instructions
        Movs,
        Cmps,
        Scas,
        Lods,
        Stos,
        Rep,
        Repe,
        Repne,

        // I/O instructions
        In,
        Out,
        Ins,
        Outs,

        // Flag control (EFLAG) instructions
        Stc,
        Clc,
        Cmc,
        Cld,
        Std,
        Lahf,
        Sahf,
        Pushf,
        Popf,
        Sti,
        Cli,

        // Segment register instructions
        Lds,
        Les,
        Lfs,
        Lgs,
        Lss,

        // Miscellaneous instructions
        Lea,
        Nop,
        Ud1,
        Ud2,
        Xlat,
        Cpuid,
        Movbe,

        // Random number generator instruction
        Rdrand,

        // BMI1, BMI2
        Andn,
        Bextr,
        Blsi,
        Blsmk,
        Blsr,
        Bzhi,
        Lzcnt,
        Mulx,
        Pdep,
        Pext,
        Rorx,
        Sarx,
        Shlx,
        Shrx,
        Tzcnt,


        // x87 FPU data transfer instructions
        Fld,
        Fst,
        Fstp,
        Fild,
        Fist,
        Fistp,
        Fbld,
        Fbstp,
        Fxchg,
        Fcmove,
        Fcmovne,
        Fcmovb,
        Fcmovbe,
        Fcmovnb,
        Fcmovnbe,
        Fcmovu,
        Fcmovnu,

        // x87 FPU basic arithmetic instructions
        Fadd,
        Faddp,
        Fiadd,
        Fsub,
        Fsubp,
        Fisub,
        Fsubr,
        Fsubrp,
        Fisubr,
        Fmul,
        Fmulp,
        Fimul,
        Fdiv,
        Fdivp,
        Fidiv,
        Fdivr,
        Fdivrp,
        Fidivr,
        Fprem,
        Fprem1,
        Fabs,
        Fchs,
        Frndint,
        Fscale,
        Fsqrt,
        Fxtract,

        // x87 FPU comparison instructions
        Fcom,
        Fcomp,
        Fcompp,
        Fucom,
        Fucomp,
        Fucompp,
        Ficom,
        Ficomp,
        Fcomi,
        Fucomi,
        Fcomip,
        Fucomip,
        Fxam,

        // x87 FPU transcendental instructions
        Fsin,
        Fcos,
        Fsincos,
        Fptan,
        Fpatan,
        F2xm1,
        Fyl2x,
        Fyl2xp1,

        // x87 FPU load constants instructions
        Fld1,
        Fldz,
        Fldpi,
        Fldl2e,
        Fldln2,
        Fldl2t,
        Fldlg2,

        // x87 FPU control instructions
        Fincstp,
        Fdecstp,
        Ffree,
        Finit,
        Fninit,
        Fclex,
        Fnclex,
        Fstcw,
        Fnstcw,
        Fldcw,
        Fstenv,
        Fnstenv,
        Fldenv,
        Fsave,
        Fnsave,
        Frstor,
        Fstsw,
        Fnstsw,
        Wait,
        Fnop,

        
        // x87 FPU and SIMD state management instructions
        Fxsave,
        Fxrstor,


        // MMX data transfer instructions
        Movd,
        Movq,

        // MMX conversion instructions
        Packsswb,
        Packssdw,
        Packuswb,
        Punpckhbw,
        Punpckhwd,
        Punpckhdq,
        Punpcklbw,
        Punpcklwd,
        Punpckldq,

        // MMX packed arithmetic instructions
        Paddb,
        Paddw,
        Paddd,
        Paddsb,
        Paddsw,
        Paddusb,
        Paddusw,
        Psubb,
        Psubw,
        Psubd,
        Psubsb,
        Psubsw,
        Psubusb,
        Psubusw,
        Pmulhw,
        Pmullw,
        Pmaddwd,

        // MMX comparison instructions
        Pcmpeqb,
        Pcmpeqw,
        Pcmpeqd,
        Pcmpgtb,
        Pcmpgtw,
        Pcmpgtd,

        // MMX logical instructions
        Pand,
        Pandn,
        Por,
        Pxor,

        // MMX shift and rotate instructions
        Psllw,
        Pslld,
        Psllq,
        Psrlw,
        Psrld,
        Psrlq,
        Psraw,
        Psrad,

        // MMX state management instructions
        Emms,


        // SSE data transfer instructions
        Movaps,
        Movups,
        Movhps,
        Movhlps,
        Movlps,
        Movlhps,
        Movmskps,
        Movss,

        // SSE packed arithmetic instructions
        Addps,
        Addss,
        Subps,
        Subss,
        Mulps,
        Mulss,
        Divps,
        Divss,
        Rcpps,
        Rcpss,
        Sqrtps,
        Sqrtss,
        Rsqrtps,
        Rsqrtss,
        Maxps,
        Maxss,
        Minps,
        Minss,

        // SSE comparison instructions
        Cmpps,
        Cmpss,
        Comiss,
        Ucomiss,

        // SSE logical instructions
        Andps,
        Andnps,
        Orps,
        Xorps,

        // SSE shuffle and unpack instructions
        Shufps,
        Unpckhps,
        Unpcklps,

        // SSE conversion instructions
        Cvtpi2ps,
        Cvtsi2ss,
        Cvtps2pi,
        Cvttps2pi,
        Cvtss2si,
        Cvttss2si,

        // SSE MXCSR state management instructions
        Ldmxcsr,
        Stmxcsr,

        // SSE 64-bit SIMD integer instructions
        Pavgb,
        Pavgw,
        Pextrw,
        Pinsrw,
        Pmaxub,
        Pmaxsw,
        Pminub,
        Pminsw,
        Pmovmskb,
        Pmulhuw,
        Psadbw,
        Pshufw,

        // SSE Cacheability control, prefetch and instruction ordering instructions
        Maskmovq,
        Movntq,
        Movntps,
        Prefetcht0,
        Prefetcht1,
        Prefetcht2,
        Prefetchnta,
        Sfence,


        // SSE2 data movement instructions
        Movapd,
        Movupd,
        Movhpd,
        Movlpd,
        Movmskpd,
        Movsd,

        // SSE2 packed arithmetic instructions
        Addpd,
        Addsd,
        Subpd,
        Subsd,
        Mulpd,
        Mulsd,
        Divpd,
        Divsd,
        Sqrtpd,
        Sqrtsd,
        Maxpd,
        Maxsd,
        Minpd,
        Minsd,

        // SSE2 logical instructions
        Andpd,
        Andnpd,
        Orpd,
        Xorpd,

        // SSE2 compare instructions
        Cmppd,
        Cmpsd,
        Comisd,
        Ucomisd,

        // SSE2 shuffle and unpack instructions
        Shufpd,
        Unpckhpd,
        Unpcklpd,

        // SSE2 conversion instructions
        Cvtpd2pi,
        Cvttpd2pi,
        Cvtpi2pd,
        Cvtpd2dq,
        Cvttpd2dq,
        Cvtdq2pd,
        Cvtps2ps,
        Cvtpd2ps,
        Cvtss2sd,
        Cvtsd2ss,
        Cvtsd2si,
        Cvttsd2si,
        Cvtsi2sd,

        // SSE2 Packed single precision floating point instructions
        Cvtdq2ps,
        Cvtps2dq,
        Cvttps2dq,

        // SSE2 128-bit SIMD integer instructions
        Movdqa,
        Movdqu,
        Movq2dq,
        Movdq2q,
        Pmuludq,
        Paddq,
        Psubq,
        Pshuflw,
        Pshufhw,
        Pshufd,
        Pslldq,
        Psrldq,
        Punpckhqdq,
        Punpcklqdq,

        // SSE2 Cacheability control and ordering instructions
        Clflush,
        Lfence,
        Mfence,
        Pause,
        Maskmovdqu,
        Movntpd,
        Movntdq,
        Movnti,


        // SSE3 x87-FP integer conversion instruction
        Fisttp,

        // SSE3 specialized 128-bit unaligned data load instruction
        Lddqu,

        // SSE3 SIMD floating-point packed ADD/SUB instructions
        Addsubps,
        Addsubpd,

        // SSE3 SIMD floating-point horizontal ADD/SUB instructions
        Haddps,
        Hsubps,
        Haddpd,
        Hsubpd,

        // SSE3 SIMD floating-point LOAD/MOVE/DUPLICATE instructions
        Movshdup,
        Movsldup,
        Movddup,

        // SSE3 agent synchronization instructions
        Monitor,
        Mwait,

        // SSE3 horizontal addition/subtraction
        Phaddw,
        Phaddsw,
        Phaddd,
        Phsubw,
        Phsubsw,
        Phsubd,

        // SSE3 packed absolute values
        Pabsb,
        Pabsw,
        Pabsd,

        // SSE3 multiply and add packed signed and unsigned bytes
        Pmaddubsw,

        // SSE3 packed multiply high with round and scale
        Pmulhrsw,

        // SSE3 packed shuffle bytes
        Pshufb,

        // SSE3 packed sign
        Psignb,
        Psignw,
        Psignd,

        // SSE3 packed align right
        Palignr,


        // SSE4.1 dword multiply instructions
        Pmulld,
        Pmuldq,

        // SSE4.1 floating-point dot product instructions
        Dppd,
        Dpps,

        // SSE4.1 streaming load hint instruction
        Movntdqa,

        // SSE4.1 packed blending instructions
        Blendpd,
        Blendps,
        Blendvpd,
        Blendvps,
        Pblendvb,
        Pblendw,

        // SSE4.1 packed integer MIN/MAX instructions
        Pminuw,
        Pminud,
        Pminsb,
        Pminsd,
        Pmaxuw,
        Pmaxud,
        Pmaxsb,
        Pmaxsd,

        // SSE4.1 floating-point round instructions with selectable rounding mode
        Roundps,
        Roundpd,
        Roundss,
        Roundsd,

        // SSE4.1 insertion and extractions from XMM registers
        Extractps,
        Insertps,
        Pinsrb,
        Pinsrd,
        Pinsrq,
        Pextrb,
        //Pextrw,
        Pextrd,
        Pextrq,

        // SSE4.1 packed integer format conversions
        Pmovsxbw,
        Pmovzxbw,
        Pmovsxbd,
        Pmovzxbd,
        Pmovsxwd,
        Pmovzxwd,
        Pmovsxbq,
        Pmovzxbq,
        Pmovsxwq,
        Pmovzxwq,
        Pmovsxdq,
        Pmovzxdq,

        // SSE4.1 improved sums of absolute differences (SAD) for 4-byte blocks
        Mpsadbw,

        // SSE4.1 horizontal search
        Phminposuw,

        // SSE4.1 packed test
        Ptest,
            
        // SSE4.1 packed qword equality comparisons
        Pcmpeqq,

        // SSE4.1 dword packing with unsigned saturation
        Packusdw,


        // SSE4.2 string and text processing instructions
        Pcmpestri,
        Pcmpestrm,
        Pcmpistri,
        Pcmpistrm,

        // SSE4.2 packed comparison SIMD integer instruction
        Pcmpgtq,

        // SSE4.2 Application targeted accelerator instructions
        Crc32,
        Popcnt,


        // AESNI and PCLMULQDQ
        Aesdec,
        Aesdeclast,
        Aesenc,
        Aesenclast,
        Aesimc,
        Aeskeygenassist,
        Pclmulqdq,


        // 16-bit floating-point conversion
        Vcvtph2ps,
        Vcvtps2ph,


        // Intel transactional synchronization extensions (TSX)
        Xabort,
        Xacquire,
        Xrelease,
        Xbegin,
        Xend,
        Xtest,


        // System instructions
        Lgdt,
        Sgdt,
        Lldt,
        Sldt,
        Ltr,
        Str,
        Lidt,
        Sidt,
        //Mov,
        Lmsw,
        Smsw,
        Clts,
        Arpl,
        Lar,
        Lsl,
        Verr,
        Verw,
        Invd,
        Wbinvd,
        Invlpg,
        Invpcid,
        Hlt,
        Rsm,
        Rdmsr,
        Wrmsr,
        Rdpmc,
        Rdtsc,
        Rdtscp,
        Sysenter,
        Sysexit,
        Xsave,
        Xsaveopt,
        Xrstor,
        Xgetbv,
        Xsetbv,
        Rdfsbase,
        Rdgsbase,
        Wrfsbase,
        Wrgsbase,


        // 64-bit mode instructions
        Cdqe,
        Cmpsq,
        Cmpxchg16b,
        Lodsq,
        Movsq,
        //Movzx,
        Stsq,
        Swapgs,
        Syscall,
        Sysret,


        // Virtual-machine extensions
        Vmptrld,
        Vmptrst,
        Vmclear,
        Vmread,
        Vmwrite,
        Vmlaunch,
        Vmresume,
        Vmxoff,
        Vmxon,
        Invept,
        Invvpid,
        Vmcall,
        Vmfunc,


        // Safer mode extensions
        Getsec
    }
}
