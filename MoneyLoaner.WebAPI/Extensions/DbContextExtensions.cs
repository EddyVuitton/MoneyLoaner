using Microsoft.EntityFrameworkCore;

namespace MoneyLoaner.WebAPI.Extensions;

public static class DbContextExtensions
{
    public static async Task<List<T>?> SqlQueryAsync<T>(this DbContext db, string sql, object[]? parameters = null, CancellationToken cancellationToken = default) where T : class
    {
        parameters ??= [];

        if (typeof(T).GetProperties().Length != 0)
        {
            return await db.Set<T>().FromSqlRaw(sql, parameters).ToListAsync(cancellationToken);
        }
        else
        {
            await db.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
            return default;
        }
    }

    public static async Task SqlQueryAsync(this DbContext db, string sql, object[]? parameters = null, CancellationToken cancellationToken = default)
    {
        parameters ??= [];

        await db.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
    }
}

public class OutputParameter<TValue>
{
    private bool _valueSet = false;

    public TValue? _value;

    public TValue? Value
    {
        get
        {
            if (!_valueSet)
                throw new InvalidOperationException("Value not set.");

            return _value;
        }
    }

    internal void SetValue(object value)
    {
        _valueSet = true;

        _value = value is null || Convert.IsDBNull(value) ? default : (TValue)value;
    }
}