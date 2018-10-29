using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EmpTester
{
    class Program
    {
        static void Main(string[] args)
        {



            string url = @"http://localhost:51740/adddep";

            HttpClient client = new HttpClient();
                                                                                        //client.DefaultRequestHeaders.Add("Accept", "application/json");
            string obj = $@"{{
                                ""Id"":""4"",
  	                            ""Name"":""HR"",
                                ""Location"":""London"",
  	                            ""Salary"":25000.0
                            }}";

            StringContent content = new StringContent(obj, Encoding.UTF8, "application/json");

                                                                                        //var res = client.GetStringAsync(url).Result;


            var res = client.PostAsync(url, content).Result; // Это удалить и раскоментить верхние 2 GET   !!! 

            Console.WriteLine(res);

            Console.ReadKey();





            #region WebClient

            //string url = @"http://localhost:51740/api/Data";

            //WebClient client = new WebClient();
            //var res = client.DownloadString(url);

            //Console.WriteLine(res);

            //Console.ReadKey();

            #endregion
        }
    }
}
