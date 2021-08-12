namespace PropertyAds.WebApp.Services.Utility
{
    using System;

    public interface IDataFormatter
    {
        string Currency<T>(T value)
            where T :   struct,
                        IComparable,
                        IComparable<T>,
                        IConvertible,
                        IEquatable<T>,
                        IFormattable;
    }
}
