using Library.MVVM.Data;
using Library.MVVM.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Library.MVVM.viewModel
{
    public class EmpViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        #region Fields

        readonly Employee _emp;
        readonly EmployeeRepository _empRepository;
        readonly JobRepository _jobRepository;
        readonly SpecialityRepository _specialityRepository;
        string[] _sexList;// лист пол
        string[] _educationList;//лист образование
        bool _isSelected;
        string _status; 

        RelayCommand _saveCommand;

        RelayCommand _takeJobCommand;
        RelayCommand _backJobCommand;
       
        RelayCommand _takeSpecialityCommand;
        RelayCommand _backSpecialityCommand;

        JobViewModel _selectedAvaliableJob;
        JobViewModel _selectedJob;
      
        SpecialityViewModel _selectedAvaliableSpeciality;
        SpecialityViewModel _selectedSpeciality;
        System.Windows.Threading.DispatcherTimer dispatcherTimer;


        #endregion // Fields

        #region Constructor

        public EmpViewModel(Employee emp, EmployeeRepository EmpRepository, JobRepository JobRepository, SpecialityRepository SpecialityRepository)
        {
            if (emp == null)
                throw new ArgumentNullException("Employee");
            if (EmpRepository == null)
                throw new ArgumentNullException("EmployeeRepository");
            if (JobRepository == null)
                throw new ArgumentNullException("JobRepository");
            if (SpecialityRepository == null)
                throw new ArgumentNullException("SpecialityRepository");

            base.DisplayName = "Сотрудник";
            _empRepository = EmpRepository;
            _jobRepository = JobRepository;
            _specialityRepository = SpecialityRepository;
            _emp = emp;
            _educationList = EducationList;
            _sexList = SexList;
            _status = string.Empty;
            
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        
            // Subscribe for notifications of when a new emp is saved.
            _jobRepository.JobAdded += this.OnJobAddedToRepository;
            _specialityRepository.SpecialityAdded += this.OnSpecialityAddedToRepository;
            this.CreateAllJobs();
            this.CreateAllSpecialitys();
        }

        void CreateAllJobs()
        {
            List<JobViewModel> all =
                (from cust in _jobRepository.GetJobs
                 select new JobViewModel(cust, _jobRepository)).ToList();

            foreach (JobViewModel cvm in all)
                cvm.PropertyChanged += this.OnJobViewModelPropertyChanged;

            this.AllJobs = new ObservableCollection<JobViewModel>(all);
            this.AllJobs.CollectionChanged += this.OnCollectionChangedJob;
        }
        void CreateAllSpecialitys()
        {
            List<SpecialityViewModel> all =
                (from cust in _specialityRepository.GetSpecialitys
                 select new SpecialityViewModel(cust, _specialityRepository)).ToList();

            foreach (SpecialityViewModel cvm in all)
                cvm.PropertyChanged += this.OnSpecialityViewModelPropertyChanged;

            this.AllSpecialitys = new ObservableCollection<SpecialityViewModel>(all);
            this.AllSpecialitys.CollectionChanged += this.OnCollectionChangedSpeciality;
        }

        #endregion // Constructor

        #region Emp Properties

        /// <summary>
        /// get/set Topic Name of emp
        /// </summary>
        public string FirstName
        {
            get { return _emp.FirstName; }
            set
            {
                if (value == _emp.FirstName)
                    return;

                _emp.FirstName = value;
                base.OnPropertyChanged("FirstName");
            }
        }

        public string SecondName
        {
            get { return _emp.SecondName; }
            set
            {
                if (value == _emp.SecondName)
                    return;

                _emp.SecondName = value;
                base.OnPropertyChanged("SecondName");
            }
        }
        public string LastName
        {
            get { return _emp.LastName; }
            set
            {
                if (value == _emp.LastName)
                    return;

                _emp.LastName = value;
                base.OnPropertyChanged("LastName");
            }
        }

        public string Sex
        {
            get { return _emp.Sex; }
            set
            {
                if (value == _emp.Sex || String.IsNullOrEmpty(value))
                    return;
                _emp.Sex = value;
                base.OnPropertyChanged("Sex");
            }
        }

        public DateTime Birthday
        {
            get { return _emp.Birthday.Date; }
            set
            {
                if (value.Date == _emp.Birthday.Date)
                    return;
                _emp.Birthday = value.Date;
                base.OnPropertyChanged("Birthday");
            }

        }

        public string BirthdayStr
        { get { return _emp.Birthday.Date.ToString("dd/MM/yyyy"); } }

        /// <summary>
        /// get/set
        /// </summary>
        public string Education
        {
            get { return _emp.Education; }
            set
            {
                if (value == _emp.Education || String.IsNullOrEmpty(value))
                    return;
                _emp.Education = value;
                base.OnPropertyChanged("Education");
            }
        }

        public string Pasport
        {
            //get { return _emp.Pasport; }
            //set
            //{
            //    if (value == _emp.Pasport || String.IsNullOrEmpty(value))
            //        return;
            //    _emp.Pasport = value;
            //    base.OnPropertyChanged("Pasport");
            //}

            get { return _emp.Pasport.ToString(); }
            set
            {
                int num;
                if (value == _emp.Pasport.ToString() || !Int32.TryParse(value, out num))
                    return;
                _emp.Pasport = num;
                base.OnPropertyChanged("Pasport");
            }
        }

        /// <summary>
        /// get/set  Salary of emp
        /// </summary>
        public string Salary
        {
            get { return _emp.Salary.ToString(); }
            set
            {
                int number;
                if (value == _emp.Salary.ToString() || !Int32.TryParse(value, out number))
                    return;
                _emp.Salary = number;
                base.OnPropertyChanged("Salary");
            }
        }


        public DateTime Date
        {
            get { return _emp.Date.Date; }
            set
            {
                if (value.Date == _emp.Date.Date)
                    return;
                _emp.Date = value.Date;
                base.OnPropertyChanged("Date");
            }

        }

        public string DateStr
        { get { return _emp.Date.Date.ToString("dd/MM/yyyy"); } }

        /// <summary>
        /// get/set Job's List of emp
        /// </summary>
        public ObservableCollection<JobViewModel> Jobs
        {
            get 
            {
                List<JobViewModel> all =
                (from cust in _emp.Jobs
                 select new JobViewModel(cust, _jobRepository)).ToList();

                foreach (JobViewModel cvm in all)
                    cvm.PropertyChanged += this.OnJobViewModelPropertyChanged;

                return new ObservableCollection<JobViewModel>(all);
            }
            set
            {
                List<Job> auth = new List<Job>();
                foreach (JobViewModel vm in value)
                    auth.Add(vm.GetJob());
                _emp.Jobs = auth;
                base.OnPropertyChanged("Jobs");                
            }
        }

        /// <summary>
        /// get/set Speciality List of emp
        /// </summary>
        public ObservableCollection<SpecialityViewModel> Specialitys
        {
            get
            {
                List<SpecialityViewModel> all =
                (from cust in _emp.Specialitys
                 select new SpecialityViewModel(cust, _specialityRepository)).ToList();

                foreach (SpecialityViewModel cvm in all)
                    cvm.PropertyChanged += this.OnSpecialityViewModelPropertyChanged;

                return new ObservableCollection<SpecialityViewModel>(all);
            }
            set
            {
                List<Speciality> auth = new List<Speciality>();
                foreach (SpecialityViewModel vm in value)
                    auth.Add(vm.GetSpeciality());
                _emp.Specialitys = auth;
                base.OnPropertyChanged("Specialitys");
            }
        }
       

        #endregion // Свойства сотрудника
        #region Presentation Properties

        public string Status
        {
            get
            {                
                return _status;
            }
        }

        public JobViewModel SelectedAvaliableJob 
        { 
            get
            {
                foreach (JobViewModel aut in AllJobs)
                    if (aut.IsSelected)
                    { _selectedAvaliableJob = aut; break; }
                return _selectedAvaliableJob;
            }
            set
            { _selectedAvaliableJob = value; }
        }

        public JobViewModel SelectedJob
        {
            get
            {
                foreach (JobViewModel aut in Jobs)
                    if (aut.IsSelected)
                    { _selectedJob = aut; break; }
                return _selectedJob;
            }
            set
            { _selectedJob = value; }
        }

        public SpecialityViewModel SelectedAvaliableSpeciality
        {
            get
            {
                foreach (SpecialityViewModel aut in AllSpecialitys)
                    if (aut.IsSelected)
                    { _selectedAvaliableSpeciality = aut; break; }
                return _selectedAvaliableSpeciality;
            }
            set
            { _selectedAvaliableSpeciality = value; }
        }

        public SpecialityViewModel SelectedSpeciality
        {
            get
            {
                foreach (SpecialityViewModel aut in Specialitys)
                    if (aut.IsSelected)
                    { _selectedSpeciality = aut; break; }
                return _selectedSpeciality;
            }
            set
            { _selectedSpeciality = value; }
        }
        
        /// <summary>
        /// get Sex's List 
        /// </summary>
        public string[] SexList
        {
            get
            {
                if (_sexList == null)
                {                    
                    _sexList = new string[]
                    {
                        "м",   "ж"
                        
                    };                    
                }
                return _sexList;
            }
        }
        public string[] EducationList
        {
            get
            {
                if (_educationList == null)
                {
                    _educationList = new string[]
                    {
                        "Высшее",   "Среднее", "Неоконченное высшее", "Неоконченное среднее", "Без образования"
                        
                    };
                }
                return _educationList;
            }
        }
        /// <summary>
        /// Возвращает имя объекта
        /// </summary>
        public override string DisplayName
        {
            get
            {
                if (this.IsNewEmp)
                {
                    return "новый сотрудник";
                }
                else
                    return _emp.FirstName;                  
            }
        }


        /// <summary>
        /// Gets/sets который выбран в UI.
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected)
                    return;

                _isSelected = value;
                base.OnPropertyChanged("IsSelected");
            }
        }

        /// <summary>
        /// Возвращает command который сохранил сотрудника.
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                        param => this.Save(),
                        param => this.CanSave
                        );
                }
                return _saveCommand;
            }
        }

        public ICommand TakeCommandJob
        {
            get
            {
                if (_takeJobCommand == null)
                {
                    _takeJobCommand = new RelayCommand(
                        param => this.TakeJob(),
                        param => this.CanTakeJob
                        );                    
                }
                return _takeJobCommand;
            }
        }

        public ICommand BackCommandJob
        {
            get
            {
                if (_backJobCommand == null)
                {
                    _backJobCommand = new RelayCommand(
                        param => this.BackJob(),
                        param => this.CanBackJob
                        );                    
                }
                return _backJobCommand;
            }
        }

        public ICommand TakeCommandSpeciality
        {
            get
            {
                if (_takeSpecialityCommand == null)
                {
                    _takeSpecialityCommand = new RelayCommand(
                        param => this.TakeSpeciality(),
                        param => this.CanTakeSpeciality
                        );
                }
                return _takeSpecialityCommand;
            }
        }

        public ICommand BackCommandSpeciality
        {
            get
            {
                if (_backSpecialityCommand == null)
                {
                    _backSpecialityCommand = new RelayCommand(
                        param => this.BackSpeciality(),
                        param => this.CanBackSpeciality
                        );
                }
                return _backSpecialityCommand;
            }
        }

        #endregion // Представление свойств

        #region Public Methods      

        /// <summary>
        /// Возвращает коллекцию всех EmpViewModel объектов
        /// </summary>
        public ObservableCollection<JobViewModel> AllJobs { get; private set; }

        /// <summary>
        /// Возвращает коллекцию всех jobVM объектов.
        /// </summary>
        public ObservableCollection<SpecialityViewModel> AllSpecialitys { get; private set; }

        /// <summary>
        ///Сохраняет сотрудников в репозиторий.  Этот метод вызывается SaveCommand.
        /// </summary>
        public void Save()
        {
            if (!_emp.IsValid)
                throw new InvalidOperationException("Не может сохранить сотрудника");

            if (this.IsNewEmp)
            {
                _empRepository.AddEmp(_emp);
                _status = "Сотрудник успешно добавлен";
            }
            else
            {
                _empRepository.ChangeEmp(_emp);
                _status = "Изменения успешно сохранены";
            }
            base.OnPropertyChanged("DisplayName");
            base.OnPropertyChanged("Status");
            hideMessage();
        }

        /// <summary>
        /// remove selected job from Job's List
        /// </summary>
        public void BackJob()
        {
            if (_selectedJob == null)
                throw new NotImplementedException("Нельзя вернуть должность");
            if (IsConteinsJob(_selectedJob))
                _emp.Jobs.Remove(_selectedJob.GetJob());
            base.OnPropertyChanged("Jobs");
        }

        /// <summary>
        /// Add selected job from job Repository to job's List
        /// </summary>
        public void TakeJob()
        {
            if (_selectedAvaliableJob == null)
                throw new NotImplementedException("Нельзя добавить должность");
            if (!IsConteinsJob(_selectedAvaliableJob))
                _emp.Jobs.Add(_selectedAvaliableJob.GetJob());                
            base.OnPropertyChanged("Jobs");
        }

        public void BackSpeciality()
        {
            if (_selectedSpeciality == null)
                throw new NotImplementedException("Нельзя вернуть специальность");
            if (IsConteinsSpeciality(_selectedSpeciality))
                _emp.Specialitys.Remove(_selectedSpeciality.GetSpeciality());
            base.OnPropertyChanged("Specialitys");
        }

        /// <summary>
        /// Add selected job from job Repository to job's List
        /// </summary>
        public void TakeSpeciality()
        {
            if (_selectedAvaliableSpeciality == null)
                throw new NotImplementedException("Нельзя добавить специальность");
            if (!IsConteinsSpeciality(_selectedAvaliableSpeciality))
                _emp.Specialitys.Add(_selectedAvaliableSpeciality.GetSpeciality());
            base.OnPropertyChanged("Specialitys");
        }

        /// <summary>
        /// Delete the job from the repository. 
        /// </summary>
        public void Remove()
        {
            if (!_emp.IsValid)
                throw new InvalidOperationException("Нельзя удалить");

            if (!this.IsNewEmp)
                _empRepository.RemoveEmp(_emp);
            base.OnPropertyChanged("DisplayName");
        }

        #endregion // Public Methods

        #region Private Helpers

                
        private void hideMessage()       ///////ВАТ
        {
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            _status = "";
            base.OnPropertyChanged("Status");
        } 

             /// <summary>
        /// Returns true if this Emp was created by the user and it has not yet
        /// been saved to the Emp repository.
        /// </summary>
        bool IsNewEmp
        {
            get { return !_empRepository.ContainsEmp(_emp); }
        }

        /// <summary>
        /// Returns true if the Emp is valid and can be saved.
        /// </summary>
        bool CanSave
        {
            get { return _emp.IsValid; }
        }

        /// <summary>
        /// Returns true if can add Job.
        /// </summary>
        bool CanTakeJob
        {
            get 
            {
                if (_selectedAvaliableJob != null && 
                    !IsConteinsJob(_selectedAvaliableJob))
                    return true;
                else 
                    return false;
            }
        }

        /// <summary>
        /// Returns true if can add Job.
        /// </summary>
        bool CanTakeSpeciality
        {
            get
            {
                if (_selectedAvaliableSpeciality != null &&
                    !IsConteinsSpeciality(_selectedAvaliableSpeciality))
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// check on conteins JobVM in Jobs Propertie of Emp
        /// </summary>
        /// <param name="job">JobViewModel</param>
        /// <returns>return true if Jobs Propertie conteins JobViewModel object</returns>
        bool IsConteinsJob(JobViewModel job)
        {
            foreach (JobViewModel aut in Jobs)
                if ((aut.ID == job.ID))
                    return true;
            return false;
        }
        

        /// <summary>
        /// check on conteins JOBVM
        /// </summary>

        bool IsConteinsSpeciality(SpecialityViewModel speciality)
        {
            foreach (SpecialityViewModel aut in Specialitys)
                if (aut.ID == speciality.ID)
                    return true;
            return false;
        }
       
        /// <summary>
        /// Returns true if can remove from Job' List.
        /// </summary>
        bool CanBackJob
        {
            get
            {
                if (_selectedJob != null && 
                   IsConteinsJob(_selectedJob))
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Returns true if can remove from Job' List.
        /// </summary>
        bool CanBackSpeciality
        {
            get
            {
                if (_selectedSpeciality != null &&
                   IsConteinsSpeciality(_selectedSpeciality))
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// уведомление об изменении
        /// </summary>
        void OnCollectionChangedJob(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (JobViewModel custVM in e.NewItems)
                    custVM.PropertyChanged += this.OnJobViewModelPropertyChanged;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (JobViewModel custVM in e.OldItems)
                    custVM.PropertyChanged -= this.OnJobViewModelPropertyChanged;
        }
        /// <summary>
        /// уведомление об изменении
        /// </summary>
        void OnCollectionChangedSpeciality(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (SpecialityViewModel custVM in e.NewItems)
                    custVM.PropertyChanged += this.OnSpecialityViewModelPropertyChanged;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (SpecialityViewModel custVM in e.OldItems)
                    custVM.PropertyChanged -= this.OnSpecialityViewModelPropertyChanged;
        }


        void OnJobViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string IsSelected = "IsSelected";

            (sender as JobViewModel).VerifyPropertyName(IsSelected);
        }

        void OnSpecialityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string IsSelected = "IsSelected";
            (sender as SpecialityViewModel).VerifyPropertyName(IsSelected);
        }

        void OnJobAddedToRepository(object sender, JobAddedEventArgs e)
        {
            var viewModel = new JobViewModel(e.NewJob, _jobRepository);
            this.AllJobs.Add(viewModel);
        }

        void OnSpecialityAddedToRepository(object sender, SpecialityAddedEventArgs e)
        {
            var viewModel = new SpecialityViewModel(e.NewSpeciality, _specialityRepository);
            this.AllSpecialitys.Add(viewModel);
        }


        #endregion // Private Helpers

        #region IDataErrorInfo Members

        string IDataErrorInfo.Error
        {
            get { return (_emp as IDataErrorInfo).Error; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string error =
                    error = (_emp as IDataErrorInfo)[propertyName];
                

                // Dirty the commands registered with CommandManager,
                // such as our Save command, so that they are queried
                // to see if they can execute now.
                CommandManager.InvalidateRequerySuggested();

                return error;
            }
        }
        
        #endregion // IDataErrorInfo Members
    }
}
