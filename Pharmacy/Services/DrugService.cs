using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Pharmacy.Models;
using Pharmacy.Models.Dto;
using Pharmacy.Repository;

namespace Pharmacy.Services
{
    public class DrugService:IDrugService
    {
        private readonly IDrugRepository _repo;
        private readonly IMapper _mapper;
        

        public DrugService(IDrugRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            
        }
        public IEnumerable<DrugSummaryDto> GetAllDrugs()
        {
            var drugs = _repo.GetAll();
            return _mapper.Map<List<DrugSummaryDto>>(drugs);
        }

        public DrugSummaryDto? GetDrugById(int id)
        {
            var drug = _repo.GetById(id);
            return drug == null ? null : _mapper.Map<DrugSummaryDto>(drug);
        }

        public DrugCreated CreateDrug(DrugCreateDto dto, string imageUrl)
        {
            // Validate business rules
            if (dto.SellingPrice < dto.PurchasingPrice)
            {
                return new DrugCreated
                {
                    Success = false,
                    ErrorMessage = "Selling price cannot be less than purchasing price"
                };
            }

            if (dto.ExpirationDate.HasValue && dto.ExpirationDate <= DateOnly.FromDateTime(DateTime.Now))
            {
                return new DrugCreated
                {
                    Success = false,
                    ErrorMessage = "Expiration date must be in the future"
                };
            }

          
            var drug = _mapper.Map<Drug>(dto);
            drug.ImageUrl = imageUrl;
            if (dto.Tags != null)
            {
                foreach (var tagName in dto.Tags.Where(t => !string.IsNullOrWhiteSpace(t)))
                {
                    var tag = _repo.GetTagByName(tagName);
                    if (tag == null)
                    {
                        tag = _repo.CreateTag(tagName);
                    }
                    drug.Tags.Add(tag);
                }
            }

            _repo.Add(drug);
            _repo.SaveChanges();

            return new DrugCreated
            {
                Success = true,
                DrugId = drug.DrugId,
                ImagePath = drug.ImageUrl
            };
        }

        public bool UpdateDrug(int id, DrugUpdateDto dto)
        {
            var existing = _repo.GetByIdWithTags(id);
            if (existing == null) return false;

            // Validate drug type
            var allowedTypes = new List<string> { "drops", "spray", "gel", "cream", "capsule", "injection", "syrup", "tablet" };
            if (!allowedTypes.Contains(dto.DrugType?.ToLower()))
                return false;

            _mapper.Map(dto, existing);

            // Update tags
            if (dto.Tags != null)
            {
                var updatedTags = new List<Tag>();
                foreach (var name in dto.Tags)
                {
                    var tag = _repo.GetTagByName(name);
                    if (tag == null) return false; // Tag doesn't exist
                    updatedTags.Add(tag);
                }
                existing.Tags = updatedTags;
            }

            _repo.SaveChanges();
            return true;
        }

        public bool DeleteDrug(int id)
        {
            var existing = _repo.GetById(id);
            if (existing == null) return false;

            _repo.Delete(id);
            return true;
        }
        public IEnumerable<DrugSummaryDto> SearchDrugs(string? name, string? barcode, string? type, string? tags)
        {
            List<string>? tagList = null;
            if (!string.IsNullOrEmpty(tags))
            {
                tagList = tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                              .ToList();
            }
            var drugsQuery = _repo.Search(name, barcode, type, tagList);
            var drugs = drugsQuery.ToList();
            return _mapper.Map<List<DrugSummaryDto>>(drugs);
        }
       
    }


}
