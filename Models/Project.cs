namespace WebApplication1.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string GitHubLink { get; set; }
        public string Category { get; set; }
    }
} 