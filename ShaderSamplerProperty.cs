using System;
using Unmanaged;

namespace Shaders
{
    /// <summary>
    /// Describes a shader property that references a sampled texture.
    /// </summary>
    public readonly struct ShaderSamplerProperty(FixedString name, DescriptorResourceKey key)
    {
        public readonly FixedString name = name;
        public readonly DescriptorResourceKey key = key;

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
