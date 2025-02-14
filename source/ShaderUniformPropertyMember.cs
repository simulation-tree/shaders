using System;
using Unmanaged;

namespace Shaders
{
    public readonly struct ShaderUniformPropertyMember : IEquatable<ShaderUniformPropertyMember>
    {
        public readonly FixedString label;
        public readonly nint type;
        public readonly byte size;
        public readonly FixedString name;

        public readonly Type Type
        {
            get
            {
                RuntimeTypeHandle handle = RuntimeTypeTable.GetHandle(type);
                return Type.GetTypeFromHandle(handle) ?? throw new();
            }
        }

        public ShaderUniformPropertyMember(FixedString label, Type type, byte size, FixedString name)
        {
            this.label = label;
            this.type = RuntimeTypeTable.GetAddress(type);
            this.size = size;
            this.name = name;
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is ShaderUniformPropertyMember member && Equals(member);
        }

        public readonly bool Equals(ShaderUniformPropertyMember other)
        {
            return label.Equals(other.label) && type.Equals(other.type) && name.Equals(other.name);
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(label, type, name);
        }

        public unsafe readonly override string ToString()
        {
            USpan<char> buffer = stackalloc char[name.Length + 32];
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

        public static bool operator ==(ShaderUniformPropertyMember left, ShaderUniformPropertyMember right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ShaderUniformPropertyMember left, ShaderUniformPropertyMember right)
        {
            return !(left == right);
        }
    }
}
