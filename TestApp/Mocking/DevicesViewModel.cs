using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Mocking
{
    public class DevicesViewModel
    {
        public IEnumerable<Device> Devices { get; }

        public Lamp SelectedLamp { get; set; }

        public ICommand SetOnCommand { get; set; }
        public ICommand SetOffCommand { get; set; }

        public DevicesViewModel()
        {
            DeviceController controller = new DeviceController();

            Devices = controller.Get();

            SetOnCommand = new RelayCommand(SetOn, CanSetOn);
            SetOffCommand = new RelayCommand(SetOff, CanSetOff);
        }

        public bool CanSetOn()
        {
            return SelectedLamp.Status == Lamp.LampStatus.Off;
        }

        public bool CanSetOff()
        {
            return SelectedLamp.Status == Lamp.LampStatus.On;
        }


        public void SetOn()
        {
            if (SelectedLamp.Status == Lamp.LampStatus.Off)
                SelectedLamp.Status = Lamp.LampStatus.On;
        }

        public void SetOff()
        {
            if (SelectedLamp.Status == Lamp.LampStatus.On)
                SelectedLamp.Status = Lamp.LampStatus.On;
        }
    }


    public class DeviceController
    {
        public IEnumerable<Device> Get()
        {
            throw new NotImplementedException();
        }
    }


    public abstract class Device
    {
        public Guid Id { get; set; }
    }

    public class Lamp : Device
    {
        public LampStatus Status { get; set; }

        public enum LampStatus
        {
            On,
            Off,
        }
    }

 

}
