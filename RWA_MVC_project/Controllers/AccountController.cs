using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RWA_MVC_project.Filters;
using RWA_MVC_project.Models;

namespace RWA_MVC_project.Controllers
{
    public class AccountController : Controller
    {
        private readonly RwaMoviesContext _context;

        public AccountController(RwaMoviesContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Login(LoginUser userViewModel)
        {
            ModelState.Clear();
            var userFromDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == userViewModel.Username);

            if (userFromDb != null)
            {
                string hashed = HashPwd(userViewModel.Password, Convert.FromBase64String(userFromDb.PwdSalt));

                if (hashed == userFromDb.PwdHash)
                {
                    string cookieUsername = userFromDb.Username;

                    CookieOptions cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(1),
                        HttpOnly = true
                    };
                    Response.Cookies.Append("username", cookieUsername, cookieOptions);

                    return Redirect("/Videos");
                }
                else
                {
                    ModelState.AddModelError("Password", "Wrong password");
                }
            }

            return View(nameof(Login));
        }

        [TypeFilter(typeof(LoginFilter))]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("username");
            return Redirect("/Account/Login");
        }

        public IActionResult Register()
        {
            ViewData["CountryOfResidenceId"] = new SelectList(_context.Countries, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(LoginUser user)
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
                return RedirectToAction(nameof(Login));
            }

            ViewData["CountryOfResidenceId"] = new SelectList(_context.Countries, "Id", "Id", user.CountryOfResidenceId);
            return View(user);
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
