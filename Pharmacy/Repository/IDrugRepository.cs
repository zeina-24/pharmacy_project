using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Models;
namespace Pharmacy.Repository
{
    public interface IDrugRepository
    {
        IEnumerable<Drug> GetAll();
        Drug? GetById(int id);
    
        IEnumerable<Drug> Search(string? name, string? barcode, string? type, List<string>? tags);
        IQueryable<Drug> FilterByName(IQueryable<Drug> query, string name);
        IQueryable<Drug> FilterByBarcode(IQueryable<Drug> query, string barcode);
        IQueryable<Drug> FilterByType(IQueryable<Drug> query, string type);
        IQueryable<Drug> FilterByTags(IQueryable<Drug> query, List<string> tags);
       IEnumerable<Drug> SearchByName(string name);
        void Add(Drug drug);
      
        public void SaveChanges();
        void Update(Drug drug);
        void Delete(int id);
        public Drug GetByIdWithTags(int id);
        public Tag CreateTag(string name);
        public Tag? GetTagByName(string name);
        




    }
}