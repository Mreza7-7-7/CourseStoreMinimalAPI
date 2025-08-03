using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseStoreMinimalAPI.DAL;
using CourseStoreMinimalAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseStoreMinimalAPI.AplicationService;

public class TeacherService(CourseDbContext ctx)
{
    #region Read
    public async Task<List<Teacher>> GetAll()
    {
        return await ctx.Teachers.AsNoTracking().ToListAsync();
    }
    public async Task<Teacher?> GetTeacherAsync(int id)
    {
        return await ctx.Teachers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }
    public async Task<List<Teacher>> Search(string FirstName = "", string LastName = "")
    {
        var teacher = ctx.Teachers.AsQueryable();
        if (!string.IsNullOrWhiteSpace(FirstName))
        {
            teacher = teacher.Where(c => c.FirstName.Contains(FirstName));
        }
        if (!string.IsNullOrWhiteSpace(LastName))
        {
            teacher = teacher.Where(c => c.LastName.Contains(LastName));
        }
        return await teacher.AsNoTracking().ToListAsync();
    }
    public async Task<bool> Exist(int id)
    {
        return await ctx.Teachers.AnyAsync(c => c.Id == id);
    }
    #endregion
    #region Command
    public async Task Update(Teacher teacher)
    {
        ctx.Teachers.Update(teacher);
        await ctx.SaveChangesAsync();
    }
    public async Task<int> Insert(Teacher teacher)
    {
        await ctx.Teachers.AddAsync(teacher);
        await ctx.SaveChangesAsync();
        return teacher.Id;
    }
    public async Task Delete(int id)
    {
        var teacher = new Teacher { Id = id };
        ctx.Teachers.Remove(teacher);
        await ctx.SaveChangesAsync();
    }
    #endregion
}

