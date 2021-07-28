namespace PropertyAds.WebApp.Services.Utility
{
    using System;

    public interface IDataFormatter
    {
        string FormatCurrency<T>(T value)
            where T :   struct,
                        IComparable,
                        IComparable<T>,
                        IConvertible,
                        IEquatable<T>,
                        IFormattable;

        string FormatDate(DateTime date);
    }
}
