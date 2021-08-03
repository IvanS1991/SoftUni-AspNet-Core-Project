﻿namespace PropertyAds.WebApp.Models.Property
{
    public class PropertySummaryViewModel
    {
        public string Id { get; set; }
        
        public int Price { get; set; }

        public string Description { get; set; }

        public string DistrictName { get; set; }

        public string PropertyTypeName { get; set; }

        public string ImageId { get; set; }
    }
}
