using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using Library.MVVM.model;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.Data.SQLite.Linq;

namespace Library.MVVM.Data
{
    public class JobList : List<Job>
    {
        string filename = "test.sqlite3";
        public List<Job> LoadJobs()
        {
            List<Job> jobs = new List<Job>();
            WorkWithSQLite load = new WorkWithSQLite(filename);
            string commandText = " Name, Id ";
            SQLiteDataReader r = load.Select("Job", commandText, "");
            if (r != null)
            {
                while (r.Read())
                {
                    Job job = Job.CreateNewJob();
                    job.Job_name = r.GetString(0);
                    job.ID = r.GetInt32(1);
                    jobs.Add(job);
               
                }
                r.Close();
            }
            load.Finish();
            return jobs;
        }

        public void Save(Job job)
        {
            WorkWithSQLite save = new WorkWithSQLite(filename);
            string colums = string.Empty;
            if (job.ID == 0)
            {
                colums = string.Format(" '{0}'",
                    job.Job_name);
                save.Insert("Job (Name)", colums);
            }
            else
            {
                colums = string.Format(" Name = '{0}'",
                    job.Job_name);
                save.Update("Job", colums, "Id = " + job.ID);
            }
            save.Finish();
        }
        //public static bool CheckIsDataAlreadyInDBorNot(String TableName,
        //String dbfield, String fieldValue)
        //{
        //    WorkWithSQLite sqldb = ;
        //    String Query = "Select * from " + TableName + " where " + dbfield + " = " + fieldValue;
        //    Cursor cursor = sqldb.rawQuery(Query, null);
        //    if (cursor.getCount() <= 0)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        public void Delete(Job job)
        {
            WorkWithSQLite delete = new WorkWithSQLite(filename);
            delete.Remove("Job", "Id = " + job.ID);
            delete.Finish();
        }
    }
}

