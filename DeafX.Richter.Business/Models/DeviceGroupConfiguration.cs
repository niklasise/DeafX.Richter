using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Models
{
    public class DeviceGroupConfiguration
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string[] Devices { get; set; }
    }
}
