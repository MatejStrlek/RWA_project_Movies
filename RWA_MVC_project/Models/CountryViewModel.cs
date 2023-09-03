using X.PagedList;

namespace RWA_MVC_project.Models
{
    public class CountryViewModel
    {
        public IEnumerable<Country> ModelList { get; set; }
        public IPagedList<Country> PagedList { get; set; }

        public string Name => ModelList?.FirstOrDefault()?.Name;
        public string Code => ModelList?.FirstOrDefault()?.Code;
    }
}
