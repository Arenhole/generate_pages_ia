using PuppeteerSharp;

namespace Services.Scraper.Indeed
{
    public class IndeedScraper : AbstractScraper
    {
        public async Task ScrapeAsync()
        {
            await _browserFetcher.DownloadAsync();

            using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = false });
            using var page = await browser.NewPageAsync();

            await page.GoToAsync("https://fr.indeed.com/jobs?q=d%C3%A9veloppeur+web&l=Nantes+%2844%29&from=searchOnHP&vjk=f5f99c4a2e13629e&advn=1042503577832136");

            var jobTitles = await page.QuerySelectorAllAsync(".jobTitle a span");

            foreach (var jobTitle in jobTitles)
            {
                var innerText = await jobTitle.EvaluateFunctionAsync<string>("element => element.innerText");
                Console.WriteLine(innerText);
            }
        }
    }
}
