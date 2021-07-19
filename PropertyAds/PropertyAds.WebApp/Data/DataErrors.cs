namespace PropertyAds.WebApp.Data
{
    public class DataErrors
    {
        public const string RequiredError = "Полето {0} е задължително.";
        public const string RangeError = "{0} може да бъде между {1} и {2}.";
        public const string StringLengthError = "{0} може да съдържа между {1} и {2} символа.";

        public const string PropertyTypeNotFoundError = "Типа имот не съществува.";
        public const string DistrictNotFoundError = "Кварталът не съществува.";
        public const string FloorGreaterThanTotalError = "Етажът трябва да е по-малък от общия брой етажи.";
    }
}
