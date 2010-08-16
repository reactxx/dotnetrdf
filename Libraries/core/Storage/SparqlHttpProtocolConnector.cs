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

#if !NO_STORAGE

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using VDS.RDF.Configuration;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;

namespace VDS.RDF.Storage
{
    /// <summary>
    /// Class for connecting to any store that implements the SPARQL Uniform HTTP Protocol for Managing Graphs
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <a href="http://www.w3.org/TR/sparql11-http-rdf-update/">SPARQL Uniform HTTP Protocol for Managing Graphs</a> is defined as part of SPARQL 1.1 and is currently a working draft so implementations are not guaranteed to be fully compliant with the draft and the protocol may change in the future.
    /// </para>
    /// <para>
    /// <strong>Note:</strong> While this connector supports the update of a Graph the Uniform HTTP Protocol only allows for the addition of data to an existing Graph and not the removal of data, therefore any calls to <see cref="SparqlHttpProtocolConnector.UpdateGraph">UpdateGraph()</see> that would require the removal of Triple(s) will result in an error.
    /// </para>
    /// </remarks>
    public class SparqlHttpProtocolConnector : IGenericIOManager, IConfigurationSerializable
    {
        private String _serviceUri;

        /// <summary>
        /// Creates a new SPARQL Uniform HTTP Protocol Connector
        /// </summary>
        /// <param name="serviceUri">URI of the Protocol Server</param>
        public SparqlHttpProtocolConnector(Uri serviceUri)
        {
            if (serviceUri == null) throw new ArgumentNullException("serviceUri", "Cannot create a connection to a Uniform HTTP Protocol store if the Service URI is null");

            this._serviceUri = serviceUri.ToString();
        }

        /// <summary>
        /// Loads a Graph from the Protocol Server
        /// </summary>
        /// <param name="g">Graph to load into</param>
        /// <param name="graphUri">URI of the Graph to load</param>
        public void LoadGraph(IGraph g, Uri graphUri)
        {
            String u = (graphUri == null) ? String.Empty : graphUri.ToString();
            this.LoadGraph(g, u);
        }

        /// <summary>
        /// Loads a Graph from the Protocol Server
        /// </summary>
        /// <param name="g">Graph to load into</param>
        /// <param name="graphUri">URI of the Graph to load</param>
        public void LoadGraph(IGraph g, string graphUri)
        {
            String retrievalUri = this._serviceUri;
            if (!graphUri.Equals(String.Empty)) retrievalUri += "?graph=" + Uri.EscapeDataString(graphUri);
            UriLoader.Load(g, new Uri(retrievalUri));
        }

        /// <summary>
        /// Saves a Graph to the Protocol Server
        /// </summary>
        /// <param name="g">Graph to save</param>
        public void SaveGraph(IGraph g)
        {
            String saveUri = this._serviceUri;
            if (g.BaseUri != null) 
            {
                saveUri += "?graph=" + Uri.EscapeDataString(g.BaseUri.ToString());
            }
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(saveUri));
                request.Method = "PUT";
                request.ContentType = MimeTypesHelper.RdfXml[0];
                FastRdfXmlWriter writer = new FastRdfXmlWriter();
                writer.Save(g, new StreamWriter(request.GetRequestStream()));

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //If we get then it was OK
                response.Close();
            }
            catch (WebException webEx)
            {
                throw new RdfStorageException("A HTTP Error occurred while trying to save a Graph to the Store", webEx);
            }

        }

        /// <summary>
        /// Updates a Graph on the Protocol Server
        /// </summary>
        /// <param name="graphUri">URI of the Graph to update</param>
        /// <param name="additions">Triples to be added</param>
        /// <param name="removals">Triples to be removed</param>
        /// <remarks>
        /// <strong>Note:</strong> The SPARQL Uniform HTTP Protocol for Graph Management only supports the addition of Triples to a Graph and does not support removal of Triples from a Graph.  If you attempt to remove Triples then an <see cref="RdfStorageException">RdfStorageException</see> will be thrown
        /// </remarks>
        public void UpdateGraph(Uri graphUri, IEnumerable<Triple> additions, IEnumerable<Triple> removals)
        {
            String u = (graphUri == null) ? String.Empty : graphUri.ToString();
            this.UpdateGraph(u, additions, removals);
        }

        /// <summary>
        /// Updates a Graph on the Protocol Server
        /// </summary>
        /// <param name="graphUri">URI of the Graph to update</param>
        /// <param name="additions">Triples to be added</param>
        /// <param name="removals">Triples to be removed</param>
        /// <remarks>
        /// <strong>Note:</strong> The SPARQL Uniform HTTP Protocol for Graph Management only supports the addition of Triples to a Graph and does not support removal of Triples from a Graph.  If you attempt to remove Triples then an <see cref="RdfStorageException">RdfStorageException</see> will be thrown
        /// </remarks>
        public void UpdateGraph(string graphUri, IEnumerable<Triple> additions, IEnumerable<Triple> removals)
        {
            if (removals.Any()) throw new RdfStorageException("Unable to Update a Graph since this update requests that Triples be removed from the Graph which the SPARQL Uniform HTTP Protocol for Graph Management does not support");

            String updateUri = this._serviceUri;
            if (!graphUri.Equals(String.Empty))
            {
                updateUri += "?graph=" + Uri.EscapeDataString(graphUri);
            }

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(updateUri));
                request.Method = "POST";
                request.ContentType = MimeTypesHelper.RdfXml[0];
                FastRdfXmlWriter writer = new FastRdfXmlWriter();
                Graph g = new Graph();
                g.Assert(additions);
                writer.Save(g, new StreamWriter(request.GetRequestStream()));

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //If we get then it was OK
                response.Close();
            }
            catch (WebException webEx)
            {
                throw new RdfStorageException("A HTTP Error occurred while trying to update a Graph in the Store", webEx);
            }
        }

        /// <summary>
        /// Gets that Updates are supported
        /// </summary>
        public bool UpdateSupported
        {
            get 
            {
                return true;
            }
        }

        /// <summary>
        /// Gets that the Store is ready
        /// </summary>
        public bool IsReady
        {
            get 
            {
                return true;
            }
        }

        /// <summary>
        /// Gets that the Store is not read-only
        /// </summary>
        public bool IsReadOnly
        {
            get 
            {
                return false;
            }
        }

        /// <summary>
        /// Disposes of the Connection
        /// </summary>
        public void Dispose()
        {
            //Nothing to dispose of
        }

        /// <summary>
        /// Gets a String representation of the connection
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "[SPARQL Uniform HTTP Protocol] " + this._serviceUri;
        }


        /// <summary>
        /// Serializes the connection's configuration
        /// </summary>
        /// <param name="context">Configuration Serialization Context</param>
        public void SerializeConfiguration(ConfigurationSerializationContext context)
        {
            INode manager = context.NextSubject;
            INode rdfType = context.Graph.CreateUriNode(new Uri(RdfSpecsHelper.RdfType));
            INode rdfsLabel = context.Graph.CreateUriNode(new Uri(NamespaceMapper.RDFS + "label"));
            INode dnrType = ConfigurationLoader.CreateConfigurationNode(context.Graph, ConfigurationLoader.PropertyType);
            INode genericManager = ConfigurationLoader.CreateConfigurationNode(context.Graph, ConfigurationLoader.ClassGenericManager);
            INode server = ConfigurationLoader.CreateConfigurationNode(context.Graph, ConfigurationLoader.PropertyServer);

            context.Graph.Assert(new Triple(manager, rdfType, genericManager));
            context.Graph.Assert(new Triple(manager, rdfsLabel, context.Graph.CreateLiteralNode(this.ToString())));
            context.Graph.Assert(new Triple(manager, dnrType, context.Graph.CreateLiteralNode(this.GetType().FullName)));
            context.Graph.Assert(new Triple(manager, server, context.Graph.CreateLiteralNode(this._serviceUri)));
        }
    }
}

#endif