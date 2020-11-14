using System;
using DragonBoatApp.PageModels;
using FreshMvvm;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DragonBoatApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            SetUpNavigation();
        }

        public void SetUpNavigation()
        {
            /*var page = FreshPageModelResolver.ResolvePageModel<ScanForDevicePageModel>();
            var basicNavContainer = new FreshNavigationContainer(page);
            MainPage = basicNavContainer;*/

            var tabbedNavigation = new FreshTabbedNavigationContainer();
            tabbedNavigation.AddTab<InfoPageModel>("Info", null);
            tabbedNavigation.AddTab<TrainPageModel>("Train", null);
            tabbedNavigation.AddTab<WorkPageModel>("Work", null);
            tabbedNavigation.AddTab<SetupPageModel>("Setup", null);
            MainPage = tabbedNavigation;

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
