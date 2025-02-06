using System;
using Unmanaged;
using Worlds;

namespace Shaders
{
    /// <summary>
    /// Describes a vertex attribute in the shader's vertex input.
    /// </summary>
    [ArrayElement]
    public readonly struct ShaderVertexInputAttribute : IEquatable<ShaderVertexInputAttribute>
    {
        public readonly FixedString name;
        public readonly byte location;
        public readonly byte binding;
        public readonly byte offset;
        public readonly nint type;
        public readonly byte size;

        public readonly Type Type
        {
            get
            {
                RuntimeTypeHandle handle = RuntimeTypeTable.GetHandle(type);
                return Type.GetTypeFromHandle(handle) ?? throw new();
            }
        }

        public ShaderVertexInputAttribute(FixedString name, byte location, byte binding, byte offset, Type type, byte size)
        {
            this.name = name;
            this.location = location;
            this.binding = binding;
            this.offset = offset;
            this.type = RuntimeTypeTable.GetAddress(type);
            this.size = size;
        }

        public ShaderVertexInputAttribute(USpan<char> name, byte location, byte binding, byte offset, nint type, byte size)
        {
            this.name = new(name);
            this.location = location;
            this.binding = binding;
            this.offset = offset;
            this.type = type;
            this.size = size;
        }

        public unsafe static ShaderVertexInputAttribute Create<T>(FixedString name, byte location, byte binding, byte offset) where T : unmanaged
        {
            return new(name, location, binding, offset, typeof(T), (byte)sizeof(T));
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is ShaderVertexInputAttribute attribute && Equals(attribute);
        }

        public readonly bool Equals(ShaderVertexInputAttribute other)
        {
            return name.Equals(other.name) && location == other.location && binding == other.binding && offset == other.offset && type.Equals(other.type);
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(name, location, binding, offset, type);
        }

        public static bool operator ==(ShaderVertexInputAttribute left, ShaderVertexInputAttribute right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ShaderVertexInputAttribute left, ShaderVertexInputAttribute right)
        {
            return !(left == right);
        }
    }
}
