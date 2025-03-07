using Unmanaged;

namespace Shaders
{
    public readonly struct ShaderPushConstant
    {
        public readonly ASCIIText256 propertyName;
        public readonly ASCIIText256 memberName;
        public readonly byte offset;
        public readonly byte size;

        public ShaderPushConstant(ASCIIText256 propertyName, ASCIIText256 memberName, byte offset, byte size)
        {
            this.propertyName = propertyName;
            this.memberName = memberName;
            this.offset = offset;
            this.size = size;
        }

        public ShaderPushConstant(USpan<char> propertyName, ASCIIText256 memberName, byte offset, byte size)
        {
            this.propertyName = new(propertyName);
            this.memberName = memberName;
            this.offset = offset;
            this.size = size;
        }
    }
}
