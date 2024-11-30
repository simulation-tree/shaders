using Worlds;

namespace Shaders.Components
{
    [Component]
    public struct IsShader
    {
        /// <summary>
        /// Incremented when the <see cref="byte"/> data changes.
        /// </summary>
        public uint version;

        public rint vertex;
        public rint fragment;

        public IsShader(rint vertex, rint fragment)
        {
            this.version = default;
            this.vertex = vertex;
            this.fragment = fragment;
        }
    }
}
