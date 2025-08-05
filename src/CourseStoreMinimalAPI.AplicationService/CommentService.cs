using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseStoreMinimalAPI.DAL;
using CourseStoreMinimalAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseStoreMinimalAPI.AplicationService;

public class CommentService(CourseStoreDbContext ctx)
{
    #region Read
    public async Task<List<Comment>> GetAll(int courseId)
    {
        return await ctx.Comments.Where(c => c.CourseId == courseId).AsNoTracking().ToListAsync();
    }
    public async Task<int> GetTotalCountAsync()
    {
        return await ctx.Comments.CountAsync();
    }
    public async Task<Comment?> GetCommentAsync(int id)
    {
        return await ctx.Comments.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> Exist(int id)
    {
        return await ctx.Comments.AnyAsync(c => c.Id == id);
    }
    #endregion
    #region Command
    public async Task Update()
    {
        await ctx.SaveChangesAsync();
    }
    public async Task<int> Insert(Comment comment)
    {
        await ctx.Comments.AddAsync(comment);
        await ctx.SaveChangesAsync();
        return comment.Id;
    }
    public async Task Delete(Comment comment)
    {
        ctx.Comments.Remove(comment);
        await ctx.SaveChangesAsync();
    }
    #endregion
}

