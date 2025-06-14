using System;
using Unmanaged;

namespace Shaders
{
    public readonly struct ShaderStorageBuffer
    {
        public readonly ASCIIText256 label;
        public readonly ASCIIText256 typeName;
        public readonly uint binding;
        public readonly uint set;
        public readonly uint byteLength;
        public readonly Flags flags;

        public ShaderStorageBuffer(ASCIIText256 label, ASCIIText256 typeName, uint binding, uint set, uint byteLength, Flags flags)
        {
            this.label = label;
            this.typeName = typeName;
            this.binding = binding;
            this.set = set;
            this.byteLength = byteLength;
            this.flags = flags;
        }

        [Flags]
        public enum Flags : byte
        {
            ReadWrite = 0,
            ReadOnly = 1,
            WriteOnly = 2
        }
    }
}
