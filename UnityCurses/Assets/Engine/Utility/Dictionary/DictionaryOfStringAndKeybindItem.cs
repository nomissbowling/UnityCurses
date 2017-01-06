using System;
using Assets.Engine.Keybind;

namespace Assets.Engine.Utility.Dictionary
{
    [Serializable]
    public sealed class DictionaryOfStringAndKeybindItem : SerializableDictionary<string, KeybindItem>
    {
    }
}