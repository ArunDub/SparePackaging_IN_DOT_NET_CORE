
using SparePackaging.Domain.Models;

namespace SparePackaging.Domain.Interfaces;

public interface IVendorItemRepository//: IRepository<VendorItem>
{
    Task<List<VendorItem>> GetVendorItemsAsync(string vendorCode, CancellationToken cancellationToken = default);
}
