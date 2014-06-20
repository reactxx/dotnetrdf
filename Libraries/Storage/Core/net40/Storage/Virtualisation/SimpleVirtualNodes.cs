/*
dotNetRDF is free and open source software licensed under the MIT License

-----------------------------------------------------------------------------

Copyright (c) 2009-2012 dotNetRDF Project (dotnetrdf-developer@lists.sf.net)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is furnished
to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;

namespace VDS.RDF.Storage.Virtualisation
{
    /// <summary>
    /// Simple implementation of a Virtual Blank Node where the virtual IDs are integers
    /// </summary>
    public class SimpleVirtualBlankNode
        : BaseVirtualBlankNode<int, int>, IEquatable<SimpleVirtualBlankNode>, IComparable<SimpleVirtualBlankNode>
    {
        /// <summary>
        /// Creates a new Virtual Blank Node
        /// </summary>
        /// <param name="id">Virtual ID</param>
        /// <param name="provider">Virtual RDF Provider</param>
        public SimpleVirtualBlankNode(int id, IVirtualRdfProvider<int, int> provider)
            : base(id, provider) { }

        /// <summary>
        /// Creates a new Virtual Blank Node
        /// </summary>
        /// <param name="id">Virtual ID</param>
        /// <param name="provider">Virtual RDF Provider</param>
        /// <param name="value">Materialised Value</param>
        public SimpleVirtualBlankNode(int id, IVirtualRdfProvider<int, int> provider, IBlankNode value)
            : base(id, provider, value) { }

        /// <summary>
        /// Determines whether this Node is equal to another virtual Blank node
        /// </summary>
        /// <param name="other">Other Blank Node</param>
        /// <returns></returns>
        public bool Equals(SimpleVirtualBlankNode other)
        {
            return this.Equals((IBlankNode)other);
        }

        /// <summary>
        /// Compares this Node to another virtual Blank node
        /// </summary>
        /// <param name="other">Other Blank Node</param>
        /// <returns></returns>
        public int CompareTo(SimpleVirtualBlankNode other)
        {
            return this.CompareTo((IBlankNode)other);
        }

        /// <summary>
        /// Method to be implemented in derived classes to provide comparison of VirtualId values
        /// </summary>
        /// <param name="other">The other virtual ID value to be compared with this node's virtual ID value.</param>
        /// <returns>The comparison result.</returns>
        public override int CompareVirtualId(int other)
        {
            return VirtualID.CompareTo(other);
        }
    }

    /// <summary>
    /// Simple implementation of a Virtual Graph Literal Node where the virtual IDs are integers
    /// </summary>
    public class SimpleVirtualGraphLiteralNode
        : BaseVirtualGraphLiteralNode<int, int>, IEquatable<SimpleVirtualGraphLiteralNode>, IComparable<SimpleVirtualGraphLiteralNode>
    {
        /// <summary>
        /// Creates a new Virtual Graph Literal Node
        /// </summary>
        /// <param name="id">Virtual ID</param>
        /// <param name="provider">Virtual RDF Provider</param>
        public SimpleVirtualGraphLiteralNode(int id, IVirtualRdfProvider<int, int> provider)
            : base(id, provider) { }

        /// <summary>
        /// Creates a new Virtual Graph Literal Node
        /// </summary>
        /// <param name="id">Virtual ID</param>
        /// <param name="provider">Virtual RDF Provider</param>
        /// <param name="value">Materialised Values</param>
        public SimpleVirtualGraphLiteralNode(int id, IVirtualRdfProvider<int, int> provider, IGraphLiteralNode value)
            : base(id, provider, value) { }

        /// <summary>
        /// Determines whether this Node is equal to another virtual Graph Literal node
        /// </summary>
        /// <param name="other">Other Graph Literal Node</param>
        /// <returns></returns>
        public bool Equals(SimpleVirtualGraphLiteralNode other)
        {
            return this.Equals((IGraphLiteralNode)other);
        }

        /// <summary>
        /// Compares this Node to another virtual Graph Literal node
        /// </summary>
        /// <param name="other">Other Graph Literal Node</param>
        /// <returns></returns>
        public int CompareTo(SimpleVirtualGraphLiteralNode other)
        {
            return this.CompareTo((IGraphLiteralNode)other);
        }

        /// <summary>
        /// Method to be implemented in derived classes to provide comparison of VirtualId values
        /// </summary>
        /// <param name="other">The other virtual ID value to be compared with this node's virtual ID value.</param>
        /// <returns>The comparison result.</returns>
        public override int CompareVirtualId(int other)
        {
            return VirtualID.CompareTo(other);
        }
    }

    /// <summary>
    /// Simple implementation of a Virtual Literal Node where the virtual IDs are integers
    /// </summary>
    public class SimpleVirtualLiteralNode
        : BaseVirtualLiteralNode<int, int>, IEquatable<SimpleVirtualLiteralNode>, IComparable<SimpleVirtualLiteralNode>
    {
        /// <summary>
        /// Creates a new Virtual Literal Node
        /// </summary>
        /// <param name="id">Virtual ID</param>
        /// <param name="provider">Virtual RDF Provider</param>
        public SimpleVirtualLiteralNode(int id, IVirtualRdfProvider<int, int> provider)
            : base(id, provider) { }

        /// <summary>
        /// Creates a new Virtual Literal Node
        /// </summary>
        /// <param name="id">Virtual ID</param>
        /// <param name="provider">Virtual RDF Provider</param>
        /// <param name="value">Materialised Value</param>
        public SimpleVirtualLiteralNode(int id, IVirtualRdfProvider<int, int> provider, ILiteralNode value)
            : base(id, provider, value) { }

        /// <summary>
        /// Determines whether this Node is equal to another virtual Literal node
        /// </summary>
        /// <param name="other">Other Literal Node</param>
        /// <returns></returns>
        public bool Equals(SimpleVirtualLiteralNode other)
        {
            return this.Equals((ILiteralNode)other);
        }

        /// <summary>
        /// Compares this Node to another virtual Literal node
        /// </summary>
        /// <param name="other">Other Literal Node</param>
        /// <returns></returns>
        public int CompareTo(SimpleVirtualLiteralNode other)
        {
            return this.CompareTo((ILiteralNode)other);
        }

        /// <summary>
        /// Method to be implemented in derived classes to provide comparison of VirtualId values
        /// </summary>
        /// <param name="other">The other virtual ID value to be compared with this node's virtual ID value.</param>
        /// <returns>The comparison result.</returns>
        public override int CompareVirtualId(int other)
        {
            return VirtualID.CompareTo(other);
        }
    }

    /// <summary>
    /// Simple implementation of a Virtual URI Node where the virtual IDs are integers
    /// </summary>
    public class SimpleVirtualUriNode
        : BaseVirtualUriNode<int, int>, IEquatable<SimpleVirtualUriNode>, IComparable<SimpleVirtualUriNode>
    {
        /// <summary>
        /// Creates a new Virtual URI Node
        /// </summary>
        /// <param name="id">Virtual ID</param>
        /// <param name="provider">Virtual RDF Provider</param>
        public SimpleVirtualUriNode(int id, IVirtualRdfProvider<int, int> provider)
            : base(id, provider) { }

        /// <summary>
        /// Creates a new Virtual URI Node
        /// </summary>
        /// <param name="id">Virtual ID</param>
        /// <param name="provider">Virtual RDF Provider</param>
        /// <param name="value">Materialised Value</param>
        public SimpleVirtualUriNode(int id, IVirtualRdfProvider<int, int> provider, IUriNode value)
            : base(id, provider, value) { }

        /// <summary>
        /// Determines whether this Node is equal to another virtual URI node
        /// </summary>
        /// <param name="other">Other URI Node</param>
        /// <returns></returns>
        public bool Equals(SimpleVirtualUriNode other)
        {
            return this.Equals((IUriNode)other);
        }

        /// <summary>
        /// Compares this Node to another virtual URI node
        /// </summary>
        /// <param name="other">Other URI Node</param>
        /// <returns></returns>
        public int CompareTo(SimpleVirtualUriNode other)
        {
            return this.CompareTo((IUriNode)other);
        }

        /// <summary>
        /// Method to be implemented in derived classes to provide comparison of VirtualId values
        /// </summary>
        /// <param name="other">The other virtual ID value to be compared with this node's virtual ID value.</param>
        /// <returns>The comparison result.</returns>
        public override int CompareVirtualId(int other)
        {
            return VirtualID.CompareTo(other);
        }
    }

    /// <summary>
    /// Simple implementation of a Virtual URI Node where the virtual IDs are integers
    /// </summary>
    public class SimpleVirtualVariableNode
        : BaseVirtualVariableNode<int, int>, IEquatable<SimpleVirtualVariableNode>, IComparable<SimpleVirtualVariableNode>
    {
        /// <summary>
        /// Creates a new Virtual Variable Node
        /// </summary>
        /// <param name="id">Virtual ID</param>
        /// <param name="provider">Virtual RDF Provider</param>
        public SimpleVirtualVariableNode(int id, IVirtualRdfProvider<int, int> provider)
            : base(id, provider) { }

        /// <summary>
        /// Creates a new Virtual Variable Node
        /// </summary>
        /// <param name="id">Virtual ID</param>
        /// <param name="provider">Virtual RDF Provider</param>
        /// <param name="value">Materialised Value</param>
        public SimpleVirtualVariableNode(int id, IVirtualRdfProvider<int, int> provider, IVariableNode value)
            : base(id, provider, value) { }

        /// <summary>
        /// Determines whether this Node is equal to another virtual variable node
        /// </summary>
        /// <param name="other">Other Variable Node</param>
        /// <returns></returns>
        public bool Equals(SimpleVirtualVariableNode other)
        {
            return this.Equals((IVariableNode)other);
        }

        /// <summary>
        /// Compares this Node to another virtual Variable node
        /// </summary>
        /// <param name="other">Other Variable Node</param>
        /// <returns></returns>
        public int CompareTo(SimpleVirtualVariableNode other)
        {
            return this.CompareTo((IVariableNode)other);
        }
        }

        /// <summary>
        /// Method to be implemented in derived classes to provide comparison of VirtualId values
        /// </summary>
        /// <param name="other">The other virtual ID value to be compared with this node's virtual ID value.</param>
        /// <returns>The comparison result.</returns>
        public override int CompareVirtualId(int other)
        {
            return VirtualID.CompareTo(other);
    }
}
