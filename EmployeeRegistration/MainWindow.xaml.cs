using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using EmployeeRegistration.Objects;
using EmployeeRegistration.Views;
using EmployeeRegistration.Presenters;

namespace EmployeeRegistration
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IView
    {
        private readonly Presenter p;

        public MainWindow()
        {
            InitializeComponent();

            p = new Presenter(this);

            //Initialize cbDepList
            ObservableCollection<Department> depList = p.GetDepList();
            foreach (var dep in depList)
                cbDepartmentsList.Items.Add($"{dep.Name}, {dep.Location}, {dep.Salary}");

            ////Dep
            btnNewDep.Click += delegate { p.AddDepartment(); };
            btnDeleteDep.Click += delegate { p.DeleteDepartment(); };


            ////Emp
            btnNewEmp.Click += delegate { p.AddEmployee(); };
            btnDeleteEmp.Click += delegate { p.DeleteEmployee(); };
            lvEmployee.SelectionChanged += delegate { p.RefreshDepartmentsListOnEMP(); };
            btnAddDepToEMP.Click += delegate { p.AddDepartmentToEMP(); };
            btnDelDepFromEMP.Click += delegate { p.DeleteDepartmentFromEMP(); };
        }

        public string NameDep
        {
            get => tbNameDepartment.Text;
            set => tbNameDepartment.Text = value;
        }

        public string LocationDep
        {
            get => tbLocationDepartment.Text;
            set => tbLocationDepartment.Text = value;
        }

        public double BasicSalaryDep
        {
            get
            {
                double num;
                if (Double.TryParse(tbBasicSalaryDepartment.Text, out num))
                {
                    return num;
                }
                else
                {
                    return 0;
                }
                    
            }
            set => tbBasicSalaryDepartment.Text = value.ToString();
        }

        public ListView ListViewDep
        {
            get => lvDepartment;
            set => lvDepartment = value;
        }

        public string NameEmp
        {
            get => tbNameEmployee.Text;
            set => tbNameEmployee.Text = value;
        }

        public string SureName
        {
            get => tbSurenameEmployee.Text;
            set => tbSurenameEmployee.Text = value;
        }

        public double SalaryEmp
        {
            get
            {
                double num;
                if (Double.TryParse(tbSalaryEmployee.Text, out num))
                {
                    return num;
                }
                else
                {
                    return 0;
                }

            }
            set => tbBasicSalaryDepartment.Text = value.ToString();
        }

        public ListView ListViewEmp
        {
            get => lvEmployee;
            set => lvEmployee = value;
        }

        public ComboBox ComboBoxDepList
        {
            get => cbDepartmentsList;
            set => cbDepartmentsList = value;
        }

        public ComboBox ComboBoxDepartmentListEMP
        {
            get => cbDepartmentsListEmp;
            set => cbDepartmentsListEmp = value;
        }

        /// <summary>
        /// On app closing dialog, if true then save all data method calling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you really want to do that?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                e.Cancel = true;
            //else
                //p.SaveAllData();
        }
    }
}
