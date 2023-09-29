using System.Security;
using Newtonsoft.Json;
using PuppeteerSharp;
using Services.Api.JsonFormats;

namespace Services.Scraper.Indeed
{
    public class IndeedScraper : AbstractScraper
    {
        private const string CookieFilePath = "fr.indeed.com.cookies.json";
        private const string JobUrl = "https://fr.indeed.com/jobs?q=d%C3%A9veloppeur+web&l=Nantes+%2844%29&from=searchOnHP&vjk=712ba212385bd43b";
        private const string cvPath = "CVS/CV-1.pdf";

        public async Task ScrapeAsync(CVFormat cv)
        {
            try
            {
                var cookies = LoadCookiesFromFile(CookieFilePath);
                IBrowser browser = await LaunchBrowserAsync();
                IPage page = await browser.NewPageAsync();

                await SetCookiesAsync(page, cookies);
                await NavigateToJobPage(page, JobUrl);

                var jobCards = await page.QuerySelectorAllAsync(".job_seen_beacon");
                foreach (var card in jobCards)
                {
                    await ProcessJobCardAsync(browser, card, page, cv);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private List<CookieParam> LoadCookiesFromFile(string path)
        {
            var jsonContent = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<CookieParam>>(jsonContent);
        }

        private async Task<IBrowser> LaunchBrowserAsync()
        {
            await _browserFetcher.DownloadAsync();
            return await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false,
                DefaultViewport = { Width = 1920, Height = 1080 }
            });
        }

        private async Task SetCookiesAsync(IPage page, List<CookieParam> cookies)
        {
            await page.SetCookieAsync(cookies.ToArray());
        }

        private async Task NavigateToJobPage(IPage page, string url)
        {
            await page.GoToAsync(url);
        }

        private async Task ProcessJobCardAsync(IBrowser browser, IElementHandle card, IPage page, CVFormat cv)
        {
            var indeedApplyable = await card.QuerySelectorAsync(".jobCardShelf .indeedApply");
            if (indeedApplyable != null)
            {
                var titleElement = await card.QuerySelectorAsync(".jobTitle a span");
                if (titleElement != null)
                {
                    var titleText = await titleElement.EvaluateFunctionAsync<string>("element => element.innerText");
                    Console.WriteLine(titleText);
                    await titleElement.ClickAsync();
                    Thread.Sleep(1000);
                    var applyButton = await page.WaitForSelectorAsync(".jobsearch-IndeedApplyButton-contentWrapper", new WaitForSelectorOptions { Timeout = 10000 });

                    if (applyButton != null)
                    {
                        Console.WriteLine("Bouton disponible");
                        await ProcessApplicationPageAsync(browser, applyButton, cv);
                    }
                }
            }
        }

        private async Task ProcessApplicationPageAsync(IBrowser browser, IElementHandle applyButton, CVFormat cv)
        {
            await applyButton.ClickAsync();
            var newPageTask = browser.WaitForTargetAsync(t => t.Url.Contains("apply.indeed.com")).ContinueWith(t => t.Result.PageAsync());
            var newPage = (await newPageTask).Result;
            await newPage.WaitForNavigationAsync();

            var newElement = await newPage.QuerySelectorAsync(".ia-JobHeader-title");
            var inputFirstName = await newPage.QuerySelectorAsync("#input-firstName");

            await inputFirstName.TypeAsync("", new PuppeteerSharp.Input.TypeOptions { Delay = 100 });
            for (int i = 0; i < 100; i++)
            {
                await newPage.Keyboard.PressAsync("Backspace");
            }
            await inputFirstName.TypeAsync(cv.FirstName);
            var inputLastName = await newPage.QuerySelectorAsync("#input-lastName");
            await inputLastName.TypeAsync("", new PuppeteerSharp.Input.TypeOptions { Delay = 100 });
            for (int i = 0; i < 100; i++)
            {
                await newPage.Keyboard.PressAsync("Backspace");
            }
            await inputLastName.EvaluateFunctionAsync("input => input.value = ''");
            await inputLastName.TypeAsync(cv.LastName);
            var inputCity = await newPage.QuerySelectorAsync("#input-location\\.city");
            if (inputCity != null)
            {
                Console.WriteLine("Input City: " + cv.City);
                await inputCity.TypeAsync(cv.City);
            }

            var inputPhone = await newPage.QuerySelectorAsync("#input-phoneNumber");
            if (inputPhone != null)
            {
                await inputPhone.TypeAsync(cv.Phone);
            }
            var inputContinue = await newPage.QuerySelectorAsync(".ia-continueButton");
            await inputContinue.ClickAsync();

            await ApplyCVAsync(newPage);

        }

        private async Task ApplyCVAsync(IPage newPage)
        {

            await newPage.WaitForNavigationAsync();
            var applyCvButton = await newPage.QuerySelectorAsync("input[type=file]");
            await applyCvButton.UploadFileAsync(cvPath);
            Thread.Sleep(1000);
            var inputContinue = await newPage.WaitForSelectorAsync(".ia-continueButton");
            await inputContinue.ClickAsync();
            await nextAsync(newPage);
        }

        private async Task nextAsync(IPage page)
        {
            var val = "";
            var retries = 0;
            while (val != "100" && retries < 5)
            {
                var progressBar = await page.QuerySelectorAsync(".ia-ProgressBar");
                val = await progressBar.EvaluateFunctionAsync<string>("progressBar => 'aria-valuenow'");
                Thread.Sleep(1000);
                var inputContinue = await page.WaitForSelectorAsync(".ia-continueButton");
                await inputContinue.ClickAsync();
                retries++;
            }
            await page.CloseAsync();

        }
    }
}