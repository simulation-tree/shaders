using Shaders.Components;
using System;
using System.Diagnostics;

namespace Shaders
{
    public static class ShaderRequestFlagsExtensions
    {
        public static ShaderType GetShaderType(this IsShaderRequest.Flags flags)
        {
            ThrowIfUnknownType(flags);

            if ((flags & IsShaderRequest.Flags.FragmentShader) != 0)
            {
                return ShaderType.Fragment;
            }

            return ShaderType.Vertex;
        }

        [Conditional("DEBUG")]
        private static void ThrowIfUnknownType(IsShaderRequest.Flags flags)
        {
            bool vertex = (flags & IsShaderRequest.Flags.VertexShader) != 0;
            bool fragment = (flags & IsShaderRequest.Flags.FragmentShader) != 0;
            if (vertex && fragment)
            {
                throw new NotSupportedException("Shader cannot be both vertex and fragment shader at the same time");
            }
            else if (!vertex && !fragment)
            {
                throw new NotSupportedException("Shader must be either vertex or fragment shader");
            }
        }
    }
}