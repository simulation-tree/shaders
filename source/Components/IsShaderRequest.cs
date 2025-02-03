using System;
using Unmanaged;
using Worlds;

namespace Shaders.Components
{
    [Component]
    public struct IsShaderRequest
    {
        public readonly ShaderType type;
        public FixedString address;
        public TimeSpan timeout;
        public TimeSpan duration;
        public Status status;

        public IsShaderRequest(ShaderType type, FixedString address, TimeSpan timeout)
        {
            this.type = type;
            this.address = address;
            this.timeout = timeout;
            duration = TimeSpan.Zero;
            status = Status.Submitted;
        }

        public readonly IsShaderRequest BecomeLoaded()
        {
            IsShaderRequest request = this;
            request.status = Status.Loaded;
            return request;
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