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
    public class AllJobsViewModel : WorkspaceViewModel
    {

        #region Fields

        readonly JobRepository _jobRepository;        

        #endregion // Fields
        
        #region Public Interface

        /// <summary>
        /// Returns a collection of all the specViewModel objects.
        /// </summary>
        public ObservableCollection<JobViewModel> AllJobs { get; private set; }

        public JobViewModel SelectedJob
        {
            get
            {
                JobViewModel _selectedJob = null;
                foreach (JobViewModel aut in AllJobs)
                    if (aut.IsSelected)
                    {
                        _selectedJob = aut;
                        break;
                    }
                return _selectedJob;
            }
        }


        public void RemoveJob()
        {
            foreach (var aut in AllJobs)
                if (aut.IsSelected)
                { 
                    aut.Remove();
                    //aut.PropertyChanged -= this.OnspecViewModelPropertyChanged;
                    AllJobs.Remove(aut); 
                    break; 
                }
            base.OnPropertyChanged("DisplayName");
        }

        #endregion // Public Interface

        #region Constructor

        public AllJobsViewModel(JobRepository jobRepository)
        {
            if (jobRepository == null)
                throw new ArgumentNullException("jobRepository");
            _jobRepository = jobRepository;

            // Subscribe for notifications of when a new spec is saved.
            _jobRepository.JobAdded += OnJobAddedToRepository;

            // Populate the Allspec collection with specViewModels.
            this.CreateAllJobs();
            base.DisplayName = "Должности";                        
        }

        void CreateAllJobs()
        {
            List<JobViewModel> all =
                (from cust in _jobRepository.GetJobs
                 select new JobViewModel(cust, _jobRepository)).ToList();

            foreach (JobViewModel cvm in all)
                cvm.PropertyChanged += this.OnJobViewModelPropertyChanged;

            this.AllJobs = new ObservableCollection<JobViewModel>(all);
            this.AllJobs.CollectionChanged += this.OnCollectionChanged;
        }
        #endregion // Constructor

        #region  Base Class Overrides

        protected override void OnDispose()
        {
            foreach (JobViewModel custVM in this.AllJobs)
                custVM.Dispose();

            this.AllJobs.Clear();
            this.AllJobs.CollectionChanged -= this.OnCollectionChanged;

            _jobRepository.JobAdded -= OnJobAddedToRepository;
        }

        #endregion // Base Class Overrides

        #region Event Handling Methods

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (JobViewModel custVM in e.NewItems)
                    custVM.PropertyChanged += this.OnJobViewModelPropertyChanged;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (JobViewModel custVM in e.OldItems)
                    custVM.PropertyChanged -= this.OnJobViewModelPropertyChanged;
        }
        /// <summary>
        /// уведомление об изменении свойства
        /// </summary>
        void OnJobViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string IsSelected = "IsSelected";

            // Make sure that the property name we're referencing is valid.
            // This is a debugging technique, and does not execute in a Release build.
            (sender as JobViewModel).VerifyPropertyName(IsSelected);

            // When a customer is selected or unselected, we must let the
            // world know that the TotalSelectedSales property has changed,
            // so that it will be queried again for a new value.
            //if (e.PropertyName == IsSelected)
            //    this.OnPropertyChanged("AboutSelected");
        }

        /// <summary>
        /// подписка на изменение коллекции
        /// </summary>
        void OnJobAddedToRepository(object sender, JobAddedEventArgs e)
        {
            var viewModel = new JobViewModel(e.NewJob, _jobRepository);
            this.AllJobs.Add(viewModel);
        }

        #endregion // Event Handling Methods
    
    }
}
