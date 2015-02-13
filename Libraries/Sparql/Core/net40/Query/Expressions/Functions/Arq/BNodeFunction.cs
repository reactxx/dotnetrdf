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

using VDS.RDF.Nodes;
using VDS.RDF.Query.Engine;
using VDS.RDF.Query.Expressions.Factories;

namespace VDS.RDF.Query.Expressions.Functions.Arq
{
    /// <summary>
    /// Represents the ARQ afn:bnode() function
    /// </summary>
    public class BNodeFunction
        : BaseUnaryExpression
    {
        /// <summary>
        /// Creates a new ARQ afn:bnode() function
        /// </summary>
        /// <param name="expr">Expression</param>
        public BNodeFunction(IExpression expr)
            : base(expr) { }

        public override IExpression Copy(IExpression argument)
        {
            return new BNodeFunction(argument);
        }

        /// <summary>
        /// Gets the value of the function in the given Evaluation Context for the given Binding ID
        /// </summary>
        /// <param name="context">Evaluation Context</param>
        /// <param name="bindingID">Binding ID</param>
        /// <returns></returns>
        public override IValuedNode Evaluate(ISolution solution, IExpressionContext context)
        {
            INode temp = this.Argument.Evaluate(solution, context);
            if (temp != null)
            {
                if (temp.NodeType == NodeType.Blank)
                {
                    return new StringNode(temp.AnonID.ToString());
                }
                throw new RdfQueryException("Cannot find the BNode Label for a non-Blank Node");
            }
            throw new RdfQueryException("Cannot find the BNode Label for a null");
        }

        /// <summary>
        /// Gets the Functor of the Expression
        /// </summary>
        public override string Functor
        {
            get
            {
                return ArqFunctionFactory.ArqFunctionsNamespace + ArqFunctionFactory.BNode;
            }
        }
    }
}