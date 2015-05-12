using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using Library.MVVM.Properties;
using System.Text.RegularExpressions;
using System.IO;

namespace Library.MVVM.model
{
    public class Job : IDataErrorInfo
    {
        #region Creation
        public static Job CreateNewJob()
        {
            return new Job();
        }

        //public static Job CreateJob(
        //    string job_name
        //   )
        //{
        //    return new Job
        //    {
        //        Job_name = job_name,

        //    };
        //}

        public Job()
        { }
        #endregion // Creation
        #region State Properties
        public int ID { get; set; }

        public string Job_name { get; set; }

        #endregion // State Properties

        #region IDataErrorInfo Members

        string IDataErrorInfo.Error { get { return null; } }
        string IDataErrorInfo.this[string propertyName]
        {
            get { return this.GetValidationError(propertyName); }
        }

        #endregion // IDataErrorInfo Members

        #region Validation
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
           "Name",       
   
        };

        string GetValidationError(string propertyName)
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
                return null;

            string error = null;

            switch (propertyName)
            {
                case "Name":
                    error = this.ValidatedJob_name();
                    break;


                default:
                    Debug.Fail("Unexpected property being validated on Job: " + propertyName);
                    break;
            }

            return error;
        }

        string ValidatedJob_name()
        {

            if (IsStringMissing(this.Job_name))
            {
                return "Введите значение";
            }

            if (IsCharValid(this.Job_name))
                return "Только кириллицей";

            return null;
        }


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
        #endregion // Validation
    }

}