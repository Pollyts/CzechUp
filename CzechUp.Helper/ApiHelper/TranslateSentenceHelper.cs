using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CzechUp.Helper.ApiHelper
{
    public static class TranslateSentenceHelper
    {
        public static async Task<TranslateSentenceResult> TranslateSentence(HttpClient client, string text, string targerLang)
        {
            Console.WriteLine("Start scrapping from https://api-free.deepl.com/v2/translate");
            var requestBody = new
            {
                text = new[] { text },
                source_lang = "CS",
                target_lang = targerLang,

            };

            string json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync("https://api-free.deepl.com/v2/translate", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<TranslateSentenceResult>(responseJson);
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error while translating sentence '{0}'. {1}", text, ex.Message));
            }
        }
    }

    public class TranslateSentenceResult
    {
        [JsonPropertyName("translations")]
        public TranslationText[] TranslationTexts { get; set; } = Array.Empty<TranslationText>();
    }

    public class TranslationText
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }
}
