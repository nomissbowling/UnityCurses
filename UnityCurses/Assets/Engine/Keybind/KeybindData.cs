using System;
using Assets.Engine.Utility.Dictionary;
using UnityEngine;

namespace Assets.Engine.Keybind
{
    [Serializable]
    public sealed class KeybindData
    {
        [SerializeField] private DictionaryOfStringAndKeybindItem _data = new DictionaryOfStringAndKeybindItem();

        public KeybindItem AddChild(string key)
        {
            if (!_data.ContainsKey(key))
                _data.Add(key, new KeybindItem());

            return _data[key];
        }

        public KeybindItem FindChild(string key)
        {
            return _data.ContainsKey(key) ? _data[key] : null;
        }
    }
}