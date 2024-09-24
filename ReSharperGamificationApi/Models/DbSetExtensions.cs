using System.Linq.Expressions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ReSharperGamificationApi.Models;

public static class DbSetExtensions
{
    public static async Task<T> FindOrAddAsync<T>(
        this DbSet<T> set, DbContext context, Expression<Func<T, bool>> predicate, T entityToAdd) where T : class
    {
        var entity = await set.FirstOrDefaultAsync(predicate);
        if (entity != null) return entity;

        await set.AddAsync(entityToAdd);
        try
        {
            await context.SaveChangesAsync();
            return entityToAdd;
        }
        catch (DbUpdateException e)
        {
            // check if entity has already been added
            if (IsUniqueConstraintViolation(e))
                return (await set.FirstOrDefaultAsync(predicate))!;

            throw;
        }
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException e)
    {
        // SQLITE_CONSTRAINT_UNIQUE error code
        return e.InnerException is SqliteException { SqliteErrorCode: 2067 };
    }
}