using System;
using Unmanaged;

namespace Shaders.Components
{
    public struct IsShaderRequest
    {
        public readonly ShaderType type;
        public ASCIIText256 address;
        public TimeSpan timeout;
        public TimeSpan duration;
        public Status status;

        public IsShaderRequest(ShaderType type, ASCIIText256 address, TimeSpan timeout)
        {
            this.type = type;
            this.address = address;
            this.timeout = timeout;
            duration = TimeSpan.Zero;
            status = Status.Submitted;
        }

        public enum Status : byte
        {
            Submitted,
            Loading,
            Loaded,
            NotFound
        }
    }
}