using Microsoft.AspNetCore.Http;
using Code_plus.Models.Domain;
using Code_plus.Models.DTO;
using Code_plus.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Code_plus.Controllers
{    //https://localhost:XXXX/api/categories
    [Route("api/[controller]")]
    [ApiController]
    public class Catergories : ControllerBase
    {
        private readonly IcaterogryRespository caterogryRespository;

        public Catergories(IcaterogryRespository caterogryRespository)
        {
            this.caterogryRespository = caterogryRespository;
        }
        //
        [HttpPost]
        public async Task<IActionResult> CreateCatergories([FromBody]CreateCategoryRequest request)
        {
            //Map DTO to Domain model
            var category = new Category
            {
                Name = request.Name,
                urlHandle = request.urlHandle

            };

            await caterogryRespository.CreateAsync(category);

            



            //Domain model to DTO

            var response = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                urlHandle = category.urlHandle
            };

          

            return Ok(response);
            

            
        }

        //GET:/api/category
        [HttpGet]
        public async Task<IActionResult> GetCatergories()
        {
            var categories= await caterogryRespository.GetAllAsync();

            var response = new List<CategoryDTO>();
            foreach (var category in categories)
            {
                response.Add(new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    urlHandle = category.urlHandle
                });
            }


            return Ok(response);
        }

        //GET:/api/categories/{id}
        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> CategoryById([FromRoute] Guid id)
        {
            var existingid = await caterogryRespository.GetCategoryById(id);

            if (existingid == null) { return NotFound(); }

            //map domain to DTO

            var response = new CategoryDTO
            {
                Id = existingid.Id,
                Name = existingid.Name,
                urlHandle = existingid.urlHandle
            };
            return Ok(response);

        }
        

        //PUT:/api/catergoies/{id}
        [HttpPut]
        [Route("{id:Guid}")]

        public async Task<IActionResult>EditCategory([FromRoute] Guid id,UpdateCatergoryDTO request)
        {
            //Convert requestDTO to Domain model
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                urlHandle = request.urlHandle
            };

            category= await caterogryRespository.UpdateAsync(category);

            if (category == null)
            {
                return NotFound();
            }
            //convert domaim to DTO
            var response = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                urlHandle = category.urlHandle
            };
            return Ok(response);

        }

        //DELETE:api/category/:{id}
        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult>DeleteCategory([FromRoute] Guid id)
        {
            var ExisitingCategory=await caterogryRespository.DeleteAsyc(id);
            if (ExisitingCategory == null) { return NotFound(); }

            //Domaim to DTo

            var response = new CategoryDTO
            {
                Id = ExisitingCategory.Id,
                Name = ExisitingCategory.Name,
                urlHandle = ExisitingCategory.urlHandle
            };
            return Ok(response);

        }



        // GET api/entities/search
        [HttpGet("search")]
        public IActionResult SearchEntities(
            [FromQuery] string search,
            [FromQuery] string? gender,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] List<string>? countries)
        {
            // Initialize a variable to hold the filtered entities
            List<Entity> filteredEntities = new List<Entity>();

            // Perform a search across entities using the provided search text
            if (!string.IsNullOrEmpty(search))
            {
                filteredEntities = YourDatabaseContext.Entities
                    .Where(e => e.Names.Any(n =>
                        n.FirstName.Contains(search) ||
                        n.MiddleName.Contains(search) ||
                        n.Surname.Contains(search)))
                    .ToList();
            }
            else
            {
                // If no search text is provided, fetch all entities
                filteredEntities = YourDatabaseContext.Entities.ToList();
            }

            // Apply gender filtering if provided
            if (!string.IsNullOrEmpty(gender))
            {
                filteredEntities = filteredEntities
                    .Where(e => e.Gender == gender)
                    .ToList();
            }

            // Apply date filtering if start date and/or end date are provided
            if (startDate != null)
            {
                filteredEntities = filteredEntities
                    .Where(e => e.Dates.Any(d => d.Date >= startDate))
                    .ToList();
            }
            if (endDate != null)
            {
                filteredEntities = filteredEntities
                    .Where(e => e.Dates.Any(d => d.Date <= endDate))
                    .ToList();
            }

            // Apply country filtering if provided
            if (countries != null && countries.Any())
            {
                filteredEntities = filteredEntities
                    .Where(e => e.Addresses.Any(a => countries.Contains(a.Country)))
                    .ToList();
            }

            // Return the filtered entities
            return Ok(filteredEntities);
        }



        // GET api/entities/search
        [HttpGet("search")]
        public IActionResult SearchEntities(
            [FromQuery] string search,
            [FromQuery] string? gender,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] List<string>? countries)
        {
            // Retrieve all entities from the database or any data source
            var allEntities = _entityService.GetAllEntities(); // You'll need to implement this method in your service layer

            // Filter entities based on the provided search query
            var filteredEntities = allEntities.Where(entity =>
                entity.Addresses != null && entity.Addresses.Any(address =>
                    (address.Country != null && address.Country.Contains(search)) ||
                    (address.AddressLine != null && address.AddressLine.Contains(search))) ||
                entity.Names != null && entity.Names.Any(name =>
                    (name.FirstName != null && name.FirstName.Contains(search)) ||
                    (name.MiddleName != null && name.MiddleName.Contains(search)) ||
                    (name.Surname != null && name.Surname.Contains(search))));

            // Apply advanced filtering
            if (!string.IsNullOrEmpty(gender))
            {
                filteredEntities = filteredEntities.Where(entity => entity.Gender == gender);
            }

            if (startDate != null)
            {
                filteredEntities = filteredEntities.Where(entity =>
                    entity.Dates.Any(date => date.Date != null && date.Date >= startDate));
            }

            if (endDate != null)
            {
                filteredEntities = filteredEntities.Where(entity =>
                    entity.Dates.Any(date => date.Date != null && date.Date <= endDate));
            }

            if (countries != null && countries.Any())
            {
                filteredEntities = filteredEntities.Where(entity =>
                    entity.Addresses != null && entity.Addresses.Any(address =>
                        address.Country != null && countries.Contains(address.Country)));
            }

            return Ok(filteredEntities);
        }
            
        
        
    }

    
   

}
 
