using Worlds;

namespace Shaders.Components
{
    [Component]
    public struct IsShader
    {
        public rint vertex;
        public rint fragment;
        public uint version;

        public IsShader(rint vertex, rint fragment)
        {
            this.version = default;
            this.vertex = vertex;
            this.fragment = fragment;
        }

        public IsShader(rint vertex, rint fragment, uint version)
        {
            this.vertex = vertex;
            this.fragment = fragment;
            this.version = version;
        }
    }
}
