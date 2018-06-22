using FirstFloor.ModernUI.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace clinic.patient
{
    public class LinkText
    {
        private ICommand _previousCommand;
        private SampleFormViewModel _formMaster;
        private int currentNumber = 0;
        public string Text { get; set; }
        public string Hyperlink { get; set; }

        public LinkText(string text, string hyperlink, SampleFormViewModel formMaster)
        {
            Text = text;
            Hyperlink = hyperlink;
            _formMaster = formMaster;
            currentNumber = int.Parse(hyperlink);
        }
        public ICommand HyperlinkCommand
        {
            get
            {
                if (_previousCommand == null)
                {
                    _previousCommand = new RelayCommand(
                        param => this.GoLink()
                    );
                }
                return _previousCommand;
            }
        }
        public void GoLink()
        {
            _formMaster.GoNext(currentNumber);
        }
    }
}
