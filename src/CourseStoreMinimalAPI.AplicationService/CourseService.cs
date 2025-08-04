using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseStoreMinimalAPI.DAL;
using CourseStoreMinimalAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseStoreMinimalAPI.AplicationService;

public class CourseService(CourseDbContext ctx)
{
    #region Read
    public async Task<List<Course>> GetAll(int pageNumber, int countInPage)
    {
        int skip = (pageNumber - 1) * countInPage;
        return await ctx.Courses.OrderBy(c => c.Title).ThenBy(c => c.StartDate).Skip(skip).Take(countInPage).AsNoTracking().ToListAsync();
    }
    public async Task<int> GetTotalCountAsync()
    {
        return await ctx.Courses.CountAsync();
    }
    public async Task<Course?> GetCourseAsync(int id)
    {
        return await ctx.Courses.FirstOrDefaultAsync(c => c.Id == id);
    }
    public async Task<List<Course>> Search(string title, bool? isOnline)
    {
        var courses = ctx.Courses.AsQueryable();
        if (!string.IsNullOrWhiteSpace(title))
        {
            courses = courses.Where(c => c.Title.Contains(title));
        }
        if (isOnline is not null)
        {
            courses = courses.Where(c => c.IsOnline == isOnline);
        }
        return await courses.AsNoTracking().OrderBy(c => c.Title).ThenBy(c => c.StartDate).ToListAsync();
    }
    public async Task<bool> Exist(int id)
    {
        return await ctx.Courses.AnyAsync(c => c.Id == id);
    }
    #endregion
    #region Command
    public async Task Update()
    {
        await ctx.SaveChangesAsync();
    }
    public async Task<int> Insert(Course course)
    {
        await ctx.Courses.AddAsync(course);
        await ctx.SaveChangesAsync();
        return course.Id;
    }
    public async Task Delete(Course course)
    {
        ctx.Courses.Remove(course);
        await ctx.SaveChangesAsync();
    }
    #endregion
}

