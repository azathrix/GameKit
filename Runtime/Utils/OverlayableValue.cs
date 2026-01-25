using System;
using System.Collections.Generic;

namespace Azathrix.GameKit.Runtime.Utils
{
    /// <summary>
    /// 支持优先级的值覆盖管理器
    /// 使用 Token 作为键，支持添加/移除覆盖值
    /// 返回最高优先级的值
    /// </summary>
    public class OverlayableValue<T>
    {
        private readonly T _defaultValue;
        private readonly List<Entry> _entries = new();

        public event Action<T> OnValueChanged;

        public T Value
        {
            get
            {
                if (_entries.Count == 0)
                    return _defaultValue;

                int highestPriority = int.MinValue;
                T result = _defaultValue;

                foreach (var entry in _entries)
                {
                    if (entry.Priority >= highestPriority)
                    {
                        highestPriority = entry.Priority;
                        result = entry.Value;
                    }
                }

                return result;
            }
        }

        public OverlayableValue(T defaultValue = default)
        {
            _defaultValue = defaultValue;
        }

        public Token SetValue(T value, int priority = 0)
        {
            var token = Token.Create();
            SetValue(token, value, priority);
            return token;
        }

        public void SetValue(Token token, T value, int priority = 0)
        {
            var oldValue = Value;

            for (int i = 0; i < _entries.Count; i++)
            {
                if (_entries[i].Token == token)
                {
                    _entries[i] = new Entry(token, value, priority);
                    NotifyIfChanged(oldValue);
                    return;
                }
            }

            _entries.Add(new Entry(token, value, priority));
            NotifyIfChanged(oldValue);
        }

        public void RemoveValue(Token token)
        {
            var oldValue = Value;

            for (int i = _entries.Count - 1; i >= 0; i--)
            {
                if (_entries[i].Token == token)
                {
                    _entries.RemoveAt(i);
                    break;
                }
            }

            NotifyIfChanged(oldValue);
        }

        public void Clear()
        {
            var oldValue = Value;
            _entries.Clear();
            NotifyIfChanged(oldValue);
        }

        private void NotifyIfChanged(T oldValue)
        {
            var newValue = Value;
            if (!EqualityComparer<T>.Default.Equals(oldValue, newValue))
            {
                OnValueChanged?.Invoke(newValue);
            }
        }

        private struct Entry
        {
            public Token Token;
            public T Value;
            public int Priority;

            public Entry(Token token, T value, int priority)
            {
                Token = token;
                Value = value;
                Priority = priority;
            }
        }
    }
}
