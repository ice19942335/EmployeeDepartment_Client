using System;
using System.Collections.Generic;
using System.Xaml;
using System.Xml;
using System.Xml.Serialization;

namespace EmployeeRegistration.Objects
{
    [Serializable]
    public class Employee
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

        private string surename;

        public string Surename
        {
            get { return surename; }
            set { this.surename = value; }
        }

        private string salary;

        public string Salary
        {
            get { return salary; }
            set { this.salary = value; }
        }

        private string deplist;

        public string Deplist
        {
            get { return deplist; }
            set { this.deplist = value; }
        }

        public Employee()
        {

        }

        private List<Department> departments = new List<Department>();

        public List<Department> Departments
        {
            get { return departments; }
            set { departments = value; }
        }


        public Employee(int id, string name, string surename, string salary, string deplist)
        {
            this.id = id;
            this.name = name;
            this.surename = surename;
            this.salary = salary;
            this.deplist = deplist;
        }

        public void AddDepartment(Department dep)
        {
            departments.Add(dep);
        }

        public override string ToString()
        {
            return $"{this.id} \t {this.Name} \t {this.surename} \t {this.salary} \t {departments.ToArray()}";
        }
    }
}
