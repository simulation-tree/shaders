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

        public ShaderSamplerProperty(USpan<char> name, DescriptorResourceKey key)
        {
            this.name = new(name);
            this.key = key;
        }

        public unsafe readonly override string ToString()
        {
            USpan<char> buffer = stackalloc char[(int)(name.Length + 32)];
            uint length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        public readonly uint ToString(USpan<char> buffer)
        {
            uint length = name.CopyTo(buffer);
            buffer[length++] = ' ';
            buffer[length++] = '(';
            length += key.ToString(buffer.Slice(length));
            buffer[length++] = ')';
            return length;
        }
    }
}
