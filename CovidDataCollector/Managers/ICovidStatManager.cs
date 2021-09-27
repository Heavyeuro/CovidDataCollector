using CovidDataCollector.Models;
using System.Threading.Tasks;

namespace CovidDataCollector.Managers
{
    public interface ICovidStatManager
    {
        public Task<BaseCovidStatModel> GetCovidStatByCountryCode(string countryCode);
    }
}
