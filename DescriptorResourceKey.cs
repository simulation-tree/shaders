using System;

namespace Shaders
{
    /// <summary>
    /// A value that refers to a set and binding combination within shaders.
    /// </summary>
    public readonly struct DescriptorResourceKey : IEquatable<DescriptorResourceKey>
    {
        /// <summary>
        /// Maximum allowed value for both the set and binding values.
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

            value = (byte)(binding << 4 | set);
        }

        public readonly override string ToString()
        {
            Span<char> buffer = stackalloc char[32];
            int length = ToString(buffer);
            return new string(buffer[..length]);
        }

        public readonly int ToString(Span<char> buffer)
        {
            Set.TryFormat(buffer, out int length);
            buffer[length++] = ':';
            Binding.TryFormat(buffer[length..], out int bindingLength);
            return length + bindingLength;
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is DescriptorResourceKey key && Equals(key);
        }

        public readonly bool Equals(DescriptorResourceKey other)
        {
            return value == other.value;
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
