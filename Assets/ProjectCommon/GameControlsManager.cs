// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 06/18/2016@1:06 AM

using System;
using System.Collections.Generic;
using System.IO;
using Assets.Engine.FileSystem;
using UnityEngine;

namespace Assets.ProjectCommon
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class DefaultKeyboardMouseValueAttribute : Attribute
    {
        //

        public DefaultKeyboardMouseValueAttribute(KeyCode key)
        {
            Value = new GameControlsManager.SystemKeyboardMouseValue(key);
        }

        public DefaultKeyboardMouseValueAttribute(MouseScroll mouseScrollDirection)
        {
            Value = new GameControlsManager.SystemKeyboardMouseValue(mouseScrollDirection);
        }

        public GameControlsManager.SystemKeyboardMouseValue Value { get; private set; }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////

    public enum MouseScroll
    {
        ScrollUp,
        ScrollDown
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////

    public abstract class GameControlsEventData
    {
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////

    public abstract class GameControlsKeyEventData : GameControlsEventData
    {
        //

        public GameControlsKeyEventData(GameControlKeys controlKey)
        {
            ControlKey = controlKey;
        }

        public GameControlKeys ControlKey { get; private set; }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////

    public class GameControlsKeyDownEventData : GameControlsKeyEventData
    {
        public GameControlsKeyDownEventData(GameControlKeys controlKey)
            : base(controlKey)
        {
        }

        public GameControlsKeyDownEventData(GameControlKeys controlKey, float strength)
            : base(controlKey)
        {
            Strength = strength;
        }

        public float Strength { get; private set; }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////

    public class GameControlsKeyUpEventData : GameControlsKeyEventData
    {
        public GameControlsKeyUpEventData(GameControlKeys controlKey)
            : base(controlKey)
        {
        }

        public GameControlsKeyUpEventData(GameControlKeys controlKey, float strength)
            : base(controlKey)
        {
            Strength = strength;
        }

        public float Strength { get; private set; }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////

    public class GameControlsMouseMoveEventData : GameControlsEventData
    {
        public GameControlsMouseMoveEventData(Vector2 mouseOffset)
        {
            MouseOffset = mouseOffset;
        }

        public Vector2 MouseOffset { get; private set; }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////

    public class GameControlsTickEventData : GameControlsEventData
    {
        public GameControlsTickEventData(float delta)
        {
            Delta = delta;
        }

        public float Delta { get; private set; }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////

    public delegate void GameControlsEventDelegate(GameControlsEventData e);

    ////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    ///     Represents the player control management.
    /// </summary>
    public sealed class GameControlsManager
    {
        public static string _keybindsPath = "user:Configs/Keybinds.config";

        [SerializeField]
        public static Vector2 _mouseSensitivity = new Vector2(1, 1);

        [SerializeField]
        public static Vector2 _baseSensitivity = new Vector2(2, 2);

        private Dictionary<GameControlKeys, GameControlItem> itemsControlKeysDictionary;

        public Vector2 BaseSensitivity
        {
            set { _baseSensitivity = value; }
            get { return _baseSensitivity; }
        }

        /// <summary>
        ///     Gets an instance of the <see cref="GameControlsManager" />.
        /// </summary>
        public static GameControlsManager Instance { get; private set; }

        public Vector2 MouseSensitivity
        {
            get { return _mouseSensitivity; }
            set { _mouseSensitivity = value; }
        }

        /// <summary>
        ///     Gets the key information collection. <b>Don't modify</b>.
        /// </summary>
        public GameControlItem[] Items { get; private set; }

        ///////////////////////////////////////////

        public event GameControlsEventDelegate GameControlsEvent;

        ///////////////////////////////////////////

        /// <summary>
        ///     Initialization the class.
        /// </summary>
        /// <returns><b>true</b> if the object successfully initialized; otherwise, <b>false</b>.</returns>
        public static void Init()
        {
            if (Instance != null)
                Debug.LogError("GameControlsManager class is already initialized.");

            Instance = new GameControlsManager();
            var ret = Instance.InitInternal();
            if (!ret)
                Shutdown();
        }

        /// <summary>
        ///     Shutdown the class.
        /// </summary>
        public static void Shutdown()
        {
            if (Instance == null)
                return;

            Instance.ShutdownInternal();
            Instance = null;
        }

        private bool InitInternal()
        {
            //create items
            {
                var controlKeyCount = 0;
                {
                    foreach (var value in Enum.GetValues(typeof(GameControlKeys)))
                    {
                        var controlKey = (GameControlKeys) value;
                        if ((int) controlKey >= controlKeyCount)
                            controlKeyCount = (int) controlKey + 1;
                    }
                }

                Items = new GameControlItem[controlKeyCount];
                for (var n = 0; n < controlKeyCount; n++)
                {
                    if (!Enum.IsDefined(typeof(GameControlKeys), n))
                    {
                        Debug.LogError("GameControlsManager: Init: Invalid \"GameControlKeys\" enumeration.");
                        return false;
                    }
                    var controlKey = (GameControlKeys) n;
                    Items[n] = new GameControlItem(controlKey);
                }

                var customControlsFile = VirtualFileSystem.GetRealPathByVirtual(_keybindsPath);
                if (File.Exists(customControlsFile))
                {
                    LoadCustomConfig();
                }
                else
                {
                    ResetKeyMouseSettings();
                    SaveCustomConfig();
                }
            }

            //itemsControlKeysDictionary
            {
                itemsControlKeysDictionary = new Dictionary<GameControlKeys, GameControlItem>();
                foreach (var item in Items)
                    itemsControlKeysDictionary.Add(item.ControlKey, item);
            }

            return true;
        }

        private void ShutdownInternal()
        {
        }

        /// <summary>
        ///     Sends the notice on pressing a system key.
        /// </summary>
        /// <param name="e">Key event arguments.</param>
        /// <returns><b>true</b> if such system key is used; otherwise, <b>false</b>.</returns>
        public bool DoKeyDown(KeyCode e)
        {
            var handled = false;

            //!!!!!slowly
            foreach (var item in Items)
                if (item.BindedKeyboardMouseValues.Count > 0)
                    foreach (var value in item.BindedKeyboardMouseValues)
                        if ((value.Type == SystemKeyboardMouseValue.Types.Key) && (value.Key == e))
                        {
                            if (GameControlsEvent != null)
                                GameControlsEvent(new GameControlsKeyDownEventData(item.ControlKey, 1));
                            handled = true;
                        }
            return handled;
        }

        /// <summary>
        ///     Sends the notice on releasing a system key.
        /// </summary>
        /// <param name="e">Key event arguments.</param>
        /// <returns><b>true</b> if such system key is used; otherwise, <b>false</b>.</returns>
        public bool DoKeyUp(KeyCode e)
        {
            var handled = false;

            //!!!!!slowly
            foreach (var item in Items)
                if (item.BindedKeyboardMouseValues.Count > 0)
                    foreach (var value in item.BindedKeyboardMouseValues)
                        if ((value.Type == SystemKeyboardMouseValue.Types.Key) && (value.Key == e))
                        {
                            if (GameControlsEvent != null)
                                GameControlsEvent(new GameControlsKeyUpEventData(item.ControlKey, 1));
                            handled = true;
                        }
            return handled;
        }

        /// <summary>
        ///     Sends the notice on cursor moved.
        /// </summary>
        /// <param name="mouseOffset">Current mouse position.</param>
        public void DoMouseMoveRelative(Vector2 mouseOffset)
        {
            if (GameControlsEvent != null)
                GameControlsEvent(new GameControlsMouseMoveEventData(mouseOffset));
        }

        public bool DoMouseMouseWheel(int delta)
        {
            var scrollDirection = delta > 0 ? MouseScroll.ScrollUp : MouseScroll.ScrollDown;

            var handled = false;
            foreach (var item in Items)
                if (item.BindedKeyboardMouseValues.Count > 0)
                    foreach (var value in item.BindedKeyboardMouseValues)
                        if ((value.Type == SystemKeyboardMouseValue.Types.MouseScrollDirection) &&
                            (value.ScrollDirection == scrollDirection))
                        {
                            if (GameControlsEvent != null)
                            {
                                GameControlsEvent(new GameControlsKeyDownEventData(item.ControlKey, delta));
                                GameControlsEvent(new GameControlsKeyUpEventData(item.ControlKey));
                            }
                            handled = true;
                        }
            return handled;
        }

        public void DoTick(float delta)
        {
            if (GameControlsEvent != null)
                GameControlsEvent(new GameControlsTickEventData(delta));
        }

        public void DoKeyUpAll()
        {
            foreach (var item in Items)
            {
                var eventData =
                    new GameControlsKeyUpEventData(item.ControlKey, 1);

                if (GameControlsEvent != null)
                    GameControlsEvent(eventData);
            }
        }

        public GameControlItem GetItemByControlKey(GameControlKeys controlKey)
        {
            GameControlItem item;
            if (!itemsControlKeysDictionary.TryGetValue(controlKey, out item))
                return null;
            return item;
        }

        public bool IsDefaultControlKey(GameControlItem item, KeyCode key)
        {
            if (item == null)
                return false;

            foreach (var value in item.DefaultKeyboardMouseValues)
                if ((value.Type == SystemKeyboardMouseValue.Types.Key) && (value.Key == key))
                    return true;

            return false;
        }

        public bool IsControlKey(GameControlItem item, KeyCode key)
        {
            if (item == null)
                return false;

            if (item.BindedKeyboardMouseValues.Count > 0)
                foreach (var value in item.BindedKeyboardMouseValues)
                    if ((value.Type == SystemKeyboardMouseValue.Types.Key) && (value.Key == key))
                        return true;

            return IsDefaultControlKey(item, key);
        }

        public bool IsControlKey(GameControlKeys controlKey, KeyCode key)
        {
            return IsControlKey(GetItemByControlKey(controlKey), key);
        }

        public bool IsAlreadyBinded(KeyCode key, out SystemKeyboardMouseValue control)
        {
            control = null;
            foreach (var item in Items)
                if (item.BindedKeyboardMouseValues.Count > 0)
                    foreach (var value in item.BindedKeyboardMouseValues)
                        if ((value.Type == SystemKeyboardMouseValue.Types.Key) && (value.Key == key))
                        {
                            control = value;
                            return true;
                        }

            return false;
        }

        public bool IsAlreadyBinded(MouseScroll scrollDirection, out SystemKeyboardMouseValue control)
        {
            control = null;
            foreach (var item in Items)
                if (item.BindedKeyboardMouseValues.Count > 0)
                    foreach (var value in item.BindedKeyboardMouseValues)
                        if ((value.Type == SystemKeyboardMouseValue.Types.MouseScrollDirection) &&
                            (value.ScrollDirection == scrollDirection))
                        {
                            control = value;
                            return true;
                        }
            return false;
        }

        public void ResetKeyMouseSettings()
        {
            foreach (var item in Items)
            {
                item.BindedKeyboardMouseValues.Clear();

                foreach (var defaultKeyboardMouseValue in item.DefaultKeyboardMouseValues)
                    item.BindedKeyboardMouseValues.Add(new SystemKeyboardMouseValue(defaultKeyboardMouseValue));
            }
        }

        public void SaveCustomConfig()
        {
            var block = new TextBlock();
            var controlBloc = block.AddChild("Controls");

            foreach (var item in Items)
            {
                var currentKeyBlock = controlBloc.AddChild(item.ControlKey.ToString());

                // Keyboard Setting
                if (item.BindedKeyboardMouseValues.Count > 0)
                {
                    var keyboardBlock = currentKeyBlock.AddChild("Keyboard");
                    foreach (var keyboardvalue in item.BindedKeyboardMouseValues)
                    {
                        var keyBlock = keyboardBlock.AddChild("Item");
                        SystemKeyboardMouseValue.Save(keyboardvalue, keyBlock);
                    }
                }
            }

            var fileName = VirtualFileSystem.GetRealPathByVirtual(_keybindsPath);
            try
            {
                var directoryName = Path.GetDirectoryName(fileName);
                if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);

                using (var writer = new StreamWriter(fileName))
                {
                    writer.Write(block.DumpToString());
                }
            }
            catch
            {
                Debug.LogError(string.Format("Saving file failed \"{0}\".", fileName));
            }
        }

        public void LoadCustomConfig()
        {
            string error;
            var customFilename = VirtualFileSystem.GetRealPathByVirtual(_keybindsPath);
            var customblock = TextBlockUtils.LoadFromRealFile(customFilename, out error);
            if (error != null)
                Debug.LogError(string.Format("Loading file failed \"{0}\"  // {1}.", error, customFilename));


            var controlBloc = customblock.FindChild("Controls");
            if (controlBloc == null)
            {
                ResetKeyMouseSettings();
                return;
            }

            foreach (var item in Items)
            {
                var currentKeyBlock = controlBloc.FindChild(item.ControlKey.ToString());
                if (currentKeyBlock == null)
                    continue;

                // keyboard Setting
                var keybordBlock = currentKeyBlock.FindChild("Keyboard");
                if ((keybordBlock != null) && (keybordBlock.Children.Count > 0))
                    foreach (var keyBlocklock in keybordBlock.Children)
                    {
                        var keyboardvalue = SystemKeyboardMouseValue.Load(keyBlocklock);
                        keyboardvalue.Parent = item;
                        item.BindedKeyboardMouseValues.Add(keyboardvalue);
                    }
            }
        }

        ///////////////////////////////////////////
        public class SystemControlValue
        {
        }

        public class SystemKeyboardMouseValue : SystemControlValue
        {
            public enum Types
            {
                Key,
                MouseScrollDirection
            }

            private GameControlItem _parent;

            public bool Unbound;

            public SystemKeyboardMouseValue()
            {
            }

            public SystemKeyboardMouseValue(SystemKeyboardMouseValue source)
            {
                Type = source.Type;
                Key = source.Key;
                ScrollDirection = source.ScrollDirection;
                _parent = source.Parent;
            }

            public SystemKeyboardMouseValue(KeyCode key)
            {
                Type = Types.Key;
                Key = key;
            }

            public SystemKeyboardMouseValue(MouseScroll mouseScrollDirection)
            {
                Type = Types.MouseScrollDirection;
                ScrollDirection = mouseScrollDirection;
            }

            public GameControlItem Parent
            {
                get { return _parent; }
                set { _parent = value; }
            }

            public Types Type { get; private set; }

            public KeyCode Key { get; private set; }

            public MouseScroll ScrollDirection { get; private set; }

            public override string ToString()
            {
                if (Unbound)
                    return string.Format("{0} - Unbound", Parent.ControlKey);

                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (Type)
                {
                    case Types.Key:
                        return string.Format("{0} - Key {1}", Parent.ControlKey, Key);
                    case Types.MouseScrollDirection:
                        return string.Format("{0} - Scroll {1}", Parent.ControlKey, ScrollDirection);
                }

                return string.Empty;
            }

            public static void Save(SystemKeyboardMouseValue item, TextBlock block)
            {
                block.SetAttribute("type", item.Type.ToString());
                switch (item.Type)
                {
                    case Types.Key:
                        block.SetAttribute("key", item.Key.ToString());
                        break;
                    case Types.MouseScrollDirection:
                        block.SetAttribute("scroll", item.ScrollDirection.ToString());
                        break;
                }
            }

            public static SystemKeyboardMouseValue Load(TextBlock block)
            {
                var value = new SystemKeyboardMouseValue();

                var type = block.GetAttribute("type");
                if (!string.IsNullOrEmpty(type))
                    value.Type = (Types) Enum.Parse(typeof(Types), type);

                var key = block.GetAttribute("key");
                if (!string.IsNullOrEmpty(key))
                    value.Key = (KeyCode) Enum.Parse(typeof(KeyCode), key);

                var scroll = block.GetAttribute("scroll");
                if (!string.IsNullOrEmpty(scroll))
                    value.ScrollDirection = (MouseScroll) Enum.Parse(typeof(MouseScroll), scroll);

                return value;
            }
        }

        ///////////////////////////////////////////

        public class GameControlItem
        {
            // ReSharper disable once FieldCanBeMadeReadOnly.Global
            public List<SystemKeyboardMouseValue> bindedKeyboardMouseValues =
                new List<SystemKeyboardMouseValue>();

            //

            public GameControlItem(GameControlKeys controlKey)
            {
                ControlKey = controlKey;

                //defaultKeyboardMouseValue
                {
                    var field = typeof(GameControlKeys).GetField(Enum.GetName(typeof(GameControlKeys), controlKey));
                    var attributes =
                        (DefaultKeyboardMouseValueAttribute[])
                        Attribute.GetCustomAttributes(field, typeof(DefaultKeyboardMouseValueAttribute));

                    DefaultKeyboardMouseValues = new SystemKeyboardMouseValue[attributes.Length];
                    for (var n = 0; n < attributes.Length; n++)
                    {
                        DefaultKeyboardMouseValues[n] = attributes[n].Value;
                        DefaultKeyboardMouseValues[n].Parent = this;
                    }
                }
            }

            public GameControlKeys ControlKey { get; private set; }

            /// <summary>
            ///     <b>Don't modify</b>.
            /// </summary>
            public SystemKeyboardMouseValue[] DefaultKeyboardMouseValues { get; private set; }

            public List<SystemKeyboardMouseValue> BindedKeyboardMouseValues
            {
                get { return bindedKeyboardMouseValues; }
            }

            public override string ToString()
            {
                if (bindedKeyboardMouseValues.Count > 0)
                    return ControlKey + " - " + bindedKeyboardMouseValues[0];
                if (DefaultKeyboardMouseValues.Length > 0)
                    return ControlKey + " - " + DefaultKeyboardMouseValues[0];

                return ControlKey + " - Unbound";
            }
        }
    }
}