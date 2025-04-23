using System;
using Types;
using Unmanaged;

namespace Shaders
{
    public readonly struct ShaderUniformPropertyMember : IEquatable<ShaderUniformPropertyMember>
    {
        public readonly ASCIIText256 label;
        public readonly TypeMetadata type;
        public readonly ASCIIText256 name;

        public ShaderUniformPropertyMember(ASCIIText256 label, TypeMetadata type, ASCIIText256 name)
        {
            this.label = label;
            this.type = type;
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

        public readonly override string ToString()
        {
            Span<char> buffer = stackalloc char[name.Length + 32];
            int length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        public readonly int ToString(Span<char> destination)
        {
            int length = name.CopyTo(destination);
            destination[length++] = ' ';
            destination[length++] = '(';
            length += type.ToString(destination.Slice(length));
            destination[length++] = ')';
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