
using SparePackaging.Application.ViewModels;

namespace SparePackaging.Application.Interfaces;

public interface IVendorItemService
{
    Task<List<VendorItemVm>> GetVendorItemsAsync(string vendorCode,CancellationToken cancellationToken = default);
}
