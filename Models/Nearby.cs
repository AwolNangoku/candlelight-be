namespace CandleLightApi.Models;
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class NearArea
{
    public int count { get; set; }
    public string id { get; set; }
    public string name { get; set; }
    public string region { get; set; }
}

public class NearRoot
{
    public List<NearArea> nearareas { get; set; }
}

