using System.Data;
using Dapper;

namespace IMDBLite.API.Converters;

public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.Value = value.ToDateTime(TimeOnly.MinValue);
    }

    public override DateOnly Parse(object value)
    {
        return value switch
        {
            DateTime dt => DateOnly.FromDateTime(dt),
            _ => throw new InvalidCastException($"Cannot convert {value.GetType()} to DateOnly")
        };
    }
}