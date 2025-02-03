using Worlds;

namespace Shaders.Tests
{
    public class ShaderEntityTests : ShaderTests
    {
        [Test]
        public void VerifyShaderProperties()
        {
            using World world = CreateWorld();
            Shader a = new(world, ShaderType.Vertex);

            Assert.That(a.IsCompliant, Is.True);
        }
    }
}