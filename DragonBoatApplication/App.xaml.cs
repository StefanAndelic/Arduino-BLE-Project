using System;
using DragonBoatApplication.PageModels;
using FreshMvvm;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DragonBoatApplication
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //main navigation 
            SetUpNavigation();
        }

        public void SetUpNavigation()
        {

            var tabbedNavigation = new FreshTabbedNavigationContainer();
            tabbedNavigation.AddTab<ScanForDevicesPageModel>("Connect", "ic_shortcut_bluetooth_connected.png", null);
            tabbedNavigation.AddTab<TrainPageModel>("Train", null);
            tabbedNavigation.AddTab<WorkPageModel>("Work", null);
            tabbedNavigation.AddTab<SetupPageModel>("Setup", "ic_shortcut_phonelink_setup.png", null);
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
