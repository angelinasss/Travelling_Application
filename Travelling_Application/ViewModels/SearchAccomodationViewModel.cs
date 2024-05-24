using Travelling_Application.Models;

namespace Travelling_Application.ViewModels
{
    public class SearchAccomodationViewModel
    {
        public List<string> Cities { get; set; }
        public List<Accomodation> Results { get; set; }
    }
}
