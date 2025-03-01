using System;

namespace Shaders
{
    [Flags]
    public enum ShaderFlags : byte
    {
        None = 0,
        Instanced = 1
    }
}