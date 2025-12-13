using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Models;

namespace Pharmacy.Repository
{
    public class DrugRepository : IDrugRepository
    {
        private readonly PharmacyDbContext _context;

        public DrugRepository(PharmacyDbContext context)
        {
            _context = context;
        }
       
        public IEnumerable<Drug> GetAll()
        {
            return _context.Drugs.Include(d => d.Tags).ToList(); 
        }

        public Drug? GetById(int id)
        {
            return _context.Drugs.Include(d=>d.Tags).FirstOrDefault(d => d.DrugId == id);
        }
        public Drug? GetByBarcode(string barcode)
        {
            return _context.Drugs.Include(d=>d.Tags).FirstOrDefault(d => d.Barcode == barcode);
        }
        public IEnumerable<Drug> SearchByName(string name)
        {
            return _context.Drugs
                           .Where(d => d.Name.Contains(name))
                           .ToList();
        }
        public IQueryable<Drug> FilterByName(IQueryable<Drug> query, string name)
        {
            return query.Where(d => d.Name.Contains(name));
        }

        public IQueryable<Drug> FilterByBarcode(IQueryable<Drug> query, string barcode)
        {
            return query.Where(d => d.Barcode.Contains(barcode));
        }

        public IQueryable<Drug> FilterByType(IQueryable<Drug> query, string type)
        {
            return query.Where(d => d.DrugType == type);
        }

        public IQueryable<Drug> FilterByTags(IQueryable<Drug> query, List<string> tags)
        {
            return query.Where(d => d.Tags.Any(t => tags.Contains(t.Name)));
        }

        public IEnumerable<Drug> Search(string? name, string? barcode, string? type, List<string>? tags)
        {
            var query = _context.Drugs.Include(d => d.Tags).AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = FilterByName(query, name);

            if (!string.IsNullOrEmpty(barcode))
                query = FilterByBarcode(query, barcode);

            if (!string.IsNullOrEmpty(type))
                query = FilterByType(query, type);

            if (tags != null && tags.Count > 0)
                query = FilterByTags(query, tags);

            return query.ToList();
        }

        public void Add(Drug drug)
        {
            if (drug == null)
                throw new ArgumentNullException(nameof(drug));

            _context.Drugs.Add(drug);
            _context.SaveChanges();
        }

        public void Update (Drug drug)
        {
           _context.Drugs.Update(drug);
            
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var drug = _context.Drugs.Find(id);
            if (drug != null)
            {
                _context.Drugs.Remove(drug);
                _context.SaveChanges();
            }
        }
        
        public Drug GetByIdWithTags(int id)
        {
            return _context.Drugs
                .Include(d => d.Tags) 
                .FirstOrDefault(d => d.DrugId == id);
        }
        public Tag CreateTag(string name)
        {
            var tag = new Tag { Name = name };
            _context.Tags.Add(tag);
            _context.SaveChanges();
            return tag;
        }
        public Tag? GetTagByName(string name)
        {
            return _context.Tags.FirstOrDefault(t => t.Name == name);
        }
    }
}
