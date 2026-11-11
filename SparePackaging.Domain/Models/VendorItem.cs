
namespace SparePackaging.Domain.Models;

public class VendorItem
{
    public string ItemCode { get; set; } = string.Empty;

    public string SaleUnit { get; set; } = string.Empty;

    public string ItemDescription { get; set; } = string.Empty;

    public string DocumentId { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public bool IsTertiary { get; set; }
}
