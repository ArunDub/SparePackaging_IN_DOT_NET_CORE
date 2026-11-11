
using AutoMapper;
using SparePackaging.Application.Interfaces;
using SparePackaging.Application.ViewModels;
using SparePackaging.Domain.Interfaces;

namespace SparePackaging.Application.Services;

public class VendorItemService : IVendorItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VendorItemService(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<VendorItemVm>> GetVendorItemsAsync(string vendorCode,CancellationToken cancellationToken = default)
    {
        var models = await _unitOfWork.VendorItemRepository.GetVendorItemsAsync(vendorCode,cancellationToken);

        return _mapper.Map<List<VendorItemVm>>(models);
    }
}
