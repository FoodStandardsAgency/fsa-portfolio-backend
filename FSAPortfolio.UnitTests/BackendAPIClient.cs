﻿using FSAPortfolio.WebAPI.DTO;
using FSAPortfolio.WebAPI.Models;
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

namespace FSAPortfolio.UnitTests
{
    public static class BackendAPIClient
    {
        internal static HttpClient client = new HttpClient();

        static BackendAPIClient()
        {
            client.BaseAddress = new Uri("http://localhost/FSAPortfolio.WebAPI/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("APIKey", ConfigurationManager.AppSettings["APIKey"]);

            var sha256 = SHA256.Create();
            var hash = BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes("password"))).Replace("-", "");
            var content = new FormUrlEncodedContent(new []
            {
                new KeyValuePair<string, string>("username", "portfolio"),
                new KeyValuePair<string, string>("password", hash),
                new KeyValuePair<string, string>("grant_type", "password"),
            });
            var result = client.PostAsync("api/Token", content).Result;
            dynamic token = JsonConvert.DeserializeObject<dynamic>(result.Content.ReadAsStringAsync().Result);
            string access_token = token.access_token;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
        }

        internal static async Task UpdateProjectAsync(ProjectUpdateModel update)
        {
            await PostAsync($"api/Projects", update);
        }

        internal static async Task<PortfolioConfigModel> GetPortfolioConfigurationAsync(string portfolio)
        {
            return await GetAsync<PortfolioConfigModel>($"api/PortfolioConfiguration/{portfolio}");
        }

        internal static async Task UpdatePortfolioConfigurationAsync(PortfolioConfigUpdateRequest update)
        {
            await PatchAsync($"api/PortfolioConfiguration/{update.ViewKey}", update);
        }

        internal static async Task<GetProjectDTO<ProjectEditViewModel>> GetProjectAsync(string projectId)
        {
            return await GetAsync<GetProjectDTO<ProjectEditViewModel>>($"api/Projects/{projectId}/edit");
        }


        private static async Task<T> GetAsync<T>(string uri)
        {
            var result = await client.GetAsync(uri);
            if (!result.IsSuccessStatusCode) throw new Exception(result.ReasonPhrase);
            var json = await result.Content.ReadAsStringAsync();
            T model = JsonConvert.DeserializeObject<T>(json);
            return model;
        }
        private static async Task PostAsync<T>(string uri, T update)
        {
            var json = JsonConvert.SerializeObject(update);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync(uri, content);
            if (!result.IsSuccessStatusCode) throw new Exception(result.ReasonPhrase);
        }
        private static async Task PatchAsync<T>(string uri, T update)
        {
            var json = JsonConvert.SerializeObject(update);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PatchAsync(uri, content);
            if (!result.IsSuccessStatusCode) throw new Exception(result.ReasonPhrase);
        }

        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri) { Content = content };
            HttpResponseMessage response = await client.SendAsync(request);
            return response;
        }
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent content)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri) { Content = content };
            HttpResponseMessage response = await client.SendAsync(request);
            return response;
        }

    }
}