using Library.MVVM.Data;
using Library.MVVM.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Library.MVVM.viewModel
{
    public class SpecialityViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        #region Fields

        readonly Speciality _speciality;
        readonly SpecialityRepository _specialityRepository;
       
        bool _isSelected;
        RelayCommand _saveCommand;
        string _status;
        System.Windows.Threading.DispatcherTimer dispatcherTimer;

        #endregion // Fields

        #region Constructor

        public SpecialityViewModel(Speciality speciality, SpecialityRepository specialityRepository)
        {
            if (speciality == null)
                throw new ArgumentNullException("Speciality");
            if (specialityRepository == null)
                throw new ArgumentNullException("SpecialityRepository");
            _speciality = speciality;
            _specialityRepository = specialityRepository;     
            _status = string.Empty;
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        }

        #endregion // Constructor

        #region Speciality Properties

        /// <summary>
        /// get/set ID of Speciality
        /// </summary>
        public int ID
        {
            get { return _speciality.ID; }
            set
            {
                if (value == _speciality.ID)
                    return;

                _speciality.ID = value;

                base.OnPropertyChanged("ID");
            }
        }

        /// <summary>
        /// get/set FirstName of spec
        /// </summary>
        public string Name
        {
            get { return _speciality.Speciality_name; }
            set
            {
                if (value == _speciality.Speciality_name)
                    return;

                _speciality.Speciality_name = value;

                base.OnPropertyChanged("Name");
            }
        }

        #endregion // Speciality Properties

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
        public Speciality GetSpeciality()
        {
            return _speciality;
        }

        
        /// <summary>
        /// список первых букв специальностей
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
        /// Gets Speciality Name for View
        /// </summary>
        public override string DisplayName
        {
            get
            {
                if (this.IsNewSpeciality)
                {
                    return "Добавить специальность";
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
        /// Gets/sets whether this Speciality is selected in the UI.
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
        /// Returns a command that saves the Speciality.
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
            if (!_speciality.IsValid)
                throw new InvalidOperationException("Cannot Save_Speciality");

            if (this.IsNewSpeciality)
            {
                _specialityRepository.AddSpeciality(_speciality);
                _status = "Специальность успешно добавлена";
            }
            else
            {
                _specialityRepository.ChangeSpeciality(_speciality);
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
            if (!_speciality.IsValid)
                throw new InvalidOperationException("Cannot Remove_Speciality");

            if (!this.IsNewSpeciality)
                _specialityRepository.RemoveSpeciality(_speciality);
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
        /// Returns true if this speciality was created by the user and it has not yet
        /// been saved to the spec repository.
        /// </summary>
        bool IsNewSpeciality
        {
            get { return !_specialityRepository.ContainsSpeciality(_speciality); }
        }

        /// <summary>
        /// Returns true if the speciality is valid and can be saved.
        /// </summary>
        bool CanSave
        {
            get { return _speciality.IsValid; }
        }

        #endregion // Private Helpers

        #region IDataErrorInfo Members

        string IDataErrorInfo.Error
        {
            get { return (_speciality as IDataErrorInfo).Error; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string error =
                    error = (_speciality as IDataErrorInfo)[propertyName];
                

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
