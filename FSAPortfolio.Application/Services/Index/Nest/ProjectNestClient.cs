using Elasticsearch.Net;
using FSAPortfolio.Application.Models;
using FSAPortfolio.Application.Services.Index.Models;
using Nest;
using Nest.JsonNetSerializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Application.Services.Index.Nest
{
    public class ProjectNestClient
    {
        private static Lazy<string> indexServerUri;
        static ProjectNestClient()
        {
            indexServerUri = new Lazy<string>(() => ConfigurationManager.AppSettings[$"elasticsearch.{nameof(indexServerUri)}"] ?? "http://localhost:9200");
        }

        public async Task<object> GetStatusAsync()
        {
            var elasticClient = new ElasticClient(getConnectionSettings(indexServerUri.Value));
            return await elasticClient.Cluster.HealthAsync();
        }

        public async Task<CreateIndexResponse> CreateProjectIndexAsync()
        {
            var elasticClient = new ElasticClient(getConnectionSettings(indexServerUri.Value));
            await elasticClient.Indices.DeleteAsync("projects");
            return await elasticClient.Indices.CreateAsync("projects", getIndexDescriptor());
        }

        public async Task<IndexResponse> IndexProjectAsync(ProjectSearchIndexModel project)
        {
            var elasticClient = new ElasticClient(getConnectionSettings(indexServerUri.Value));
            var response = await elasticClient.IndexDocumentAsync(project);
            return response;
        }

        public async Task<IEnumerable<ProjectSearchIndexModel>> SearchProjectIndex(string term)
        {
            var elasticClient = new ElasticClient(getConnectionSettings(indexServerUri.Value));
            var searchResponse = await elasticClient.SearchAsync<ProjectSearchIndexModel>(s => s
            .Query(q => q.MultiMatch(c => c
                .Fields(f => f
                    .Field(p => p.project_name)
                    .Field(p => p.short_desc)
                    .Field(p => p.forward_look)
                    .Field(p => p.rag))
                .Query(term).Fuzziness(Fuzziness.EditDistance(1)) // TODO: configure fuzziness
            )));

            return searchResponse.Documents;
        }

        internal async Task<DeleteResponse> DeleteProjectAsync(string projectId)
        {
            var elasticClient = new ElasticClient(getConnectionSettings(indexServerUri.Value));
            var deleteResponse = await elasticClient.DeleteAsync<ProjectSearchIndexModel>(projectId);
            return deleteResponse;
        }

        private Func<CreateIndexDescriptor, ICreateIndexRequest> getIndexDescriptor()
        {
            return c => c.Map<ProjectSearchIndexModel>(m => m.Properties(getProjectViewModelProperties()));
        }


        private ConnectionSettings getConnectionSettings(string uriSetting)
        {
            var uri = new Uri(uriSetting);
            var pool = new SingleNodeConnectionPool(uri);
            var connection = new HttpConnection();
            IPropertyMappingProvider propertyMappingProvider = new IgnoreDataMemberPropertyMappingProvider();

            ConnectionSettings.SourceSerializerFactory sourceSerializerFactory = (serializer, values) => new ProjectSerializer(serializer, values);

            var settings = new ConnectionSettings(
                pool,
                connection,
                sourceSerializerFactory,
                propertyMappingProvider)
                .DefaultMappingFor<ProjectSearchIndexModel>(
                    i => i
                    .IndexName("projects")
                    .IdProperty(e => e.project_id))
            //.DisableDirectStreaming() // TODO: remove
            ;
            return settings;
        }

        private Func<PropertiesDescriptor<ProjectSearchIndexModel>, IPromise<IProperties>> getProjectViewModelProperties()
        {
            return ps => ps
                .Keyword(s => s.Name(e => e.PortfolioViewKey).Norms(false))
                .Text(s => s.Name(e => e.project_name).Boost(3))
                .Text(s => s.Name(e => e.short_desc).Boost(2))
                .Text(s => s.Name(e => e.forward_look).Boost(1))
                .Keyword(s => s.Name(e => e.Priority).Norms(false))
                .Keyword(s => s.Name(e => e.PriorityGroup).Norms(false))
                .Keyword(s => s.Name(e => e.rag).Norms(false))
                ;
        }
               
    }

    public class IgnoreDataMemberPropertyMappingProvider : IPropertyMappingProvider
    {
        public IPropertyMapping CreatePropertyMapping(MemberInfo memberInfo)
        {
            return null;
        }
    }

    public class ProjectSerializer : ConnectionSettingsAwareSerializerBase
    {
        public ProjectSerializer(IElasticsearchSerializer builtinSerializer, IConnectionSettingsValues connectionSettings) : base(builtinSerializer, connectionSettings)
        {
        }

        protected override ConnectionSettingsAwareContractResolver CreateContractResolver()
        {
            return new ProjectContractResolver(ConnectionSettings);
        }
    }


    public class ProjectContractResolver : ConnectionSettingsAwareContractResolver
    {
        private static Dictionary<string, string> includedProperties = new Dictionary<string, string>() {
            { nameof(ProjectSearchIndexModel.PortfolioViewKey), null },
            { nameof(ProjectSearchIndexModel.project_id), null },
            { nameof(ProjectSearchIndexModel.project_name), null },
            { nameof(ProjectSearchIndexModel.Priority), null },
            { nameof(ProjectSearchIndexModel.PriorityGroup), null },
            { nameof(ProjectSearchIndexModel.short_desc), null },
            { nameof(ProjectSearchIndexModel.forward_look), null },
            { nameof(ProjectSearchIndexModel.rag), null }
        };

        public ProjectContractResolver(IConnectionSettingsValues connectionSettings) : base(connectionSettings)
        {
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            if (type == typeof(ProjectSearchIndexModel))
            {
                foreach (var property in properties)
                {
                    if (includedProperties.ContainsKey(property.UnderlyingName))
                    {
                        property.Ignored = false;
                        if (includedProperties[property.UnderlyingName] == null)
                            property.PropertyName = base.GetResolvedPropertyName(property.UnderlyingName);
                        else
                            property.PropertyName = base.GetResolvedPropertyName(includedProperties[property.UnderlyingName]);
                    }
                    else
                    {
                        property.Ignored = true;
                    }
                }
            }
            return properties;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            return property;
        }
    }

}
