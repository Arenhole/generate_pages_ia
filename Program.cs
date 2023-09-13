using Newtonsoft.Json;

class Program
{
    static async Task Main()
    {
        using HttpClient client = new();
        try
        {
            string url = "https://jsonplaceholder.typicode.com/users";
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonContent = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<HttpClientDemo.User[]>(jsonContent);

                Console.WriteLine("Liste des utilisateurs :");
                if (users is not null)
                {
                    foreach (var user in users)
                    {
                        Console.WriteLine(user.Name);
                    }
                }
            }
            else
            {
                Console.WriteLine("Erreur lors de la récupération des données : " + response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Une erreur s'est produite : " + ex.Message);
        }
    }
}

