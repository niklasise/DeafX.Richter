namespace DeafX.Richter.Business.Models.ZWay
{
    public class ZWayMetrics
    {
        public string icon { get; set; }
        public string title { get; set; }
        public object level { get; set; }
        public string change { get; set; }
        public string probeTitle { get; set; }
        public string scaleTitle { get; set; }
        public string text { get; set; }

        public override bool Equals(object obj)
        {
            var otherMetrics = obj as ZWayMetrics;

            if(otherMetrics == null)
            {
                return false;
            }

            return (level == null && otherMetrics.level == null) || level.Equals(otherMetrics.level);
        }
    }

}
