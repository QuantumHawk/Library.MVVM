using System;
using Library.MVVM.model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.MVVM.Data
{
  public  class SpecialityAddedEventArgs: EventArgs
    {
        public SpecialityAddedEventArgs(Speciality newSpeciality)
        {
            this.NewSpeciality = newSpeciality;
        }

        public Speciality NewSpeciality { get; private set; }
    }
}
