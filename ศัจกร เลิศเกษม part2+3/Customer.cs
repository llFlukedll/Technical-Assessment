public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime CreatedDate { get; set; }
    public override string ToString()
    {
        return $"Customer ID: {Id}\n" +
               $"Name: {Name}\n" +
               $"Email: {Email}\n"+
               $"Created Date: {CreatedDate:yyyy-MM-dd HH:mm:ss}";
    }
}