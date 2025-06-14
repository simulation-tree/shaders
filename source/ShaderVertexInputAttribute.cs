using System;
using Types;
using Unmanaged;

namespace Shaders
{
    /// <summary>
    /// Describes a vertex attribute in the shader's vertex input.
    /// </summary>
    public readonly struct ShaderVertexInputAttribute : IEquatable<ShaderVertexInputAttribute>
    {
        public readonly ASCIIText256 name;
        public readonly uint location;
        public readonly uint binding;
        public readonly uint offset;
        public readonly TypeMetadata type;

        public ShaderVertexInputAttribute(ASCIIText256 name, uint location, uint binding, uint offset, TypeMetadata type)
        {
            this.name = name;
            this.location = location;
            this.binding = binding;
            this.offset = offset;
            this.type = type;
        }

        public static ShaderVertexInputAttribute Create<T>(ASCIIText256 name, uint location, uint binding, uint offset) where T : unmanaged
        {
            TypeMetadata type = MetadataRegistry.GetType<T>();
            return new(name, location, binding, offset, type);
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is ShaderVertexInputAttribute attribute && Equals(attribute);
        }

        public readonly bool Equals(ShaderVertexInputAttribute other)
        {
            return name.Equals(other.name) && location == other.location && binding == other.binding && offset == other.offset && type == other.type;
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