namespace DeafX.Richter.Business.Models
{
    public class ZWayConfiguration
    {
        public string Adress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ZWayDeviceConfiguration[] Devices { get; set; }
    }
}
