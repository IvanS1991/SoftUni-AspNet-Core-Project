namespace PropertyAds.WebApp.Models.Property
{
    using System;
    using System.Collections.Generic;

    public class PropertyDetailsViewModel : PropertyViewModelBase
    {
        public int Price { get; set; }

        public decimal Area { get; set; }

        public decimal UsableArea { get; set; }

        public int Floor { get; set; }

        public int TotalFloors { get; set; }

        public int Year { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public int VisitedCount { get; set; }

        public string OwnerId { get; set; }

        public string OwnerName { get; set; }

        public string Type { get; set; }

        public string District { get; set; }

        public IEnumerable<string> ImageIds { get; set; }
    }
}
