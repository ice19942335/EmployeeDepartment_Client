using System;
using System.Xml;
using System.Xml.Serialization;

namespace EmployeeRegistration.Objects
{
    [Serializable]
    public class Department
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { this.id = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { this.name = value; }
        }

        private string location;

        public string Location
        {
            get { return location; }
            set { this.location = value; }
        }

        private string salary;

        public string Salary
        {
            get { return salary; }
            set { this.salary = value; }
        }

        public Department()
        {

        }

        public Department(int id, string name, string location, string salary)
        {
            this.id = id;
            this.name = name;
            this.location = location;
            this.salary = salary;
        }

        public override string ToString()
        {
            return $"{this.id} \t {this.Name} \t {this.Location} \t {this.salary}";
        }
    }
}
