using FSAPortfolio.WebAPI.DTO;
using FSAPortfolio.Application.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FSAPortfolio.ApiClient;

namespace FSAPortfolio.UnitTests.APIClients
{
    public static class TestBackendAPIClient
    {
        private static BackendAPIClient client;
        internal static HttpClient httpClient => client.httpClient;
        static TestBackendAPIClient()
        {
            client = new BackendAPIClient("http://localhost/FSAPortfolio.WebAPI/", TestSettings.APIKey, TestSettings.TestUser, TestSettings.TestUserPassword);
        }

        internal static async Task<T> GetAsync<T>(string uri)
        {
            return await client.GetAsync<T>(uri);
        }
        internal static async Task<T> GetAsync<T>(string uri, Dictionary<string, string> queryValues) => await client.GetAsync<T>(uri, queryValues);
        internal static async Task PostAsync<T>(string uri, T update) => await client.PostAsync<T>(uri, update);
        internal static async Task PatchAsync<T>(string uri, T update) => await client.PatchAsync<T>(uri, update);
        internal static async Task<TResponse> PatchAsync<TRequest, TResponse>(string uri, TRequest request) => await client.PatchAsync<TRequest, TResponse>(uri, request);
        internal static async Task DeleteAsync(string uri) => await client.DeleteAsync(uri);
    }
}
