using System;

namespace Fantasm.Disassembler.Tests
{
    [Flags]
    public enum Compatibility
    {
        Valid = 0,

        NotEncodable64 = 1,
        NotSupported64 = 2,
        Invalid64 = 3,

        NotEncodable32 = 4,
        Invalid32 = 8,

        Compatibility32 = 12,
        Compatibility64 = 3,
    }
}
