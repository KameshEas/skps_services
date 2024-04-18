using Mopups.Services;
using skps_services.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace skps_services.ViewModels
{
    public class Services
    {
        public string Products { get; set; }
        
        public string ImgSrc { get; set; }
    }
    public class ServicesViewModel
    {
        public ICommand ShowBottomSheetCommand => new Command<Services>(async (Products) => await ShowBottomSheet(Products));

        private async Task ShowBottomSheet(Services Products)
        {
            var booknow = new BookNowView();
            MopupService.Instance.PushAsync(booknow);
        }

        public ObservableCollection<Services> Services { get; set; }

        public ServicesViewModel()
        {

            Services = new ObservableCollection<Services>
            {
                new Services() { Products = "Mixie", ImgSrc = "shop.jpeg"},
                new Services() { Products = "Grinder", ImgSrc = "shop.jpeg" },
                new Services() { Products = "Fridge", ImgSrc = "shop.jpeg" },
                new Services() { Products = "Induction Stove", ImgSrc = "shop.jpeg" },
                new Services() { Products = "Microoven", ImgSrc = "shop.jpeg" },

            };
        }
    }
}
