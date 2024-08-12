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

        public Shader()
        {
            throw new InvalidOperationException("Cannot create a shader without a world.");
        }

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
            entity.AddComponent(new IsShader(vertex.entity.value, fragment.entity.value));
            entity.CreateList<Entity, ShaderPushConstant>();
            entity.CreateList<Entity, ShaderVertexInputAttribute>();
            entity.CreateList<Entity, ShaderUniformProperty>();
            entity.CreateList<Entity, ShaderSamplerProperty>();

            world.Submit(new DataUpdate());
            world.Submit(new ShaderUpdate());
            world.Poll();
        }

        /// <summary>
        /// Creates a new shader using the provided data entities.
        /// <para>Data is expected to be UTF8 bytes.</para>
        /// </summary>
        public Shader(World world, eint vertex, eint fragment)
        {
            entity = new(world);
            entity.AddComponent(new IsShader(vertex, fragment));
            entity.CreateList<Entity, ShaderPushConstant>();
            entity.CreateList<Entity, ShaderVertexInputAttribute>();
            entity.CreateList<Entity, ShaderUniformProperty>();
            entity.CreateList<Entity, ShaderSamplerProperty>();

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
    }
}
