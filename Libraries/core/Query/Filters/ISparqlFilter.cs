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
using VDS.RDF.Query.Expressions;

namespace VDS.RDF.Query.Filters
{
    /// <summary>
    /// Indicates when a Filter is applied
    /// </summary>
    public enum FilterApplicationMode
    {
        /// <summary>
        /// Filter is applied prior to Bindings being committed
        /// </summary>
        /// <remarks>
        /// Means the Filter operates over the temporary bindings
        /// </remarks>
        PreCommit,
        /// <summary>
        /// Filter is applied after Bindings are committed
        /// </summary>
        /// <remarks>
        /// Means the Filter operates over the current set of 'valid' bindings
        /// </remarks>
        PostCommit
    }

    /// <summary>
    /// Interface for Classes which implement SPARQL Filter Functions
    /// </summary>
    public interface ISparqlFilter
    {
        /// <summary>
        /// Evaluates a Filter in the given Evaluation Context
        /// </summary>
        /// <param name="context">Evaluation Context</param>
        void Evaluate(SparqlEvaluationContext context);

        /// <summary>
        /// Gets the enumeration of Variables that are used in the Filter
        /// </summary>
        IEnumerable<String> Variables
        {
            get;
        }

        /// <summary>
        /// Gets the Expression that this Filter uses
        /// </summary>
        ISparqlExpression Expression
        {
            get;
        }
    }
}
