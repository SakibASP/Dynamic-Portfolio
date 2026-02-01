namespace Portfolio.Utils;

public static class GenerateParameter
{
    public static SaveRequestModel<T> SingleModel<T>(T _item, string? _userName, DateTime _bdCurrentTime)
    {
        return new SaveRequestModel<T>
        {
            Item = _item,
            UserName = _userName,
            BdCurrentTime = _bdCurrentTime
        };
    }

    public static SaveRequestModel<T> IdModel<T>(int? _id, string? _userName, DateTime _bdCurrentTime)
    {
        return new SaveRequestModel<T>
        {
            Id = _id,
            UserName = _userName,
            BdCurrentTime = _bdCurrentTime
        };
    }

    public static SaveRequestModel<T> ListModel<T>(IList<T>? _items, DateTime _bdCurrentTime)
    {
        return new SaveRequestModel<T>
        {
            Items = _items,
            BdCurrentTime = _bdCurrentTime
        };
    }
}
