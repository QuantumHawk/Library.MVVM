using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;
using Library.MVVM.model;
using Library.MVVM.Data;


namespace Library.MVVM.Data
{
    public class EmployeeRepository
    {
        #region Fields

        readonly ObservableCollection<Employee> _employees;
        

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Creates a new repository of Emp.
        /// </summary>
        public EmployeeRepository()
        {
            _employees = LoadEmployees();         
        }

        #endregion // Constructor

        #region Public Interface

        /// <summary>
        /// Raised when a Emp is placed into the repository.
        /// </summary>
        public event EventHandler<EmployeeAddedEventArgs> EmpAdded;

        /// <summary>
        /// Places the specified emp into the repository.
        /// If the emp is already in the repository, an
        /// exception is not thrown.
        /// </summary>
        public void AddEmp(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException("Employee");

            if (!_employees.Contains(employee))
            {
                EmployeeList bColl = new EmployeeList();
                bColl.Save(employee);
                ObservableCollection<Employee> LoadedEmployees = LoadEmployees();
                Employee b = LoadedEmployees.Last();
                employee.ID = b.ID;
                JobEmployee AoB = new JobEmployee();
                AoB.Save(employee);
                SpecialityEmp PB = new SpecialityEmp();
                PB.Save(employee);
                _employees.Add(employee);
                if (this.EmpAdded != null)
                    this.EmpAdded(this, new EmployeeAddedEventArgs(employee));                
            }
        }

        /// <summary>
        /// remove Emp from repository
        /// </summary>
        public void RemoveEmp(Employee Employee)
        {
            if (Employee == null)
                throw new ArgumentNullException("Employee");
            if (_employees.Contains(Employee))
            {
                bool result = _employees.Remove(Employee);
                if (result)
                {
                    EmployeeList bColl = new EmployeeList();
                    JobEmployee AoB = new JobEmployee();
                    SpecialityEmp PB = new SpecialityEmp();
                    PB.DeleteEmp(Employee);
                    AoB.DeleteEmp(Employee);
                    bColl.Delete(Employee);
                }
            }
        }
        /// <summary>
        /// Change Emp in repository
        /// </summary>
        public void ChangeEmp(Employee Employee)
        {
            if (Employee == null)
                throw new ArgumentNullException("Employee");
            if (_employees.Contains(Employee))
            {

                EmployeeList bColl = new EmployeeList();
                bColl.Save(Employee);
                JobEmployee AoB = new JobEmployee();
                AoB.Save(Employee);
                SpecialityEmp SL = new SpecialityEmp();
                SL.Save(Employee);
            }
        }

        /// <summary>
        /// Returns true if the specified emp exists in the
        /// repository, or false if it is not.
        /// </summary>
        public bool ContainsEmp(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException("employee");

            return _employees.Contains(employee);
        }

        /// <summary>
        /// Returns a shallow-copied list of all Emp in the repository.
        /// </summary>
        public List<Employee> GetEmployees()
        {
            return new List<Employee>(_employees);
        }

        #endregion // Public Interface

        #region Private Helpers

        private ObservableCollection<Employee> LoadEmployees()
        {
            EmployeeList bColl = new EmployeeList();
            List<Employee> listEmployee = bColl.LoadEmployees().ToList();
            JobEmployee AoB = new JobEmployee();
            JobList aColl = new JobList();
            SpecialityEmp PB = new SpecialityEmp();
            SpecialityList pColl = new SpecialityList();
            List<Job> AllJobs = aColl.LoadJobs();
            List<Speciality> AllSpeciality = pColl.LoadSpecialitys();
            List<int> IDs;
            List<int> specialityIDs;
            List<Job> jobs;
            List<Speciality> specialitys;
            List<Employee> listEmployee1 = new List<Employee>();
            foreach (Employee b in listEmployee)
            {                
                IDs = AoB.LoadJobs(b);
                specialityIDs = PB.LoadSpecialitys(b);
                jobs = new List<Job>();
                foreach (int id in IDs)
                {
                    foreach (Job aut in AllJobs)
                        if (id == aut.ID)
                            jobs.Add(aut);                   
                }
                specialitys = new List<Speciality>();
                foreach (int id in specialityIDs)
                {
                    foreach (Speciality aut in AllSpeciality)
                        if (id == aut.ID)
                            specialitys.Add(aut);
                }
                b.Jobs = jobs;
                b.Specialitys = specialitys;
                listEmployee1.Add(b);
            }
            return new ObservableCollection<Employee>(listEmployee1);
        }
        
        #endregion // Private Helpers
    }
}