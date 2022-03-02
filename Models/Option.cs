namespace ePollApi.Models
{
    public class Option
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int Votes { get; set; } = 0;
    }
}
