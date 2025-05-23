using Unmanaged;

namespace Shaders.Components
{
    public struct IsShaderRequest
    {
        public readonly ShaderType type;
        public ASCIIText256 address;
        public double timeout;
        public double duration;
        public Status status;

        public IsShaderRequest(ShaderType type, ASCIIText256 address, double timeout)
        {
            this.type = type;
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
    }
}