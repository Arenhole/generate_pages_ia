using Services.Api;
using Services.Api.JsonFormats;
using Services.PdfAnalyzer;
using Services.Scraper.Indeed;

class Program
{
    static async Task Main()
    {

        string content = PdfAnalyzer.analyze("CVS/CV-1.pdf");
        GptApi gptApi = new GptApi();
        CVFormat res = await gptApi.AnalyzePdfAsync(content);
        Console.WriteLine(res);
        IndeedScraper scraper = new();
        await scraper.ScrapeAsync(res);
    }
}

