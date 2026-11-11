
using Oracle.ManagedDataAccess.Client;
using SparePackaging.Domain.Interfaces;
using SparePackaging.Domain.Models;
using SparePackaging.Infrastructure.Persistence;
using System.Data;

namespace SparePackaging.Infrastructure.Repositories;

public class VendorItemRepository : OracleRepository, IVendorItemRepository
{
    private readonly OracleConnectionFactory _factory;

    public VendorItemRepository(OracleConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<List<VendorItem>> GetVendorItemsAsync( string vendorCode,CancellationToken cancellationToken = default)
    {
        var items = new List<VendorItem>();

        if (string.IsNullOrWhiteSpace(vendorCode))
            return items;

        using var connection = _factory.CreateConnection();

        await connection.OpenAsync(cancellationToken);

        using var command = CreateCommand(connection,"USP_GET_VENDOR_ITEMS");

        command.Parameters.Add("P_VENDOR_CODE",OracleDbType.Varchar2).Value = vendorCode;

        command.Parameters.Add("P_RESULT",OracleDbType.RefCursor).Direction = ParameterDirection.Output;

        using var reader =await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            items.Add(new VendorItem
            {
                ItemCode = GetString(reader["ITEM_CODE"]),
                SaleUnit = GetString(reader["SALE_UNIT"]),
                ItemDescription = GetString(
                    reader["ITEM_DESCRIPTION"]),
                DocumentId = GetString(reader["DOCUMENT_ID"]),
                Status = GetString(reader["STATUS"]),
                IsTertiary = GetInt(reader["IS_TERTIARY"]) == 1
            });
        }

        return items;
    }
}
