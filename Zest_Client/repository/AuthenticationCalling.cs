using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Zest_Client
{
    public class AuthenticationCalling
    {

        public async Task<string> TokenCalling(string username, string password)
        {

            //string url = ConfigurationManager.AppSettings["url"];
            //HttpClient cons = new HttpClient();
            //cons.BaseAddress = new Uri(url);
            //cons.DefaultRequestHeaders.Accept.Clear();
            //cons.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //var tag = new AuthenticationRequest { UserName = username, Password = password, AuthenticationType = "form" };

            //string req = JsonConvert.SerializeObject(tag);
            //HttpContent content = new StringContent(tag.ToString(), Encoding.UTF8, "application/json");
            //HttpResponseMessage res = cons.PostAsync(url+"api/Authentication/UserLogin", new StringContent(@"{""RequestJSON"":" + req + "}", Encoding.Default, "application/json")).Result;


            //var data = new ServiceResponse();
            //try
            //{
            //    data = await res.Content.ReadAsAsync<ServiceResponse>();
            //}
            //catch (Exception ex)
            //{

            //}
            //var token = data.ResponseJSON.AuthorizationToken;
            //string t = token.ToString();
            return "ok";

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

}
