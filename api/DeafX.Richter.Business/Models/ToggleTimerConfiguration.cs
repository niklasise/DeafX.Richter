using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Models
{
    public class ToggleTimerConfiguration
    {

        public string DeviceToToggle { get; set; }

        public bool StateToToggle { get; set; }

        public DateTime ExpirationTime { get; set; }

    }
}
