using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConditionalAPIClient.Service
{
    public class Client : IClient
    {
        private readonly HttpClient httpClient;
        private readonly string baseURL = "http://146.190.130.247:5011/donbest";
        public Client(HttpClient _httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> ApiRequest()
        {
            return await httpClient.GetStringAsync(baseURL);
        }
    }
}
