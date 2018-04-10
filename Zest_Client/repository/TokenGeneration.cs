using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Zest_Client.repository
{
    public class TokenGeneration
    {

        public static async Task TokenCalling()
        {
            HttpClient cons = new HttpClient();
            cons.BaseAddress = new Uri("http://localhost:57144/");
            cons.DefaultRequestHeaders.Accept.Clear();
            cons.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var tag = new AuthenticationRequest { UserName = "test", Password = "test", AuthenticationType = "form" };

            string req = JsonConvert.SerializeObject(tag);
            HttpContent content = new StringContent(tag.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage res = cons.PostAsync("http://localhost:57144/api/Authentication/UserLogin", new StringContent(@"{""RequestJSON"":" + req + "}", Encoding.Default, "application/json")).Result;
            var data = await res.Content.ReadAsAsync<ServiceResponse>();
            var token = data.ResponseJSON.AuthorizationToken;

        }
    }
    public class AuthenticationResponse
    {
        public int EmpID { get; set; }
        public string AuthorizationToken { get; set; }
    }
    public class ServiceResponse
    {
        public string Status { get; set; }
        public string ServerDateTime { get; set; }
        public string ErrorList { get; set; }
        public AuthenticationResponse ResponseJSON { get; set; }
    }
    public class AuthenticationRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AuthenticationType { get; set; }
    }
    public class TestRequest
    {
        public int id { get; set; }
    }

    public class Product
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public Product ResponseJSON { get; set; }
    }
}
        
