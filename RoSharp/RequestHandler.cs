using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace RoSharp
{
    /// <summary>
    /// The main class for handling Restful API Requests
    /// </summary>
    public class RequestHandler
    {
        HttpClientHandler handler;
        HttpClient client;
        public RequestHandler()
        {
            this.handler = new HttpClientHandler();
            this.client = new HttpClient(this.handler);
        }

        /// <summary>
        /// Makes an HTTP GET Request
        /// </summary>
        /// <param name="url">The url to make the request to</param>
        /// <returns>A dictionary of the JSON result</returns>
        /// <exception cref="JsonException">The result was not encoded in JSON</exception>
        public async Task<string> Get(string url)
        {
            using HttpResponseMessage response = await client.GetAsync(url);
            // Make sure we can successfully reach the API
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
            
            
        }
    }
}