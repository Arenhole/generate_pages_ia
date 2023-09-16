using PuppeteerSharp;
using PuppeteerSharp.Input;

namespace Services.Scraper.Indeed
{
    public class IndeedScraper : AbstractScraper
    {
        public async Task ScrapeAsync()
        {
            try
            {


                await _browserFetcher.DownloadAsync();

                using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = false, DefaultViewport = { Width = 1920, Height = 1080 } });
                using var page = await browser.NewPageAsync();

                await page.GoToAsync("https://fr.indeed.com/jobs?q=d%C3%A9veloppeur+web&l=Nantes+%2844%29&from=searchOnHP&vjk=712ba212385bd43b");

                var jobCards = await page.QuerySelectorAllAsync(".job_seen_beacon");

                foreach (var card in jobCards)
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

                            // Attendez que le bouton "Postuler" soit disponible.
                            Thread.Sleep(1000);
                            var applyButton = await page.WaitForSelectorAsync(".jobsearch-IndeedApplyButton-contentWrapper", new WaitForSelectorOptions { Timeout = 10000 });


                            if (applyButton != null)
                            {
                                Console.WriteLine("Bouton disponible");

                                // Cliquez pour ouvrir la nouvelle page
                                await applyButton.ClickAsync();
                                // Écoutez les nouveaux onglets ouvertes
                                var newPageTask = browser.WaitForTargetAsync(t => t.Url.Contains("apply.indeed.com")).ContinueWith(t => t.Result.PageAsync());



                                // Attendez que la nouvelle page soit chargée
                                var newPage = (await newPageTask).Result;
                                await newPage.WaitForNavigationAsync();

                                // Vous pouvez maintenant interagir avec la nouvelle page
                                var newElement = await newPage.QuerySelectorAsync(".ia-JobHeader-title");
                                if (newElement != null)
                                {
                                    var titleText2 = await newElement.EvaluateFunctionAsync<string>("element => element.innerText");
                                    if (titleText2 != null)
                                    {

                                        Console.WriteLine($"2: {titleText2}");
                                    }
                                }

                                // ... autres interactions avec newPage

                                // Si vous souhaitez fermer cette nouvelle page après avoir terminé
                                await newPage.CloseAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }



    }
}

