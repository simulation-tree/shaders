using System;
using Unmanaged;
using Worlds;

namespace Shaders
{
    /// <summary>
    /// Describes a uniform buffer object shader property.
    /// </summary>
    [ArrayElement]
    public readonly struct ShaderUniformProperty : IEquatable<ShaderUniformProperty>
    {
        public readonly FixedString label;
        public readonly byte binding;
        public readonly byte set;

        /// <summary>
        /// Size of the uniform buffer object in bytes.
        /// </summary>
        public readonly uint size;

        public ShaderUniformProperty(FixedString name, byte binding, byte set, uint size)
        {
            this.label = name;
            this.binding = binding;
            this.set = set;
            this.size = size;
        }

        public ShaderUniformProperty(USpan<char> name, byte binding, byte set, uint size)
        {
            this.label = new(name);
            this.binding = binding;
            this.set = set;
            this.size = size;
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is ShaderUniformProperty property && Equals(property);
        }

        public readonly bool Equals(ShaderUniformProperty other)
        {
            return label.Equals(other.label) && binding == other.binding && set == other.set && size == other.size;
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(label, binding, set, size);
        }

        public unsafe readonly override string ToString()
        {
            USpan<char> buffer = stackalloc char[(int)(FixedString.Capacity + 16)];
            uint length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        public readonly uint ToString(USpan<char> buffer)
        {
            uint length = label.CopyTo(buffer);
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
