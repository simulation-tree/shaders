using Unmanaged;

namespace Shaders
{
    public readonly struct ShaderUniformPropertyMember
    {
        public readonly FixedString label;
        public readonly RuntimeType type;
        public readonly FixedString name;

        public ShaderUniformPropertyMember(FixedString label, RuntimeType type, FixedString name)
        {
            this.label = label;
            this.type = type;
            this.name = name;
        }

        public unsafe readonly override string ToString()
        {
            USpan<char> buffer = stackalloc char[(int)(FixedString.Capacity + 16)];
            uint length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        public readonly uint ToString(USpan<char> buffer)
        {
            uint length = name.CopyTo(buffer);
            buffer[length++] = ' ';
            buffer[length++] = '(';
            length += type.ToString(buffer.Slice(length));
            buffer[length++] = ')';
            return length;
        }
    }
}
