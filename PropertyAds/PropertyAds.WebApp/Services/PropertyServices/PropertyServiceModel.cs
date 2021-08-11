namespace PropertyAds.WebApp.Services.PropertyServices
{
    using PropertyAds.WebApp.Services.DistrictServices;
    using System;
    using System.Collections.Generic;

    public class PropertyServiceModel
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

        public DateTime LastModifiedOn { get; set; }

        public int VisitedCount { get; set; }

        public bool IsFlagged { get; set; }

        public string OwnerId { get; set; }

        public string Owner { get; set; }

        public PropertyTypeServiceModel Type { get; set; }

        public DistrictServiceModel District { get; set; }

        public IEnumerable<string> ImageIds { get; set; }
    }
}
