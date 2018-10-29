using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EmployeeRegistration.Views;
using EmployeeRegistration.Models;
using EmployeeRegistration.Objects;
using System.Windows;

namespace EmployeeRegistration.Presenters
{
    public class Presenter
    {
        private readonly IView view;
        private readonly ModelDB model;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        public Presenter(IView view)
        {
            model = new ModelDB();
            this.view = view;
            view.ListViewDep.ItemsSource = model.DbDepartment;
            view.ListViewEmp.ItemsSource = model.DbEmployee;
        }

        //===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//

        #region DEP

        /// <summary>
        /// Getting departments list for initialization on start of application
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Department> GetDepList()
        {
            return model.DbDepartment;
        }

        /// <summary>
        /// Adding new department
        /// </summary>
        public void AddDepartment()
        {
            if (view.BasicSalaryDep != 0)
                model.AddDepartment(view.NameDep, view.LocationDep, view.BasicSalaryDep);
            else
                MessageBox.Show("Basic salary field can be a number only");

            RefreshDepListCB();
        }

        /// <summary>
        /// Deleting department
        /// </summary>
        public void DeleteDepartment()
        {
            if (view.ListViewDep.SelectedIndex != -1)
                model.DeleteDepartment(view.ListViewDep.SelectedIndex);
            else
                MessageBox.Show("Selet item from list");

            RefreshDepListCB();

        }

        /// <summary>
        /// Refreshing ComboBox - Departments list !!!Not on employee list!!!
        /// </summary>
        private void RefreshDepListCB()
        {
            ObservableCollection<Department> depList = model.DbDepartment;
            view.ComboBoxDepList.Items.Clear();
            foreach (var dep in depList)
                view.ComboBoxDepList.Items.Add($"{dep.Name}, {dep.Location}, {dep.Salary}");
        }

        #endregion

        #region EMP

        /// <summary>
        /// Adding new employee
        /// </summary>
        public void AddEmployee()
        {
            model.AddEmployee(view.NameEmp, view.SureName, view.SalaryEmp);
        }

        /// <summary>
        /// Deleting Selected employee from list
        /// </summary>
        public void DeleteEmployee()
        {
            if (view.ListViewEmp.SelectedIndex != -1)
                model.DeleteEmployee(view.ListViewEmp.SelectedIndex);
            else
                MessageBox.Show("Selet item from list");
        }

        /// <summary>
        /// Adding department to the employee if selected
        /// </summary>
        public void AddDepartmentToEMP()
        {
            if (view.ListViewEmp.SelectedIndex != -1 && view.ComboBoxDepList.SelectedIndex != -1)
            {
                model.AddDepToEmp(view.ListViewEmp.SelectedIndex, view.ComboBoxDepList.SelectedIndex);
                RefreshDepartmentsListOnEMP();
            }
            else
            {
                MessageBox.Show("Have to be selected Employee from list and Deparment from combo box at the bottom");
            }
        }

        /// <summary>
        /// Deleting department from employee
        /// </summary>
        public void DeleteDepartmentFromEMP()
        {
            if (view.ListViewEmp.SelectedIndex != -1 && view.ComboBoxDepartmentListEMP.SelectedIndex != -1)
            {
                model.DelDepFromEmp(view.ListViewEmp.SelectedIndex, view.ComboBoxDepartmentListEMP.SelectedIndex);
                RefreshDepartmentsListOnEMP();
            }
            else
            {
                MessageBox.Show("Have to be selected Employee from list and Deparment from combo box at the bottom");
            }
        }

        /// <summary>
        /// Refreshing Combobox - Departmentlist on employee
        /// </summary>
        public void RefreshDepartmentsListOnEMP()
        {
            List<Department> depList = new List<Department>();

            Employee emp = model.DbEmployee.ElementAt(view.ListViewEmp.SelectedIndex);
            string[] depListID = emp.Deplist.Split(',');

            //List<int> evenNumbers = list.FindAll(i => (i % 2) == 0);
            try
            {
                foreach (var item in depListID)
                {
                    Department dep = model.DbDepartment.First(e => e.Id == Int32.Parse(item));
                    if (dep != null)
                        depList.Add(dep);
                }
            }
            catch (Exception e) { }
            
            view.ComboBoxDepartmentListEMP.Items.Clear();

            foreach (var dep in depList)
                view.ComboBoxDepartmentListEMP.Items.Add($"{dep.Name}, {dep.Location}, £{dep.Salary}");
        }

        #endregion

        //===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//===//
    }
}