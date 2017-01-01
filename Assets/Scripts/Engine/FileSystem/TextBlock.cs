using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;
using Assets.Scripts.Engine.Utility;
using UnityEngine;

namespace Assets.Scripts.Engine.FileSystem
{
    /// <summary>
    ///     The class allows to store the text information in the hierarchical form.
    ///     Supports creation of children and attributes.
    /// </summary>
    public class TextBlock
    {
        private List<Attribute> _attributeList = new List<Attribute>();
        private ReadOnlyCollection<Attribute> _attributes;
        private string _name;
        private List<TextBlock> _textBlockList = new List<TextBlock>();
        private ReadOnlyCollection<TextBlock> _textBlocks;

        /// <summary>
        ///     It is applied only to creation root blocks. Not for creation of children.
        /// </summary>
        /// <example>
        ///     Example of creation of the block and filling by data.
        ///     <code>
        /// TextBlock block = new TextBlock();
        /// TextBlock childBlock = block.AddChild( "childBlock", "child block data" );
        /// childBlock.SetAttribute( "attribute", "attribute value" );
        /// </code>
        /// </example>
        /// <seealso cref="M:Assets.Scripts.Engine.FileSystem.TextBlock.AddChild(System.String,System.String)" />
        /// <seealso cref="M:Assets.Scripts.Engine.FileSystem.TextBlock.SetAttribute(System.String,System.String)" />
        public TextBlock()
        {
            _textBlocks = new ReadOnlyCollection<TextBlock>(_textBlockList);
            _attributes = new ReadOnlyCollection<Attribute>(_attributeList);
        }

        /// <summary>Gets the parent block.</summary>
        public TextBlock Parent { get; private set; }

        /// <summary>Gets or set block name.</summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;

                _name = value;
                if (!string.IsNullOrEmpty(_name))
                    return;

                Debug.LogError("TextBlock: set Name: \"name\" is null or empty.");
            }
        }

        /// <summary>Gets or set block string data.</summary>
        public string Data { get; set; }

        /// <summary>Gets the children collection.</summary>
        public IList<TextBlock> Children
        {
            get { return _textBlocks; }
        }

        /// <summary>Gets the attributes collection.</summary>
        public IList<Attribute> Attributes
        {
            get { return _attributes; }
        }

        /// <summary>Finds child block by name.</summary>
        /// <param name="name">The block name.</param>
        /// <returns>
        ///     <see cref="T:Assets.Scripts.Engine.FileSystem.TextBlock" /> if the block has been exists; otherwise,
        ///     <b>null</b>.
        /// </returns>
        public TextBlock FindChild(string name)
        {
            for (var index = 0; index < _textBlockList.Count; ++index)
            {
                var textBlock = _textBlockList[index];
                if (textBlock.Name == name)
                    return textBlock;
            }

            return null;
        }

        /// <summary>Creates the child block.</summary>
        /// <param name="name">The block name.</param>
        /// <param name="data">The block data string.</param>
        /// <returns>The child block.</returns>
        /// <remarks>Names of blocks can repeat.</remarks>
        public TextBlock AddChild(string name, string data)
        {
            if (string.IsNullOrEmpty(name))
                Debug.LogError("TextBlock: AddChild: \"name\" is null or empty.");

            var textBlock = new TextBlock
            {
                Parent = this,
                _name = name,
                Data = data
            };

            _textBlockList.Add(textBlock);
            return textBlock;
        }

        /// <summary>Creates the child block.</summary>
        /// <param name="name">The block name.</param>
        /// <returns>The child block.</returns>
        /// <remarks>Names of blocks can repeat.</remarks>
        public TextBlock AddChild(string name)
        {
            return AddChild(name, string.Empty);
        }

        /// <summary>Deletes child block.</summary>
        /// <param name="child">The child block.</param>
        public void DeleteChild(TextBlock child)
        {
            _textBlockList.Remove(child);
            child.Parent = null;
            child._name = "";
            child.Data = "";
            child._textBlockList = null;
            child._attributeList = null;
        }

        /// <summary>Attaches the child block.</summary>
        /// <param name="child">The child block.</param>
        /// <returns></returns>
        public void AttachChild(TextBlock child)
        {
            if (child.Parent != null)
                Debug.LogError(
                    "TextBlock: AddChild: Unable to attach. Block is already attached to another block. child.Parent != null.");
            child.Parent = this;
            _textBlockList.Add(child);
        }

        /// <summary>Detaches child block without deleting.</summary>
        /// <param name="child">The child block.</param>
        public void DetachChild(TextBlock child)
        {
            _textBlockList.Remove(child);
            child.Parent = null;
        }

        /// <summary>Returns the attribute value by name.</summary>
        /// <param name="name">The attribute name.</param>
        /// <param name="defaultValue">Default value. If the attribute does not exist that this value will return.</param>
        /// <returns>The attribute value if the attribute exists; otherwise, default value.</returns>
        public string GetAttribute(string name, string defaultValue)
        {
            foreach (var attribute in _attributeList)
                if (attribute.Name == name)
                    return attribute.Value;

            return defaultValue;
        }

        /// <summary>Returns the attribute value by name.</summary>
        /// <param name="name">The attribute name.</param>
        /// <returns>The attribute value if the attribute exists; otherwise, empty string.</returns>
        public string GetAttribute(string name)
        {
            return GetAttribute(name, string.Empty);
        }

        /// <summary>Checks existence of attribute.</summary>
        /// <param name="name">The attribute name.</param>
        /// <returns><b>true</b> if the block exists; otherwise, <b>false</b>.</returns>
        public bool IsAttributeExist(string name)
        {
            for (var index = 0; index < _attributeList.Count; ++index)
                if (_attributeList[index].Name == name)
                    return true;

            return false;
        }

        /// <summary>
        ///     Sets attribute. If such already there is that rewrites him.
        /// </summary>
        /// <param name="name">The attribute name.</param>
        /// <param name="value">The attribute value.</param>
        public void SetAttribute(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
                Debug.LogError("TextBlock: AddChild: \"name\" is null or empty.");

            if (value == null)
                Debug.LogError("TextBlock: AddChild: \"value\" is null.");

            for (var index = 0; index < _attributeList.Count; ++index)
            {
                var attribute = _attributeList[index];
                if (attribute.Name == name)
                {
                    attribute._value = value;
                    return;
                }
            }

            _attributeList.Add(new Attribute
            {
                _name = name,
                _value = value
            });
        }

        /// <summary>Deletes attribute if he exists.</summary>
        /// <param name="name">The attribute name.</param>
        public void DeleteAttribute(string name)
        {
            for (var index = 0; index < _attributeList.Count; ++index)
                if (name == _attributeList[index]._name)
                {
                    var attribute = _attributeList[index];

                    var str1 = string.Empty;
                    attribute._name = str1;

                    var str2 = string.Empty;
                    attribute._value = str2;

                    _attributeList.RemoveAt(index);
                    break;
                }
        }

        /// <summary>Deletes all attributes.</summary>
        public void DeleteAllAttributes()
        {
            _attributeList.Clear();
        }

        private static string A([In] int obj0)
        {
            var str = "";
            for (var index = 0; index < obj0; ++index)
                str += "\t";
            return str;
        }

        /// <summary>
        ///     Returns a string containing all data about the block and his children.
        /// </summary>
        /// <returns>A string containing all data about the block and his children.</returns>
        /// <remarks>
        ///     This method is applied at preservation of data of the block in a file.
        /// </remarks>
        /// <example>
        ///     Example of preservation of data of the block in a file.
        ///     <code>
        /// TextBlock block = ...
        /// StreamWriter writer = new StreamWriter( fileName );
        /// writer.Write( block.DumpToString() );
        /// writer.Close();
        /// </code>
        /// </example>
        /// <seealso cref="M:Assets.Scripts.Engine.FileSystem.TextBlock.Parse(System.String,System.String@)" />
        public string DumpToString()
        {
            // Dumps the current instance of this text block into a string representation of itself.
            var stringBuilder = new StringBuilder(JSONExtensions.Serialize(this));
            return stringBuilder.ToString();
        }

        /// <summary>
        ///     Returns a string that represents the current text block.
        /// </summary>
        /// <returns>A string that represents the current text block.</returns>
        public override string ToString()
        {
            var str = string.Format("Name: \"{0}\"", _name);

            if (!string.IsNullOrEmpty(Data))
                str += string.Format(", Data: \"{0}\"", Data);

            return str;
        }

        /// <summary>
        ///     Parses the text with data of the block and his children.
        /// </summary>
        /// <param name="str">The data string.</param>
        /// <param name="errorString">The information on an error.</param>
        /// <returns><see cref="T:Engine.FileSystem.TextBlock" /> if the block has been parsed; otherwise, <b>null</b>.</returns>
        /// <seealso cref="M:Engine.FileSystem.TextBlock.DumpToString" />
        /// <remarks>
        ///     For convenience of loading of blocks there is auxiliary class <see cref="T:Engine.FileSystem.TextBlockUtils" />.
        /// </remarks>
        /// <example>
        ///     Example of loading of data of the block from a stream.
        ///     <code>
        /// FileStream stream = ...;
        /// StreamReader streamReader = new StreamReader( stream );
        /// string error;
        /// TextBlock block = TextBlock.Parse( streamReader.ReadToEnd(), out error );
        /// streamReader.Dispose();
        /// </code>
        /// </example>
        public static TextBlock Parse(string str, out string errorString)
        {
            errorString = string.Empty;

            try
            {
                return JSONExtensions.Deserialize<TextBlock>(str);
            }
            catch (Exception err)
            {
                errorString = err.Message;
            }

            return null;
        }

        /// <summary>
        ///     Defines <see cref="T:Assets.Scripts.Engine.FileSystem.TextBlock" /> attribute.
        /// </summary>
        public sealed class Attribute
        {
            internal string _name;
            internal string _value;

            internal Attribute()
            {
            }

            /// <summary>Gets the attribute name.</summary>
            public string Name
            {
                get { return _name; }
            }

            /// <summary>Gets the attribute value.</summary>
            public string Value
            {
                get { return _value; }
            }

            /// <summary>
            ///     Returns a string that represents the current attribute.
            /// </summary>
            /// <returns>A string that represents the current attribute.</returns>
            public override string ToString()
            {
                return string.Format("Name: \"{0}\", Value \"{1}\"", _name, _value);
            }
        }
    }
}