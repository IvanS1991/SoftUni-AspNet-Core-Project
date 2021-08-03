using System;
using System.Collections.Generic;

namespace PropertyAds.WebApp.Models.Property
{
    public class PropertyDetailsViewModel
    {
        public string Id { get; set; }

        public int Price { get; set; }

        public decimal Area { get; set; }

        public decimal UsableArea { get; set; }

        public int Floor { get; set; }

        public int TotalFloors { get; set; }

        public int Year { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public int VisitedCount { get; set; }

        public string OwnerName { get; set; }

        public string Type { get; set; }

        public string District { get; set; }

        public IEnumerable<string> ImageIds { get; set; }
    }
}
