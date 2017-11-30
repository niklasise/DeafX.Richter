using DeafX.Richter.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeafX.Richter.Business.Models
{

    public delegate void OnToggleTimerExpiredHandler(ToggleTimer sender);

    public class ToggleTimer
    {
        private DateTime _expirationTime;

        internal IToggleDeviceInternal DeviceToToggle { get; private set; }

        public bool StateToToggle { get; private set; }

        public event OnToggleTimerExpiredHandler OnTimerExpired;

        public bool Expired { get; set; }

        public int RemainingSeconds
        {
            get
            {
                return Convert.ToInt32((_expirationTime - DateTime.Now).TotalSeconds);
            }
        }

        internal ToggleTimer(IToggleDeviceInternal deviceToToggle, bool stateToToggle, DateTime expirationTime)
        {
            _expirationTime = expirationTime;
            DeviceToToggle = deviceToToggle;
            StateToToggle = stateToToggle;

            StartTimer();
        }

        private async void StartTimer()
        {
            var now = DateTime.Now;

            if(_expirationTime < now)
            {
                throw new ArgumentException("ExpirationTime has already expired");
            }

            var timeLeft = _expirationTime - now;

            await Task.Delay(timeLeft);

            Expired = true;

            OnTimerExpired?.Invoke(this);
        }

    }
}
