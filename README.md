# Shaders
Definitions for shader programs imported from GLSL vertex and fragment sources into SPV.

### Importing shaders
```cs
using World world = new();
Shader shader = new(world, "*/unlit.vert.glsl", "*/unlit.frag.glsl");
while (!shader.Is())
{
    world.Submit(new DataUpdate()); //to load the bytes
    world.Submit(new ShaderUpdate()); //load import the shader from the bytes
    world.Poll();
}
```

### Reading properties
After a shader is imported, its properties can be read for later setup with other systems:
```cs
ReadOnlySpan<ShaderVertexInputAttribute> vertexAttributes = shader.VertexAttributes;
ReadOnlySpan<ShaderUniformProperty> uniformProperties = shader.UniformProperties;
ReadOnlySpan<ShaderSamplerProperty> samplerProperties = shader.SamplerProperties;
ReadOnlySpan<ShaderPushConstant> pushConstants = shader.PushConstants;
```