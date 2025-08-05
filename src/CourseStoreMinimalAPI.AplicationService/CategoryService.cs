using CourseStoreMinimalAPI.DAL;
using CourseStoreMinimalAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace CourseStoreMinimalAPI.AplicationService
{
    public class CategoryService(CourseStoreDbContext ctx)
    {

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await ctx.Categories.OrderBy(c => c.Name).ThenBy(c => c.Id).AsNoTrackingWithIdentityResolution().ToListAsync();
        }
        public async Task<Category?> GetCategoriesAsync(int id)
        {
            return await ctx.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<bool> Exist(int id)
        {
            return await ctx.Categories.AnyAsync(c => c.Id == id);
        }
        public async Task<int> Insert(Category category)
        {
            ctx.Categories.Add(category);
            await ctx.SaveChangesAsync();
            return category.Id;
        }

        public async Task UpdateAsync(Category category)
        {
            ctx.Update(category);
            await ctx.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await ctx.Categories.Where(c => c.Id == id).ExecuteDeleteAsync();
        }
    }
}
