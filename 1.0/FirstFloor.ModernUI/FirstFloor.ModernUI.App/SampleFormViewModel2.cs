using FirstFloor.ModernUI.Presentation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FirstFloor.ModernUI.App
{
    public class SampleFormViewModel2
        : NotifyPropertyChanged, IDataErrorInfo
    {
        private FormModel _data;
        private string _firstName = "George";
        private string _lastName = "";
        private string _address = "";

        private bool _firstNameInvalid = false;
        private bool _lastNameInvalid = false;
        private bool _addressInvalid = false;

        private int _pageNumber = 1;
        private ICommand _previousCommand, _nextCommand, _saveCommand, _invalidCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                        param => this.SaveObjectAndGoNext()
                    );
                }
                return _saveCommand;
            }
        }
        public ICommand NextCommand
        {
            get
            {
                if (_nextCommand == null)
                {
                    _nextCommand = new RelayCommand(
                        param => this.GoNext()
                    );
                }
                return _nextCommand;
            }
        }

        public ICommand PreviousCommand
        {
            get
            {
                if (_previousCommand == null)
                {
                    _previousCommand = new RelayCommand(
                        param => this.GoBack()
                    );
                }
                return _previousCommand;
            }
        }
        public SampleFormViewModel2()
        {
            _data = new FormModel(LastName, FirstName, Address);
        }
        private void SaveObjectAndGoNext()
        {
            _data = new FormModel(LastName, FirstName, Address);
            _data.SetInvalid(LastNameInvalid, FirstNameInvalid, AddressInvalid);
            string prefix = @".\" + LastName + "_" + FirstName;

            string fileName = prefix + "\\" + prefix + "_" + PageNumber + ".txt";
            System.IO.Directory.CreateDirectory(prefix);
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(fileName))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, _data);
            }
        }
        private void GoBack()
        {
            GetForm(false);
        }
        private void GoNext()
        {
            GetForm(true);
        }
        private void GetForm(bool goNext)
        {
            if (goNext)
            {
                if (PageNumber < 67)
                {
                    ++PageNumber;
                }

            }
            else
            {
                if (PageNumber > 1)
                {
                    --PageNumber;
                }
            }
            string prefix = @".\" + LastName + "_" + FirstName;
            string fileName = prefix + "\\" + prefix + "_" + PageNumber + ".txt";
            bool foundForm = false;
            try
            {
                // deserialize JSON directly from a file
                using (StreamReader file = File.OpenText(fileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    FormModel form = (FormModel)serializer.Deserialize(file, typeof(FormModel));

                    SetModel(form);
                    foundForm = true;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (!foundForm)
                {
                    InitFields();
                }
            }
        }
        private void InitFields()
        {
            FirstName = "";
            LastName = "";
            Address = "";
            FirstNameInvalid = false;
            LastNameInvalid = false;
            AddressInvalid = false;
        }
        private void SetModel(FormModel form)
        {
            FirstName = form.firstName;
            LastName = form.lastName;
            Address = form.address;

            FirstNameInvalid = form.firstNameInvalid;
        }
        public int PageNumber
        {
            get { return this._pageNumber; }
            set
            {
                if (this._pageNumber != value)
                {
                    this._pageNumber = value;
                    OnPropertyChanged("PageNumber");
                }
            }
        }
        public bool LastNameInvalid
        {
            get { return this._lastNameInvalid; }
            set
            {
                if (this._lastNameInvalid != value)
                {
                    this._lastNameInvalid = value;
                    OnPropertyChanged("LastNameInvalid");
                }
            }
        }
        public bool FirstNameInvalid
        {
            get { return this._firstNameInvalid; }
            set
            {
                if (this._firstNameInvalid != value)
                {
                    this._firstNameInvalid = value;
                    OnPropertyChanged("FirstNameInvalid");
                }
            }
        }
        public bool AddressInvalid
        {
            get { return this._addressInvalid; }
            set
            {
                if (this._addressInvalid != value)
                {
                    this._addressInvalid = value;
                    OnPropertyChanged("AddressInvalid");
                }
            }
        }
        public string FirstName
        {
            get { return this._firstName; }
            set
            {
                if (this._firstName != value)
                {
                    this._firstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        public string LastName
        {
            get { return this._lastName; }
            set
            {
                if (this._lastName != value)
                {
                    this._lastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        public string Address
        {
            get { return this._address; }
            set
            {
                if (this._address != value)
                {
                    this._address = value;
                    OnPropertyChanged("Address");
                }
            }
        }


        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "FirstName")
                {
                    return string.IsNullOrEmpty(this._firstName) ? "Required value" : null;
                }
                if (columnName == "LastName")
                {
                    return string.IsNullOrEmpty(this._lastName) ? "Required value" : null;
                }
                return null;
            }
        }
        private class FormModel
        {
            Dictionary<string, bool> _invalidValues = new Dictionary<string, bool>();

            public FormModel(string firstname, string lastname, string address)
            {
                firstName = firstname;
                lastName = lastname;
                this.address = address;
            }

            public string firstName { get; set; }
            public string lastName { get; set; }
            public string address { get; set; }

            public bool firstNameInvalid { get; set; }
            public bool lastNameInvalid { get; set; }
            public bool addressInvalid { get; set; }

            public void AddInvalidValues(string key)
            {
                _invalidValues.Add(key, true);
            }

            public void removeInvalidValues(string key)
            {
                _invalidValues.Add(key, false);
            }

            public void SetInvalid(bool ln, bool fn, bool ad)
            {
                firstNameInvalid = fn;
                lastNameInvalid = ln;
                addressInvalid = ad;
            }
        }
    }
}
