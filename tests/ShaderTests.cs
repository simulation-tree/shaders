using Data;
using Types;
using Worlds;
using Worlds.Tests;

namespace Shaders.Tests
{
    public abstract class ShaderTests : WorldTests
    {
        static ShaderTests()
        {
            TypeRegistry.Load<ShadersTypeBank>();
            TypeRegistry.Load<DataTypeBank>();
        }

        protected override Schema CreateSchema()
        {
            Schema schema = base.CreateSchema();
            schema.Load<ShadersSchemaBank>();
            schema.Load<DataSchemaBank>();
            return schema;
        }
    }
}