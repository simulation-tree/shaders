using Unmanaged;
using Worlds;

namespace Shaders
{
    [ArrayElement]
    public readonly struct ShaderPushConstant
    {
        public readonly FixedString propertyName;
        public readonly FixedString memberName;
        public readonly byte offset;
        public readonly byte size;

        public ShaderPushConstant(FixedString propertyName, FixedString memberName, byte offset, byte size)
        {
            this.propertyName = propertyName;
            this.memberName = memberName;
            this.offset = offset;
            this.size = size;
        }

        public ShaderPushConstant(USpan<char> propertyName, FixedString memberName, byte offset, byte size)
        {
            this.propertyName = new(propertyName);
            this.memberName = memberName;
            this.offset = offset;
            this.size = size;
        }
    }
}
