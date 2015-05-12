using Library.MVVM.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Library.MVVM.viewModel
{
    public class AllEmpsViewModel : WorkspaceViewModel
    {
        #region Fields

        readonly EmployeeRepository _empRepository;
        readonly JobRepository _jobRepository;
        readonly SpecialityRepository _specialityRepository;

        #endregion // Поля

        #region Constructor

        public AllEmpsViewModel(EmployeeRepository EmployeeRepository, JobRepository JobRepository, SpecialityRepository SpecialityRepository)
        {
            if (EmployeeRepository == null)
                throw new ArgumentNullException("employeeRepository");

            _empRepository = EmployeeRepository;
            _jobRepository = JobRepository;
            _specialityRepository = SpecialityRepository;

            // Subscribe for notifications of when a new emp is saved.
            _empRepository.EmpAdded += this.OnEmpAddedToRepository;

            // Populate the AllEmps collection with EmpViewModels.
            this.CreateAllEmps();            
            base.DisplayName = "Сотрудники";  
        }

        void CreateAllEmps()
        {
            List<EmpViewModel> all =
                (from cust in _empRepository.GetEmployees()
                 select new EmpViewModel(cust, _empRepository, _jobRepository, _specialityRepository)).ToList();

            foreach (EmpViewModel cvm in all)
                cvm.PropertyChanged += this.OnEmpViewModelPropertyChanged;

            this.AllEmps = new ObservableCollection<EmpViewModel>(all);
            this.AllEmps.CollectionChanged += this.OnCollectionChanged;
        }

        #endregion // Constructor

        #region Public Interface

        /// <summary>
        /// Returns a collection of all the EmpViewModel objects.
        /// </summary>
        public ObservableCollection<EmpViewModel> AllEmps { get; private set; }

        /// <summary>
        /// Returns job, who is select
        /// </summary>
        public EmpViewModel SelectedJob
        {
            get
            {
                EmpViewModel _selectedJob = null;
                foreach (EmpViewModel aut in AllEmps)
                    if (aut.IsSelected)
                    {
                        _selectedJob = aut;
                        break;
                    }
                return _selectedJob;
            }
        }



        /// <summary>
        /// Returns Job Name of Emp
        /// </summary>
        public string JobsSelected
        {
            get
            {
                string emps = string.Empty;
                foreach (EmpViewModel emp in AllEmps)
                    if (emp.IsSelected)
                    {
                        var _jobs = emp.Jobs;
                        foreach (var job in _jobs)
                        {
                            emps += job.GetsName + "  ";
                        }
                        break; 
                    }

                return emps;
            }
        }

        /// <summary>
        /// Returns spec Name of Emp
        /// </summary>
        public string SpecialitysSelected
        {
            get
            {
                string specialitys = string.Empty;
                foreach (EmpViewModel emp in AllEmps)
                    if (emp.IsSelected)
                    {
                        var _specialitys = emp.Specialitys;
                        foreach (var speciality in _specialitys)
                        {
                            specialitys += speciality.GetsName + "  ";
                        }
                        break;
                    }
                return specialitys;
            }
        }

        /// <summary>
        /// Delete emp from EmpRepository
        /// </summary>
        public void RemoveEmp()
        {
            foreach (var job in AllEmps)
                if (job.IsSelected)
                {
                    job.Remove();
                    job.PropertyChanged -= this.OnEmpViewModelPropertyChanged;
                    AllEmps.Remove(job);
                    break;
                }
        }
        #endregion // Public Interface

        #region  Base Class Overrides

        protected override void OnDispose()
        {
            foreach (EmpViewModel custVM in this.AllEmps)
                custVM.Dispose();

            this.AllEmps.Clear();
            this.AllEmps.CollectionChanged -= this.OnCollectionChanged;

            _empRepository.EmpAdded -= this.OnEmpAddedToRepository;
        }

        #endregion // Base Class Overrides

        #region Event Handling Methods

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (EmpViewModel custVM in e.NewItems)
                    custVM.PropertyChanged += this.OnEmpViewModelPropertyChanged;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (EmpViewModel custVM in e.OldItems)
                    custVM.PropertyChanged -= this.OnEmpViewModelPropertyChanged;
        }

        void OnEmpViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string IsSelected = "IsSelected";

            // Make sure that the property name we're referencing is valid.
            // This is a debugging technique, and does not execute in a Release build.
            (sender as EmpViewModel).VerifyPropertyName(IsSelected);
            
            // When a customer is selected or unselected, we must let the
            // world know that the TotalSelectedSales property has changed,
            // so that it will be queried again for a new value.
            if (e.PropertyName == IsSelected)
            {
             
                this.OnPropertyChanged("JobsSelected");
                this.OnPropertyChanged("SpecialitysSelected");
            }
        }

        void OnEmpAddedToRepository(object sender, EmployeeAddedEventArgs e)
        {
            var viewModel = new EmpViewModel(e.NewEmp, _empRepository, _jobRepository, _specialityRepository);
            this.AllEmps.Add(viewModel);
        }

        #endregion // Event Handling Methods
    }
}
