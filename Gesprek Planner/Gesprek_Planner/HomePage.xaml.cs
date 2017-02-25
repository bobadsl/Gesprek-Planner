using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormsPlugin.Iconize;
using ODS_Meester_Aafjes.Interfaces;
using ODS_Meester_Aafjes.Models;
using Plugin.Settings;
using Xamarin.Forms;

namespace ODS_Meester_Aafjes.Pages
{
    public partial class HomePage : ContentPage
    {
        List<MasterPageItem> GotoPageList = new List<MasterPageItem>();
        public HomePage()
        {
            GotoPageList.Add(new MasterPageItem {Icon = "fa-envelope", TargetType = typeof(Contact), Title="Contact"});
            InitializeComponent();
            IconButton twitterButton = new IconButton {Text = "fa-twitter", IsVisible = (bool)App.Database.MetaDataTable.GetValueByKey("twitterActive") };
            twitterButton.Clicked += GotoTwitterPage;
            IconButton facebookButton = new IconButton {Text = "fa-facebook", IsVisible = (bool)App.Database.MetaDataTable.GetValueByKey("facebookActive") };
            facebookButton.Clicked += GotoFacebookPage;

            Grid.Children.Add(twitterButton);
            Grid.Children.Add(facebookButton);
        }

        private void GotoTwitterPage(object sender, EventArgs e)
        {
            DependencyService.Get<ISocialLinkOpener>().OpenTwitterLink(App.Database.MetaDataTable.GetByKey("twitterAccount").Result.Get());
        }

        private void GotoFacebookPage(object sender, EventArgs e)
        {
            DependencyService.Get<ISocialLinkOpener>().OpenFacebookLink("Ods-Meester-Aafjes-554723344611117");
        }
    }
}
