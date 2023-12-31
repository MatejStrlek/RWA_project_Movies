﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWA_MVC_project.Filters;
using RWA_MVC_project.Models;
using RWA_MVC_project.Repos;
using RWA_MVC_project.Services;

namespace RWA_MVC_project.Controllers
{
    [TypeFilter(typeof(LoginFilter))]
    public class NotificationsController : Controller
    {
        private const string SenderMail = "RWA_project@algebra.hr";

        private readonly RwaMoviesContext _context;
        private readonly IMailSender _mailSender;
        private readonly IEmailMessageRepo _emailMessageRepo;

        public NotificationsController(RwaMoviesContext context, IMailSender mailSender, IEmailMessageRepo emailMessageRepo)
        {
            _context = context;
            _mailSender = mailSender;
            _emailMessageRepo = emailMessageRepo;
        }

        // GET: Notifications
        public async Task<IActionResult> Index()
        {
            string usernamefromCookie = Request.Cookies["username"];
            var user = _context.Users.FirstOrDefault(u => u.Username == usernamefromCookie);

            return _context.Notifications != null ? 
                          View(await _context.Notifications
                          .Where(n => n.ReceiverEmail == user.Email)
                          .ToListAsync()) :
                          Problem("Entity set 'RwaMoviesContext.Notifications'  is null.");
        }

        // GET: Notifications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Notifications == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // GET: Notifications/Create
        [TypeFilter(typeof(AdministratorFilter))]
        public IActionResult Create()
        {
            string usernamefromCookie = Request.Cookies["username"];
            var user = _context.Users.FirstOrDefault(u => u.Username == usernamefromCookie);

            ViewData["Emails"] = _context.Users.Select(u => u.Email)
                   .Where(e => e != user.Email)
                   .ToList();

            return View();
        }

        // POST: Notifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Create([Bind("Id,CreatedAt,ReceiverEmail,Subject,Body,SentAt")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                string usernamefromCookie = Request.Cookies["username"];
                var user = _context.Users.FirstOrDefault(u => u.Username == usernamefromCookie);

                var formData = HttpContext.Request.Form;

                string subject = formData["Subject"];
                string body = formData["Body"];
                string receiverEmail = formData["ReceiverEmail"];               

                if (user != null)
                    _mailSender.SendMail(SenderMail, receiverEmail, subject, body);

                _context.Add(notification);
                await _context.SaveChangesAsync();
                return View("NotificationSendResult");
            }
            return View(notification);
        }

        // GET: Notifications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Notifications == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Notifications == null)
            {
                return Problem("Entity set 'RwaMoviesContext.Notifications'  is null.");
            }
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificationExists(int id)
        {
          return (_context.Notifications?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
