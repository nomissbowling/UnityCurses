using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Engine.FileSystem
{
    /// <summary>
    ///     The class allows to store the text information in the hierarchical form.
    ///     Supports creation of children and attributes.
    /// </summary>
    public class TextBlock
    {
        private string L;
        private ReadOnlyCollection<TextBlock> m;
        private List<TextBlock> M = new List<TextBlock>();
        private ReadOnlyCollection<Attribute> n;
        private List<Attribute> N = new List<Attribute>();

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
            m = new ReadOnlyCollection<TextBlock>(M);
            n = new ReadOnlyCollection<Attribute>(N);
        }

        /// <summary>Gets the parent block.</summary>
        public TextBlock Parent { get; private set; }

        /// <summary>Gets or set block name.</summary>
        public string Name
        {
            get { return L; }
            set
            {
                if (L == value)
                    return;

                L = value;
                if (!string.IsNullOrEmpty(L))
                    return;

                Debug.LogError("TextBlock: set Name: \"name\" is null or empty.");
            }
        }

        /// <summary>Gets or set block string data.</summary>
        public string Data { get; set; }

        /// <summary>Gets the children collection.</summary>
        public IList<TextBlock> Children
        {
            get { return m; }
        }

        /// <summary>Gets the attributes collection.</summary>
        public IList<Attribute> Attributes
        {
            get { return n; }
        }

        /// <summary>Finds child block by name.</summary>
        /// <param name="name">The block name.</param>
        /// <returns>
        ///     <see cref="T:Assets.Scripts.Engine.FileSystem.TextBlock" /> if the block has been exists; otherwise,
        ///     <b>null</b>.
        /// </returns>
        public TextBlock FindChild(string name)
        {
            for (var index = 0; index < M.Count; ++index)
            {
                var textBlock = M[index];
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
            var textBlock = new TextBlock();
            textBlock.Parent = this;
            textBlock.L = name;
            textBlock.Data = data;
            M.Add(textBlock);
            return textBlock;
        }

        /// <summary>Creates the child block.</summary>
        /// <param name="name">The block name.</param>
        /// <returns>The child block.</returns>
        /// <remarks>Names of blocks can repeat.</remarks>
        public TextBlock AddChild(string name)
        {
            return AddChild(name, "");
        }

        /// <summary>Deletes child block.</summary>
        /// <param name="child">The child block.</param>
        public void DeleteChild(TextBlock child)
        {
            M.Remove(child);
            child.Parent = null;
            child.L = "";
            child.Data = "";
            child.M = null;
            child.N = null;
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
            M.Add(child);
        }

        /// <summary>Detaches child block without deleting.</summary>
        /// <param name="child">The child block.</param>
        public void DetachChild(TextBlock child)
        {
            M.Remove(child);
            child.Parent = null;
        }

        /// <summary>Returns the attribute value by name.</summary>
        /// <param name="name">The attribute name.</param>
        /// <param name="defaultValue">Default value. If the attribute does not exist that this value will return.</param>
        /// <returns>The attribute value if the attribute exists; otherwise, default value.</returns>
        public string GetAttribute(string name, string defaultValue)
        {
            for (var index = 0; index < N.Count; ++index)
            {
                var attribute = N[index];
                if (attribute.Name == name)
                    return attribute.Value;
            }
            return defaultValue;
        }

        /// <summary>Returns the attribute value by name.</summary>
        /// <param name="name">The attribute name.</param>
        /// <returns>The attribute value if the attribute exists; otherwise, empty string.</returns>
        public string GetAttribute(string name)
        {
            return GetAttribute(name, "");
        }

        /// <summary>Checks existence of attribute.</summary>
        /// <param name="name">The attribute name.</param>
        /// <returns><b>true</b> if the block exists; otherwise, <b>false</b>.</returns>
        public bool IsAttributeExist(string name)
        {
            for (var index = 0; index < N.Count; ++index)
                if (N[index].Name == name)
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
            for (var index = 0; index < N.Count; ++index)
            {
                var attribute = N[index];
                if (attribute.Name == name)
                {
                    attribute.ai = value;
                    return;
                }
            }
            N.Add(new Attribute
            {
                aI = name,
                ai = value
            });
        }

        /// <summary>Deletes attribute if he exists.</summary>
        /// <param name="name">The attribute name.</param>
        public void DeleteAttribute(string name)
        {
            for (var index = 0; index < N.Count; ++index)
                if (name == N[index].aI)
                {
                    var attribute = N[index];
                    var str1 = "";
                    attribute.aI = str1;
                    var str2 = "";
                    attribute.ai = str2;
                    N.RemoveAt(index);
                    break;
                }
        }

        /// <summary>Deletes all attributes.</summary>
        public void DeleteAllAttributes()
        {
            N.Clear();
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
            var stringBuilder = new StringBuilder();
            return stringBuilder.ToString();
        }

        /// <summary>
        ///     Returns a string that represents the current text block.
        /// </summary>
        /// <returns>A string that represents the current text block.</returns>
        public override string ToString()
        {
            var str = string.Format("Name: \"{0}\"", L);
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
            return E.Parse(str, out errorString);
        }

        /// <summary>
        ///     Defines <see cref="T:Assets.Scripts.Engine.FileSystem.TextBlock" /> attribute.
        /// </summary>
        public sealed class Attribute
        {
            internal string ai;
            internal string aI;

            internal Attribute()
            {
            }

            /// <summary>Gets the attribute name.</summary>
            public string Name
            {
                get { return aI; }
            }

            /// <summary>Gets the attribute value.</summary>
            public string Value
            {
                get { return ai; }
            }

            /// <summary>
            ///     Returns a string that represents the current attribute.
            /// </summary>
            /// <returns>A string that represents the current attribute.</returns>
            public override string ToString()
            {
                return string.Format("Name: \"{0}\", Value \"{1}\"", aI, ai);
            }
        }
    }
}