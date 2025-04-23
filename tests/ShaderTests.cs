using Types;
using Worlds;
using Worlds.Tests;

namespace Shaders.Tests
{
    public abstract class ShaderTests : WorldTests
    {
        static ShaderTests()
        {
            MetadataRegistry.Load<ShadersMetadataBank>();
        }

        protected override Schema CreateSchema()
        {
            Schema schema = base.CreateSchema();
            schema.Load<ShadersSchemaBank>();
            return schema;
        }
    }
}