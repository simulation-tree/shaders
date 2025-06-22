using System;
using Unmanaged;

namespace Shaders.Components
{
    public struct IsShaderRequest
    {
        public readonly Flags flags;
        public ASCIIText256 address;
        public double timeout;
        public double duration;
        public Status status;

        public IsShaderRequest(Flags flags, ASCIIText256 address, double timeout)
        {
            this.flags = flags;
            this.address = address;
            this.timeout = timeout;
            duration = 0;
            status = Status.Submitted;
        }

        public enum Status : byte
        {
            Submitted,
            Loading,
            Loaded,
            NotFound
        }

        [Flags]
        public enum Flags : byte
        {
            None = 0,
            FragmentShader = 1,
            VertexShader = 2
        }
    }
}