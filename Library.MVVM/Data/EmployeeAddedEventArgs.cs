using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.MVVM.model;

namespace Library.MVVM.Data
{
   public class EmployeeAddedEventArgs : EventArgs
    {
        public EmployeeAddedEventArgs(Employee newEmp)
        {
            this.NewEmp = newEmp;
        }

        public Employee NewEmp { get; private set; }
    }
}
