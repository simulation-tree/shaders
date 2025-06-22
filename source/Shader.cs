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

        public readonly ushort Version
        {
            get
            {
                ThrowIfNotLoaded();

                return GetComponent<IsShader>().version;
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

        public readonly ReadOnlySpan<ShaderStorageBuffer> StorageBuffers
        {
            get
            {
                ThrowIfNotLoaded();

                return GetArray<ShaderStorageBuffer>();
            }
        }

        /// <summary>
        /// Creates an empty shader entity.
        /// </summary>
        public Shader(World world, ShaderType type)
        {
            this.world = world;
            Schema schema = world.Schema;
            int shaderType = schema.GetComponentType<IsShader>();
            BitMask arrayTypes = schema.GetArrayTypes<ShaderVertexInputAttribute, ShaderUniformProperty, ShaderSamplerProperty, ShaderPushConstant, ShaderUniformPropertyMember, ShaderStorageBuffer, ShaderStorageBufferMember>();
            value = world.CreateEntity(new Definition(new BitMask(shaderType), arrayTypes));
            world.SetComponent(value, shaderType, new IsShader(1, type));
        }

        /// <summary>
        /// Creates a request to load a shader from the given <paramref name="address"/>.
        /// </summary>
        public Shader(World world, ASCIIText256 address, ShaderType type, double timeout = default)
        {
            this.world = world;
            IsShaderRequest.Flags flags = default;
            if (type == ShaderType.Vertex)
            {
                flags |= IsShaderRequest.Flags.VertexShader;
            }
            else if (type == ShaderType.Fragment)
            {
                flags |= IsShaderRequest.Flags.FragmentShader;
            }

            value = world.CreateEntity(new IsShaderRequest(flags, address, timeout));
        }

        readonly void IEntity.Describe(ref Archetype archetype)
        {
            archetype.AddComponentType<IsShader>();
        }

        /// <summary>
        /// Retrieves how many members the property with the name <paramref name="uniformProperty"/> contains.
        /// </summary>
        public readonly int GetUniformPropertyMemberCount(ReadOnlySpan<char> uniformProperty)
        {
            ThrowIfNotLoaded();

            long hash = uniformProperty.GetLongHashCode();
            ReadOnlySpan<ShaderUniformPropertyMember> allMembers = GetArray<ShaderUniformPropertyMember>();
            int count = 0;
            for (int i = 0; i < allMembers.Length; i++)
            {
                if (allMembers[i].uniformPropertyNameHash == hash)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Retrieves how many members the property with the name <paramref name="uniformProperty"/> contains.
        /// </summary>
        public readonly int GetUniformPropertyMemberCount(ASCIIText256 uniformProperty)
        {
            ThrowIfNotLoaded();

            long hash = uniformProperty.GetLongHashCode();
            ReadOnlySpan<ShaderUniformPropertyMember> allMembers = GetArray<ShaderUniformPropertyMember>();
            int count = 0;
            for (int i = 0; i < allMembers.Length; i++)
            {
                if (allMembers[i].uniformPropertyNameHash == hash)
                {
                    count++;
                }
            }

            return count;
        }

        public readonly ShaderUniformPropertyMember GetUniformPropertyMember(ReadOnlySpan<char> uniformProperty, int index)
        {
            ThrowIfNotLoaded();

            long propertyNameHash = uniformProperty.GetLongHashCode();
            ReadOnlySpan<ShaderUniformPropertyMember> allMembers = GetArray<ShaderUniformPropertyMember>();
            int count = 0;
            for (int i = 0; i < allMembers.Length; i++)
            {
                if (allMembers[i].uniformPropertyNameHash == propertyNameHash)
                {
                    if (count == index)
                    {
                        return allMembers[i];
                    }

                    count++;
                }
            }

            throw new IndexOutOfRangeException($"No member found at index {index} for uniform property `{uniformProperty}`");
        }

        public readonly ShaderUniformPropertyMember GetUniformPropertyMember(ASCIIText256 uniformProperty, int index)
        {
            ThrowIfNotLoaded();

            long propertyNameHash = uniformProperty.GetLongHashCode();
            ReadOnlySpan<ShaderUniformPropertyMember> allMembers = GetArray<ShaderUniformPropertyMember>();
            int count = 0;
            for (int i = 0; i < allMembers.Length; i++)
            {
                if (allMembers[i].uniformPropertyNameHash == propertyNameHash)
                {
                    if (count == index)
                    {
                        return allMembers[i];
                    }

                    count++;
                }
            }

            throw new IndexOutOfRangeException($"No member found at index {index} for uniform property `{uniformProperty}`");
        }

        public readonly int GetStorageBufferMemberCount(ReadOnlySpan<char> storageBuffer)
        {
            ThrowIfNotLoaded();

            long hash = storageBuffer.GetLongHashCode();
            ReadOnlySpan<ShaderStorageBufferMember> allMembers = GetArray<ShaderStorageBufferMember>();
            int count = 0;
            for (int i = 0; i < allMembers.Length; i++)
            {
                if (allMembers[i].storageBufferNameHash == hash)
                {
                    count++;
                }
            }

            return count;
        }

        public readonly int GetStorageBufferMemberCount(ASCIIText256 storageBuffer)
        {
            ThrowIfNotLoaded();

            long hash = storageBuffer.GetLongHashCode();
            ReadOnlySpan<ShaderStorageBufferMember> allMembers = GetArray<ShaderStorageBufferMember>();
            int count = 0;
            for (int i = 0; i < allMembers.Length; i++)
            {
                if (allMembers[i].storageBufferNameHash == hash)
                {
                    count++;
                }
            }

            return count;
        }

        public readonly ShaderStorageBufferMember GetStorageBufferMember(ReadOnlySpan<char> storageBuffer, int index)
        {
            ThrowIfNotLoaded();

            long bufferNameHash = storageBuffer.GetLongHashCode();
            ReadOnlySpan<ShaderStorageBufferMember> allMembers = GetArray<ShaderStorageBufferMember>();
            int count = 0;
            for (int i = 0; i < allMembers.Length; i++)
            {
                if (allMembers[i].storageBufferNameHash == bufferNameHash)
                {
                    if (count == index)
                    {
                        return allMembers[i];
                    }

                    count++;
                }
            }

            throw new IndexOutOfRangeException($"No member found at index {index} for storage buffer `{storageBuffer}`");
        }

        public readonly ShaderStorageBufferMember GetStorageBufferMember(ASCIIText256 storageBuffer, int index)
        {
            ThrowIfNotLoaded();

            long bufferNameHash = storageBuffer.GetLongHashCode();
            ReadOnlySpan<ShaderStorageBufferMember> allMembers = GetArray<ShaderStorageBufferMember>();
            int count = 0;
            for (int i = 0; i < allMembers.Length; i++)
            {
                if (allMembers[i].storageBufferNameHash == bufferNameHash)
                {
                    if (count == index)
                    {
                        return allMembers[i];
                    }

                    count++;
                }
            }

            throw new IndexOutOfRangeException($"No member found at index {index} for storage buffer `{storageBuffer}`");
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