using System;
using Unmanaged;

namespace Shaders
{
    public readonly struct ShaderPushConstant
    {
        public readonly ASCIIText256 propertyName;
        public readonly ASCIIText256 memberName;
        public readonly uint offset;
        public readonly uint size;

        public ShaderPushConstant(ASCIIText256 propertyName, ASCIIText256 memberName, uint offset, uint size)
        {
            this.propertyName = propertyName;
            this.memberName = memberName;
            this.offset = offset;
            this.size = size;
        }

        public ShaderPushConstant(ReadOnlySpan<char> propertyName, ASCIIText256 memberName, uint offset, uint size)
        {
            this.propertyName = new(propertyName);
            this.memberName = memberName;
            this.offset = offset;
            this.size = size;
        }
    }
}
