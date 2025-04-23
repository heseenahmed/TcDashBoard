namespace TCDashBoard.Models
{
    public class Category
    {

        public int Id { get; set; }
        public string title { get; set; }
        public ICollection<Works> Works { get; set; } = new List<Works>();
    }
}
