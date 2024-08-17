using Simulation;

namespace Shaders.Components
{
    public struct IsShader
    {
        public uint version;
        public rint vertexReference;
        public rint fragmentReference;

        public IsShader(rint vertexReference, rint fragmentReference)
        {
            this.version = default;
            this.vertexReference = vertexReference;
            this.fragmentReference = fragmentReference;
        }
    }
}
