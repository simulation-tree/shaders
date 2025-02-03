using Worlds;

namespace Shaders.Components
{
    [ArrayElement]
    public readonly struct ShaderByte
    {
        private readonly byte value;

        public ShaderByte(byte value)
        {
            this.value = value;
        }
    }
}