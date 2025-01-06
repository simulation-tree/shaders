using Data;
using Data.Components;
using Shaders.Components;
using System;
using Unmanaged;
using Worlds;

namespace Shaders
{
    public readonly struct Shader : IEntity, IEquatable<Shader>
    {
        private readonly Entity entity;

        public readonly USpan<byte> VertexBytes
        {
            get
            {
                IsShader component = entity.GetComponent<IsShader>();
                uint vertexEntity = entity.GetReference(component.vertex);
                return entity.GetWorld().GetArray<BinaryData>(vertexEntity).As<byte>();
            }
        }

        public readonly USpan<byte> FragmentBytes
        {
            get
            {
                IsShader component = entity.GetComponent<IsShader>();
                uint fragmentEntity = entity.GetReference(component.fragment);
                return entity.GetWorld().GetArray<BinaryData>(fragmentEntity).As<byte>();
            }
        }

        public readonly Entity Vertex
        {
            get
            {
                IsShader component = entity.GetComponent<IsShader>();
                return new(entity.GetWorld(), entity.GetReference(component.vertex));
            }
        }

        public readonly Entity Fragment
        {
            get
            {
                IsShader component = entity.GetComponent<IsShader>();
                return new(entity.GetWorld(), entity.GetReference(component.fragment));
            }
        }

        public readonly USpan<ShaderVertexInputAttribute> VertexAttributes => entity.GetArray<ShaderVertexInputAttribute>();
        public readonly USpan<ShaderUniformProperty> UniformProperties => entity.GetArray<ShaderUniformProperty>();
        public readonly USpan<ShaderSamplerProperty> SamplerProperties => entity.GetArray<ShaderSamplerProperty>();
        public readonly USpan<ShaderPushConstant> PushConstants => entity.GetArray<ShaderPushConstant>();

        readonly uint IEntity.Value => entity.GetEntityValue();
        readonly World IEntity.World => entity.GetWorld();

        readonly void IEntity.Describe(ref Archetype archetype)
        {
            archetype.AddComponentType<IsShader>();
            archetype.AddArrayElementType<ShaderVertexInputAttribute>();
            archetype.AddArrayElementType<ShaderUniformProperty>();
            archetype.AddArrayElementType<ShaderSamplerProperty>();
            archetype.AddArrayElementType<ShaderPushConstant>();
            archetype.AddArrayElementType<ShaderUniformPropertyMember>();
        }

#if NET
        [Obsolete("Default constructor not available", true)]
        public Shader()
        {
            throw new InvalidOperationException("Cannot create a shader without a world.");
        }
#endif

        public Shader(World world, uint existingEntity)
        {
            entity = new(world, existingEntity);
        }

        /// <summary>
        /// Creates a new shader+request from the given vertex and fragment data addresses.
        /// </summary>
        public Shader(World world, Address vertexAddress, Address fragmentAddress)
        {
            DataRequest vertex = new(world, vertexAddress);
            DataRequest fragment = new(world, fragmentAddress);
            entity = new Entity<IsShaderRequest>(world, new IsShaderRequest((rint)1, (rint)2));
            entity.AddReference(vertex);
            entity.AddReference(fragment);
        }

        public readonly void Dispose()
        {
            entity.Dispose();
        }

        public readonly override string ToString()
        {
            return entity.ToString();
        }

        public readonly uint GetVersion()
        {
            IsShader component = entity.GetComponent<IsShader>();
            return component.version;
        }

        public readonly uint GetMemberCount(USpan<char> uniformProperty)
        {
            return GetMemberCount(new FixedString(uniformProperty));
        }

        public readonly uint GetMemberCount(FixedString uniformProperty)
        {
            USpan<ShaderUniformPropertyMember> allMembers = entity.GetArray<ShaderUniformPropertyMember>();
            uint count = 0;
            foreach (ShaderUniformPropertyMember member in allMembers)
            {
                if (member.label == uniformProperty)
                {
                    count++;
                }
            }

            return count;
        }

        public readonly ShaderUniformPropertyMember GetMember(USpan<char> uniformProperty, uint index)
        {
            return GetMember(new FixedString(uniformProperty), index);
        }

        public readonly ShaderUniformPropertyMember GetMember(FixedString uniformProperty, uint index)
        {
            USpan<ShaderUniformPropertyMember> allMembers = entity.GetArray<ShaderUniformPropertyMember>();
            uint count = 0;
            foreach (ShaderUniformPropertyMember member in allMembers)
            {
                if (member.label == uniformProperty)
                {
                    if (count == index)
                    {
                        return member;
                    }

                    count++;
                }
            }

            throw new IndexOutOfRangeException($"No member found at index {index} for uniform property `{uniformProperty}`");
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is Shader shader && Equals(shader);
        }

        public readonly bool Equals(Shader other)
        {
            return entity.Equals(other.entity);
        }

        public readonly override int GetHashCode()
        {
            return entity.GetHashCode();
        }

        public static bool operator ==(Shader left, Shader right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Shader left, Shader right)
        {
            return !(left == right);
        }

        public static implicit operator Entity(Shader shader)
        {
            return shader.entity;
        }
    }
}
