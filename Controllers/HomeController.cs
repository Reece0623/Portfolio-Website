using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using System.Text.Json;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _projectsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/projects.json");

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private List<Project> ReadProjects()
        {
            if (!System.IO.File.Exists(_projectsFilePath))
            {
                return new List<Project>();
            }
            var json = System.IO.File.ReadAllText(_projectsFilePath);
            return JsonSerializer.Deserialize<List<Project>>(json) ?? new List<Project>();
        }

        private void WriteProjects(List<Project> projects)
        {
            var json = JsonSerializer.Serialize(projects, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_projectsFilePath, json);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CSharp()
        {
            var projects = ReadProjects().Where(p => p.Category == "CSharp").ToList();
            return View(projects);
        }

        public IActionResult Python()
        {
            var projects = ReadProjects().Where(p => p.Category == "Python").ToList();
            return View(projects);
        }

        public IActionResult SQL()
        {
            var projects = ReadProjects().Where(p => p.Category == "SQL").ToList();
            return View(projects);
        }

        public IActionResult Java()
        {
            var projects = ReadProjects().Where(p => p.Category == "Java").ToList();
            return View(projects);
        }

        public IActionResult PowerAutomate()
        {
            var projects = ReadProjects().Where(p => p.Category == "PowerAutomate").ToList();
            return View(projects);
        }

        [HttpPost]
        public IActionResult AddProject(string title, string description, string gitHubLink, string category)
        {
            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                return Unauthorized();
            }

            var projects = ReadProjects();
            projects.Add(new Project
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                GitHubLink = gitHubLink,
                Category = category
            });
            WriteProjects(projects);

            return RedirectToAction(category);
        }

        [HttpPost]
        public IActionResult DeleteProject(Guid id, string category)
        {
            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                return Unauthorized();
            }

            var projects = ReadProjects();
            var projectToRemove = projects.FirstOrDefault(p => p.Id == id);
            if (projectToRemove != null)
            {
                projects.Remove(projectToRemove);
                WriteProjects(projects);
            }

            return RedirectToAction(category);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
