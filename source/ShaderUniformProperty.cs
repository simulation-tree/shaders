using System;
using Unmanaged;

namespace Shaders
{
    /// <summary>
    /// Describes a uniform buffer object shader property.
    /// </summary>
    public readonly struct ShaderUniformProperty : IEquatable<ShaderUniformProperty>
    {
        public readonly ASCIIText256 label;
        public readonly ASCIIText256 typeName;
        public readonly uint binding;
        public readonly uint set;
        public readonly uint byteLength;

        public ShaderUniformProperty(ASCIIText256 name, ASCIIText256 typeName, uint binding, uint set, uint byteLength)
        {
            this.label = name;
            this.typeName = typeName;
            this.binding = binding;
            this.set = set;
            this.byteLength = byteLength;
        }

        public ShaderUniformProperty(ReadOnlySpan<char> name, ReadOnlySpan<char> typeName, uint binding, uint set, uint byteLength)
        {
            this.label = new(name);
            this.typeName = new(typeName);
            this.binding = binding;
            this.set = set;
            this.byteLength = byteLength;
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is ShaderUniformProperty property && Equals(property);
        }

        public readonly bool Equals(ShaderUniformProperty other)
        {
            return label.Equals(other.label) && binding == other.binding && set == other.set && byteLength == other.byteLength;
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(label, binding, set, byteLength);
        }

        public unsafe readonly override string ToString()
        {
            Span<char> buffer = stackalloc char[label.Length + 32];
            int length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        public readonly int ToString(Span<char> buffer)
        {
            int length = label.CopyTo(buffer);
            buffer[length++] = ' ';
            buffer[length++] = '(';
            length += set.ToString(buffer.Slice(length));
            buffer[length++] = ',';
            buffer[length++] = ' ';
            length += binding.ToString(buffer.Slice(length));
            buffer[length++] = ')';
            return length;
        }

        public static bool operator ==(ShaderUniformProperty left, ShaderUniformProperty right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ShaderUniformProperty left, ShaderUniformProperty right)
        {
            return !(left == right);
        }
    }
}