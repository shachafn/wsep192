using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure.ExternalServices
{
    public class BaseHttpClient
    {
        protected static HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://cs-bgu-wsep.herokuapp.com/")
        };
        private ILogger<BaseHttpClient> _logger;

        public BaseHttpClient(ILogger<BaseHttpClient> logger)
        {
            this._logger = logger;
        }

        protected string Post(Dictionary<string,string> values)
        {
            var content = new FormUrlEncodedContent(values);
            _logger.LogDebug($"Sending Values {JsonConvert.SerializeObject(values)} to https://cs-bgu-wsep.herokuapp.com/");
            var response = client.PostAsync("https://cs-bgu-wsep.herokuapp.com/", content);
            response.Wait();
            var body = response.Result.Content.ReadAsStringAsync();
            body.Wait();
            return body.Result;
        }
    }
}
