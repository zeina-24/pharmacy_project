using Pharmacy.Models.Dto;

namespace Pharmacy.Services
{
    public interface IDrugService
    {
        IEnumerable<DrugSummaryDto> GetAllDrugs();
        DrugSummaryDto? GetDrugById(int id);
        IEnumerable<DrugSummaryDto> SearchDrugs(string? name, string? barcode, string? type, string? tags);
        DrugCreated CreateDrug(DrugCreateDto dto, string image);
        bool UpdateDrug(int id, DrugUpdateDto dto);
        bool DeleteDrug(int id);
    }
}
