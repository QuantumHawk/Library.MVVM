using System;
using Library.MVVM.model;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Library.MVVM.Data
{
    public class SpecialityList : List<Speciality>
    {
        string filename = "test.sqlite3";
        public List<Speciality> LoadSpecialitys()
        {
            List<Speciality> specialitys = new List<Speciality>();
            WorkWithSQLite load = new WorkWithSQLite(filename);
            string commandText = " Name, ID ";
            SQLiteDataReader r = load.Select("Speciality", commandText, "");
            if (r != null)
            {
                while (r.Read())
                {
                    Speciality speciality = Speciality.CreateNewSpeciality();
                    speciality.Speciality_name = r.GetString(0);
                    speciality.ID = r.GetInt32(1);
                    specialitys.Add(speciality);
                }
                r.Close();
            }
            load.Finish();
            return specialitys;
        }

        public void Save(Speciality speciality)
        {
            WorkWithSQLite save = new WorkWithSQLite(filename);
            string colums = string.Empty;
            if (speciality.ID == 0)
            {
                colums = string.Format(" '{0}'",
                    speciality.Speciality_name);
                save.Insert("Speciality (Name)", colums);
            }
            else
            {
                colums = string.Format(" Name = '{0}'",
                    speciality.Speciality_name);
                save.Update("Speciality", colums, "ID = " + speciality.ID);
            }
            save.Finish();
        }

        public void Delete(Speciality speciality)
        {
            WorkWithSQLite delete = new WorkWithSQLite(filename);
            delete.Remove("Speciality", "ID = " + speciality.ID);
            delete.Finish();
        }
    }
}

