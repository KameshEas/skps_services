using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace skps_services.ViewModels
{
    public class ProfileViewModel
    {
        public ICommand NavigateCommand => new Command(async () => await Shell.Current.GoToAsync("//ContactUs"));

    }
}
