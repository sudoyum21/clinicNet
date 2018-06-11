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
    public class SampleFormViewModel
        : NotifyPropertyChanged, IDataErrorInfo
    {
        private readonly int MaxPage = 4;
        private FormModel _data;
        private string _firstName = "";
        private string _lastName ="";
        private string _address = "";
        private string _codePostal = "";
        private string _phone = "";
        private string _nas = "";
        private string _nam = "";
        private string _spoken = "";
        private string _written = "";
        private string _srcRef = "";
        private string _ddn = "";

        private string _user = "";
        private string _invalidPages = "";
        
        //private string _firstName = "";
        //private string _lastName = "";
        //private string _address = "";

        private bool _firstNameInvalid = false;
        private bool _lastNameInvalid = false;
        private bool _addressInvalid = false;
        private bool _codePostalInvalid = false;
        private bool _phoneInvalid = false;
        private bool _nasInvalid = false;
        private bool _namInvalid = false;
        private bool _spokenInvalid = false;
        private bool _writtenInvalid = false;
        private bool _srcRefInvalid = false;
        private bool _ddnInvalid = false;

        private int _pageNumber = 1;
        private ICommand _previousCommand, _nextCommand, _saveCommand, _openAdminCommand, _saveAdminCommand, _clearAdminCommand, _validCommand;
        
        public ICommand ClearAdminCommand
        {
            get
            {
                if (_clearAdminCommand == null)
                {
                    _clearAdminCommand = new RelayCommand(
                        param => this.InitFields()
                    );
                }
                return _clearAdminCommand;
            }
        }
        public ICommand OpenAdminCommand
        {
            get
            {
                if (_openAdminCommand == null)
                {
                    _openAdminCommand = new RelayCommand(
                        param => this.GetForm(true,1,true)
                    );
                }
                return _openAdminCommand;
            }
        }
        public ICommand SaveAdminCommand
        {
            get
            {
                if (_saveAdminCommand == null)
                {
                    _saveAdminCommand = new RelayCommand(
                        param => this.SaveObjectAndGoNext(true)
                    );
                }
                return _saveAdminCommand;
            }
        }
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
        public ICommand ValidateCommand
        {
            get
            {
                if (_validCommand == null)
                {
                    _validCommand = new RelayCommand(
                        param => this.Validate()
                    );
                }
                return _validCommand;
            }
        }
        public SampleFormViewModel()
        {
            _data = new FormModel(LastName, FirstName, Address, CodePostal, Phone, Nas, Nam, Spoken, Written, SrcRef, Ddn, User);
        }
        public void SaveObjectAndGoNext(bool admin = false)
        {
            _data = new FormModel(LastName, FirstName, Address, CodePostal, Phone, Nas, Nam, Spoken, Written, SrcRef, Ddn, User);
            _data.SetInvalid(LastNameInvalid, FirstNameInvalid, AddressInvalid, CodePostalInvalid, PhoneInvalid, NasInvalid, NamInvalid, SpokenInvalid
                ,WrittenInvalid, SrcRefInvalid, DdnInvalid);
            string prefix = @".\";
            if (!admin)
            {
                User = SingletonUser.user.ToString();
            }
            prefix += User;      
            string fileName = prefix + "\\" + PageNumber;
            if (admin)
            {
                fileName += "original_";
                //SingletonUser.user = User;
            }
            //else
            //{
            //    User = SingletonUser.user.ToString();
            //}
            fileName += ".txt"; 
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
        private void Validate()
        {
            InvalidPages = "";
            FormModel formOriginal = null, formToCheck = null;
            string prefix = @".\";
            prefix += User;
            string fileName = prefix + "\\" + PageNumber;
            fileName += "original_";      
            fileName += ".txt";
            try
            {
                // deserialize JSON directly from a file
                using (StreamReader file = File.OpenText(fileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    formOriginal = (FormModel)serializer.Deserialize(file, typeof(FormModel));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            if(formOriginal != null)
            {
             
                for (int i = 1; i < MaxPage; ++i)
                {
                    bool errorPresent = false;
                    prefix = @".\";
                    prefix += User;
                    fileName = prefix + "\\" + i;
                    fileName += ".txt";
                    try
                    {
                        // deserialize JSON directly from a file
                        using (StreamReader file = File.OpenText(fileName))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            formToCheck = (FormModel)serializer.Deserialize(file, typeof(FormModel));

                            formToCheck.addressInvalid = formToCheck.address != formOriginal.address;
                            errorPresent |= formToCheck.addressInvalid;
                            formToCheck.codePostalInvalid = formToCheck.codePostal != formOriginal.codePostal;
                            errorPresent |= formToCheck.codePostalInvalid;
                            formToCheck.ddnInvalid = formToCheck.ddn != formOriginal.ddn;
                            errorPresent |= formToCheck.ddnInvalid;
                            formToCheck.firstNameInvalid = formToCheck.firstName != formOriginal.firstName;
                            errorPresent |= formToCheck.firstNameInvalid;
                            formToCheck.lastNameInvalid = formToCheck.lastName != formOriginal.lastName;
                            errorPresent |= formToCheck.lastNameInvalid;
                            formToCheck.namInvalid = formToCheck.nam != formOriginal.nam;
                            errorPresent |= formToCheck.namInvalid;
                            formToCheck.nasInvalid = formToCheck.nas != formOriginal.nas;
                            errorPresent |= formToCheck.nasInvalid;
                            formToCheck.phoneInvalid = formToCheck.phone != formOriginal.phone;
                            errorPresent |= formToCheck.phoneInvalid;
                            formToCheck.spokenInvalid = formToCheck.spoken != formOriginal.spoken;
                            errorPresent |= formToCheck.spokenInvalid;
                            formToCheck.writtenInvalid = formToCheck.written != formOriginal.written;
                            errorPresent |= formToCheck.writtenInvalid;

                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        errorPresent = true;
                    }
                    if (errorPresent)
                    {
                        if(i < MaxPage-1)
                        {
                            InvalidPages += i + ",";
                        } else
                        {
                            InvalidPages += i;
                        }
                        string pr = @".\";
                        pr += User;
                        string fn = pr + "\\" + i;
                        fn += ".txt";
                        System.IO.Directory.CreateDirectory(prefix);
                        JsonSerializer serializer2 = new JsonSerializer();
                        using (StreamWriter sw = new StreamWriter(fileName))
                        using (JsonWriter writer = new JsonTextWriter(sw))
                        {
                            serializer2.Serialize(writer, formToCheck);
                        }
                    }               
                }
            }
            
            
        }

        private void GetForm(bool goNext, int pageNo = 0, bool admin = false)
        {
            InitFields();
            if(pageNo > 0)
            {
                PageNumber = pageNo;
            }
            else
            {
                if (goNext)
                {
                    if (PageNumber < MaxPage)
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
            }
            string prefix = @".\";
            if (!admin)
            {
               User = SingletonUser.user.ToString();
            } 
            prefix += User;
            string fileName = prefix + "\\" + PageNumber;
            if (admin)
            {
                fileName += "original_";
                //SingletonUser.user = User.ToString();
            } 
            fileName += ".txt";
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
        private void InitFields(bool admin = false)
        {
            bool valueBool = false;
            string valueStr = "";
            FirstName = valueStr;
            LastName = valueStr;
            Address = valueStr;
            CodePostal = valueStr;
            Phone = valueStr;
            Nas = valueStr;
            Nam = valueStr;
            Spoken = valueStr;
            Written = valueStr;
            SrcRef = valueStr;
            Ddn = valueStr;
            //User = valueStr;
            //if (admin)
            //{
            //    SingletonUser.user = User;
            //}
     

            FirstNameInvalid = valueBool;
            LastNameInvalid = valueBool;
            AddressInvalid = valueBool;
            CodePostalInvalid = valueBool;
            PhoneInvalid = valueBool;
            NasInvalid = valueBool;
            NamInvalid = valueBool;
            SpokenInvalid = valueBool;
            WrittenInvalid = valueBool;
            SrcRefInvalid = valueBool;
            DdnInvalid = valueBool;
        }
        private void SetModel(FormModel form)
        {
            FirstName = form.firstName;
            LastName = form.lastName;
            Address = form.address;
            CodePostal = form.codePostal;
            Phone = form.phone;
            Nas = form.nas;
            Nam = form.nam;          
            Spoken = form.spoken;
            Written = form.written;
            SrcRef = form.srcRef;
            Ddn = form.ddn;
            //User = form.user;

            FirstNameInvalid = form.firstNameInvalid;
            LastNameInvalid = form.lastNameInvalid;
            AddressInvalid = form.addressInvalid;
            CodePostalInvalid = form.codePostalInvalid;
            PhoneInvalid = form.phoneInvalid;
            NasInvalid = form.nasInvalid;
            NamInvalid = form.namInvalid;
            SpokenInvalid = form.spokenInvalid;
            WrittenInvalid = form.writtenInvalid;
            SrcRefInvalid = form.srcRefInvalid;
            DdnInvalid = form.ddnInvalid;
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
        public string InvalidPages
        {
            get { return this._invalidPages; }
            set
            {
                if (this._invalidPages != value)
                {
                    this._invalidPages = value;
                    OnPropertyChanged("InvalidPages");
                }
            }
        }
        public string User
        {
            get { return this._user; }
            set
            {
                if (this._user != value)
                {
                    this._user = value;
                    SingletonUser.user = value.ToString();
                    OnPropertyChanged("User");
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
        public bool CodePostalInvalid
        {
            get { return this._codePostalInvalid; }
            set
            {
                if (this._codePostalInvalid != value)
                {
                    this._codePostalInvalid = value;
                    OnPropertyChanged("CodePostalInvalid");
                }
            }
        }
        public bool PhoneInvalid
        {
            get { return this._phoneInvalid; }
            set
            {
                if (this._phoneInvalid != value)
                {
                    this._phoneInvalid = value;
                    OnPropertyChanged("PhoneInvalid");
                }
            }
        }
        public bool NasInvalid
        {
            get { return this._nasInvalid; }
            set
            {
                if (this._nasInvalid != value)
                {
                    this._nasInvalid = value;
                    OnPropertyChanged("NasInvalid");
                }
            }
        }
        public bool NamInvalid
        {
            get { return this._namInvalid; }
            set
            {
                if (this._namInvalid != value)
                {
                    this._namInvalid = value;
                    OnPropertyChanged("NamInvalid");
                }
            }
        }
        public bool SpokenInvalid
        {
            get { return this._spokenInvalid; }
            set
            {
                if (this._spokenInvalid != value)
                {
                    this._spokenInvalid = value;
                    OnPropertyChanged("SpokenInvalid");
                }
            }
        }
        public bool WrittenInvalid
        {
            get { return this._writtenInvalid; }
            set
            {
                if (this._writtenInvalid != value)
                {
                    this._writtenInvalid = value;
                    OnPropertyChanged("WrittenInvalid");
                }
            }
        }
        public bool SrcRefInvalid
        {
            get { return this._srcRefInvalid; }
            set
            {
                if (this._srcRefInvalid != value)
                {
                    this._srcRefInvalid = value;
                    OnPropertyChanged("SrcRefInvalid");
                }
            }
        }
        public bool DdnInvalid
        {
            get { return this._ddnInvalid; }
            set
            {
                if (this._ddnInvalid != value)
                {
                    this._ddnInvalid = value;
                    OnPropertyChanged("DdnInvalid");
                }
            }
        }
        public string Ddn
        {
            get { return this._ddn; }
            set
            {
                if (this._ddn != value)
                {
                    this._ddn = value;
                    OnPropertyChanged("Ddn");
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
        public string CodePostal
        {
            get { return this._codePostal; }
            set
            {
                if (this._codePostal != value)
                {
                    this._codePostal = value;
                    OnPropertyChanged("CodePostal");
                }
            }
        }

        public string Phone
        {
            get { return this._phone; }
            set
            {
                if (this._phone != value)
                {
                    this._phone = value;
                    OnPropertyChanged("Phone");
                }
            }
        }

        public string Nas
        {
            get { return this._nas; }
            set
            {
                if (this._nas != value)
                {
                    this._nas = value;
                    OnPropertyChanged("Nas");
                }
            }
        }
        public string Nam
        {
            get { return this._nam; }
            set
            {
                if (this._nam != value)
                {
                    this._nam = value; 
                    OnPropertyChanged("Nam");
                }
            }
        }

        public string Spoken
        {
            get { return this._spoken; }
            set
            {
                if (this._spoken != value)
                {
                    this._spoken = value;
                    OnPropertyChanged("Spoken");
                }
            }
        }

        public string Written
        {
            get { return this._written; }
            set
            {
                if (this._written != value)
                {
                    this._written = value;
                    OnPropertyChanged("Written");
                }
            }
        }


        public string SrcRef
        {
            get { return this._srcRef; }
            set
            {
                if (this._srcRef != value)
                {
                    this._srcRef = value;
                    OnPropertyChanged("SrcRef");
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
        public class FormModel
        {
            Dictionary<string, bool> _invalidValues = new Dictionary<string, bool>();

            public FormModel(string lastname, string firstname, string address, string code, string phone, string nas
                , string nam, string sp, string wr, string src, string ddn, string user)
            {
                this.firstName = firstname;
                lastName = lastname;
                this.address = address;
                codePostal = code;
                this.phone = phone;
                this.nas = nas;
                this.nam = nam;
                spoken = sp;
                written = wr;
                srcRef = src;
                this.ddn = ddn;
                this.user = user;
            }

            public string firstName { get; set; }
            public string lastName { get; set; }
            public string address { get; set; }
            public string codePostal { get; set; }
            public string phone { get; set; }
            public string nas { get; set; }
            public string nam { get; set; }
            public string spoken { get; set; }
            public string written { get; set; }
            public string srcRef { get; set; }
            public string ddn { get; set; }
            public string user { get; set; }

            public bool firstNameInvalid { get; set; }
            public bool lastNameInvalid { get; set; }
            public bool addressInvalid { get; set; }
            public bool codePostalInvalid { get; set; }
            public bool phoneInvalid { get; set; }
            public bool nasInvalid { get; set; }
            public bool namInvalid { get; set; }
            public bool spokenInvalid { get; set; }
            public bool writtenInvalid { get; set; }
            public bool srcRefInvalid { get; set; }
            public bool ddnInvalid { get; set; }

            public void AddInvalidValues(string key)
            {
                _invalidValues.Add(key, true);
            }

            public void removeInvalidValues(string key)
            {
                _invalidValues.Add(key, false);
            }

            public void SetInvalid(bool ln, bool fn, bool ad, bool code, bool phone, bool nas, bool nam, bool sp, bool wr, bool src, bool ddn)
            {
                firstNameInvalid = fn;
                lastNameInvalid = ln;
                addressInvalid = ad;
                codePostalInvalid = code;
                phoneInvalid = phone;
                nasInvalid = nas;
                namInvalid = nam;
                spokenInvalid = sp;
                writtenInvalid = wr;
                srcRefInvalid = src;
                this.ddnInvalid = ddn;
            }
        }
    }
}




//public ICommand InvalidCommand
//{
//    get
//    {
//        if (_invalidCommand == null)
//        {
//            _invalidCommand = new RelayCommand(
//                param => this.SetInvalid(param.ToString())
//            );
//        }
//        return _invalidCommand;
//    }
//}