namespace TCDashBoard.Dtos
{
    public class WorkDto
    {
        public string title { get; set; }
        public string Description { get; set; }
        public IFormFile WorkImage { get; set; }
        public int CategoryId { get; set; }
    }
}
