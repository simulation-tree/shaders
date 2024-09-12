using Data;
using Shaders.Components;
using Simulation;
using System;
using Unmanaged;

namespace Shaders
{
    public readonly struct Shader : IEntity
    {
        public readonly Entity entity;

        public readonly USpan<ShaderVertexInputAttribute> VertexAttributes => entity.GetArray<ShaderVertexInputAttribute>();
        public readonly USpan<ShaderUniformProperty> UniformProperties => entity.GetArray<ShaderUniformProperty>();
        public readonly USpan<ShaderSamplerProperty> SamplerProperties => entity.GetArray<ShaderSamplerProperty>();
        public readonly USpan<ShaderPushConstant> PushConstants => entity.GetArray<ShaderPushConstant>();

        readonly uint IEntity.Value => entity.value;
        readonly World IEntity.World => entity.world;
        readonly Definition IEntity.Definition => new Definition().AddComponentType<IsShader>().AddArrayTypes<ShaderVertexInputAttribute, ShaderUniformProperty, ShaderSamplerProperty, ShaderPushConstant, ShaderUniformPropertyMember>();

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
        public Shader(World world, USpan<char> vertexAddress, USpan<char> fragmentAddress)
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

        public readonly uint GetVersion()
        {
            IsShader component = entity.GetComponentRef<IsShader>();
            return component.version;
        }

        public readonly USpan<byte> GetVertexBytes()
        {
            IsShader component = entity.GetComponentRef<IsShader>();
            Entity vertexShader = entity.GetReference<Entity>(component.vertex);
            return vertexShader.GetArray<byte>();
        }

        public readonly USpan<byte> GetFragmentBytes()
        {
            IsShader component = entity.GetComponentRef<IsShader>();
            Entity fragmentShader = entity.GetReference<Entity>(component.fragment);
            return fragmentShader.GetArray<byte>();
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
    }
}
