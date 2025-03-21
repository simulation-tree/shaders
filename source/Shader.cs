using Shaders.Components;
using System;
using System.Diagnostics;
using Unmanaged;
using Worlds;

namespace Shaders
{
    public readonly partial struct Shader : IEntity
    {
        public readonly bool IsLoaded
        {
            get
            {
                if (TryGetComponent(out IsShaderRequest request))
                {
                    return request.status == IsShaderRequest.Status.Loaded;
                }

                return IsCompliant;
            }
        }

        public readonly ShaderType Type
        {
            get
            {
                ThrowIfNotLoaded();

                return GetComponent<IsShader>().type;
            }
        }

        public readonly ReadOnlySpan<byte> Bytes
        {
            get
            {
                ThrowIfNotLoaded();

                return GetArray<ShaderByte>().AsSpan<byte>();
            }
        }

        public readonly bool IsInstanced
        {
            get
            {
                ThrowIfNotLoaded();

                ShaderFlags flags = GetComponent<IsShader>().flags;
                return (flags & ShaderFlags.Instanced) == ShaderFlags.Instanced;
            }
        }

        public readonly ReadOnlySpan<ShaderVertexInputAttribute> VertexInputAttributes
        {
            get
            {
                ThrowIfNotLoaded();

                return GetArray<ShaderVertexInputAttribute>();
            }
        }

        public readonly ReadOnlySpan<ShaderUniformProperty> UniformProperties
        {
            get
            {
                ThrowIfNotLoaded();

                return GetArray<ShaderUniformProperty>();
            }
        }

        public readonly ReadOnlySpan<ShaderSamplerProperty> SamplerProperties
        {
            get
            {
                ThrowIfNotLoaded();

                return GetArray<ShaderSamplerProperty>();
            }
        }

        public readonly ReadOnlySpan<ShaderPushConstant> PushConstants
        {
            get
            {
                ThrowIfNotLoaded();

                return GetArray<ShaderPushConstant>();
            }
        }

        /// <summary>
        /// Creates an empty shader entity.
        /// </summary>
        public Shader(World world, ShaderType type)
        {
            this.world = world;
            value = world.CreateEntity(new IsShader(1, type, ShaderFlags.None));
            world.CreateArray<ShaderVertexInputAttribute>(value);
            world.CreateArray<ShaderUniformProperty>(value);
            world.CreateArray<ShaderSamplerProperty>(value);
            world.CreateArray<ShaderPushConstant>(value);
            world.CreateArray<ShaderUniformPropertyMember>(value);
        }

        /// <summary>
        /// Creates a request to load a shader from the given <paramref name="address"/>.
        /// </summary>
        public Shader(World world, ASCIIText256 address, ShaderType type, TimeSpan timeout = default)
        {
            this.world = world;
            value = world.CreateEntity(new IsShaderRequest(type, address, timeout));
        }

        readonly void IEntity.Describe(ref Archetype archetype)
        {
            archetype.AddComponentType<IsShader>();
        }

        /// <summary>
        /// Retrieves how many members the property with the name <paramref name="uniformProperty"/> contains.
        /// </summary>
        public readonly int GetMemberCount(ReadOnlySpan<char> uniformProperty)
        {
            return GetMemberCount(new ASCIIText256(uniformProperty));
        }

        /// <summary>
        /// Retrieves how many members the property with the name <paramref name="uniformProperty"/> contains.
        /// </summary>
        public readonly int GetMemberCount(ASCIIText256 uniformProperty)
        {
            ThrowIfNotLoaded();

            ReadOnlySpan<ShaderUniformPropertyMember> allMembers = GetArray<ShaderUniformPropertyMember>();
            int count = 0;
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
            return GetMember(new ASCIIText256(uniformProperty), index);
        }

        public readonly ShaderUniformPropertyMember GetMember(ASCIIText256 uniformProperty, uint index)
        {
            ThrowIfNotLoaded();

            ReadOnlySpan<ShaderUniformPropertyMember> allMembers = GetArray<ShaderUniformPropertyMember>();
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

        [Conditional("DEBUG")]
        private readonly void ThrowIfNotLoaded()
        {
            if (!IsLoaded)
            {
                throw new InvalidOperationException($"Shader `{value}` is not loaded");
            }
        }
    }
}