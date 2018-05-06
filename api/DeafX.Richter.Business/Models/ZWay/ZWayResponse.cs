namespace DeafX.Richter.Business.Models.ZWay
{
    public class ZWayResponse<T>
    {
        public T data { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public object error { get; set; }
    }
}
