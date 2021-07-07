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

namespace FSAPortfolio.ApiClient
{
    public class BackendAPIClient
    {
        public HttpClient httpClient = new HttpClient();

        public BackendAPIClient(string baseAddress, string apiKey, string userName, string userPassword)
        {
            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("APIKey", apiKey);

            var sha256 = SHA256.Create();
            var hash = BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(userPassword))).Replace("-", "");
            var content = new FormUrlEncodedContent(new []
            {
                new KeyValuePair<string, string>("username", userName),
                new KeyValuePair<string, string>("password", hash),
                new KeyValuePair<string, string>("grant_type", "password"),
            });
            var result = httpClient.PostAsync("api/Token", content).Result;
            dynamic token = JsonConvert.DeserializeObject<dynamic>(result.Content.ReadAsStringAsync().Result);
            string access_token = token.access_token;

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            var result = await httpClient.GetAsync(uri);
            if (!result.IsSuccessStatusCode) throw new Exception(result.ReasonPhrase);
            var json = await result.Content.ReadAsStringAsync();
            T model = JsonConvert.DeserializeObject<T>(json);
            return model;
        }
        public async Task<T> GetAsync<T>(string uri, Dictionary<string, string> queryValues)
        {
            using (var queryBuilder = new FormUrlEncodedContent(queryValues))
            {
                var queryString = queryBuilder.ReadAsStringAsync().Result;
                return await GetAsync<T>($"{uri}?{queryString}");
            }
        }
        public async Task PostAsync<T>(string uri, T update)
        {
            var json = JsonConvert.SerializeObject(update);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync(uri, content);
            if (!result.IsSuccessStatusCode) throw new Exception(result.ReasonPhrase);
        }
        public async Task PatchAsync<T>(string uri, T update)
        {
            var json = JsonConvert.SerializeObject(update);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await httpClient.PatchAsync(uri, content);
            if (!result.IsSuccessStatusCode) throw new Exception(result.ReasonPhrase);
        }
        public async Task<TResponse> PatchAsync<TRequest, TResponse>(string uri, TRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await httpClient.PatchAsync(uri, content);
            if (!result.IsSuccessStatusCode) throw new Exception(result.ReasonPhrase);
            json = await result.Content.ReadAsStringAsync();
            TResponse response = JsonConvert.DeserializeObject<TResponse>(json);
            return response;
        }

        public async Task DeleteAsync(string uri)
        {
            await httpClient.DeleteAsync(uri);
        }
    }

    public static class HttpClientExtensions
    {
        public async static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri) { Content = content };
            HttpResponseMessage response = await client.SendAsync(request);
            return response;
        }
        public async static Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent content)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri) { Content = content };
            HttpResponseMessage response = await client.SendAsync(request);
            return response;
        }

    }
}
