using Microsoft.AspNetCore.Mvc;
using TaskFlow.Data;
using TaskFlow.Models;

namespace TaskFlow.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskFlowDbContext _context;

        public TaskController(TaskFlowDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tasks = _context.TaskItems.ToList();
            return View(tasks);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TaskItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var task = new TaskItem
                {
                    Title = model.Title,
                    Description = model.Description,
                    IsCompleted = false
                };

                _context.TaskItems.Add(task);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
