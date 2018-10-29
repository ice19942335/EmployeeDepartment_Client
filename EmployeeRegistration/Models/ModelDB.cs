using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xaml;
using EmployeeRegistration.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EmployeeRegistration.Models
{
    public class ModelDB
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;
                                            Initial Catalog=Lesson_7;
                                            Integrated Security=True;
                                            Pooling=False";

        //Collections to keep data in client app
        private ObservableCollection<Department> DB_DEP = new ObservableCollection<Department>();
        private ObservableCollection<Employee> DB_EMP = new ObservableCollection<Employee>();

        //Return dep and emp
        public ObservableCollection<Department> DbDepartment
        {
            get { return DB_DEP; }
        }
        public ObservableCollection<Employee> DbEmployee
        {
            get { return DB_EMP; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ModelDB()
        {
            DB_DEP = GetDataDep();
            DB_EMP = GetDataEmp();
        }

        //===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//

        #region DEP

        /// <summary>
        /// GetData Departments from API
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<Department> GetDataDep()
        {
            string url = @"http://localhost:51740/getdeplist";
            HttpClient client = new HttpClient();

            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var json = client.GetStringAsync(url).Result;
                DB_DEP = JsonConvert.DeserializeObject<ObservableCollection<Department>>(json);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return DB_DEP;
        }

        /// <summary>
        /// Adding new department adn sending data to API
        /// </summary>
        /// <param name="name"></param>
        /// <param name="location"></param>
        /// <param name="salary"></param>
        public void AddDepartment(string name, string location, double salary)
        {
            string url = @"http://localhost:51740/adddep";
            HttpClient client = new HttpClient();
            string jsObj = "";
            try
            {
                if (DB_DEP.Count > 0)
                {
                    jsObj = $@"{{
                                    ""Id"":{DB_DEP.ElementAt(DB_DEP.Count - 1).Id + 1},
  	                                ""Name"":""{name}"",
                                    ""Location"":""{location}"",
  	                                ""Salary"":""{salary}""
                                  }}";
                }
                else
                {
                    jsObj = $@"{{
                                    ""Id"":{0},
  	                                ""Name"":""{name}"",
                                    ""Location"":""{location}"",
  	                                ""Salary"":""{salary}""
                                  }}";
                }


                StringContent content = new StringContent(jsObj, Encoding.UTF8, "application/json");
                var res = client.PostAsync(url, content).Result;
                string[] resArr = res.ToString().Split(',');
                if (resArr[0] == "StatusCode: 201")
                {
                    if (DB_DEP.Count > 0)
                    {
                        DB_DEP.Add(new Department(
                            DB_DEP.ElementAt(DB_DEP.Count - 1).Id + 1,
                            name,
                            location,
                            salary.ToString())
                        );
                    }
                    else
                    {
                        string urlTemp = @"http://localhost:51740/getdeplist";
                        HttpClient clientTemp = new HttpClient();
                        ObservableCollection<Department> list = new ObservableCollection<Department>();
                        try
                        {
                            clientTemp.DefaultRequestHeaders.Add("Accept", "application/json");
                            var json = clientTemp.GetStringAsync(urlTemp).Result;
                            list = JsonConvert.DeserializeObject<ObservableCollection<Department>>(json);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }

                        DB_DEP.Add(list.ElementAt(0));
                    }
                }
                else if (res.ToString() == String.Empty)
                {
                    MessageBox.Show("Server not responding try again later");
                }
                else
                {
                    MessageBox.Show("Somethink went wrong please, try again");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Deleting department and sending data to API
        /// </summary>
        /// <param name="selected"></param>
        public void DeleteDepartment(int selected)
        {
            Department depSelected = DB_DEP.ElementAt(selected);
            WebClient webClient = new WebClient();
            string url = $@"http://localhost:51740/deldep/{depSelected.Id}";

            try
            {
                var res = webClient.DownloadString(url);

                if (Convert.ToBoolean(res))
                {
                    DB_DEP.RemoveAt(selected);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        #endregion

        #region EMP

        /// <summary>
        /// GetData Employee from API
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<Employee> GetDataEmp()
        {
            string url = @"http://localhost:51740/getemplist";
            HttpClient client = new HttpClient();

            try
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var json = client.GetStringAsync(url).Result;
                DB_EMP = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return DB_EMP;
        }

        /// <summary>
        /// Adding new Employee and send Data to API
        /// </summary>
        /// <param name="name"></param>
        /// <param name="surename"></param>
        /// <param name="salary"></param>
        public void AddEmployee(string name, string surename, double salary)
        {
            string url = @"http://localhost:51740/addemp";
            HttpClient client = new HttpClient();
            string jsObj = "";
            try
            {
                if (DB_EMP.Count > 0)
                {
                    jsObj = $@"{{
                                    ""Id"":{DB_EMP.ElementAt(DB_EMP.Count - 1).Id + 1},
  	                                ""Name"":""{name}"",
                                    ""Surename"":""{surename}"",
  	                                ""Salary"":""{salary}"",
                                    ""Deplist"":""{0}"",
                                  }}";
                }
                else
                {
                    jsObj = $@"{{
                                    ""Id"":{0},
  	                                ""Name"":""{name}"",
                                    ""Surename"":""{surename}"",
  	                                ""Salary"":""{salary}"",
                                    ""Deplist"":""{0}"",
                                  }}";
                }


                StringContent content = new StringContent(jsObj, Encoding.UTF8, "application/json");
                var res = client.PostAsync(url, content).Result;
                string[] resArr = res.ToString().Split(',');
                if (resArr[0] == "StatusCode: 201")
                {
                    if (DB_EMP.Count > 0)
                    {
                        DB_EMP.Add(new Employee(
                            DB_EMP.ElementAt(DB_EMP.Count - 1).Id + 1,
                            name,
                            surename,
                            salary.ToString(),
                            "0"
                            )
                        );
                    }
                    else
                    {
                        string urlTemp = @"http://localhost:51740/getemplist";
                        HttpClient clientTemp = new HttpClient();
                        ObservableCollection<Employee> list = new ObservableCollection<Employee>();
                        try
                        {
                            clientTemp.DefaultRequestHeaders.Add("Accept", "application/json");
                            var json = clientTemp.GetStringAsync(urlTemp).Result;
                            list = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }

                        DB_EMP.Add(list.ElementAt(0));
                    }
                }
                else if (res.ToString() == String.Empty)
                {
                    MessageBox.Show("Server not responding try again later");
                }
                else if (resArr[0] == "StatusCode: 400")
                {
                    MessageBox.Show("Bad request, 400");
                }
                else
                {
                    MessageBox.Show("Somethink went wrong please, try again");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Deleting employee and sending data to API to delete from DB
        /// </summary>
        /// <param name="selected"></param>
        public void DeleteEmployee(int selected)
        {
            Employee empSelected = DB_EMP.ElementAt(selected);
            WebClient webClient = new WebClient();
            string url = $@"http://localhost:51740/delemp/{empSelected.Id}";

            try
            {
                var res = webClient.DownloadString(url);

                if (Convert.ToBoolean(res))
                {
                    DB_EMP.RemoveAt(selected);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Adding department to to Employee sending data to API
        /// </summary>
        /// <param name="empSelected"></param>
        /// <param name="depSelected"></param>
        public void AddDepToEmp(int empSelected, int depSelected)
        {
            Department dep = DB_DEP.ElementAt(depSelected);
            Employee emp = DB_EMP.ElementAt(empSelected);

            string depListString = emp.Deplist;
            string[] arr = depListString.Split(',');

            if (arr.Contains(dep.Id.ToString()))
            {
                MessageBox.Show("This department is already added");
            }
            else
            {
                depListString += $"{dep.Id},";

                string url = @"http://localhost:51740/adddeptoemp";
                HttpClient client = new HttpClient();
                string jsObj = "";
                try
                {
                    jsObj = $@"{{
                                    ""Id"":{DB_EMP.ElementAt(empSelected).Id},
  	                                ""Name"":""{emp.Name}"",
                                    ""Surename"":""{emp.Surename}"",
  	                                ""Salary"":""{emp.Salary}"",
                                    ""Deplist"":""{depListString}"",
                           }}";

                    StringContent content = new StringContent(jsObj, Encoding.UTF8, "application/json");
                    var res = client.PostAsync(url, content).Result;
                    string[] resArr = res.ToString().Split(',');
                    if (resArr[0] == "StatusCode: 201")
                    {
                        DB_EMP.ElementAt(empSelected).Deplist = depListString;
                    }
                    else if (res.ToString() == String.Empty)
                    {
                        MessageBox.Show("Server not responding try again later");
                    }
                    else if (resArr[0] == "StatusCode: 400")
                    {
                        MessageBox.Show("Bad request, 400");
                    }
                    else
                    {
                        MessageBox.Show("Somethink went wrong please, try again");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            
        }

        /// <summary>
        /// Deleting department from Employee and sending data to API
        /// </summary>
        /// <param name="empSelected"></param>
        /// <param name="depSelected"></param>
        public void DelDepFromEmp(int empSelected, int depSelected)
        {
            Employee emp = DB_EMP.ElementAt(empSelected);

            string depListString = emp.Deplist;
            string[] depListArr = depListString.Split(',');

            var depList = depListArr.ToList();
            depList.RemoveAt(depSelected);

            depListString = "";
            for (int i = 0; i < depList.Count - 1; i++)
                depListString += $"{depList.ElementAt(i)},";



            string url = @"http://localhost:51740/deldepfromemp";
            HttpClient client = new HttpClient();
            string jsObj = "";
            try
            {
                jsObj = $@"{{
                                    ""Id"":{DB_EMP.ElementAt(empSelected).Id},
  	                                ""Name"":""{emp.Name}"",
                                    ""Surename"":""{emp.Surename}"",
  	                                ""Salary"":""{emp.Salary}"",
                                    ""Deplist"":""{depListString}"",
                           }}";



                StringContent content = new StringContent(jsObj, Encoding.UTF8, "application/json");
                var res = client.PostAsync(url, content).Result;
                string[] resArr = res.ToString().Split(',');
                if (resArr[0] == "StatusCode: 201")
                {
                    DB_EMP.ElementAt(empSelected).Deplist = depListString;
                }
                else if (res.ToString() == String.Empty)
                {
                    MessageBox.Show("Server not responding try again later");
                }
                else if (resArr[0] == "StatusCode: 400")
                {
                    MessageBox.Show("Bad request, 400");
                }
                else
                {
                    MessageBox.Show("Somethink went wrong please, try again");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        #endregion

        //===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//
    }
}
