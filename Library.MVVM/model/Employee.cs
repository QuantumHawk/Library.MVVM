using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Library.MVVM.Properties;
using System.IO;

namespace Library.MVVM.model
{
    [Serializable]
    public class Employee : IDataErrorInfo
    {

        #region Creation
        public static Employee CreateNewEmployee()
        {
            return new Employee();
        }

                protected Employee()
        {
            Jobs = new List<Job>();
            Specialitys = new List<Speciality>();
            
        }

        #endregion // Создание

                #region State Properties
                /// <summary>
                /// get/set id
                /// </summary>
        public Int32 ID { get; set; }
        /// <summary>
        /// get/set FirstName
        /// </summary>
        public string FirstName { get; set;}
        /// <summary>
        /// get/set SecondName
        /// </summary>     
        public string SecondName { get; set; }
        /// <summary>
        /// get/set LastName
        /// </summary>
        public string LastName { get; set; }
        public string Sex { get; set; }
        public DateTime Birthday { get; set; }
        public string Education { get; set; }
        /// <summary>
        /// Gets/sets ключи. Сотрудники могут занимать несколько должностей и иметь специальностей ,  
        /// поэтому эти значения находятся в отдельных классах
        /// </summary>
        public List<Job> Jobs { get; set; }
        public List<Speciality> Specialitys { get; set; }
        public Int32 Pasport { get; set; }
        public Int32 Salary { get; set; }
        public DateTime Date { get; set; }
       #endregion 

        #region IDataErrorInfo Errors
        string IDataErrorInfo.Error { get { return null; } }

        string IDataErrorInfo.this[string propertyName] 
        {
            get { return this.GetValidationError(propertyName); }
        }
        #endregion // IDataErrorInfo Members

        #region Validation
        /// <summary>
        /// Возвращает true если объект создан без ошибки.
        /// </summary>
        public bool IsValid
        {
            get
            {
                foreach (string property in ValidatedProperties)
                
                    if (GetValidationError(property) != null)
                        return false;

                    return true;
                
            }
        }

 
        static readonly string[] ValidatedProperties = 
        {
            "FirstName",
            "SecondName",
            "LastName",
            "Sex",
            "Birthday",
            "Education",
            "Jobs",
            "Specialitys",
            "Pasport",
            "Salary",
            "Date",
           
        };

        string GetValidationError(string propertyName)            
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
                return null;

            string error = null;

            switch (propertyName)
            {
                case "FirstName":
                    error = this.ValidatedFirstName();
                    break;

                case "SecondName":
                    error = this.ValidatedSecondName();
                    break;

                case "LastName":
                    error = this.ValidatedLastName();
                    break;

                case "Sex":
                    error = this.ValidatedSex();
                    break;

                case "Birthday":
                    error = this.ValidatedBirthday();
                    break;

                case "Education":
                    error = this.ValidatedEducation();
                    break;

                case "Jobs":
                    error = this.ValidatedJob();
                    break;

                case "Specialitys":
                    error = this.ValidatedSpeciality();
                    break;

                case "Pasport":
                    error = this.ValidatedPasport();
                    break;

                case "Salary":
                    error = this.ValidatedSalary();
                    break;

                case "Date":
                    error = this.ValidatedDate();
                    break;

                default:
                    Debug.Fail("Unexpected property being validated on Employee: " + propertyName);
                    break;
            }

            return error;
        }


        string ValidatedFirstName()
        {

            if (IsStringMissing(this.FirstName))
            {
                return "Введите значение";
            }

            if (IsCharValid(this.FirstName))
            {
                return "Только кириллицей";
            }
            return null;
        }

        string ValidatedSecondName()
        {

            if (IsStringMissing(this.SecondName))
            {
                return "Введите значение";
            }

            if (IsCharValid(this.SecondName))
            {
                return "Только кириллицей";
            }
            return null;
        }

        string ValidatedLastName()
        {

            if (IsStringMissing(this.LastName))
            {
                return "Введите значение";
            }

            if (IsCharValid(this.LastName))
            {
                return "Только кириллицей";
            }
            return null;

        }

        string ValidatedSex()
        {
            if (IsStringMissing(this.Sex))
            {
                return "Выберите значение";
            }
            return null;
        }

        string ValidatedBirthday()
        {
            if (!IsValidateDate (this.Birthday))
            {
                return "Нет даты";
            }
            return null;
        }
  
        string ValidatedEducation()
        {
            if (IsStringMissing(this.Education))
            {
                return "Выберите значение";
            }
            return null;
        }
        string ValidatedJob()
        {
            if (this.Jobs == null || this.Jobs.Count == 0)
            {
                return "Выберите значение";
            }
            return null;
        }

        string ValidatedSpeciality()
        {
            if (this.Specialitys == null || this.Specialitys.Count == 0)
            {
                return "Выберите значение";
            }
            return null;
        }

        string ValidatedPasport()
        {
            if (!IsValidatePasport(this.Pasport.ToString()))
            {
                return "Только 9 цифр";
            }
            return null;
        }

        string ValidatedSalary()
        {
            if (!IsValidateSalary(this.Salary))
            {
                return "Только целое значение";
            }
            return null;
        }


        string ValidatedDate()
        {
            if (!IsValidateDat(this.Date))
            {
                return "Нет даты";
            }
            return null;
        }

        //string ValidatedData()
        //{
        //    if (!IsValidDate(this.Date))
        //    {
        //        return Resources.Employee_Error_Missing_Data;
        //    }
        //    else if (!IsValidDate(this.Date))
        //    {
        //        return Resources.Employee_Error_Incorrect_Date;
        //    }
        //    return null;
        //}




        static bool IsStringMissing(string value)
        {
            return
               String.IsNullOrEmpty(value) ||
               value.Trim() == String.Empty;
        }

        static bool IsCharValid(string value)
        {
            char[] l = value.ToCharArray();
            foreach (char c in l)

                if (!((c < 'А' || c > 'я') && c != '\b' && c != '.'))
                {
                    return false;
                }


            return true;
        }

        //static bool IsValidatePasport(int value)
        //{
        //    try
        //    {
        //        Convert.ToInt32(value);
        //    }
        //    catch { return false; }

        //    if (Convert.ToInt32(value) < 9)
        //    {
        //        return true;
        //    } else
        //    {
        //        return false;
        //    }
        //}

        static bool IsValidateSalary(int value)
        {

            if (value > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //static bool IsValidatePasport(int value)
        //{
        //    try
        //    {
        //        Convert.ToDecimal(value);
        //    }
        //    catch { return false; }

        //    if (Convert.ToDecimal(value) > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        static bool IsValidatePasport(string value)
        {
            if (IsStringMissing(value))
                return false;

            string pattern = @"[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"; //количество знаков

            return Regex.IsMatch(value, pattern, RegexOptions.IgnoreCase);
        }
        
        static bool IsValidateDate(DateTime value)
        {
            if (value.Date.ToString("dd/MM/yyyy") == "01.01.0001" || !(value.Year < 1996) || !(value.Year > 1960))
            {
                return false;
            }
                else
            {
                return true;
            }
        
        }
        static bool IsValidateDat(DateTime value)
        {
            if (value.Date.ToString("dd/MM/yyyy") == "01.01.0001")
            {
                return false;
            }
            else
            {
                return value == DateTime.Today;
            }

        }

        #endregion // Валидация
    }
}
