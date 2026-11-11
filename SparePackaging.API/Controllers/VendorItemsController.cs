using Microsoft.AspNetCore.Mvc;
using SparePackaging.Application.Helpers;
using SparePackaging.Application.Interfaces;
namespace SparePackaging.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VendorItemsController : ControllerBase
{
    private readonly IVendorItemService _vendorItemService;

    public VendorItemsController(IVendorItemService vendorItemService)
    {
        _vendorItemService = vendorItemService;
    }

    [HttpGet("GetVendorItems")]
    public async Task<IActionResult> GetVendorItems([FromQuery] string vendorCode, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(vendorCode))
        {
            var badRequest =ApiResponseBuilder.GenerateBadRequest( "Vendor Code is required.", null);

            return BadRequest(badRequest);
        }

        var items = await _vendorItemService.GetVendorItemsAsync(vendorCode,cancellationToken);

        var response = ApiResponseBuilder.GenerateOK(items,200,"Vendor items retrieved successfully.", "Vendor items loaded successfully.");

        return Ok(response);
    }
}
