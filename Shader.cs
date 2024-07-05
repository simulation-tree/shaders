using Data;
using Data.Events;
using Shaders.Components;
using Shaders.Events;
using Simulation;
using System;
using Unmanaged.Collections;

namespace Shaders
{
    public readonly struct Shader : IDisposable
    {
        public readonly Entity entity;
        public readonly DataRequest vertex;
        public readonly DataRequest fragment;

        private readonly UnmanagedList<ShaderVertexInputAttribute> vertexAttributes;
        private readonly UnmanagedList<ShaderUniformProperty> uniformProperties;
        private readonly UnmanagedList<ShaderSamplerProperty> samplerProperties;

        public readonly ReadOnlySpan<byte> RawVertexBytes => vertex.entity.GetCollection<byte>().AsSpan();
        public readonly ReadOnlySpan<byte> RawFragmentBytes => fragment.entity.GetCollection<byte>().AsSpan();

        /// <summary>
        /// The expected vertex input attributes.
        /// </summary>
        public readonly ReadOnlySpan<ShaderVertexInputAttribute> VertexAttributes => vertexAttributes.AsSpan();

        /// <summary>
        /// The uniform buffer object properties of this shader.
        /// </summary>
        public readonly ReadOnlySpan<ShaderUniformProperty> UniformProperties => uniformProperties.AsSpan();

        /// <summary>
        /// The sampler uniform properties of this shader.
        /// </summary>
        public readonly ReadOnlySpan<ShaderSamplerProperty> SamplerProperties => samplerProperties.AsSpan();

        public Shader()
        {
            throw new InvalidOperationException("Cannot create a shader without a world.");
        }

        public Shader(World world, EntityID existingEntity)
        {
            entity = new(world, existingEntity);
            IsShader shader = entity.GetComponent<IsShader>();
            vertex = new(world, shader.vertex);
            fragment = new(world, shader.fragment);
            vertexAttributes = entity.GetCollection<ShaderVertexInputAttribute>();
            uniformProperties = entity.GetCollection<ShaderUniformProperty>();
            samplerProperties = entity.GetCollection<ShaderSamplerProperty>();
        }

        /// <summary>
        /// Creates a shader from the given vertex and fragment addresses.
        /// </summary>
        public Shader(World world, ReadOnlySpan<char> vertexAddress, ReadOnlySpan<char> fragmentAddress)
        {
            vertex = new(world, vertexAddress);
            fragment = new(world, fragmentAddress);
            entity = new(world);
            entity.AddComponent(new IsShader(vertex.entity, fragment.entity));
            vertexAttributes = entity.CreateCollection<ShaderVertexInputAttribute>();
            uniformProperties = entity.CreateCollection<ShaderUniformProperty>();
            samplerProperties = entity.CreateCollection<ShaderSamplerProperty>();

            world.Submit(new DataUpdate());
            world.Submit(new ShaderUpdate());
            world.Poll();
        }

        public readonly void Dispose()
        {
            foreach (ShaderUniformProperty property in uniformProperties)
            {
                property.Dispose();
            }

            vertex.Dispose();
            fragment.Dispose();
            entity.Dispose();
        }

        public readonly override string ToString()
        {
            return entity.ToString();
        }
    }
}
