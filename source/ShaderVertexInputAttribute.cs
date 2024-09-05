using Unmanaged;

namespace Shaders
{
    /// <summary>
    /// Describes a vertex attribute in the shader's vertex input.
    /// </summary>
    public readonly struct ShaderVertexInputAttribute
    {
        public readonly FixedString name;
        public readonly byte location;
        public readonly byte binding;
        public readonly byte offset;
        public readonly RuntimeType type;

        public readonly ushort Size => type.Size;

        public ShaderVertexInputAttribute(FixedString name, byte location, byte binding, byte offset, RuntimeType type)
        {
            this.name = name;
            this.location = location;
            this.binding = binding;
            this.offset = offset;
            this.type = type;
        }

        public ShaderVertexInputAttribute(USpan<char> name, byte location, byte binding, byte offset, RuntimeType type)
        {
            this.name = new(name);
            this.location = location;
            this.binding = binding;
            this.offset = offset;
            this.type = type;
        }

        public static ShaderVertexInputAttribute Create<T>(FixedString name, byte location, byte binding, byte offset) where T : unmanaged
        {
            return new(name, location, binding, offset, RuntimeType.Get<T>());
        }
    }
}
