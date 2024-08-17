using Data;
using Data.Events;
using Shaders.Components;
using Shaders.Events;
using Simulation;
using System;
using Unmanaged;

namespace Shaders
{
    public readonly struct Shader : IShader, IDisposable
    {
        private readonly Entity entity;

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
            entity.AddComponent(new IsShader(vertexReference, fragmentReference));
            entity.CreateList<ShaderPushConstant>();
            entity.CreateList<ShaderVertexInputAttribute>();
            entity.CreateList<ShaderUniformProperty>();
            entity.CreateList<ShaderSamplerProperty>();

            //todo: remove this
            world.Submit(new DataUpdate());
            world.Submit(new ShaderUpdate());
            world.Poll();
        }

        /// <summary>
        /// Creates a new shader using the provided existing data entities.
        /// <para>Data is expected to be UTF8 bytes.</para>
        /// </summary>
        public Shader(World world, eint vertexData, eint fragmentData)
        {
            entity = new(world);
            rint vertexReference = entity.AddReference(vertexData);
            rint fragmentReference = entity.AddReference(fragmentData);
            entity.AddComponent(new IsShader(vertexReference, fragmentReference));
            entity.CreateList<ShaderPushConstant>();
            entity.CreateList<ShaderVertexInputAttribute>();
            entity.CreateList<ShaderUniformProperty>();
            entity.CreateList<ShaderSamplerProperty>();

            //todo: remove this
            world.Submit(new ShaderUpdate());
            world.Poll();
        }

        public Shader(World world, FixedString vertexAddress, FixedString fragmentAddress)
        {
            DataRequest vertex = new(world, vertexAddress);
            DataRequest fragment = new(world, fragmentAddress);
            entity = new(world);
            rint vertexReference = entity.AddReference(vertex);
            rint fragmentReference = entity.AddReference(fragment);
            entity.AddComponent(new IsShader(vertexReference, fragmentReference));
            entity.CreateList<ShaderPushConstant>();
            entity.CreateList<ShaderVertexInputAttribute>();
            entity.CreateList<ShaderUniformProperty>();
            entity.CreateList<ShaderSamplerProperty>();

            //todo: remove this
            world.Submit(new DataUpdate());
            world.Submit(new ShaderUpdate());
            world.Poll();
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
            Entity vertexShader = entity.GetReference<Entity>(component.vertexReference);
            return vertexShader.GetList<byte>().AsSpan();
        }

        public readonly ReadOnlySpan<byte> GetFragmentBytes()
        {
            IsShader component = entity.GetComponent<IsShader>();
            Entity fragmentShader = entity.GetReference<Entity>(component.fragmentReference);
            return fragmentShader.GetList<byte>().AsSpan();
        }

        public readonly ReadOnlySpan<ShaderVertexInputAttribute> GetVertexAttributes()
        {
            return entity.GetList<ShaderVertexInputAttribute>().AsSpan();
        }

        public readonly ReadOnlySpan<ShaderUniformProperty> GetUniformProperties()
        {
            return entity.GetList<ShaderUniformProperty>().AsSpan();
        }

        public readonly ReadOnlySpan<ShaderSamplerProperty> GetSamplerProperties()
        {
            return entity.GetList<ShaderSamplerProperty>().AsSpan();
        }

        public readonly ReadOnlySpan<ShaderPushConstant> GetPushConstants()
        {
            return entity.GetList<ShaderPushConstant>().AsSpan();
        }

        public static implicit operator Entity(Shader shader)
        {
            return shader.entity;
        }
    }
}
