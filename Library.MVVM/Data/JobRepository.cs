using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.MVVM.model;
using System.Collections.ObjectModel;
using Library.MVVM.Data;

namespace Library.MVVM.Data
{
    public class JobRepository
    {
       #region Fields

        readonly ObservableCollection<Job> _job;
        //readonly string _filename = "Date/job.xml";

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Creates a new repository of job.

        public JobRepository()
        {            
            _job = new ObservableCollection<Job>(LoadJobs());
        }

        #endregion // Constructor

        #region Public Interface

        /// <summary>
        /// Raised when a job is placed into the repository.
        /// </summary>
        public event EventHandler<JobAddedEventArgs> JobAdded;

        /// <summary>
        /// Places the specified job into the repository.
        /// If the job is already in the repository, an
        /// exception is not thrown.
        /// </summary>
        public void AddJob(Job Job)
        {
            if (Job == null)
                throw new ArgumentNullException("Job");
            if (!_job.Contains(Job))
            {
                JobList aColl = new JobList();
                aColl.Save(Job);
                ObservableCollection<Job> LoaddJob = LoadJobs();
                Job job = LoaddJob.Last();
                Job.ID = job.ID;                
                _job.Add(Job);
                if (this.JobAdded != null)
                    this.JobAdded(this, new JobAddedEventArgs(Job));  
            }            
        }

        /// <summary>
        /// remove job from repository
        /// </summary>
        public void RemoveJob(Job Job)
        {
            if (Job == null)
                throw new ArgumentNullException("Job");
            if (_job.Contains(Job))
            {
                bool result = _job.Remove(Job);
                if (result)
                {
                    JobList aColl = new JobList();
                    JobEmployee JoE = new JobEmployee();
                    aColl.Delete(Job);
                    JoE.DeleteJob(Job);
                }
            }
        }
        
        /// <summary>
        /// Change job in repository
        /// </summary>
        public void ChangeJob(Job Job)
        {
            if (Job == null)
                throw new ArgumentNullException("Job");
            if (_job.Contains(Job))
            {

                JobList aColl = new JobList();
                aColl.Save(Job);
            }
        }


        /// <summary>
        /// Returns true if the specified job exists in the
        /// repository, or false if it is not.
        /// </summary>
        public bool ContainsJob(Job Job)
        {
            if (Job == null)
                throw new ArgumentNullException("Job");

            return _job.Contains(Job);
        }

        /// <summary>
        /// Returns a shallow-copied list of all job in the repository.
        /// </summary>
        public ObservableCollection<Job> GetJobs
        {
            get
            {
                return new ObservableCollection<Job>(_job);
            }
        }

        #endregion // Public Interface

        #region Private Helpers

        public ObservableCollection<Job> LoadJobs()
        {
            JobList aColl = new JobList();
            return new ObservableCollection<Job> (aColl.LoadJobs().ToList());
        }

        
        #endregion // Private Helpers
    }
}




