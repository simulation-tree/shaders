using Types;
using Unmanaged.Tests;
using Worlds;

namespace Shaders.Tests
{
    public abstract class ShaderTests : UnmanagedTests
    {
        static ShaderTests()
        {
            TypeRegistry.Load<Shaders.TypeBank>();
            TypeRegistry.Load<Data.TypeBank>();
        }

        protected virtual Schema CreateSchema()
        {
            Schema schema = new();
            schema.Load<Shaders.SchemaBank>();
            schema.Load<Data.SchemaBank>();
            return schema;
        }

        protected virtual World CreateWorld()
        {
            return new(CreateSchema());
        }
    }
}