using System;
using ODS_Meester_Aafjes.Models;
using Plugin.Settings;
using Xamarin.Forms;
using GroupPasswordPage = ODS_Meester_Aafjes.Pages.Options.GroupPasswordPage;

namespace ODS_Meester_Aafjes.Pages
{
    public class MainPage : MasterDetailPage
    {
        public static MasterPage MasterPage;
        public static Page DetailPage;
        public MainPage()
        {
            MasterPage = new MasterPage();
            this.MasterBehavior = MasterBehavior.Popover;
            Master = MasterPage;
            Detail = CrossSettings.Current.GetValueOrDefault("FirstRun", true) ? new NavigationPage(new GroupPasswordPage()) : new NavigationPage(new HomePage());
            MasterPage.MasterPages.ItemSelected += OnItemSelected;
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Page navigateTo = (Page) Activator.CreateInstance(item.TargetType);
                var navigationPage = Detail as NavigationPage;
                MasterPage.MasterPages.SelectedItem = null;
                if (item.TargetType == Detail.GetType() ) return;
                Detail.Navigation.PushAsync(navigateTo);
//                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
//                IsPresented = false;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }
    }
}
