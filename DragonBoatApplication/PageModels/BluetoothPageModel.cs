using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using FreshMvvm;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using Xamarin.Forms;

namespace DragonBoatApplication.PageModels
{
    public class BluetoothPageModel : FreshBasePageModel, INotifyPropertyChanged
    {

        IDevice device;
        IBluetoothLE ble;
        IAdapter adapter;

        IDevice dv;

        public string devicename { get; set; }

        public string DeviceName
        {
            get { return devicename; }
            set
            {

                devicename = value;
            }
        }

        private string connecting;

        public string Connecting
        {
            get { return connecting; }
            set
            {


                connecting = value;
                OnPropertyChanged();


            }
        }

        public string output { get; set; }

        public string Output
        {
            get { return output; }
            set
            {

                output = value;
                OnPropertyChanged();

            }
        }

        //string UUID2 = "00001101-0000-1000-8000-00805f9b34fb";

        //string UUID1 = "00000000-0000-1000-8000-00805f9b34fb";

        string UUID = "00000000-0000-0000-0000-000000000000";


        public Command Disconnect { get; set; }

        public Command Scan { get; set; }

        bool disconnect = true;

        public ObservableCollection<IDevice> deviceList;


        public BluetoothPageModel()
        {


            Disconnect = new Command(DisconnectDevice);

            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;

            Connecting = "Connecting to the device ...";


            ConnectToDevice();


        }


        private async void DisconnectDevice()

        {
            try
            {
                await adapter.DisconnectDeviceAsync(dv);

                await CoreMethods.DisplayAlert("Notice", "Connection cancelled", "OK");

            }
            catch (DeviceConnectionException ex)
            {

                await CoreMethods.DisplayAlert("Notice", ex.Message.ToString(), "OK");
            }

            disconnect = true;
            await CoreMethods.PushPageModel<ScanForDevicesPageModel>();
        }

        private async void ConnectToDevice()
        {



            try
            {


                if (dv != null)
                {
                    await adapter.ConnectToDeviceAsync(dv);

                    while (disconnect == false)
                    {
                        var service = await dv.GetServiceAsync(Guid.Parse(UUID));
                        var characteristic = await service.GetCharacteristicAsync(Guid.Parse(UUID));

                        characteristic.ValueUpdated += (o, args) =>
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                var bytes = args.Characteristic.Value;

                                string result = Encoding.Default.GetString(bytes);



                                Output += result + "\\n";
                                //Console.WriteLine(output);

                            });
                        };

                        await characteristic.StartUpdatesAsync();

                    }

                }
                else
                {

                    Connecting = "Arduino cannot be discovered by the phone, press disconnect button and check Arduino's BT before connecting again";
                }




            }
            catch (DeviceConnectionException ex)
            {
                //Could not connect to the device
                await CoreMethods.DisplayAlert("Notice", ex.Message.ToString(), "OK");

                await CoreMethods.DisplayAlert("Info", "Check the UUID or check if Arduino is on", "OK");

                Connecting = "Press disconnect button and try to connect to Arduino again";

                disconnect = true;
            }
        }




        public override void Init(object initData)
        {
            base.Init(initData);
            dv = (IDevice)initData;
            if (dv.Name == null)
            {
                devicename = "";
            }
            else
            {
                devicename = dv.Name;

            }

        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(sender: this, new PropertyChangedEventArgs(propertyName));
        }

    }


}

