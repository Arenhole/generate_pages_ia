using Newtonsoft.Json;
using Services.Scraper.Indeed;

class Program
{
    static async Task Main()
    {
        IndeedScraper scraper = new();
        await scraper.ScrapeAsync();
    }
}

