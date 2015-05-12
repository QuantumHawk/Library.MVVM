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
  
    public class Speciality : IDataErrorInfo
    {
        #region Creation

        public static Speciality CreateNewSpeciality()
        {
            return new Speciality();
        }

        protected Speciality()
        { }

        #endregion // Creation

        #region State Properties

        public Int32 ID { get; set; }
        public string Speciality_name { get; set; }

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
                    error = this.ValidatedSpeciality_name();
                    break;


                default:
                    Debug.Fail("Unexpected property being validated on Speciality: " + propertyName);
                    break;
            }

            return error;
        }

        string ValidatedSpeciality_name()
        {
            if (IsStringMissing(this.Speciality_name))
            {
                return "Введите значение";
            }

            if (IsCharValid(this.Speciality_name))
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


