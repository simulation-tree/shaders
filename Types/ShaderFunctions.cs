using Shaders;
using Shaders.Components;
using System;

public static class ShaderFunctions
{
    public static ReadOnlySpan<byte> GetRawVertexBytes<T>(this T shader) where T : unmanaged, IShader
    {
        IsShader shaderComponent = shader.GetComponent<T, IsShader>();
        return shader.GetList<T, byte>(shaderComponent.vertex).AsSpan();
    }

    public static ReadOnlySpan<byte> GetRawFragmentBytes<T>(this T shader) where T : unmanaged, IShader
    {
        IsShader shaderComponent = shader.GetComponent<T, IsShader>();
        return shader.GetList<T, byte>(shaderComponent.fragment).AsSpan();
    }

    public static ReadOnlySpan<ShaderVertexInputAttribute> GetVertexAttributes<T>(this T shader) where T : unmanaged, IShader
    {
        return shader.GetList<T, ShaderVertexInputAttribute>().AsSpan();
    }

    public static ReadOnlySpan<ShaderUniformProperty> GetUniformProperties<T>(this T shader) where T : unmanaged, IShader
    {
        return shader.GetList<T, ShaderUniformProperty>().AsSpan();
    }

    public static ReadOnlySpan<ShaderSamplerProperty> GetSamplerProperties<T>(this T shader) where T : unmanaged, IShader
    {
        return shader.GetList<T, ShaderSamplerProperty>().AsSpan();
    }
}