using Worlds;

namespace Shaders.Tests
{
    public class ShaderEntityTests : ShaderTests
    {
        [Test]
        public void VerifyShaderProperties()
        {
            using World world = CreateWorld();
            Shader a = new(world);

            Assert.That(a.Is(), Is.True);
        }
    }
}