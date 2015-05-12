using Library.MVVM.model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.MVVM.Data
{
 public   class JobEmployee
    {
     string filename = "test.sqlite3";

        public List<int> LoadJobs(Employee employee)
        {
            List<int> jobs = new List<int>();
            WorkWithSQLite load = new WorkWithSQLite(filename);
            string commandText = " ID_job ";
            SQLiteDataReader r = load.Select("JobEmp", commandText, "ID_emp = " + employee.ID);
            while (r.Read())
            {
                int job = r.GetInt32(0);
                jobs.Add(job);
            }
            r.Close();
            load.Finish();
            return jobs;
        }

        public void Save(Employee employee)
        {
            WorkWithSQLite save = new WorkWithSQLite(filename);
            string colums = string.Empty;
            save.Remove("JobEmp", "ID_emp = " + employee.ID);
            foreach (Job job in employee.Jobs)
            {
                colums = string.Format(" '{0}', '{1}' ", job.ID, employee.ID);
                save.Insert("JobEmp (ID_job, ID_emp)", colums);
            }
            save.Finish();
        }

        public void DeleteJob(Job job)
        {
            WorkWithSQLite delete = new WorkWithSQLite(filename);
            delete.Remove("JobEmp", "ID_job = " + job.ID);
            delete.Finish();
        }

        public void DeleteEmp(Employee job)
        {
            WorkWithSQLite delete = new WorkWithSQLite(filename);
            delete.Remove("JobEmp", "ID_emp = " + job.ID);
            delete.Finish();
        }
    }
}
