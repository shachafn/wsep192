using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Infrastructure.ExternalServices
{
    public class BaseHttpClient
    {
        protected static HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://cs-bgu-wsep.herokuapp.com/")
        };

        protected string Post(Dictionary<string,string> values)
        {
            var content = new FormUrlEncodedContent(values);
            var response = client.PostAsync("https://cs-bgu-wsep.herokuapp.com/", content);
            response.Wait();
            var body = response.Result.Content.ReadAsStringAsync();
            body.Wait();
            return body.Result;
        }
    }
}
