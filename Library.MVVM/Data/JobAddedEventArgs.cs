using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.MVVM.model;

namespace Library.MVVM.Data
{
   public class JobAddedEventArgs :EventArgs
    {
         public JobAddedEventArgs(Job newJob)
        {
            this.NewJob = newJob;
        }

        public Job NewJob { get; private set; }
    }
}
