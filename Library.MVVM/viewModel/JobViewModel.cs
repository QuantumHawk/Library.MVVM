using Library.MVVM.Data;
using Library.MVVM.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Library.MVVM.viewModel
{
    public class JobViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        #region Fields

        readonly Job _job;
        readonly JobRepository _jobRepository;
        bool _isSelected;
        RelayCommand _saveCommand;
        string _status;
        System.Windows.Threading.DispatcherTimer dispatcherTimer;

        #endregion // Fields

        #region Constructor

        public JobViewModel(Job job, JobRepository jobRepository)
        {
            if (job == null)
                throw new ArgumentNullException("Job");
            if (jobRepository == null)
                throw new ArgumentNullException("JobRepository");
            _job = job;
            _jobRepository = jobRepository;     
            _status = string.Empty;
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        }

        #endregion // Constructor

        #region Job Properties

        /// <summary>
        /// get/set ID of job
        /// </summary>
        public int ID
        {
            get { return _job.ID; }
            set
            {
                if (value == _job.ID)
                    return;

                _job.ID = value;

                base.OnPropertyChanged("ID");
            }
        }

        /// <summary>
        /// get/set FirstName of spec
        /// </summary>
        public string Name
        {
            get { return _job.Job_name; }
            set
            {
                if (value == _job.Job_name)
                    return;

                _job.Job_name = value;

                base.OnPropertyChanged("Name");
            }
        }

        #endregion // spec Properties

        #region Presentation Properties

        public string Status
        {
            get
            {
                return _status;
            }
        }

        /// <summary>
        /// get spec
        /// </summary>
        /// <returns>spec as model</returns>
        public Job GetJob()
        {
            return _job;
        }

      
        /// </summary>
        public string LetterABC
        {
            get 
            {
                string letter = string.Empty;
                if (Name.Length > 0)
                    letter += Name[0];
                return letter;
            }
        }

        /// <summary>
        /// Gets spec Name for View
        /// </summary>
        public override string DisplayName
        {
            get
            {
                if (this.IsNewJob)
                {
                    return "Добавить должность";
                }
                else
                   return GetsName;
            }
        }

        /// <summary>
        /// Gets autheors Name in format : lastname + firstName or lastName A. A.
        /// </summary>
        public string GetsName
        {
            get
            {
                return Name;
            }
        }

        /// <summary>
        /// Gets/sets whether this spec is selected in the UI.
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
        /// Returns a command that saves the spec.
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


        #endregion // Presentation Properties

        #region Public Methods

        /// <summary>
        /// Saves the spec to the repository.  This method is invoked by the SaveCommand.
        /// </summary>
        public void Save()
        {
            if (!_job.IsValid)
                throw new InvalidOperationException("Ошибка");

            if (this.IsNewJob)
            {
                _jobRepository.AddJob(_job);
                _status = "Должность успешно добавлена";
            }
            else
            {
                _jobRepository.ChangeJob(_job);
                _status = "изменения сохранены";
            }
            base.OnPropertyChanged("DisplayName");
            base.OnPropertyChanged("Status");
            hideMessage();
        }

        /// <summary>
        /// Delete the spec from the repository. 
        /// </summary>
        public void Remove()
        {
            if (!_job.IsValid)
                throw new InvalidOperationException("Cannot Remove_Job");

            if (!this.IsNewJob)
                _jobRepository.RemoveJob(_job);
            base.OnPropertyChanged("DisplayName");
        }

        #endregion // Public Methods

        #region Private Helpers

        private void hideMessage()
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
        /// Returns true if this spec was created by the user and it has not yet
        /// been saved to the spec repository.
        /// </summary>
        bool IsNewJob
        {
            get { return !_jobRepository.ContainsJob(_job); }
        }

        /// <summary>
        /// Returns true if the spec is valid and can be saved.
        /// </summary>
        bool CanSave
        {
            get { return _job.IsValid; }
        }

        #endregion // Private Helpers

        #region IDataErrorInfo Members

        string IDataErrorInfo.Error
        {
            get { return (_job as IDataErrorInfo).Error; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string error =
                    error = (_job as IDataErrorInfo)[propertyName];
                

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
