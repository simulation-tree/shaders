using Worlds;

namespace Shaders.Components
{
    [Component]
    public struct IsShaderRequest
    {
        public uint version;
        public rint vertex;
        public rint fragment;

        public IsShaderRequest(rint vertex, rint fragment)
        {
            version = default;
            this.vertex = vertex;
            this.fragment = fragment;
        }
    }
}
