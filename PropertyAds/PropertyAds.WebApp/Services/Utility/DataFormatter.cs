namespace PropertyAds.WebApp.Services.Utility
{
    using System;
    using System.Globalization;

    public class DataFormatter : IDataFormatter
    {
        private readonly CultureInfo currencyCultureInfo = new CultureInfo("fr-FR");

        public string Currency<T>(T value)
            where T : struct,
                        IComparable,
                        IComparable<T>,
                        IConvertible,
                        IEquatable<T>,
                        IFormattable
        {
            return value.ToString("C0", this.currencyCultureInfo);
        }
    }
}
