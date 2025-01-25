using Types;
using Worlds;
using Worlds.Tests;

namespace Shaders.Tests
{
    public abstract class ShaderTests : WorldTests
    {
        static ShaderTests()
        {
            TypeRegistry.Load<Shaders.TypeBank>();
            TypeRegistry.Load<Data.Core.TypeBank>();
        }

        protected override Schema CreateSchema()
        {
            Schema schema = base.CreateSchema();
            schema.Load<Shaders.SchemaBank>();
            schema.Load<Data.Core.SchemaBank>();
            return schema;
        }
    }
}