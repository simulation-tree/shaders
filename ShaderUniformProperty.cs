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
        //todo: fault: kinda dirty having a component thats also disposable, it requires a system to manage the disposal of the members

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

        public readonly byte Binding => key.Binding;
        public readonly byte Set => key.Set;

        public ShaderUniformProperty(FixedString name, DescriptorResourceKey key, ReadOnlySpan<Member> members)
        {
            this.name = name;
            this.key = key;
            this.members = new(members);
            size = default;
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
            size = default;
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
        public readonly struct Member
        {
            public readonly RuntimeType type;
            public readonly FixedString name;

            public Member(RuntimeType type, FixedString name)
            {
                this.type = type;
                this.name = name;
            }

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
                length += type.ToString(buffer[length..]);
                buffer[length++] = ')';
                return length;
            }
        }
    }
}
