using FirstFloor.ModernUI.Presentation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace clinic.patient
{
    public class SampleFormViewModel
        : NotifyPropertyChanged, IDataErrorInfo
    {
        private readonly int MaxPage = 67;
        private FormModel _data;
        private FormModel _data2;

        private string _firstName = "";
        private string _lastName = "";
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

        private string _firstName2 = "";
        private string _lastName2 = "";
        private string _address2 = "";
        private string _codePostal2 = "";
        private string _phone2 = "";
        private string _nas2 = "";
        private string _nam2 = "";
        private string _spoken2 = "";
        private string _written2 = "";
        private string _srcRef2 = "";
        private string _ddn2 = "";

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
        private bool _invalidPagesState = false;

        private bool _firstNameInvalid2 = false;
        private bool _lastNameInvalid2 = false;
        private bool _addressInvalid2 = false;
        private bool _codePostalInvalid2 = false;
        private bool _phoneInvalid2 = false;
        private bool _nasInvalid2 = false;
        private bool _namInvalid2 = false;
        private bool _spokenInvalid2 = false;
        private bool _writtenInvalid2 = false;
        private bool _srcRefInvalid2 = false;
        private bool _ddnInvalid2 = false;

        private int _pageNumber = 1;
        private ICommand _previousCommand, _nextCommand, _saveCommand, _openAdminCommand, _saveAdminCommand, _clearAdminCommand, _validCommand;
        private ObservableCollection<LinkText> _linkItems;
        private readonly string OriginalName = "original";

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
                        param => OpenForm()
                    );
                }
                return _openAdminCommand;
            }
        }
        public void OpenForm()
        {
            this.GetForm(true, false, PageNumber);
            this.GetForm(true, true, PageNumber);
        }
        public ICommand SaveAdminCommand
        {
            get
            {
                if (_saveAdminCommand == null)
                {
                    _saveAdminCommand = new RelayCommand(
                        param => this.SaveForm(true)
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
                        param => this.SaveForm()
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
            _data2 = new FormModel(LastName, FirstName, Address, CodePostal, Phone, Nas, Nam, Spoken, Written, SrcRef, Ddn, User);
            _linkItems = new ObservableCollection<LinkText>();
            TestLinkInvalid();
            //Mediator.Register("InitOther", InitOther);
            //Mediator.Register("Next", GoNextAll);
            //Mediator.Register("Back", GoBackAll);
        }
        private void TestLinkInvalid()
        {
            for(int i = 0; i < 10; ++i)
            {
                if(i == 0)
                {
                    LinkItems.Add(new LinkText("", i + "", this));
                } else
                {
                    LinkItems.Add(new LinkText("|", i + "", this));
                }
                
            }
        }
        public void InitOther(object args)
        {
            string prefix = @".\";
            User = SingletonUser.user.ToString();
            prefix += User;
            string fileName = prefix + "\\" + PageNumber;
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
        public void SaveForm(bool admin = false)
        {
            FormModel data = null;
            if (!admin)
            {
                _data = new FormModel(LastName, FirstName, Address, CodePostal, Phone, Nas, Nam, Spoken, Written, SrcRef, Ddn, User);
                _data.SetInvalid(LastNameInvalid, FirstNameInvalid, AddressInvalid, CodePostalInvalid, PhoneInvalid, NasInvalid, NamInvalid, SpokenInvalid
                    , WrittenInvalid, SrcRefInvalid, DdnInvalid);
                data = _data;
            }
            else
            {
                _data2 = new FormModel(LastName2, FirstName2, Address2, CodePostal2, Phone2, Nas2, Nam2, Spoken2, Written2, SrcRef2, Ddn2, User);
                _data2.SetInvalid(LastNameInvalid2, FirstNameInvalid2, AddressInvalid2, CodePostalInvalid2, PhoneInvalid2, NasInvalid2,
                    NamInvalid2, SpokenInvalid2, WrittenInvalid2, SrcRefInvalid2, DdnInvalid2);
                data = _data2;
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
                fileName += OriginalName;
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
                serializer.Serialize(writer, data);
            }
        }
        public void GoNext(int page)
        {
            if (!String.IsNullOrEmpty(SingletonUser.user))
            {
                GetForm(false, false, page);
                GetForm(false, true, page);
            }
        }
        private void GoBack()
        {
            if (!String.IsNullOrEmpty(SingletonUser.user))
            {
                SaveForm();
                GetForm(false);
                GetForm(false, true, PageNumber);
            }
        }
        private void GoNext()
        {
            if (!String.IsNullOrEmpty(SingletonUser.user))
            {
                SaveForm();
                GetForm(true);
                GetForm(true, true, PageNumber);
            }
        }
        private void Validate()
        {
            InvalidPages = "";
            for (int i = 1; i < MaxPage; ++i)
            {
                FormModel formOriginal = null, formToCheck = null;
                string prefix = @".\";
                prefix += User;
                string fileName = prefix + "\\" + i;
                fileName += OriginalName;
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
                bool errorPresent = false;
                bool foundFile = false;
                prefix = @".\";
                prefix += User;
                fileName = prefix + "\\" + i;
                fileName += ".txt";
                if (formOriginal != null)
                {
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
                            formToCheck.srcRefInvalid = formToCheck.srcRef != formOriginal.srcRef;
                            errorPresent |= formToCheck.srcRefInvalid;
                            foundFile = true;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        errorPresent = true;
                    }
                    if (errorPresent)
                    {
                        if (InvalidPages == "")
                        {
                            InvalidPages = "Pages : ";
                        }
                        InvalidPages += i + " | ";
                        if (i == 0)
                        {
                            LinkItems.Add(new LinkText("", i + "", this));
                        }
                        else
                        {
                            LinkItems.Add(new LinkText("|", i + "", this));
                        }                        
                    }
                    else
                    {

                    }
                    if (foundFile)
                    {
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
            if(InvalidPages.Length > 0 && InvalidPages.EndsWith("| "))
            {
                InvalidPages = InvalidPages.Remove(InvalidPages.Length - 2,2);
            }
            OpenForm();
        }

        private void GetForm(bool goNext, bool admin = false, int pageNo = 0)
        {
            int idxPage = pageNo;
            InitFields(admin);
            if (!admin && pageNo == 0)
            {
                if (goNext)
                {
                    if (PageNumber < MaxPage - 1)
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
                idxPage = PageNumber;
            }
            string prefix = @".\";
            if (!admin)
            {
                User = SingletonUser.user.ToString();
            }
            prefix += User;
            string fileName = prefix + "\\" + idxPage;
            if (admin)
            {
                fileName += OriginalName;
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

                    SetModel(form, admin);
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
            if (admin)
            {
                FirstName2 = valueStr;
                LastName2 = valueStr;
                Address2 = valueStr;
                CodePostal2 = valueStr;
                Phone2 = valueStr;
                Nas2 = valueStr;
                Nam2 = valueStr;
                Spoken2 = valueStr;
                Written2 = valueStr;
                SrcRef2 = valueStr;
                Ddn2 = valueStr;

                FirstNameInvalid2 = valueBool;
                LastNameInvalid2 = valueBool;
                AddressInvalid2 = valueBool;
                CodePostalInvalid2 = valueBool;
                PhoneInvalid2 = valueBool;
                NasInvalid2 = valueBool;
                NamInvalid2 = valueBool;
                SpokenInvalid2 = valueBool;
                WrittenInvalid2 = valueBool;
                SrcRefInvalid2 = valueBool;
                DdnInvalid2 = valueBool;
            }
            else
            {
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
        }
        private void SetModel(FormModel form, bool admin = false)
        {
            if (admin)
            {
                FirstName2 = form.firstName;
                LastName2 = form.lastName;
                Address2 = form.address;
                CodePostal2 = form.codePostal;
                Phone2 = form.phone;
                Nas2 = form.nas;
                Nam2 = form.nam;
                Spoken2 = form.spoken;
                Written2 = form.written;
                SrcRef2 = form.srcRef;
                Ddn2 = form.ddn;
                //User = form.user;

                FirstNameInvalid2 = form.firstNameInvalid;
                LastNameInvalid2 = form.lastNameInvalid;
                AddressInvalid2 = form.addressInvalid;
                CodePostalInvalid2 = form.codePostalInvalid;
                PhoneInvalid2 = form.phoneInvalid;
                NasInvalid2 = form.nasInvalid;
                NamInvalid2 = form.namInvalid;
                SpokenInvalid2 = form.spokenInvalid;
                WrittenInvalid2 = form.writtenInvalid;
                SrcRefInvalid2 = form.srcRefInvalid;
                DdnInvalid2 = form.ddnInvalid;
            }
            else
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
        public bool InvalidPagesState
        {
            get { return this._invalidPagesState; }
            set
            {
                if (this._invalidPagesState != value)
                {
                    this._invalidPagesState = value;
                    OnPropertyChanged("InvalidPagesState");
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



        public bool LastNameInvalid2
        {
            get { return this._lastNameInvalid2; }
            set
            {
                if (this._lastNameInvalid2 != value)
                {
                    this._lastNameInvalid2 = value;
                    OnPropertyChanged("LastNameInvalid2");
                }
            }
        }
        public bool FirstNameInvalid2
        {
            get { return this._firstNameInvalid2; }
            set
            {
                if (this._firstNameInvalid2 != value)
                {
                    this._firstNameInvalid2 = value;
                    OnPropertyChanged("FirstNameInvalid2");
                }
            }
        }
        public bool AddressInvalid2
        {
            get { return this._addressInvalid2; }
            set
            {
                if (this._addressInvalid2 != value)
                {
                    this._addressInvalid2 = value;
                    OnPropertyChanged("AddressInvalid2");
                }
            }
        }
        public bool CodePostalInvalid2
        {
            get { return this._codePostalInvalid2; }
            set
            {
                if (this._codePostalInvalid2 != value)
                {
                    this._codePostalInvalid2 = value;
                    OnPropertyChanged("CodePostalInvalid2");
                }
            }
        }
        public bool PhoneInvalid2
        {
            get { return this._phoneInvalid; }
            set
            {
                if (this._phoneInvalid2 != value)
                {
                    this._phoneInvalid2 = value;
                    OnPropertyChanged("PhoneInvalid2");
                }
            }
        }
        public bool NasInvalid2
        {
            get { return this._nasInvalid2; }
            set
            {
                if (this._nasInvalid2 != value)
                {
                    this._nasInvalid2 = value;
                    OnPropertyChanged("NasInvalid2");
                }
            }
        }
        public bool NamInvalid2
        {
            get { return this._namInvalid2; }
            set
            {
                if (this._namInvalid2 != value)
                {
                    this._namInvalid2 = value;
                    OnPropertyChanged("NamInvalid2");
                }
            }
        }
        public bool SpokenInvalid2
        {
            get { return this._spokenInvalid2; }
            set
            {
                if (this._spokenInvalid2 != value)
                {
                    this._spokenInvalid2 = value;
                    OnPropertyChanged("SpokenInvalid2");
                }
            }
        }
        public bool WrittenInvalid2
        {
            get { return this._writtenInvalid2; }
            set
            {
                if (this._writtenInvalid2 != value)
                {
                    this._writtenInvalid2 = value;
                    OnPropertyChanged("WrittenInvalid2");
                }
            }
        }
        public bool SrcRefInvalid2
        {
            get { return this._srcRefInvalid2; }
            set
            {
                if (this._srcRefInvalid2 != value)
                {
                    this._srcRefInvalid2 = value;
                    OnPropertyChanged("SrcRefInvalid2");
                }
            }
        }
        public bool DdnInvalid2
        {
            get { return this._ddnInvalid2; }
            set
            {
                if (this._ddnInvalid2 != value)
                {
                    this._ddnInvalid2 = value;
                    OnPropertyChanged("DdnInvalid2");
                }
            }
        }
        public string Ddn2
        {
            get { return this._ddn2; }
            set
            {
                if (this._ddn2 != value)
                {
                    this._ddn2 = value;
                    OnPropertyChanged("Ddn2");
                }
            }
        }
        public string FirstName2
        {
            get { return this._firstName2; }
            set
            {
                if (this._firstName2 != value)
                {
                    this._firstName2 = value;
                    OnPropertyChanged("FirstName2");
                }
            }
        }

        public string LastName2
        {
            get { return this._lastName2; }
            set
            {
                if (this._lastName2 != value)
                {
                    this._lastName2 = value;
                    OnPropertyChanged("LastName2");
                }
            }
        }

        public string Address2
        {
            get { return this._address2; }
            set
            {
                if (this._address2 != value)
                {
                    this._address2 = value;
                    OnPropertyChanged("Address2");
                }
            }
        }
        public string CodePostal2
        {
            get { return this._codePostal2; }
            set
            {
                if (this._codePostal2 != value)
                {
                    this._codePostal2 = value;
                    OnPropertyChanged("CodePostal2");
                }
            }
        }

        public string Phone2
        {
            get { return this._phone2; }
            set
            {
                if (this._phone2 != value)
                {
                    this._phone2 = value;
                    OnPropertyChanged("Phone2");
                }
            }
        }

        public string Nas2
        {
            get { return this._nas2; }
            set
            {
                if (this._nas2 != value)
                {
                    this._nas2 = value;
                    OnPropertyChanged("Nas2");
                }
            }
        }
        public string Nam2
        {
            get { return this._nam2; }
            set
            {
                if (this._nam2 != value)
                {
                    this._nam2 = value;
                    OnPropertyChanged("Nam2");
                }
            }
        }

        public string Spoken2
        {
            get { return this._spoken2; }
            set
            {
                if (this._spoken2 != value)
                {
                    this._spoken2 = value;
                    OnPropertyChanged("Spoken2");
                }
            }
        }

        public string Written2
        {
            get { return this._written2; }
            set
            {
                if (this._written2 != value)
                {
                    this._written2 = value;
                    OnPropertyChanged("Written2");
                }
            }
        }


        public string SrcRef2
        {
            get { return this._srcRef2; }
            set
            {
                if (this._srcRef2 != value)
                {
                    this._srcRef2 = value;
                    OnPropertyChanged("SrcRef2");
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
                //if (columnName == "FirstName")
                //{
                //    return string.IsNullOrEmpty(this._firstName) ? "Required value" : null;
                //}
                //if (columnName == "LastName")
                //{
                //    return string.IsNullOrEmpty(this._lastName) ? "Required value" : null;
                //}
                if (columnName == "User")
                {
                    return string.IsNullOrEmpty(this._user) ? "Required value" : null;
                }
                return null;
            }
        }
        public ObservableCollection<LinkText> LinkItems
        {
            get
            {
                return _linkItems;
            }
            set
            {
                this._linkItems = value;
                OnPropertyChanged("LinkItems");
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