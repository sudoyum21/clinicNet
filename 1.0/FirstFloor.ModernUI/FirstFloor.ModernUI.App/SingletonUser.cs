using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstFloor.ModernUI.App
{
    public static class SingletonUser 
    {
        public static string user = "";
        public static SampleFormViewModel form;
        public static SampleFormViewModel formOriginal;
        public static void ValidateForm(SampleFormViewModel.FormModel f)
        {
            form.AddressInvalid = form.Address != formOriginal.Address;
            form.AddressInvalid = form.Address != formOriginal.Address;
            form.AddressInvalid = form.Address != formOriginal.Address;
            form.AddressInvalid = form.Address != formOriginal.Address;
            form.AddressInvalid = form.Address != formOriginal.Address;
            form.AddressInvalid = form.Address != formOriginal.Address;
            form.AddressInvalid = form.Address != formOriginal.Address;
            form.AddressInvalid = form.Address != formOriginal.Address;
            form.AddressInvalid = form.Address != formOriginal.Address;
            form.AddressInvalid = form.Address != formOriginal.Address;
            form.SaveObjectAndGoNext();
        }
    } 
}
