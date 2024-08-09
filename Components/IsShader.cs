using Simulation;

namespace Shaders.Components
{
    public struct IsShader
    {
        public readonly uint version;
        public readonly eint vertex;
        public readonly eint fragment;

        public IsShader(uint version, eint vertex, eint fragment)
        {
            this.version = version;
            this.vertex = vertex;
            this.fragment = fragment;
        }

        public IsShader(eint vertex, eint fragment)
        {
            this.version = default;
            this.vertex = vertex;
            this.fragment = fragment;
        }
    }
}
