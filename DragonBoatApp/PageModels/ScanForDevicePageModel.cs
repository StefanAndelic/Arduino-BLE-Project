using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FreshMvvm;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using System.Text;
using DragonBoatApp.Models;

namespace DragonBoatApp.PageModels
{
    public class ScanForDevicePageModel : FreshBasePageModel
    {


        IBluetoothLE ble;
        IAdapter adapter;
        ObservableCollection<IDevice> deviceList;
        IDevice device;


        public Command Scan { get; set; }

        public Command Dashboard { get; set; }

        /*private Models.Device _selectedDevice { get; set; }

        public Models.Device SelectedDevice
        {
            get { return _selectedDevice; }

            set
            {
                if (_selectedDevice != value)
                {
                    _selectedDevice = value;


                }

            }
        }*/

        public ScanForDevicePageModel()
        {
            Scan = new Command(ScanforAvailableDevices);

            Dashboard = new Command(GoToDashboardPage);

            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            deviceList = new ObservableCollection<IDevice>();


        }

        private void GoToDashboardPage()
        {
            var tabbedNavigation = new FreshTabbedNavigationContainer("secondNavPage");
            tabbedNavigation.AddTab<InfoPageModel>("Info", null);
            tabbedNavigation.AddTab<TrainPageModel>("Train", null);
            tabbedNavigation.AddTab<WorkPageModel>("Work", null);
            tabbedNavigation.AddTab<SetupPageModel>("Setup", null);

            CoreMethods.SwitchOutRootNavigation("secondNavPage");



        }


        /*private void GoToBubbleLeaderProfilePage()
        {
            var user = SelectedInstructor;
            CoreMethods.PushPageModel<BubbleLeaderProfilePageModel>(user);

        }*/

        private async void ScanforAvailableDevices()
        {
            //CoreMethods.DisplayAlert("Scanning for devices", "OK", "Info");

            // BT state 

            /*var state = ble.State;

            if (state == BluetoothState.Off)
            {
                CoreMethods.DisplayAlert("BT is off", state.ToString(), "turn on !");
            }
            else
            {
                //CoreMethods.DisplayAlert("Notice", state.ToString(), "OK !");


            }*/

            try
            {
                //deviceList.Clear();
                adapter.DeviceDiscovered += (s, a) =>
                {
                    deviceList.Add(a.Device);
                };

                //We have to test if the device is scanning 
                if (!ble.Adapter.IsScanning)
                {
                    await adapter.StartScanningForDevicesAsync();

                }
            }
            catch (Exception ex)
            {
                await CoreMethods.DisplayAlert("Notice", ex.Message.ToString(), "Error !");
            }



        }

    }
}
