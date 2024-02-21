﻿using System.Net.Http.Headers;
using System.Text.Json;

namespace CatCoffeePlatformRazorPages.Common
{
    public class ApiHelper
    {
        private readonly HttpClient _client;
        private readonly string _apiUrl;
        private readonly bool _serializeOption;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiHelper"/> class.
        /// </summary>
        /// <param name="ApiResource">The API resource (e.g., "products").</param>
        /// <param name="SerializeOption">The json serialize case insensitive option (default is true).</param>
        public ApiHelper(string ApiResource, bool? SerializeOption = true)
        {
            _client = new HttpClient();
            _serializeOption = SerializeOption ?? true;

            var content = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(content);

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _apiUrl = config.GetSection("ApiUrl").Value + ApiResource + "/";
        }

        private JsonSerializerOptions GetJsonSerializerOptions()
            => new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = _serializeOption
            };

        /// <summary>
        /// Get list of T from API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List of T or default</returns>
        public async Task<T?> GetAsync<T>()
        {
            HttpResponseMessage response = await _client.GetAsync(_apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(data, GetJsonSerializerOptions());
            }
            return default;
        }

        /// <summary>
        /// Get T from API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns>T or default</returns>
        public async Task<T?> GetAsync<T>(string id)
        {
            HttpResponseMessage response = await _client.GetAsync(_apiUrl + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(data, GetJsonSerializerOptions());
            }
            return default;
        }

        /// <summary>
        /// Create new T from API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>True if success, otherwise false</returns>
        public async Task<bool> PostAsync<T>(T item)
        {
            HttpResponseMessage response = await _client
                .PostAsJsonAsync(_apiUrl, item);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Update T from API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>True if success, otherwise false</returns>
        public async Task<bool> PutAsync<T>(string id, T item)
        {
            HttpResponseMessage response = await _client
                .PutAsJsonAsync(_apiUrl + id, item);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Delete T from API
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if deleted (status 200), otherwise false</returns>
        public async Task<bool> DeleteAsync(string id)
        {
            HttpResponseMessage response = await _client
                .DeleteAsync(_apiUrl + id);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            HttpResponseMessage response = await _client
                .DeleteAsync(_apiUrl + id);
            return response.IsSuccessStatusCode;
        }
    }
}
