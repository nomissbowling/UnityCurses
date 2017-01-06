using System;
using Assets.Engine.Utility.Dictionary;
using UnityEngine;

namespace Assets.Engine.Keybind
{
    [Serializable]
    public sealed class KeybindData
    {
        [SerializeField] public DictionaryOfStringAndKeybindItem Data = new DictionaryOfStringAndKeybindItem();

        public KeybindItem AddChild(string key)
        {
            if (!Data.ContainsKey(key))
                Data.Add(key, new KeybindItem());

            return Data[key];
        }

        public KeybindItem FindChild(string key)
        {
            return Data.ContainsKey(key) ? Data[key] : null;
        }
    }
}