using System;

namespace Shaders
{
    public readonly struct ResourceKey : IEquatable<ResourceKey>
    {
        public const byte MaxSetOrBinding = 15;

        private readonly byte value;

        public readonly byte Set => (byte)(value >> 4);
        public readonly byte Binding => (byte)(value & 0x0F);

        public ResourceKey(byte set, byte binding)
        {
            if (set > MaxSetOrBinding || binding > MaxSetOrBinding)
            {
                throw new ArgumentOutOfRangeException();
            }

            value = (byte)(set << 4 | binding);
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
            return obj is ResourceKey key && Equals(key);
        }

        public readonly bool Equals(ResourceKey other)
        {
            return value == other.value;
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(value);
        }

        public static bool operator ==(ResourceKey left, ResourceKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ResourceKey left, ResourceKey right)
        {
            return !(left == right);
        }
    }
}
