using System;
using Types;
using Unmanaged;

namespace Shaders
{
    public readonly struct ShaderStorageBufferMember
    {
        public readonly long storageBufferNameHash;
        public readonly TypeMetadata type;
        public readonly ASCIIText256 name;
        public readonly uint offset;
        public readonly Flags flags;

        public ShaderStorageBufferMember(long storageBufferNameHash, TypeMetadata type, ASCIIText256 name, uint offset, Flags flags)
        {
            this.storageBufferNameHash = storageBufferNameHash;
            this.type = type;
            this.name = name;
            this.offset = offset;
            this.flags = flags;
        }

        [Flags]
        public enum Flags : byte
        {
            None = 0,
            Array = 1
        }
    }
}