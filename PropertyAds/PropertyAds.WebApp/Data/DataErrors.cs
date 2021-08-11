namespace PropertyAds.WebApp.Data
{
    public class DataErrors
    {
        public const string RequiredError = "Полето {0} е задължително.";
        public const string RangeError = "{0} може да бъде между {1} и {2}.";
        public const string StringLengthError = "{0} може да съдържа между {1} и {2} символа.";
        public const string CollectionRangeError = "Броят {0} може да бъде между {1} и {2}.";

        public const string PropertyNotFoundError = "Имотът не съществува.";
        public const string PropertyTypeNotFoundError = "Типа имот не съществува.";
        public const string DistrictNotFoundError = "Кварталът не съществува.";
        public const string WatchlistNotFoundError = "Списъкът не съществува.";
        public const string FloorGreaterThanTotalError = "Етажът трябва да е по-малък от общия брой етажи.";
        public const string UsableAreaGreaterThanAreaError = "Използваемата площ трябва да е по-малка или равна на общата площ.";
        public const string OnlyImagesAllowedError = "Поддържат се само картинки с формат JPEG и размер до 500кб.";
    }
}
