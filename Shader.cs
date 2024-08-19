using Data;
using Shaders.Components;
using Simulation;
using System;
using Unmanaged;
using Unmanaged.Collections;

namespace Shaders
{
    public readonly struct Shader : IShader, IDisposable
    {
        private readonly Entity entity;

        public readonly ReadOnlySpan<ShaderVertexInputAttribute> VertexAttributes => entity.GetList<ShaderVertexInputAttribute>().AsSpan();
        public readonly ReadOnlySpan<ShaderUniformProperty> UniformProperties => entity.GetList<ShaderUniformProperty>().AsSpan();
        public readonly ReadOnlySpan<ShaderSamplerProperty> SamplerProperties => entity.GetList<ShaderSamplerProperty>().AsSpan();
        public readonly ReadOnlySpan<ShaderPushConstant> PushConstants => entity.GetList<ShaderPushConstant>().AsSpan();
        public readonly bool IsLoaded => entity.ContainsComponent<IsShader>();

        World IEntity.World => entity.world;
        eint IEntity.Value => entity.value;

#if NET
        [Obsolete("Default constructor not available", true)]
        public Shader()
        {
            throw new InvalidOperationException("Cannot create a shader without a world.");
        }
#endif

        public Shader(World world, eint existingEntity)
        {
            entity = new(world, existingEntity);
        }

        /// <summary>
        /// Creates a new shader from the given vertex and fragment data addresses.
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

        public Shader(World world, FixedString vertexAddress, FixedString fragmentAddress)
        {
            DataRequest vertex = new(world, vertexAddress);
            DataRequest fragment = new(world, fragmentAddress);
            entity = new(world);
            rint vertexReference = entity.AddReference(vertex);
            rint fragmentReference = entity.AddReference(fragment);
            entity.AddComponent(new IsShaderRequest(vertexReference, fragmentReference));
        }

        public readonly void Dispose()
        {
            entity.Dispose();
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
            return vertexShader.GetList<byte>().AsSpan();
        }

        public readonly ReadOnlySpan<byte> GetFragmentBytes()
        {
            IsShader component = entity.GetComponent<IsShader>();
            Entity fragmentShader = entity.GetReference<Entity>(component.fragment);
            return fragmentShader.GetList<byte>().AsSpan();
        }

        public readonly uint GetMemberCount(ReadOnlySpan<char> uniformProperty)
        {
            return GetMemberCount(new FixedString(uniformProperty));
        }

        public readonly uint GetMemberCount(FixedString uniformProperty)
        {
            UnmanagedList<ShaderUniformPropertyMember> allMembers = entity.GetList<ShaderUniformPropertyMember>();
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
            UnmanagedList<ShaderUniformPropertyMember> allMembers = entity.GetList<ShaderUniformPropertyMember>();
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
