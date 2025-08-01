using CourseStoreMinimalAPI.DAL;
using CourseStoreMinimalAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace CourseStoreMinimalAPI.AplicationService
{
    public class CategoryService(CourseDbContext _ctx)
    {
        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _ctx.Categories.OrderBy(c => c.Name).ThenBy(c => c.Id).AsNoTrackingWithIdentityResolution().ToListAsync();
        }
        public async Task<Category?> GetCategoriesAsync(int id)
        {
            return await _ctx.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<int> Insert(Category category)
        {
            _ctx.Categories.Add(category);
            await _ctx.SaveChangesAsync();
            return category.Id;
        }
    }
}
