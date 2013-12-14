using System.IO;

namespace Fantasm.Disassembler.Tests
{
    public class InstructionReaderTestBase
    {
        protected static InstructionReader ReadBytes(params byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            return new InstructionReader(stream, ExecutionMode.CompatibilityMode);
        }

        protected static InstructionReader ReadBytes16(params byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            return new InstructionReader(stream, ExecutionMode.CompatibilityMode, false);
        }

        protected static InstructionReader ReadBytes64(params byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            return new InstructionReader(stream, ExecutionMode.Allow64Bit, true);
        }
    }
}
