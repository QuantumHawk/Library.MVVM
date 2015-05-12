using Library.MVVM.Data;
using Library.MVVM.model;
using Library.MVVM.viewModel;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace Library.MVVM.model
{
    public class MainWindowViewModel : WorkspaceViewModel
    {
        #region Fields


        readonly JobRepository _jobRepository;
        readonly SpecialityRepository _specialityRepository;
        readonly EmployeeRepository _empRepository;
        ObservableCollection<WorkspaceViewModel> _workspaces;


        #endregion // Fields

        #region Constructor

        public MainWindowViewModel()
        {            
            _jobRepository = new JobRepository();
            _specialityRepository = new SpecialityRepository();
            _empRepository = new EmployeeRepository();
        }

        #endregion // Constructor

        #region Public Methods

        RelayCommand commandNewJob;
        RelayCommand commandNewEmployee;
        RelayCommand commandNewSpeciality;

        RelayCommand commandShowJobs;
        RelayCommand commandShowEmployees;
        RelayCommand commandShowSpecialitys;

        RelayCommand commandChangeJob; 
        RelayCommand commandChangeEmployee;
        RelayCommand commandChangeSpeciality;

        RelayCommand commandRemoveJob;
        RelayCommand commandRemoveEmployee;
        RelayCommand commandRemoveSpeciality;
        
        public ICommand NewJob
        {
            get
            {
                if (commandNewJob == null)
                {
                    commandNewJob = new RelayCommand(x => CreateNewJob());
                }
                return commandNewJob;
            }
        }
        public ICommand ShowJobs
        {
            get
            {
                if (commandShowJobs == null)
                {
                    commandShowJobs = new RelayCommand(x => ShowAllJobs());
                }
                return commandShowJobs;
            }
        }
        public ICommand ChangeJob
        {
            get
            {
                if (commandChangeJob == null)
                {
                    commandChangeJob = new RelayCommand(
                        x => ChangeSelectedJob(),
                        x => canChangeJob());
                }
                return commandChangeJob;
            }
        }
        public ICommand RemoveJob
        {
            get
            {
                if (commandRemoveJob == null)
                {
                    commandRemoveJob = new RelayCommand(
                        x => RemoveSelectedJob(),
                        x => canChangeJob());
                }
                return commandRemoveJob;
            }
        }
        public ICommand NewEmployee
        {
            get
            {
                if (commandNewEmployee == null)
                {
                    commandNewEmployee = new RelayCommand(x => CreateNewEmployee());
                }
                return commandNewEmployee;
            }
        }
        public ICommand ShowEmployees
        {
            get
            {
                if (commandShowEmployees == null)
                {
                    commandShowEmployees = new RelayCommand(x => ShowAllEmployees());
                }
                return commandShowEmployees;
            }
        }
        public ICommand ChangeEmployee
        {
            get
            {
                if (commandChangeEmployee == null)
                {
                    commandChangeEmployee = new RelayCommand(
                        x => ChangeSelectedEmployee(),
                        x => canChangeEmployee());
                }
                return commandChangeEmployee;
            }
        }
        public ICommand RemoveEmployee
        {
            get
            {
                if (commandRemoveEmployee == null)
                {
                    commandRemoveEmployee = new RelayCommand(
                        x => RemoveSelectedEmployee(),
                        x => canChangeEmployee());
                }
                return commandRemoveEmployee;
            }
        }

        public ICommand NewSpeciality
        {
            get
            {
                if (commandNewSpeciality == null)
                {
                    commandNewSpeciality = new RelayCommand(x => CreateNewSpeciality());
                }
                return commandNewSpeciality;
            }
        }
        public ICommand ShowSpecialitys
        {
            get
            {
                if (commandShowSpecialitys == null)
                {
                    commandShowSpecialitys = new RelayCommand(x => ShowAllSpecialitys());
                }
                return commandShowSpecialitys;
            }
        }
        public ICommand ChangeSpeciality
        {
            get
            {
                if (commandChangeSpeciality == null)
                {
                    commandChangeSpeciality = new RelayCommand(
                        x => ChangeSelectedSpeciality(),
                        x => canChangeSpeciality());
                }
                return commandChangeSpeciality;
            }
        }
        public ICommand RemoveSpeciality
        {
            get
            {
                if (commandRemoveSpeciality == null)
                {
                    commandRemoveSpeciality = new RelayCommand(
                        x => RemoveSelectedSpeciality(),
                        x => canChangeSpeciality());
                }
                return commandRemoveSpeciality;
            }
        }

        #endregion //Public Methods
        
        #region Workspaces

        /// <summary>
        /// Returns the collection of available workspaces to display.
        /// A 'workspace' is a ViewModel that can request to be closed.
        /// </summary>
        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_workspaces == null)
                {
                    _workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _workspaces.CollectionChanged += this.OnWorkspacesChanged;
                }
                return _workspaces;
            }
        }

        void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }

        void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            workspace.Dispose();
            this.Workspaces.Remove(workspace);
        }

        #endregion // Workspaces

        #region Private Helpers

        void CreateNewJob()
        {
            Job newJob = Job.CreateNewJob();
            JobViewModel workspace = new JobViewModel(newJob, _jobRepository);
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }

        void CreateNewEmployee()
        {
            Employee newEmployee = Employee.CreateNewEmployee();
            EmpViewModel workspace = new EmpViewModel(newEmployee, _empRepository, _jobRepository, _specialityRepository);
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }

        void CreateNewSpeciality()
        {
            Speciality newSpeciality = Speciality.CreateNewSpeciality();
            SpecialityViewModel workspace = new SpecialityViewModel(newSpeciality, _specialityRepository);
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }

        void ChangeSelectedJob()
        {
            AllJobsViewModel _workspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllJobsViewModel)
                as AllJobsViewModel;
            JobViewModel workspace = _workspace.SelectedJob;
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }

        void ChangeSelectedEmployee()
        {
            AllEmpsViewModel _workspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllEmpsViewModel)
                as AllEmpsViewModel;
            EmpViewModel workspace = _workspace.SelectedJob;//////////
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }

        void ChangeSelectedSpeciality()
        {
            AllSpecialitysViewModel _workspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllSpecialitysViewModel)
                as AllSpecialitysViewModel;
            SpecialityViewModel workspace = _workspace.SelectedSpeciality;////???
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }

        void RemoveSelectedJob()
        {
            AllJobsViewModel _workspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllJobsViewModel)
                as AllJobsViewModel;
            _workspace.RemoveJob();   
        }

        void RemoveSelectedEmployee()
        {
            AllEmpsViewModel _workspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllEmpsViewModel)
                as AllEmpsViewModel;
            _workspace.RemoveEmp();            
        }
        void RemoveSelectedSpeciality()
        {
            AllSpecialitysViewModel _workspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllSpecialitysViewModel)
                as AllSpecialitysViewModel;
            _workspace.RemoveSpeciality();
        }

        void ShowAllJobs()
        {
            AllJobsViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllJobsViewModel)
                as AllJobsViewModel;

            if (workspace == null)
            {
                workspace = new AllJobsViewModel(_jobRepository);
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        void ShowAllEmployees()
        {
            AllEmpsViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllEmpsViewModel)
                as AllEmpsViewModel;

            if (workspace == null)
            {
                workspace = new AllEmpsViewModel(_empRepository, _jobRepository, _specialityRepository);
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        void ShowAllSpecialitys()
        {
            AllSpecialitysViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllSpecialitysViewModel)
                as AllSpecialitysViewModel;

            if (workspace == null)
            {
                workspace = new AllSpecialitysViewModel(_specialityRepository);
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);            
        }
        bool canChangeJob()
        {
            bool result = false;
            AllJobsViewModel _workspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllJobsViewModel)
                as AllJobsViewModel;
            if (_workspace != null)
            {
                JobViewModel workspace = _workspace.SelectedJob;
                if (workspace != null)
                    result = true;
            }
            return result;
        }
        bool canChangeEmployee()
        {
            bool result = false;
            AllEmpsViewModel _workspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllEmpsViewModel)
                as AllEmpsViewModel;
            if (_workspace != null)
            {                 
                EmpViewModel workspace = _workspace.SelectedJob;
                if (workspace != null )
                    result = true;
            }
            return result;
        }

        bool canChangeSpeciality()
        {
            bool result = false;
            AllSpecialitysViewModel _workspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllSpecialitysViewModel)
                as AllSpecialitysViewModel;
            if (_workspace != null)
            {
                SpecialityViewModel workspace = _workspace.SelectedSpeciality;
                if (workspace != null)
                    result = true;
            }
            return result;
        }
        #endregion // Private Helpers
    }
}
