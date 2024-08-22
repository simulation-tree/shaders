using System;
using Unmanaged;

namespace Shaders
{
    /// <summary>
    /// Describes a uniform buffer object shader property.
    /// </summary>
    public readonly struct ShaderUniformProperty
    {
        public readonly FixedString label;
        public readonly DescriptorResourceKey key;

        /// <summary>
        /// Size of the uniform buffer object in bytes.
        /// </summary>
        public readonly uint size;

        public ShaderUniformProperty(FixedString name, DescriptorResourceKey key, uint size)
        {
            this.label = name;
            this.key = key;
            this.size = size;
        }

        public ShaderUniformProperty(ReadOnlySpan<char> name, DescriptorResourceKey key, uint size)
        {
            this.label = new(name);
            this.key = key;
            this.size = size;
        }

        public readonly override string ToString()
        {
            Span<char> buffer = stackalloc char[FixedString.MaxLength + 16];
            int length = ToString(buffer);
            return new string(buffer[..length]);
        }

        public readonly int ToString(Span<char> buffer)
        {
            int length = label.CopyTo(buffer);
            buffer[length++] = ' ';
            buffer[length++] = '(';
            length += key.ToString(buffer[length..]);
            buffer[length++] = ')';
            return length;
        }
    }
}
