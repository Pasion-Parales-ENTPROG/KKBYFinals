namespace KKBYFinals.Models;

public class Holiday
{
    public DateTime Date { get; set; }
    public string LocalName { get; set; } = "";
    public string Name { get; set; } = "";
    public string CountryCode { get; set; } = "";
    public List<string> Types { get; set; } = new();
}
