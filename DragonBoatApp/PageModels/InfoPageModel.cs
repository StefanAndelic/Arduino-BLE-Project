using System;
using System.Collections.ObjectModel;
using FreshMvvm;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;

namespace DragonBoatApp.PageModels
{
    public class InfoPageModel : FreshBasePageModel
    {
        IBluetoothLE ble;
        IAdapter adapter;
        ObservableCollection<IDevice> deviceList;
        IDevice device;

        public Command Scan { get; set; }


        public InfoPageModel()
        {
            Scan = new Command(ScanforAvailableDevices);

            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            deviceList = new ObservableCollection<IDevice>();
        }


        private async void ScanforAvailableDevices()
        {


            try
            {
                deviceList.Clear();
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


        public void CheckState()
        {
            // BT state 

            var state = ble.State;

            if (state == BluetoothState.Off)
            {
                CoreMethods.DisplayAlert("BT is off", state.ToString(), "turn on !");
            }
            else
            {
                CoreMethods.DisplayAlert("BT is not available", state.ToString(), "OK !");


            }
        }

        private void GoToSelectedDevicePage()
        {
            //var user = SelectedInstructor;
            //CoreMethods.PushPageModel<BubbleLeaderProfilePageModel>(user);

        }
    }

}
