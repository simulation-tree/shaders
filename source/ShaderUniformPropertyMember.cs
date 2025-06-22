using System;
using Types;
using Unmanaged;

namespace Shaders
{
    public readonly struct ShaderUniformPropertyMember : IEquatable<ShaderUniformPropertyMember>
    {
        public readonly long uniformPropertyNameHash;
        public readonly TypeMetadata type;
        public readonly ASCIIText256 name;
        public readonly uint offset;

        public ShaderUniformPropertyMember(long uniformPropertyNameHash, TypeMetadata type, ASCIIText256 name, uint offset)
        {
            this.uniformPropertyNameHash = uniformPropertyNameHash;
            this.type = type;
            this.name = name;
            this.offset = offset;
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is ShaderUniformPropertyMember member && Equals(member);
        }

        public readonly bool Equals(ShaderUniformPropertyMember other)
        {
            return uniformPropertyNameHash.Equals(other.uniformPropertyNameHash) && type.Equals(other.type) && name.Equals(other.name) && offset.Equals(other.offset);
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(uniformPropertyNameHash, type, name, offset);
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