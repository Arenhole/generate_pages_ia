using PuppeteerSharp;

namespace Services.Scraper
{
    public abstract class AbstractScraper : IScraper, IDisposable
    {
        protected BrowserFetcher _browserFetcher = new BrowserFetcher();

        public void Dispose()
        {
            _browserFetcher?.Dispose();
        }
    }
}
