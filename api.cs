using System.Reflection;

using RestSharp;
using Newtonsoft.Json;
using System.Configuration;

namespace LineNotify
{
    using Interface.Data;
    using Interface.Func;

    public class api : Iapi<Result>
    {
        string GetAPI_URL()
        {
            var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            return config.AppSettings.Settings["API_URL"].Value;
        }

        public Result notify(string token, string message)
        {
            string API_URL = GetAPI_URL();

            using (RestClient client = new RestClient(API_URL))
            {
                RestRequest request = new RestRequest("api/notify", Method.Post);

                request.AddHeader("Authorization", $"Bearer {token}");
                request.AddParameter("message", message);

                var RestResponse = client.Execute(request);
                if (RestResponse.Content is null) throw new Exception("RestResponse.Content is null");

                var Result = JsonConvert.DeserializeObject<Result>(RestResponse.Content);
                if (Result is null) throw new Exception("Result is null");

                return Result;
            }
        }
    }
}
