using System;

namespace Shaders.Components
{
    public struct IsShader : IEquatable<IsShader>
    {
        public ushort version;
        public ShaderType type;

        public IsShader(ushort version, ShaderType type)
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