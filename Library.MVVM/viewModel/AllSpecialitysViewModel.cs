using Library.MVVM.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.MVVM.viewModel
{
    public class AllSpecialitysViewModel : WorkspaceViewModel
    {

        #region Fields

        readonly SpecialityRepository _specialityRepository;        

        #endregion // Fields
        
        #region Public Interface

        /// <summary>
        /// Returns a collection of all the specViewModel objects.
        /// </summary>
        public ObservableCollection<SpecialityViewModel> AllSpecialitys { get; private set; }

        public SpecialityViewModel SelectedSpeciality
        {
            get
            {
                SpecialityViewModel _selectedSpeciality = null;
                foreach (SpecialityViewModel aut in AllSpecialitys)
                    if (aut.IsSelected)
                    {
                        _selectedSpeciality = aut;
                        break;
                    }
                return _selectedSpeciality;
            }
        }


        public void RemoveSpeciality()
        {
            foreach (var aut in AllSpecialitys)
                if (aut.IsSelected)
                { 
                    aut.Remove();
                    AllSpecialitys.Remove(aut); 
                    break; 
                }
            base.OnPropertyChanged("DisplayName");
        }

        #endregion // Public Interface

        #region Constructor

        public AllSpecialitysViewModel(SpecialityRepository specialityRepository)
        {
            if (specialityRepository == null)
                throw new ArgumentNullException("specialityRepository");
            _specialityRepository = specialityRepository;

            // Subscribe for notifications of when a new spec is saved.
            _specialityRepository.SpecialityAdded += OnSpecialityAddedToRepository;

            // Populate the Allspecs collection with specViewModels.
            this.CreateAllSpecialitys();
            base.DisplayName = "Специальности";                        
        }

        void CreateAllSpecialitys()
        {
            List<SpecialityViewModel> all =
                (from cust in _specialityRepository.GetSpecialitys
                 select new SpecialityViewModel(cust, _specialityRepository)).ToList();

            foreach (SpecialityViewModel cvm in all)
                cvm.PropertyChanged += this.OnSpecialityViewModelPropertyChanged;

            this.AllSpecialitys = new ObservableCollection<SpecialityViewModel>(all);
            this.AllSpecialitys.CollectionChanged += this.OnCollectionChanged;
        }
        #endregion // Constructor

        #region  Base Class Overrides

        protected override void OnDispose()
        {
            foreach (SpecialityViewModel custVM in this.AllSpecialitys)
                custVM.Dispose();

            this.AllSpecialitys.Clear();
            this.AllSpecialitys.CollectionChanged -= this.OnCollectionChanged;

            _specialityRepository.SpecialityAdded -= OnSpecialityAddedToRepository;
        }

        #endregion // Base Class Overrides

        #region Event Handling Methods

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (SpecialityViewModel custVM in e.NewItems)
                    custVM.PropertyChanged += this.OnSpecialityViewModelPropertyChanged;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (SpecialityViewModel custVM in e.OldItems)
                    custVM.PropertyChanged -= this.OnSpecialityViewModelPropertyChanged;
        }
        /// <summary>
        /// уведомление об изменении свойства
        /// </summary>
        void OnSpecialityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string IsSelected = "IsSelected";

            // Make sure that the property name we're referencing is valid.
            // This is a debugging technique, and does not execute in a Release build.
            (sender as SpecialityViewModel).VerifyPropertyName(IsSelected);

            // When a customer is selected or unselected, we must let the
            // world know that the TotalSelectedSales property has changed,
            // so that it will be queried again for a new value.
            //if (e.PropertyName == IsSelected)
            //    this.OnPropertyChanged("AboutSelected");
        }

        /// <summary>
        /// подписка на изменение коллекции
        /// </summary>
        void OnSpecialityAddedToRepository(object sender, SpecialityAddedEventArgs e)
        {
            var viewModel = new SpecialityViewModel(e.NewSpeciality, _specialityRepository);
            this.AllSpecialitys.Add(viewModel);
        }

        #endregion // Event Handling Methods
    
    }
}
