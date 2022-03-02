namespace ePollApi.Models
{
    public class PollWithOptions : Poll
    {
        public IList<Option> Options { get; set; } = new List<Option>();
    }
}
