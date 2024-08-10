using Shaders;
using Shaders.Components;
using System;

public static class ShaderFunctions
{
    public static uint GetVersion<T>(this T shader) where T : IShader
    {
        IsShader component = shader.GetComponent<T, IsShader>();
        return component.version;
    }

    public static ReadOnlySpan<byte> GetVertexBytes<T>(this T shader) where T : IShader
    {
        IsShader component = shader.GetComponent<T, IsShader>();
        return shader.GetList<T, byte>(component.vertex).AsSpan();
    }

    public static ReadOnlySpan<byte> GetFragmentBytes<T>(this T shader) where T : IShader
    {
        IsShader component = shader.GetComponent<T, IsShader>();
        return shader.GetList<T, byte>(component.fragment).AsSpan();
    }

    public static ReadOnlySpan<ShaderVertexInputAttribute> GetVertexAttributes<T>(this T shader) where T : IShader
    {
        return shader.GetList<T, ShaderVertexInputAttribute>().AsSpan();
    }

    public static ReadOnlySpan<ShaderUniformProperty> GetUniformProperties<T>(this T shader) where T : IShader
    {
        return shader.GetList<T, ShaderUniformProperty>().AsSpan();
    }

    public static ReadOnlySpan<ShaderSamplerProperty> GetSamplerProperties<T>(this T shader) where T : IShader
    {
        return shader.GetList<T, ShaderSamplerProperty>().AsSpan();
    }

    public static ReadOnlySpan<ShaderPushConstant> GetPushConstants<T>(this T shader) where T : IShader
    {
        return shader.GetList<T, ShaderPushConstant>().AsSpan();
    }
}