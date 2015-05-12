using Library.MVVM.model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite.EF6;
using System.Data.SQLite.Linq;


namespace Library.MVVM.Data
{
    public class SpecialityEmp
    {
        string filename = "test.sqlite3";

        public List<int> LoadSpecialitys(Employee employee)
        {
            List<int> specialitys = new List<int>();
            WorkWithSQLite load = new WorkWithSQLite(filename);
            string commandText = " ID_spec";
            SQLiteDataReader r = load.Select("SpecEmp", commandText, "ID_emp = " + employee.ID);
            while (r.Read())
            {
                int speciality = r.GetInt32(0);
                specialitys.Add(speciality);
            }
            r.Close();
            load.Finish();
            return specialitys;
        }

        public void Save(Employee employee)
        {
            WorkWithSQLite save = new WorkWithSQLite(filename);
            string colums = string.Empty;
            save.Remove("SpecEmp", "ID_emp = " + employee.ID);
            foreach (Speciality speciality in employee.Specialitys)
            {
                colums = string.Format(" '{0}', '{1}' ", speciality.ID, employee.ID);
                save.Insert("SpecEmp (ID_spec, ID_emp)", colums);
            }
            save.Finish();
        }

        public void DeleteSpeciality(Speciality speciality)
        {
            WorkWithSQLite delete = new WorkWithSQLite(filename);
            delete.Remove("SpecEmp", "ID_spec = " + speciality.ID);
            delete.Finish();
        }

        public void DeleteEmp(Employee speciality)
        {
            WorkWithSQLite delete = new WorkWithSQLite(filename);
            delete.Remove("SpecEmp", "ID_emp = " + speciality.ID);
            delete.Finish();
        }
    }
}
