using System;
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

        public readonly override string ToString()
        {
            Span<char> buffer = stackalloc char[FixedString.MaxLength + 16];
            int length = ToString(buffer);
            return new string(buffer[..length]);
        }

        public readonly int ToString(Span<char> buffer)
        {
            int length = name.ToString(buffer);
            buffer[length++] = ' ';
            buffer[length++] = '(';
            length += type.ToString(buffer[length..]);
            buffer[length++] = ')';
            return length;
        }
    }
}
