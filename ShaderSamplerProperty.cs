using System;
using Unmanaged;

namespace Shaders
{
    /// <summary>
    /// Describes a shader property that references a sampled texture.
    /// </summary>
    public readonly struct ShaderSamplerProperty
    {
        public readonly FixedString name;
        public readonly DescriptorResourceKey key;

        public ShaderSamplerProperty(FixedString name, DescriptorResourceKey key)
        {
            this.name = name;
            this.key = key;
        }

        public readonly override string ToString()
        {
            Span<char> buffer = stackalloc char[name.Length + 32];
            int length = ToString(buffer);
            return new string(buffer[..length]);
        }

        public readonly int ToString(Span<char> buffer)
        {
            int length = name.CopyTo(buffer);
            buffer[length++] = ' ';
            buffer[length++] = '(';
            length += key.ToString(buffer[length..]);
            buffer[length++] = ')';
            return length;
        }
    }
}
