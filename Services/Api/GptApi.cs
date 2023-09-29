using System.Text;
using Newtonsoft.Json;
using OpenAI_API;
using Services.Api.JsonFormats;

namespace Services.Api
{



    public class GptApi
    {
        private readonly string API_KEY;
        private readonly OpenAIAPI client;

        private const string jsonFormatFilePath = "jsonformat.json";

        public GptApi()
        {
            DotNetEnv.Env.Load(".env.local");
            API_KEY = DotNetEnv.Env.GetString("CHAT_GPT_KEY");
            client = new OpenAIAPI(API_KEY);
        }

        public async Task<CVFormat> AnalyzePdfAsync(string content)
        {
            try
            {
                string jsonContent = File.ReadAllText(jsonFormatFilePath);
                string instruction = "Peux-tu m'analyser et me retourner dans le format suivant en json sans texte a côté et m'ajouter une ligne dans le json renseignant le métier le plus probable dans ce format 'metier_probable': '...' que cette persone veut exercer: ";
                instruction += jsonContent;
                instruction += "le CV suivant :";
                content = instruction + content;
                var chat = client.Chat.CreateConversation();
                chat.AppendUserInput(content);
                StringBuilder result = new StringBuilder();

                await foreach (var res in chat.StreamResponseEnumerableFromChatbotAsync())
                {
                    result.Append(res);
                }
                CVFormat cv = JsonConvert.DeserializeObject<CVFormat>(result.ToString());
                return cv;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw; // Rethrow the exception to notify the caller
            }
        }
    }
}
