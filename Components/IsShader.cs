using Simulation;

namespace Shaders.Components
{
    public struct IsShader(eint vertex, eint fragment)
    {
        public eint vertex = vertex;
        public eint fragment = fragment;

        /// <summary>
        /// Changed when the <see cref="byte"/> collections have updated and the shader needs to update.
        /// </summary>
        public bool changed = true;
    }
}
