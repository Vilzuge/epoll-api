namespace ePollApi.Models
{
    public class PollPost : Poll
    {
        public IList<string> Options { get; set; } = new List<string>();
    }
}
