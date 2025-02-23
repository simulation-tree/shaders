using System;

namespace Shaders.Components
{
    public readonly struct IsShader : IEquatable<IsShader>
    {
        public readonly uint version;
        public readonly ShaderType type;

        public IsShader(uint version, ShaderType type)
        {
            this.version = version;
            this.type = type;
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is IsShader shader && Equals(shader);
        }

        public readonly bool Equals(IsShader other)
        {
            return version == other.version && type == other.type;
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(version, type);
        }

        public readonly IsShader IncrementVersion()
        {
            return new IsShader(version + 1, type);
        }

        public static bool operator ==(IsShader left, IsShader right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IsShader left, IsShader right)
        {
            return !(left == right);
        }
    }
}