using System;

namespace Azathrix.GameKit.Runtime.Utils
{
    /// <summary>
    /// 唯一标识符令牌，用于管理状态生命周期
    /// </summary>
    public readonly struct Token : IEquatable<Token>
    {
        private static int _counter;

        private readonly int _id;

        public bool IsValid => _id != 0;

        private Token(int id)
        {
            _id = id;
        }

        public static Token Create()
        {
            return new Token(++_counter);
        }

        public bool Equals(Token other) => _id == other._id;

        public override bool Equals(object obj) => obj is Token other && Equals(other);

        public override int GetHashCode() => _id;

        public static bool operator ==(Token left, Token right) => left.Equals(right);

        public static bool operator !=(Token left, Token right) => !left.Equals(right);

        public override string ToString() => $"Token({_id})";
    }
}
