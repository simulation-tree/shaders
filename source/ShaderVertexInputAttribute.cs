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
        public readonly byte location;
        public readonly byte binding;
        public readonly byte offset;
        public readonly byte size;
        public readonly long typeHash;

        public readonly TypeLayout Type => TypeRegistry.Get(typeHash);

        public ShaderVertexInputAttribute(ASCIIText256 name, byte location, byte binding, byte offset, TypeLayout type, byte size)
        {
            this.name = name;
            this.location = location;
            this.binding = binding;
            this.offset = offset;
            this.typeHash = type.Hash;
            this.size = size;
        }

        public ShaderVertexInputAttribute(ReadOnlySpan<char> name, byte location, byte binding, byte offset, long typeHash, byte size)
        {
            this.name = new(name);
            this.location = location;
            this.binding = binding;
            this.offset = offset;
            this.typeHash = typeHash;
            this.size = size;
        }

        public unsafe static ShaderVertexInputAttribute Create<T>(ASCIIText256 name, byte location, byte binding, byte offset) where T : unmanaged
        {
            TypeLayout type = TypeRegistry.Get<T>();
            return new(name, location, binding, offset, type, (byte)sizeof(T));
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is ShaderVertexInputAttribute attribute && Equals(attribute);
        }

        public readonly bool Equals(ShaderVertexInputAttribute other)
        {
            return name.Equals(other.name) && location == other.location && binding == other.binding && offset == other.offset && typeHash.Equals(other.typeHash);
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(name, location, binding, offset, typeHash);
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