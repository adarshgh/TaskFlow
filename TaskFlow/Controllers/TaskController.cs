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

        public IActionResult Index(string? sortBy = null)
        {
            var tasks = _context.TaskItems.AsQueryable();

            switch (sortBy)
            {
                case "title":
                    tasks = tasks.OrderBy(t => t.Title);
                    break;
                case "created_at":
                    tasks = tasks.OrderBy(t => t.CreatedAt);
                    break;
                case "status":
                    tasks = tasks.OrderBy(t => t.IsCompleted);
                    break;
                default:
                    tasks = tasks.OrderByDescending(t => t.CreatedAt); // Default: Newest first
                    break;
            }

            return View(tasks.ToList());
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

        public IActionResult Edit(int id)
        {
            var task = _context.TaskItems.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            var viewModel = new TaskItemViewModel
            {
                Title = task.Title,
                Description = task.Description
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(int id, TaskItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var task = _context.TaskItems.Find(id);
                if (task == null)
                {
                    return NotFound();
                }

                task.Title = model.Title;
                task.Description = model.Description;
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var task = _context.TaskItems.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var task = _context.TaskItems.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.TaskItems.Remove(task);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult MarkAsComplete(int id)
        {
            var task = _context.TaskItems.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            task.IsCompleted = true;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UndoCompletion(int id)
        {
            var task = _context.TaskItems.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            task.IsCompleted = false;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
