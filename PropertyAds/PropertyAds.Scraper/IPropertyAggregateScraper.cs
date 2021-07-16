using PropertyAds.Scraper.Contracts;
using PropertyAds.Scraper.Models;
using System.Collections.Generic;

namespace PropertyAds.Scraper
{
    public interface IPropertyAggregateScraper : IScraper<List<PropertyAggregateDTO>>
    {
    }
}
