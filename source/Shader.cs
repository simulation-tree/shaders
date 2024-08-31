using Data;
using Shaders.Components;
using Simulation;
using System;
using Unmanaged;

namespace Shaders
{
    public readonly struct Shader : IEntity
    {
        private readonly Entity entity;

        public readonly ReadOnlySpan<ShaderVertexInputAttribute> VertexAttributes => entity.GetArray<ShaderVertexInputAttribute>();
        public readonly ReadOnlySpan<ShaderUniformProperty> UniformProperties => entity.GetArray<ShaderUniformProperty>();
        public readonly ReadOnlySpan<ShaderSamplerProperty> SamplerProperties => entity.GetArray<ShaderSamplerProperty>();
        public readonly ReadOnlySpan<ShaderPushConstant> PushConstants => entity.GetArray<ShaderPushConstant>();

        World IEntity.World => entity;
        uint IEntity.Value => entity;

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
        public Shader(World world, ReadOnlySpan<char> vertexAddress, ReadOnlySpan<char> fragmentAddress)
        {
            DataRequest vertex = new(world, vertexAddress);
            DataRequest fragment = new(world, fragmentAddress);
            entity = new(world);
            rint vertexReference = entity.AddReference(vertex);
            rint fragmentReference = entity.AddReference(fragment);
            entity.AddComponent(new IsShaderRequest(vertexReference, fragmentReference));
        }

        /// <summary>
        /// Creates a new shader+request from the given vertex and fragment data addresses.
        /// </summary>
        public Shader(World world, FixedString vertexAddress, FixedString fragmentAddress)
        {
            DataRequest vertex = new(world, vertexAddress);
            DataRequest fragment = new(world, fragmentAddress);
            entity = new(world);
            rint vertexReference = entity.AddReference(vertex);
            rint fragmentReference = entity.AddReference(fragment);
            entity.AddComponent(new IsShaderRequest(vertexReference, fragmentReference));
        }

        public readonly override string ToString()
        {
            return entity.ToString();
        }

        Query IEntity.GetQuery(World world)
        {
            return new(world, RuntimeType.Get<IsShader>());
        }

        public readonly uint GetVersion()
        {
            IsShader component = entity.GetComponent<IsShader>();
            return component.version;
        }

        public readonly ReadOnlySpan<byte> GetVertexBytes()
        {
            IsShader component = entity.GetComponent<IsShader>();
            Entity vertexShader = entity.GetReference<Entity>(component.vertex);
            return vertexShader.GetArray<byte>();
        }

        public readonly ReadOnlySpan<byte> GetFragmentBytes()
        {
            IsShader component = entity.GetComponent<IsShader>();
            Entity fragmentShader = entity.GetReference<Entity>(component.fragment);
            return fragmentShader.GetArray<byte>();
        }

        public readonly uint GetMemberCount(ReadOnlySpan<char> uniformProperty)
        {
            return GetMemberCount(new FixedString(uniformProperty));
        }

        public readonly uint GetMemberCount(FixedString uniformProperty)
        {
            Span<ShaderUniformPropertyMember> allMembers = entity.GetArray<ShaderUniformPropertyMember>();
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

        public readonly ShaderUniformPropertyMember GetMember(ReadOnlySpan<char> uniformProperty, uint index)
        {
            return GetMember(new FixedString(uniformProperty), index);
        }

        public readonly ShaderUniformPropertyMember GetMember(FixedString uniformProperty, uint index)
        {
            Span<ShaderUniformPropertyMember> allMembers = entity.GetArray<ShaderUniformPropertyMember>();
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

        public static implicit operator Entity(Shader shader)
        {
            return shader.entity;
        }
    }
}
