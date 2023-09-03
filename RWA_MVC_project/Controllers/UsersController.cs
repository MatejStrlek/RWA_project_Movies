using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using RWA_MVC_project.Filters;
using RWA_MVC_project.Models;
using System.Drawing.Printing;
using System.Security.Cryptography;
using System.Text;

namespace RWA_MVC_project.Controllers
{
    [TypeFilter(typeof(LoginFilter))]
    public class UsersController : Controller
    {
        private readonly RwaMoviesContext _context;

        public UsersController(RwaMoviesContext context)
        {
            _context = context;
        }

        // GET: Users
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Index()
        {
            if (Request.Cookies.ContainsKey("searchUsers"))
            {
                string searchText = Request.Cookies["searchUsers"];

                var rwaUsersContext = _context.Users.Include(u => u.CountryOfResidence)
                                    .Where(u => u.FirstName.Contains(searchText)
                                    || u.LastName.Contains(searchText)
                                    || u.Username.Contains(searchText)
                                    || u.CountryOfResidence.Name.Contains(searchText));

                ViewData["searchUsers"] = searchText;

                 return View(await rwaUsersContext.ToListAsync());
            }
            else
            {
                var rwaUsersContext = _context.Users.Include(u => u.CountryOfResidence);

                return View(await rwaUsersContext.ToListAsync());
            }                     
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.CountryOfResidence)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        [TypeFilter(typeof(AdministratorFilter))]
        public IActionResult Create()
        {
            ViewData["CountryOfResidenceId"] = new SelectList(_context.Countries, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Create(LoginUser user)
        {
            ModelState.Clear();

            byte[] saltBytes = Salt();
            string saltString = Convert.ToBase64String(saltBytes);

            string randomToken = GenerateRandomToken();

            User userForDb = new User
            {
                CreatedAt = DateTime.Now,
                DeletedAt = null,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PwdHash = HashPwd(user.Password, saltBytes),
                PwdSalt = saltString,
                Phone = user.Phone,
                IsConfirmed = false,
                SecurityToken = randomToken,
                CountryOfResidenceId = user.CountryOfResidenceId
            };

            if (ModelState.IsValid)
            {
                _context.Add(userForDb);
                await _context.SaveChangesAsync();
                return Redirect("/Users");
            }

            ViewData["CountryOfResidenceId"] = new SelectList(_context.Countries, "Id", "Id", user.CountryOfResidenceId);
            return View(user);
        }

        [TypeFilter(typeof(AdministratorFilter))]
        public IActionResult Search(string searchText)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                Response.Cookies.Append("searchUsers", searchText);
            }

            searchText = !string.IsNullOrEmpty(searchText) ? searchText : Request.Cookies["searchUsers"];

            var users = _context.Users
                .Include(u => u.CountryOfResidence)
                .Where(u => u.Username.Contains(searchText) ||
                u.FirstName.Contains(searchText) ||
                u.LastName.Contains(searchText) ||
                u.Email.Contains(searchText) ||
                u.Phone!.Contains(searchText) ||
                u.CountryOfResidence.Name.Contains(searchText));

            return View("Index", users);
        }

        public IActionResult ClearSearch()
        {
            Response.Cookies.Delete("searchUsers");
            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Edit/5
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["CountryOfResidenceId"] = new SelectList(_context.Countries, "Id", "Name", user.CountryOfResidenceId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreatedAt,DeletedAt,Username,FirstName,LastName,Email,PwdHash,PwdSalt,Phone,IsConfirmed,SecurityToken,CountryOfResidenceId")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            var existingUser = await _context.Users.FindAsync(id);
            _context.Entry(existingUser).State = EntityState.Detached;

            user.PwdHash = existingUser.PwdHash;
            user.PwdSalt = existingUser.PwdSalt;

            ModelState.Remove("CountryOfResidence");
            ModelState.Remove("PwdHash");
            ModelState.Remove("PwdSalt");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryOfResidenceId"] = new SelectList(_context.Countries, "Id", "Id", user.CountryOfResidenceId);
            return View(user);
        }

        // GET: Users/Delete/5
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.CountryOfResidence)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (_context.Users == null)
                {
                    return Problem("Entity set 'RwaMoviesContext.Users'  is null.");
                }
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        private bool UserExists(int id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }       

        private string GenerateRandomToken()
        {
            int length = 6;
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }

            return new string(result);
        }

        private byte[] Salt()
        {
            int saltSize = 16;
            byte[] saltBytes = new byte[saltSize];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            return saltBytes;
        }

        private string HashPwd(string password, byte[] saltBytes)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];

                Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
                Buffer.BlockCopy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);

                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
