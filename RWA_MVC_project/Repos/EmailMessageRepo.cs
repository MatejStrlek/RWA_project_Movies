using RWA_MVC_project.Models;

namespace RWA_MVC_project.Repos
{
    public interface IEmailMessageRepo
    {
        IEnumerable<Notification> GetEmailMessages();
    }

    public class EmailMessageRepo : IEmailMessageRepo
    {
        private readonly RwaMoviesContext _context;

        public EmailMessageRepo(RwaMoviesContext context)
        {
            _context = context;
        }

        public IEnumerable<Notification> GetEmailMessages()
        {
            return _context.Notifications.ToList();
        }
    }
}
