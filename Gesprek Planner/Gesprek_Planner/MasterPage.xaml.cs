using System;
using System.Collections.Generic;
using ODS_Meester_Aafjes.Models;
using ODS_Meester_Aafjes.Pages.Options;
using Xamarin.Forms;
using GroupPasswordPage = ODS_Meester_Aafjes.Pages.Options.GroupPasswordPage;

namespace ODS_Meester_Aafjes.Pages
{
    public partial class MasterPage : ContentPage
    {
        public static Page navigateTo;
        public ListView MasterPages;
        public MasterPage()
        {
            InitializeComponent();
            MasterPages = listView;
            var masterPageItems = new List<MasterPageItem>
            {
                new MasterPageItem
                {
                    TargetType = typeof(HomePage),
                    Title = "Home",
                    Icon = "fa-home"
                },
                new MasterPageItem
                {
                    TargetType = typeof(GroupPasswordPage),
                    Title = "Groeps inlog",
                    Icon = "fa-key"
                },
                new MasterPageItem
                {
                    TargetType = typeof(Contact),
                    Title = "Contact",
                    Icon = "fa-envelope"
                },
                new MasterPageItem
                {
                    TargetType = typeof(SchoolWiki),
                    Title = "School wiki",
                    Icon = "fa-globe"
                },
                new MasterPageItem
                {
                    TargetType = typeof(OptionsMainPage),
                    Title = "Opties",
                    Icon = "fa-gears"
                }
            };

            listView.ItemsSource = masterPageItems;
        }
    }
}
