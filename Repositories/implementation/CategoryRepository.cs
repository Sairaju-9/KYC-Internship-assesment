using Code_plus.Data;
using Code_plus.Models.Domain;
using Code_plus.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Code_plus.Repositories.implementation
{
    public class CategoryRepository : IcaterogryRespository
    {
        private readonly ApllicationDbContext dbContext;

        public CategoryRepository(ApllicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            return category;

        }

        

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await dbContext.Categories.ToListAsync();

        }

        public async Task<Category> GetCategoryById(Guid id)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(x=> x.Id==id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
           var existingcategory= await dbContext.Categories.FirstOrDefaultAsync(x => x.Id== category.Id);
            if (existingcategory != null)
            {
                dbContext.Entry(existingcategory).CurrentValues.SetValues(category);
                await dbContext.SaveChangesAsync();
                return category;
            }
            return null;
        }

        public async Task<Category?> DeleteAsyc(Guid id)
        {
           var ExistingCategory= await dbContext.Categories.FirstOrDefaultAsync(x=> x.Id == id);

            if (ExistingCategory != null)
            {
                dbContext.Categories.Remove(ExistingCategory);
                dbContext.SaveChanges();
                return ExistingCategory;
            }
            
            return null;
        }


    }
}
