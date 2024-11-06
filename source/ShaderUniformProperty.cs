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
    }
}
