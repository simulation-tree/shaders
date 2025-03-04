using System;
using Types;
using Unmanaged;

namespace Shaders
{
    public readonly struct ShaderUniformPropertyMember : IEquatable<ShaderUniformPropertyMember>
    {
        public readonly FixedString label;
        public readonly long typeHash;
        public readonly byte size;
        public readonly FixedString name;

        public readonly TypeLayout Type => TypeRegistry.Get(typeHash);

        public ShaderUniformPropertyMember(FixedString label, TypeLayout type, byte size, FixedString name)
        {
            this.label = label;
            this.typeHash = type.Hash;
            this.size = size;
            this.name = name;
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is ShaderUniformPropertyMember member && Equals(member);
        }

        public readonly bool Equals(ShaderUniformPropertyMember other)
        {
            return label.Equals(other.label) && typeHash.Equals(other.typeHash) && name.Equals(other.name);
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(label, typeHash, name);
        }

        public readonly override string ToString()
        {
            USpan<char> buffer = stackalloc char[name.Length + 32];
            uint length = ToString(buffer);
            return buffer.GetSpan(length).ToString();
        }

        public readonly uint ToString(USpan<char> buffer)
        {
            uint length = name.CopyTo(buffer);
            buffer[length++] = ' ';
            buffer[length++] = '(';
            length += Type.ToString(buffer.Slice(length));
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