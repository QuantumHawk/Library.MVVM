using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using Library.MVVM.model;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.Data.SQLite.Linq;
using Library.MVVM.Data;

namespace Library.MVVM.Data
{
    public class EmployeeList : List<Employee>
    {
        string filename = "test.sqlite3";
        public List<Employee> LoadEmployees()
        {
            List<Employee> employees = new List<Employee>();
            WorkWithSQLite load = new WorkWithSQLite(filename);
            string commandText = " FirstName, SecondName, LastName, Sex, Birthday, Education, Pasport,Salary, Date, ID";
            SQLiteDataReader r = load.Select("Employee", commandText, "");
            if (r != null)
            {
                while (r.Read())
                {
                    Employee employee = Employee.CreateNewEmployee();
                    employee.FirstName = r.GetString(0);
                    employee.SecondName = r.GetString(1);
                    employee.LastName = r.GetString(2);
                    employee.Sex = r.GetString(3);
                    employee.Birthday = Convert.ToDateTime(r["Birthday"]);
                    employee.Education = r.GetString(5);
                    employee.Pasport = r.GetInt32(6);
                    employee.Salary = r.GetInt32(7);
                    employee.Date = Convert.ToDateTime(r["Date"]);
                    employee.ID = r.GetInt32(9);
                    employees.Add(employee);
                }
                r.Close();
            }
            load.Finish();
            return employees;
        }

        public void Save(Employee employee)
        {
            WorkWithSQLite save = new WorkWithSQLite(filename);
            string colums = string.Empty;
            if (employee.ID == 0)
            {
                colums = string.Format(" '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}','{8}'",
                    employee.FirstName, employee.SecondName, employee.LastName, employee.Sex, employee.Birthday, employee.Education, employee.Pasport, employee.Salary, employee.Date);
                save.Insert("Employee (FirstName, SecondName, LastName, Sex, Birthday, Education, Pasport, Salary,  Date)", colums);
            }
            else
            {
                colums = string.Format(" FirstName = '{0}', SecondName = '{1}', LastName = '{2}', Sex = '{3}', Birthday = '{4}', Education = '{5}', Pasport = '{6}', Salary = '{7}', Date = '{8}'",
                      employee.FirstName, employee.SecondName, employee.LastName, employee.Sex, employee.Birthday, employee.Education, employee.Pasport, employee.Salary, employee.Date);
                save.Update("Employee", colums, "ID = " + employee.ID);
            }
            save.Finish();
        }


        public void Delete(Employee employee)
        {
            WorkWithSQLite delete = new WorkWithSQLite(filename);
            delete.Remove("Employee", "ID = " + employee.ID);
            delete.Finish();
        }
    }
}
