using System;
using Assets.Engine.Utility.Dictionary;
using UnityEngine;

namespace Assets.Engine.Keybind
{
    /// <summary>
    ///     Holds reference data in key-value format for keybind data master class that holds dictionary of all game control
    ///     keys and the system keys they are binded to in this object as attributes.
    /// </summary>
    [Serializable]
    public sealed class KeybindItem
    {
        /// <summary>
        ///     Reference to all of the binds for a particular key in the keybind data master table.
        /// </summary>
        [SerializeField] private DictionaryOfStringAndString _binds = new DictionaryOfStringAndString();

        /// <summary>
        ///     Reference to all of the binds for a particular key in the keybind data master table.
        /// </summary>
        public DictionaryOfStringAndString Binds
        {
            get { return _binds; }
        }

        public void SetAttribute(string key, string value)
        {
            // Add the key if it does not exist.
            if (!_binds.ContainsKey(key))
                _binds.Add(key, value);
        }

        public string GetAttribute(string key)
        {
            return _binds.ContainsKey(key) ? _binds[key] : string.Empty;
        }
    }
}