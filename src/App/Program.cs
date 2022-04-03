// var date = DateTime.Now;
// var dateonly = DateOnly.FromDateTime(DateTime.Now);

// var dateYesterday = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
var oi = new oi();
Console.WriteLine(new oi().ToString());

public class oi
{
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public string Name { get; set; } = "oi";

    public string Sobrenome { get; set; } = "tudo bem?";
}