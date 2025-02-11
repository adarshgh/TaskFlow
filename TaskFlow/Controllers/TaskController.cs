using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
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
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            Console.WriteLine($"User ID: {userId}, Role: {userRole}");

            IQueryable<TaskItem> tasks = _context.TaskItems.Include(t => t.User);

            if (userRole != "Admin") // ✅ FIX: Ensure Admins see all tasks
            {
                tasks = tasks.Where(t => t.UserId == userId || t.UserId == null); // Users see their own tasks
            }

            Console.WriteLine($"Tasks Retrieved: {tasks.Count()}");

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
                    tasks = tasks.OrderByDescending(t => t.CreatedAt);
                    break;
            }

            return View(tasks.ToList());
        }



        public IActionResult Create()
        {
            ViewBag.Users = new SelectList(_context.Users, "Id", "Username"); // ✅ FIX: Allow all users to assign tasks
            return View();
        }



        [HttpPost]
        public IActionResult Create(TaskItem model)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine($"Creating Task: Title={model.Title}, Description={model.Description}, UserId={model.UserId}");

                var task = new TaskItem
                {
                    Title = model.Title,
                    Description = model.Description,
                    IsCompleted = false,
                    UserId = model.UserId > 0 ? model.UserId : null // ✅ Ensure UserId is set correctly
                };

                _context.TaskItems.Add(task);
                _context.SaveChanges();

                Console.WriteLine("Task Created Successfully!");
                return RedirectToAction("Index");
            }

            Console.WriteLine("Task Creation Failed! ModelState is not valid.");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Validation Error: {error.ErrorMessage}");
            }

            ViewBag.Users = new SelectList(_context.Users.Where(u => u.Role != "Admin"), "Id", "Username");
            return View(model);
        }


        public IActionResult Edit(int id)
        {
            var task = _context.TaskItems.Include(t => t.User).FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            ViewBag.Users = new SelectList(_context.Users.Where(u => u.Role != "Admin"), "Id", "Username", task.UserId);

            return View(task);
        }

        [HttpPost]
        public IActionResult Edit(int id, TaskItem model)
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
                task.UserId = model.UserId; // Assign task to selected user

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Users = new SelectList(_context.Users.Where(u => u.Role != "Admin"), "Id", "Username", model.UserId);
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
