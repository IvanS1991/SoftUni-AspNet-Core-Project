namespace PropertyAds.WebApp.Models.Validation
{
    using System;
    using System.Collections;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property)]
    public class CollectionRangeAttribute : ValidationAttribute
    {
        private readonly int minItems;
        private readonly int maxItems;

        public CollectionRangeAttribute(int minItems, int maxItems)
        {
            this.minItems = minItems;
            this.maxItems = maxItems;
        }

        public override bool IsValid(object collection)
        {
            if (collection is IList list)
            {
                return list.Count >= minItems && list.Count <= maxItems;
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(this.ErrorMessage, name, this.minItems, this.maxItems);
        }
    }
}
