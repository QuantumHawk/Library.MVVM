using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.MVVM.model;
using System.Collections.ObjectModel;
using Library.MVVM.Data;


namespace Library.MVVM.Data
{
    public class SpecialityRepository
    {
        #region Fields

        readonly ObservableCollection<Speciality> _specialitys;
        //readonly string _filename = "Date/specs.xml";

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Creates a new repository of specs.

        public SpecialityRepository()
        {
            _specialitys = new ObservableCollection<Speciality>(LoadSpecialitys());
        }

        #endregion // Constructor

        #region Public Interface

        /// <summary>
        /// Raised when a spec is placed into the repository.
        /// </summary>
        public event EventHandler<SpecialityAddedEventArgs> SpecialityAdded;

        /// <summary>
        /// Places the specified spec into the repository.
        /// If the spec is already in the repository, an
        /// exception is not thrown.
        /// </summary>
        public void AddSpeciality(Speciality Speciality)
        {
            if (Speciality == null)
                throw new ArgumentNullException("Speciality");
            if (!_specialitys.Contains(Speciality))
            {
                SpecialityList aColl = new SpecialityList();
                aColl.Save(Speciality);
                ObservableCollection<Speciality> LoaddSpecialitys = LoadSpecialitys();
                Speciality aut = LoaddSpecialitys.Last();
                Speciality.ID = aut.ID;
                _specialitys.Add(Speciality);
                if (this.SpecialityAdded != null)
                    this.SpecialityAdded(this, new SpecialityAddedEventArgs(Speciality));  
            }            
        }

        /// <summary>
        /// remove spec from repository
        /// </summary>
        public void RemoveSpeciality(Speciality Speciality)
        {
            if (Speciality == null)
                throw new ArgumentNullException("Speciality");
            if (_specialitys.Contains(Speciality))
            {
                bool result = _specialitys.Remove(Speciality);
                if (result)
                {
                    SpecialityList aColl = new SpecialityList();
                    SpecialityEmp AoB = new SpecialityEmp();
                    aColl.Delete(Speciality);
                    AoB.DeleteSpeciality(Speciality);
                }
            }
        }
        
        /// <summary>
        /// Change spec in repository
        /// </summary>
        public void ChangeSpeciality(Speciality Speciality)
        {
            if (Speciality == null)
                throw new ArgumentNullException("Speciality");
            if (_specialitys.Contains(Speciality))
            {
            
                SpecialityList aColl = new SpecialityList();
                aColl.Save(Speciality);
            }
        }


        /// <summary>
        /// Returns true if the specified spec exists in the
        /// repository, or false if it is not.
        /// </summary>
        public bool ContainsSpeciality(Speciality Speciality)
        {
            if (Speciality == null)
                throw new ArgumentNullException("Speciality");

            return _specialitys.Contains(Speciality);
        }

        /// <summary>
        /// Returns a shallow-copied list of all spec in the repository.
        /// </summary>
        public ObservableCollection<Speciality> GetSpecialitys
        {
            get
            {
                return new ObservableCollection<Speciality>(_specialitys);
            }
        }


        public ObservableCollection<Speciality> LoadSpecialitys()
        {
            SpecialityList aColl = new SpecialityList();
            return new ObservableCollection<Speciality>(aColl.LoadSpecialitys().ToList());
        }

        #endregion // Public Interface
    }
}
