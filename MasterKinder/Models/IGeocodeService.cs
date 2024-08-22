using MasterKinder.Services;

namespace MasterKinder.Models
{
    public interface IGeocodeService
    {
        Task<GeocodeResult> GeocodeAddress(string address);
    }
}
