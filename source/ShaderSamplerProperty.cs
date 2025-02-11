using System;
using Unmanaged;
using Worlds;

namespace Shaders
{
    /// <summary>
    /// Describes a shader property that references a sampled texture.
    /// </summary>
    [ArrayElement]
    public readonly struct ShaderSamplerProperty : IEquatable<ShaderSamplerProperty>
    {
        public readonly FixedString name;
        public readonly byte binding;
        public readonly byte set;

        public ShaderSamplerProperty(FixedString name, byte binding, byte set)
        {
            this.name = name;
            this.binding = binding;
            this.set = set;
        }

        public ShaderSamplerProperty(USpan<char> name, byte binding, byte set)
        {
            this.name = new(name);
            this.binding = binding;
            this.set = set;
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is ShaderSamplerProperty property && Equals(property);
        }

        public readonly bool Equals(ShaderSamplerProperty other)
        {
            return name.Equals(other.name) && binding == other.binding && set == other.set;
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(name, binding, set);
        }

        public unsafe readonly override string ToString()
        {
            USpan<char> buffer = stackalloc char[name.Length + 32];
            uint length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        public readonly uint ToString(USpan<char> buffer)
        {
            uint length = name.CopyTo(buffer);
            buffer[length++] = ' ';
            buffer[length++] = '(';
            length += binding.ToString(buffer.Slice(length));
            buffer[length++] = ',';
            buffer[length++] = ' ';
            length += set.ToString(buffer.Slice(length));
            buffer[length++] = ')';
            return length;
        }

        public static bool operator ==(ShaderSamplerProperty left, ShaderSamplerProperty right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ShaderSamplerProperty left, ShaderSamplerProperty right)
        {
            return !(left == right);
        }
    }
}
