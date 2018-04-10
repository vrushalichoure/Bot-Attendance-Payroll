using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Zest_Client
{
   public class ProductCalling
    {
        public async Task<string> ProductDetails(string token)
        {


            HttpClient cons = new HttpClient();
            cons.BaseAddress = new Uri("http://localhost:57144/");
            cons.DefaultRequestHeaders.Accept.Clear();
            cons.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            cons.DefaultRequestHeaders.Add("Authorization", token);
            var id = new TestRequest { id = 2 };
            string proreq = JsonConvert.SerializeObject(id);
            HttpContent procontent = new StringContent(id.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage prores = cons.PostAsync("http://localhost:57144/api/Products/GetProduct", new StringContent(@"{""RequestJSON"":" + proreq + "}", Encoding.Default, "application/json")).Result;
            var prodata = await prores.Content.ReadAsAsync<Product>();
            var proname = prodata.ResponseJSON.Name;
            string name = proname.ToString();
            return name;

        }
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

