﻿/*

Copyright Robert Vesse 2009-10
rvesse@vdesign-studios.com

------------------------------------------------------------------------

This file is part of dotNetRDF.

dotNetRDF is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

dotNetRDF is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with dotNetRDF.  If not, see <http://www.gnu.org/licenses/>.

------------------------------------------------------------------------

dotNetRDF may alternatively be used under the LGPL or MIT License

http://www.gnu.org/licenses/lgpl.html
http://www.opensource.org/licenses/mit-license.php

If these licenses are not suitable for your intended use please contact
us at the above stated email address to discuss alternative
terms.

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VDS.RDF.Parsing;
using VDS.RDF.Query.Patterns;

namespace VDS.RDF.Query.Expressions
{
    #region Conditional Expressions

    /// <summary>
    /// Class representing Conditional Or expressions
    /// </summary>
    public class OrExpression : BaseBinaryExpression
    {
        /// <summary>
        /// Creates a new Conditional Or Expression
        /// </summary>
        /// <param name="leftExpr">Left Hand Expression</param>
        /// <param name="rightExpr">Right Hand Expression</param>
        public OrExpression(ISparqlExpression leftExpr, ISparqlExpression rightExpr) : base(leftExpr, rightExpr) { }

        /// <summary>
        /// Computes the Effective Boolean Value of this Expression as evaluated for a given Binding
        /// </summary>
        /// <param name="context">Evaluation Context</param>
        /// <param name="bindingID">Binding ID</param>
        /// <returns></returns>
        public override bool EffectiveBooleanValue(SparqlEvaluationContext context, int bindingID)
        {
            //Lazy Evaluation for efficiency
            try
            {
                bool leftResult = this._leftExpr.EffectiveBooleanValue(context, bindingID);
                if (leftResult)
                {
                    //If the LHS is true it doesn't matter about any subsequenct results
                    return true;
                }
                else
                {
                    //If the LHS is false then we have to evaluate the RHS
                    return this._rightExpr.EffectiveBooleanValue(context, bindingID);
                }
            }
            catch
            {
                //If there's an Error on the LHS we return true only if the RHS evaluates to true
                //Otherwise we throw the Error
                bool rightResult = this._rightExpr.EffectiveBooleanValue(context, bindingID);
                if (rightResult)
                {
                    return true;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the String representation of this Expression
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            if (this._leftExpr is IBinaryOperator)
            {
                output.Append("(" + this._leftExpr.ToString() + ")");
            }
            else
            {
                output.Append(this._leftExpr.ToString());
            }
            output.Append(" || ");
            if (this._rightExpr is IBinaryOperator)
            {
                output.Append("(" + this._rightExpr.ToString() + ")");
            }
            else
            {
                output.Append(this._rightExpr.ToString());
            }
            return output.ToString();
        }
    }

    /// <summary>
    /// Class representing Conditional And expressions
    /// </summary>
    public class AndExpression : BaseBinaryExpression
    {
        /// <summary>
        /// Creates a new Conditional And Expression
        /// </summary>
        /// <param name="leftExpr">Left Hand Expression</param>
        /// <param name="rightExpr">Right Hand Expression</param>
        public AndExpression(ISparqlExpression leftExpr, ISparqlExpression rightExpr) : base(leftExpr, rightExpr) { }

        /// <summary>
        /// Computes the Effective Boolean Value of this Expression as evaluated for a given Binding
        /// </summary>
        /// <param name="context">Evaluation Context</param>
        /// <param name="bindingID">Binding ID</param>
        /// <returns></returns>
        public override bool EffectiveBooleanValue(SparqlEvaluationContext context, int bindingID)
        {
            //Lazy Evaluation for Efficiency
            try
            {
                bool leftResult = this._leftExpr.EffectiveBooleanValue(context, bindingID);
                if (!leftResult)
                {
                    //If the LHS is false then no subsequent results matter
                    return false;
                }
                else
                {
                    //If the LHS is true then we have to continue by evaluating the RHS
                    return this._rightExpr.EffectiveBooleanValue(context, bindingID);
                }
            }
            catch
            {
                //If we encounter an error on the LHS then we return false only if the RHS is false
                //Otherwise we error
                bool rightResult = this._rightExpr.EffectiveBooleanValue(context, bindingID);
                if (!rightResult)
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the String representation of this Expression
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            if (this._leftExpr is IBinaryOperator)
            {
                output.Append("(" + this._leftExpr.ToString() + ")");
            }
            else
            {
                output.Append(this._leftExpr.ToString());
            }
            output.Append(" && ");
            if (this._rightExpr is IBinaryOperator)
            {
                output.Append("(" + this._rightExpr.ToString() + ")");
            }
            else
            {
                output.Append(this._rightExpr.ToString());
            }
            return output.ToString();
        }
    }

    /// <summary>
    /// Class representing Negation Expressions
    /// </summary>
    public class NegationExpression : BaseUnaryExpression
    {
        /// <summary>
        /// Creates a new Negation Expression
        /// </summary>
        /// <param name="expr">Expression to Negate</param>
        public NegationExpression(ISparqlExpression expr) : base(expr) { }

        /// <summary>
        /// Computes the Effective Boolean Value of this Expression as evaluated for a given Binding
        /// </summary>
        /// <param name="context">Evaluation Context</param>
        /// <param name="bindingID">Binding ID</param>
        /// <returns></returns>
        public override bool EffectiveBooleanValue(SparqlEvaluationContext context, int bindingID)
        {
            return !this._expr.EffectiveBooleanValue(context, bindingID);
        }

        /// <summary>
        /// Gets the String representation of this Expression
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "!" + this._expr.ToString();
        }
    }

    #endregion

    #region Relational Expressions

    /// <summary>
    /// Class representing Relational Equality expressions
    /// </summary>
    public class EqualsExpression : BaseBinaryExpression
    {
        /// <summary>
        /// Creates a new Equality Expression
        /// </summary>
        /// <param name="leftExpr">Left Hand Expression</param>
        /// <param name="rightExpr">Right Hand Expression</param>
        public EqualsExpression(ISparqlExpression leftExpr, ISparqlExpression rightExpr) : base(leftExpr, rightExpr) { }

        /// <summary>
        /// Computes the Effective Boolean Value of this Expression as evaluated for a given Binding
        /// </summary>
        /// <param name="context">Evaluation Context</param>
        /// <param name="bindingID">Binding ID</param>
        /// <returns></returns>
        public override bool EffectiveBooleanValue(SparqlEvaluationContext context, int bindingID)
        {
            INode x = this._leftExpr.Value(context, bindingID);
            INode y = this._rightExpr.Value(context, bindingID);

            return SparqlSpecsHelper.Equality(x, y);
        }

        /// <summary>
        /// Gets the String representation of this Expression
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            if (this._leftExpr is IBinaryOperator)
            {
                output.Append("(" + this._leftExpr.ToString() + ")");
            }
            else
            {
                output.Append(this._leftExpr.ToString());
            }
            output.Append(" = ");
            if (this._rightExpr is IBinaryOperator)
            {
                output.Append("(" + this._rightExpr.ToString() + ")");
            }
            else
            {
                output.Append(this._rightExpr.ToString());
            }
            return output.ToString();
        }
    }

    /// <summary>
    /// Class representing Relational Non-Equality expressions
    /// </summary>
    public class NotEqualsExpression : BaseBinaryExpression
    {
        /// <summary>
        /// Creates a new Non-Equality Expression
        /// </summary>
        /// <param name="leftExpr">Left Hand Expression</param>
        /// <param name="rightExpr">Right Hand Expression</param>
        public NotEqualsExpression(ISparqlExpression leftExpr, ISparqlExpression rightExpr) : base(leftExpr, rightExpr) { }

        /// <summary>
        /// Computes the Effective Boolean Value of this Expression as evaluated for a given Binding
        /// </summary>
        /// <param name="context">Evaluation Context</param>
        /// <param name="bindingID">Binding ID</param>
        /// <returns></returns>
        public override bool EffectiveBooleanValue(SparqlEvaluationContext context, int bindingID)
        {
            INode x = this._leftExpr.Value(context, bindingID);
            INode y = this._rightExpr.Value(context, bindingID);

            return SparqlSpecsHelper.Inequality(x, y);
        }

        /// <summary>
        /// Gets the String representation of this Expression
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            if (this._leftExpr is IBinaryOperator)
            {
                output.Append("(" + this._leftExpr.ToString() + ")");
            }
            else
            {
                output.Append(this._leftExpr.ToString());
            }
            output.Append(" != ");
            if (this._rightExpr is IBinaryOperator)
            {
                output.Append("(" + this._rightExpr.ToString() + ")");
            }
            else
            {
                output.Append(this._rightExpr.ToString());
            }
            return output.ToString();
        }
    }

    /// <summary>
    /// Class representing Relational Less Than Expressions
    /// </summary>
    public class LessThanExpression : BaseBinaryExpression
    {
        private SparqlNodeComparer _comparer = new SparqlNodeComparer();

        /// <summary>
        /// Creates a new Less Than Relational Expression
        /// </summary>
        /// <param name="leftExpr">Left Hand Expression</param>
        /// <param name="rightExpr">Right Hand Expression</param>
        public LessThanExpression(ISparqlExpression leftExpr, ISparqlExpression rightExpr) : base(leftExpr, rightExpr) { }

        /// <summary>
        /// Computes the Effective Boolean Value of this Expression as evaluated for a given Binding
        /// </summary>
        /// <param name="context">Evaluation Context</param>
        /// <param name="bindingID">Binding ID</param>
        /// <returns></returns>
        public override bool EffectiveBooleanValue(SparqlEvaluationContext context, int bindingID)
        {
            INode a, b;
            a = this._leftExpr.Value(context, bindingID);
            b = this._rightExpr.Value(context, bindingID);

            if (a == null) throw new RdfQueryException("Cannot evaluate a < when one argument is Null");
            int compare = this._comparer.Compare(a, b);// a.CompareTo(b);
            return (compare < 0);
        }

        /// <summary>
        /// Gets the String representation of this Expression
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            if (this._leftExpr is IBinaryOperator)
            {
                output.Append("(" + this._leftExpr.ToString() + ")");
            }
            else
            {
                output.Append(this._leftExpr.ToString());
            }
            output.Append(" < ");
            if (this._rightExpr is IBinaryOperator)
            {
                output.Append("(" + this._rightExpr.ToString() + ")");
            }
            else
            {
                output.Append(this._rightExpr.ToString());
            }
            return output.ToString();
        }
    }

    /// <summary>
    /// Class representing Relational Less Than or Equal To Expressions
    /// </summary>
    public class LessThanOrEqualToExpression : BaseBinaryExpression
    {
        private SparqlNodeComparer _comparer = new SparqlNodeComparer();

        /// <summary>
        /// Creates a new Less Than or Equal To Relational Expression
        /// </summary>
        /// <param name="leftExpr">Left Hand Expression</param>
        /// <param name="rightExpr">Right Hand Expression</param>
        public LessThanOrEqualToExpression(ISparqlExpression leftExpr, ISparqlExpression rightExpr) : base(leftExpr, rightExpr) { }

        /// <summary>
        /// Computes the Effective Boolean Value of this Expression as evaluated for a given Binding
        /// </summary>
        /// <param name="context">Evaluation Context</param>
        /// <param name="bindingID">Binding ID</param>
        /// <returns></returns>
        public override bool EffectiveBooleanValue(SparqlEvaluationContext context, int bindingID)
        {
            INode a, b;
            a = this._leftExpr.Value(context, bindingID);
            b = this._rightExpr.Value(context, bindingID);

            if (a == null)
            {
                if (b == null)
                {
                    return true;
                }
                else
                {
                    throw new RdfQueryException("Cannot evaluate a <= when one argument is a Null");
                }
            }

            int compare = this._comparer.Compare(a, b);// a.CompareTo(b);
            return (compare <= 0);
        }

        /// <summary>
        /// Gets the String representation of this Expression
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            if (this._leftExpr is IBinaryOperator)
            {
                output.Append("(" + this._leftExpr.ToString() + ")");
            }
            else
            {
                output.Append(this._leftExpr.ToString());
            }
            output.Append(" <= ");
            if (this._rightExpr is IBinaryOperator)
            {
                output.Append("(" + this._rightExpr.ToString() + ")");
            }
            else
            {
                output.Append(this._rightExpr.ToString());
            }
            return output.ToString();
        }
    }

    /// <summary>
    /// Class representing Relational Greater Than Expressions
    /// </summary>
    public class GreaterThanExpression : BaseBinaryExpression
    {
        private SparqlNodeComparer _comparer = new SparqlNodeComparer();

        /// <summary>
        /// Creates a new Greater Than Relational Expression
        /// </summary>
        /// <param name="leftExpr">Left Hand Expression</param>
        /// <param name="rightExpr">Right Hand Expression</param>
        public GreaterThanExpression(ISparqlExpression leftExpr, ISparqlExpression rightExpr) : base(leftExpr, rightExpr) { }

        /// <summary>
        /// Computes the Effective Boolean Value of this Expression as evaluated for a given Binding
        /// </summary>
        /// <param name="context">Evaluation Context</param>
        /// <param name="bindingID">Binding ID</param>
        /// <returns></returns>
        public override bool EffectiveBooleanValue(SparqlEvaluationContext context, int bindingID)
        {
            INode a, b;
            a = this._leftExpr.Value(context, bindingID);
            b = this._rightExpr.Value(context, bindingID);

            if (a == null) throw new RdfQueryException("Cannot evaluate a > when one argument is Null");

            int compare = this._comparer.Compare(a, b);//a.CompareTo(b);
            return (compare > 0);
        }

        /// <summary>
        /// Gets the String representation of this Expression
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            if (this._leftExpr is IBinaryOperator)
            {
                output.Append("(" + this._leftExpr.ToString() + ")");
            }
            else
            {
                output.Append(this._leftExpr.ToString());
            }
            output.Append(" > ");
            if (this._rightExpr is IBinaryOperator)
            {
                output.Append("(" + this._rightExpr.ToString() + ")");
            }
            else
            {
                output.Append(this._rightExpr.ToString());
            }
            return output.ToString();
        }
    }

    /// <summary>
    /// Class representing Relational Greater Than or Equal To Expressions
    /// </summary>
    public class GreaterThanOrEqualToExpression : BaseBinaryExpression
    {
        private SparqlNodeComparer _comparer = new SparqlNodeComparer();

        /// <summary>
        /// Creates a new Greater Than or Equal To Relational Expression
        /// </summary>
        /// <param name="leftExpr">Left Hand Expression</param>
        /// <param name="rightExpr">Right Hand Expression</param>
        public GreaterThanOrEqualToExpression(ISparqlExpression leftExpr, ISparqlExpression rightExpr) : base(leftExpr, rightExpr) { }

        /// <summary>
        /// Computes the Effective Boolean Value of this Expression as evaluated for a given Binding
        /// </summary>
        /// <param name="context">Evaluation Context</param>
        /// <param name="bindingID">Binding ID</param>
        /// <returns></returns>
        public override bool EffectiveBooleanValue(SparqlEvaluationContext context, int bindingID)
        {
            INode a, b;
            a = this._leftExpr.Value(context, bindingID);
            b = this._rightExpr.Value(context, bindingID);

            if (a == null)
            {
                if (b == null)
                {
                    return true;
                }
                else
                {
                    throw new RdfQueryException("Cannot evaluate a >= when one argument is null");
                }
            }

            int compare = this._comparer.Compare(a, b);// a.CompareTo(b);
            return (compare >= 0);
        }

        /// <summary>
        /// Gets the String representation of this Expression
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            if (this._leftExpr is IBinaryOperator)
            {
                output.Append("(" + this._leftExpr.ToString() + ")");
            }
            else
            {
                output.Append(this._leftExpr.ToString());
            }
            output.Append(" >= ");
            if (this._rightExpr is IBinaryOperator)
            {
                output.Append("(" + this._rightExpr.ToString() + ")");
            }
            else
            {
                output.Append(this._rightExpr.ToString());
            }
            return output.ToString();
        }
    }

    #endregion

    /// <summary>
    /// Represents a Null/Unknown Expression
    /// </summary>
    /// <remarks>
    /// <para>
    /// Used as a placeholder for expressions that cannot be parsed since the Uri of the function is not known and no registered <see cref="ISparqlCustomExpressionFactory">ISparqlCustomExpressionFactory</see> is capable of turning it into a valid <see cref="ISparqlExpression">ISparqlExpression</see>
    /// </para>
    /// </remarks>
    public class NullExpression : ISparqlExpression
    {
        /// <summary>
        /// Gets a Null since this expression represents Null or an Unknown expression (whose value cannot be evaluated)
        /// </summary>
        /// <param name="context">Evaluation Context</param>
        /// <param name="bindingID">Binding ID</param>
        /// <returns></returns>
        public INode Value(SparqlEvaluationContext context, int bindingID)
        {
            return null;
        }

        /// <summary>
        /// Computes the Effective Boolean Value of this Expression as evaluated for a given Binding
        /// </summary>
        /// <param name="context">Evaluation Context</param>
        /// <param name="bindingID">Binding ID</param>
        /// <returns></returns>
        public bool EffectiveBooleanValue(SparqlEvaluationContext context, int bindingID)
        {
            return false;
        }

        /// <summary>
        /// Gets the String representation of this Expression
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Empty;
        }

        /// <summary>
        /// Gets an Empty enumerable since a Null expression term doesn't use variables
        /// </summary>
        public IEnumerable<String> Variables
        {
            get
            {
                return Enumerable.Empty<String>();
            }
        }
    }
}