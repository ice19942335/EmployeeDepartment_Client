using System.Windows.Controls;

namespace EmployeeRegistration.Views
{
    public interface IView
    {
        //Department
        string NameDep { get; set; }
        string LocationDep { get; set; }
        double BasicSalaryDep { get; set; }
        ListView ListViewDep { get; set; }

        //Employee
        string NameEmp { get; set; }
        string SureName { get; set; }
        double SalaryEmp { get; set; }
        ListView ListViewEmp { get; set; }
        ComboBox ComboBoxDepList { get; set; }
        ComboBox ComboBoxDepartmentListEMP { get; set; }

    }
}
