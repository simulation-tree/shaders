namespace Shaders.Components
{
    public readonly struct IsShader
    {
        public readonly uint version;
        public readonly ShaderType type;

        public IsShader(uint version, ShaderType type)
        {
            this.version = version;
            this.type = type;
        }

        public readonly IsShader IncrementVersion()
        {
            return new IsShader(version + 1, type);
        }
    }
}