using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWA_MVC_project.Filters;
using RWA_MVC_project.Models;
using X.PagedList;

namespace RWA_MVC_project.Controllers
{
    [TypeFilter(typeof(LoginFilter))]
    public class CountriesController : Controller
    {
        private readonly RwaMoviesContext _context;

        public CountriesController(RwaMoviesContext context)
        {
            _context = context;
        }

        // GET: Countries
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;

            var pagedModel = await _context.Countries.ToPagedListAsync(pageNumber, pageSize);
            var viewModel = new CountryViewModel
            {
                ModelList = pagedModel,
                PagedList = pagedModel
            };          

            if (viewModel == null)
            {
                return Problem("Entity set 'RwaMoviesContext.Countries' is null.");
            }

            return View(viewModel);
        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: Countries/Create
        [TypeFilter(typeof(AdministratorFilter))]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Create([Bind("Id,Code,Name")] Country country)
        {
            if (ModelState.IsValid)
            {
                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        public IActionResult Search(string searchText, int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;

            var countriesQuery = _context.Countries
                .Where(c => c.Name.Contains(searchText) ||
                c.Code.Contains(searchText));

            var countriesPaged = countriesQuery.ToPagedList(pageNumber, pageSize);

            var viewModelCountries = new CountryViewModel
            {
                ModelList = countriesPaged,
                PagedList = countriesPaged
            };

            return View("Index", viewModelCountries);
        }

        // GET: Countries/Edit/5
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name")] Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.Id))
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
            return View(country);
        }

        // GET: Countries/Delete/5
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (_context.Countries == null)
                {
                    return Problem("Entity set 'RwaMoviesContext.Countries'  is null.");
                }
                var country = await _context.Countries.FindAsync(id);
                if (country != null)
                {
                    _context.Countries.Remove(country);
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

        private bool CountryExists(int id)
        {
          return (_context.Countries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
