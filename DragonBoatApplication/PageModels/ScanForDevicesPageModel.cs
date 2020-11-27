using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FreshMvvm;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using Xamarin.Forms;

namespace DragonBoatApplication.PageModels
{
    public class ScanForDevicesPageModel : FreshBasePageModel
    {
        public Command Connect { get; set; }

        IBluetoothLE ble;
        IAdapter adapter;
        public ObservableCollection<IDevice> deviceList;

        public IDevice deviceFound;
        public IDevice d;



        public ScanForDevicesPageModel()
        {
            Connect = new Command(ConnectToESP32);

            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;

            deviceList = new ObservableCollection<IDevice>(); // initialise the list 



        }
        private async void ScanForDevices()
        {
            try
            {
                deviceList.Clear();
                adapter.DeviceDiscovered += (s, a) =>
                {
                    deviceList.Add(a.Device);



                    d = a.Device;


                    if (!string.IsNullOrWhiteSpace(a.Device.Name) && a.Device.Name.Contains("ESP32test"))
                    {

                        deviceFound = a.Device;
                    }

                };




                await adapter.StartScanningForDevicesAsync();


            }
            catch (Exception ex)
            {
                await CoreMethods.DisplayAlert("Notice", ex.Message.ToString(), "Error !");
            }

        }


        public void CheckState()
        {
            var state = ble.State;

            if (state == BluetoothState.Off)
            {
                CoreMethods.DisplayAlert("Bluethooth is off", state.ToString(), "turn Bluetooth on and press connect again!");
            }

            else if (state == BluetoothState.On)
            {

                ScanForDevices();
                if (deviceFound == null)
                {
                    deviceFound = d;
                }

                CoreMethods.PushPageModel<BluetoothPageModel>(deviceFound);
            }

        }

        private void ConnectToESP32()
        {

            CheckState();

        }

    }

}

