using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Source.ProjectCommon.Keybind
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
        [SerializeField] public Dictionary<string, string> Binds = new Dictionary<string, string>();

        public void SetAttribute(string key, string value)
        {
            // Add the key if it does not exist.
            if (!Binds.ContainsKey(key))
                Binds.Add(key, value);
        }

        public string GetAttribute(string key)
        {
            return Binds.ContainsKey(key) ? Binds[key] : string.Empty;
        }
    }
}