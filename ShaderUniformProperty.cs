using System;
using Unmanaged;
using Unmanaged.Collections;

namespace Shaders
{
    /// <summary>
    /// Describes a uniform buffer object shader property.
    /// </summary>
    public readonly struct ShaderUniformProperty : IDisposable
    {
        public readonly FixedString name;
        public readonly DescriptorResourceKey key;

        /// <summary>
        /// Size of the uniform buffer object in bytes.
        /// </summary>
        public readonly uint size;

        private readonly UnmanagedArray<Member> members;

        /// <summary>
        /// All members of the uniform buffer object.
        /// </summary>
        public readonly ReadOnlySpan<Member> Members => members.AsSpan();

        public ShaderUniformProperty(FixedString name, DescriptorResourceKey key, ReadOnlySpan<Member> members)
        {
            this.name = name;
            this.key = key;
            this.members = new(members);
            foreach (Member member in members)
            {
                size += member.type.Size;
            }
        }

        public ShaderUniformProperty(ReadOnlySpan<char> name, DescriptorResourceKey key, ReadOnlySpan<Member> members)
        {
            this.name = new(name);
            this.key = key;
            this.members = new(members);
            foreach (Member member in members)
            {
                size += member.type.Size;
            }
        }

        public readonly void Dispose()
        {
            members.Dispose();
        }

        public readonly override string ToString()
        {
            Span<char> buffer = stackalloc char[32 + name.Length];
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

        /// <summary>
        /// A member of a uniform buffer object.
        /// </summary>
        public readonly struct Member(RuntimeType type, FixedString name)
        {
            public readonly RuntimeType type = type;
            public readonly FixedString name = name;

            public readonly override string ToString()
            {
                Span<char> buffer = stackalloc char[256 + name.Length];
                int length = ToString(buffer);
                return new string(buffer[..length]);
            }

            public readonly int ToString(Span<char> buffer)
            {
                int length = name.CopyTo(buffer);
                buffer[length++] = ' ';
                buffer[length++] = '(';
                //length += type.value.ToString(buffer[length..]);
                buffer[length++] = ')';
                return length;
            }
        }
    }
}
