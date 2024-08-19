using Shaders;
using Shaders.Components;
using Simulation;
using System.Threading;
using System.Threading.Tasks;

public static class ShaderFunctions
{
    /// <summary>
    /// Awaits until shader data is loaded from its requested address.
    /// </summary>
    public static async Task UntilLoaded<T>(this T shader, CancellationToken cancellation = default) where T : unmanaged, IShader
    {
        World world = shader.World;
        eint shaderEntity = shader.Value;
        while (!world.ContainsComponent<IsShader>(shaderEntity))
        {
            await Task.Delay(1, cancellation);
        }
    }
}