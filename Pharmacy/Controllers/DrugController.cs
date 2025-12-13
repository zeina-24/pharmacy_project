using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Models;
using Pharmacy.Models.Dto;
using Pharmacy.Repository;
using Pharmacy.Services;


namespace Pharmacy.Controllers
{
   
        [ApiController]
        [Route("api/[controller]")]
        public class DrugsController : ControllerBase
        {
            private readonly IDrugService _drugService;
            private readonly IWebHostEnvironment _env;
            private readonly List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png" };
            private const long MaxAllowedSize = 100 * 1024 * 1024;

            public DrugsController(IDrugService drugService, IWebHostEnvironment env)
            {
                _drugService = drugService;
                _env = env;
            }

            // GET: api/drugs
            [HttpGet]
      //  [Authorize(Roles = "admin,cashier")]
        public IActionResult GetAllDrugs()
            {
                var drugs = _drugService.GetAllDrugs();
                return Ok(drugs);
            }

            // GET: api/drugs/5
            [HttpGet("{id}")]
       // [Authorize(Roles = "admin,cashier")]

        public IActionResult GetDrugById(int id)
            {
                var drug = _drugService.GetDrugById(id);
                if (drug == null)
                    return NotFound(new { message = "Drug not found" });
                return Ok(drug);
            }

            // GET: api/drugs/search
            [HttpGet("search")]
       // [Authorize(Roles = "admin,cashier")]

        public IActionResult SearchDrugs(string? name, string? barcode, string? type, string? tags)
            {
                var drugs = _drugService.SearchDrugs(name, barcode, type, tags);
                return Ok(drugs);
            }

            // POST: api/drugs
            [HttpPost]
        //[Authorize(Roles = "admin")]

        public IActionResult AddDrug([FromForm] DrugCreateDto dto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (dto.ImageUrl == null)
                    return BadRequest("Image is required");

                
                string ext = Path.GetExtension(dto.ImageUrl.FileName).ToLower();
                if (!_allowedExtensions.Contains(ext))
                    return BadRequest("Only .jpg and .png images are allowed!");
                if (dto.ImageUrl.Length > MaxAllowedSize)
                    return BadRequest("Max allowed size for Image is 100MB");
                string uploads = Path.Combine(_env.WebRootPath, "images");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                string fileName = Guid.NewGuid().ToString() + ext;
                string filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    dto.ImageUrl.CopyTo(stream);
                }

                string imageUrl = $"images/{fileName}";

                // Call service to create drug
                var result = _drugService.CreateDrug(dto, imageUrl);

                if (!result.Success)
                    return BadRequest(result.ErrorMessage);

                return Ok(new
                {
                    success = true,
                    msg = "Drug created",
                    drugId = result.DrugId,
                    image = result.ImagePath
                });
            }

            // PUT: api/drugs/5
            [HttpPut("{id}")]
       // [Authorize(Roles = "admin")]

        public IActionResult UpdateDrug(int id, [FromBody] DrugUpdateDto dto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updated = _drugService.UpdateDrug(id, dto);

                if (!updated)
                    return BadRequest(new { success = false, message = "Update failed" });

                return Ok(new { success = true, message = "Drug updated successfully" });
            }

            // DELETE: api/drugs/5
            [HttpDelete("{id}")]
      //  [Authorize(Roles = "admin")]

        public IActionResult Delete(int id)
            {
                var deleted = _drugService.DeleteDrug(id);

                if (!deleted)
                    return NotFound(new { message = "Drug not found" });

            return Ok(new { success = true, message = "Drug Deleted successfully" });
             }

       } 
    
}
