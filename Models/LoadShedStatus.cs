namespace CandleLightApi.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<StatusRoot>(myJsonResponse);
    public class Capetown
    {
        public string name { get; set; }
        public List<NextStage> next_stages { get; set; }
        public string stage { get; set; }
        public DateTime stage_updated { get; set; }
    }

    public class Eskom
    {
        public string name { get; set; }
        public List<NextStage> next_stages { get; set; }
        public string stage { get; set; }
        public DateTime stage_updated { get; set; }
    }

    public class NextStage
    {
        public string stage { get; set; }
        public DateTime stage_start_timestamp { get; set; }
    }

    public class StatusRoot
    {
        public LoadShedStatus status { get; set; }
    }

    public class LoadShedStatus
    {
        public Capetown capetown { get; set; }
        public Eskom eskom { get; set; }
    }


}
