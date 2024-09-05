using System;
using Unmanaged;

namespace Shaders
{
    /// <summary>
    /// A value that locates a resource inside a shader.
    /// </summary>
    public readonly struct DescriptorResourceKey : IEquatable<DescriptorResourceKey>
    {
        /// <summary>
        /// Maximum allowed value for both the binding and set values.
        /// </summary>
        public const byte MaxSetOrBindingValue = 15;

        private readonly byte value;

        public readonly byte Set => (byte)(value >> 4);
        public readonly byte Binding => (byte)(value & 0x0F);

        public DescriptorResourceKey(byte binding, byte set)
        {
            if (set >= MaxSetOrBindingValue || binding >= MaxSetOrBindingValue)
            {
                throw new ArgumentOutOfRangeException();
            }

            value = (byte)(set << 4 | binding);
        }

        public unsafe readonly override string ToString()
        {
            USpan<char> buffer = stackalloc char[32];
            uint length = ToString(buffer);
            return new string(buffer.pointer, 0, (int)length);
        }

        public readonly uint ToString(USpan<char> buffer)
        {
            uint length = 0;
            length += Binding.ToString(buffer);  
            buffer[length++] = ':';
            length += Set.ToString(buffer.Slice(length));
            return length;
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is DescriptorResourceKey key && Equals(key);
        }

        public readonly bool Equals(DescriptorResourceKey other)
        {
            return value == other.value;
        }

        public readonly bool Equals(byte binding, byte set)
        {
            return value == (byte)(set << 4 | binding);
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(value);
        }

        public static bool operator ==(DescriptorResourceKey left, DescriptorResourceKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DescriptorResourceKey left, DescriptorResourceKey right)
        {
            return !(left == right);
        }
    }
}
