﻿namespace Fantasm.Disassembler
{
    /// <summary>
    /// The addressable registers of the processor.
    /// </summary>
    public enum Register
    {
        None,

        Al,
        Bl,
        Cl,
        Dl,
        Ah,
        Bh,
        Ch,
        Dh,
        Dil,
        Sil,
        Bpl,
        Spl,
        R8L,
        R9L,
        R10L,
        R11L,
        R12L,
        R13L,
        R14L,
        R15L,

        Ax,
        Bx,
        Cx,
        Dx,
        Di,
        Si,
        Bp,
        Sp,
        R8W,
        R9W,
        R10W,
        R11W,
        R12W,
        R13W,
        R14W,
        R15W,

        Eax,
        Ebx,
        Ecx,
        Edx,
        Edi,
        Esi,
        Ebp,
        Esp,
        R8D,
        R9D,
        R10D,
        R11D,
        R12D,
        R13D,
        R14D,
        R15D,

        Rax,
        Rbx,
        Rcx,
        Rdx,
        Rdi,
        Rsi,
        Rbp,
        Rsp,
        R8,
        R9,
        R10,
        R11,
        R12,
        R13,
        R14,
        R15
/*
        Cs,
        Ds,
        Ss,
        Es,
        Fs,
        Gs,

        Eflags,
        Eip,
*/
    }
}
