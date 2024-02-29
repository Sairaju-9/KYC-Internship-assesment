using Code_plus.Models.Domain;

namespace Code_plus.Repositories.Interface
{
    public interface IcaterogryRespository
    {
        Task<Category> CreateAsync(Category category);

        Task<IEnumerable<Category>> GetAllAsync();

        Task<Category> GetCategoryById(Guid id);

        Task<Category?>UpdateAsync(Category category);

        Task<Category?> DeleteAsyc(Guid id);
        
    }

}
