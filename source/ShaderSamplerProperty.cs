using System;
using Unmanaged;

namespace Shaders
{
    /// <summary>
    /// Describes a shader property that references a sampled texture.
    /// </summary>
    public readonly struct ShaderSamplerProperty : IEquatable<ShaderSamplerProperty>
    {
        public readonly ASCIIText256 name;
        public readonly byte binding;
        public readonly byte set;

        public ShaderSamplerProperty(ASCIIText256 name, byte binding, byte set)
        {
            this.name = name;
            this.binding = binding;
            this.set = set;
        }

        public ShaderSamplerProperty(System.Span<char> name, byte binding, byte set)
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
            Span<char> buffer = stackalloc char[name.Length + 32];
            int length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        public readonly int ToString(Span<char> destination)
        {
            int length = name.CopyTo(destination);
            destination[length++] = ' ';
            destination[length++] = '(';
            length += binding.ToString(destination.Slice(length));
            destination[length++] = ',';
            destination[length++] = ' ';
            length += set.ToString(destination.Slice(length));
            destination[length++] = ')';
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
