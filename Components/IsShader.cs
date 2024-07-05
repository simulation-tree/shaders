using Simulation;

namespace Shaders.Components
{
    public struct IsShader(EntityID vertex, EntityID fragment)
    {
        public EntityID vertex = vertex;
        public EntityID fragment = fragment;

        /// <summary>
        /// Changed when the <see cref="byte"/> collections have updated and the shader needs to update.
        /// </summary>
        public bool changed = true;
    }
}
