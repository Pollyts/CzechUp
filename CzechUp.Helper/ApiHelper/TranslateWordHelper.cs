using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace CzechUp.Helper.ApiHelper
{
    public static class TranslateWordHelper
    {
        public static async Task<TranslateWordResponse?> TranslateWord(HttpClient client, string word, string targetLang)
        {
            Console.WriteLine("Start scrapping from https://slovnik.seznam.cz/api/slovnik?dictionary");
            try
            {
                HttpResponseMessage response = await client.GetAsync(string.Format("https://slovnik.seznam.cz/api/slovnik?dictionary={0}&query={1}", targetLang, word));

                if (response.IsSuccessStatusCode)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<TranslateWordResponse>(responseJson);
                    return result;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error while translating word '{0}'. {1}", word, ex.Message));
            }
            
        }
    }

    public class TranslateWordResponse
    {
        [JsonPropertyName("translate")]
        public List<MainInfo> MainInfo { get; set; }

        [JsonPropertyName("relations")]
        public Relations Relations { get; set; }

        [JsonPropertyName("ftx_samp")]
        public List<ExampleSentence> ExampleSentences { get; set; }
    }

    public class MainInfo
    {
        [JsonPropertyName("head")]
        public Head Head { get; set; }

        [JsonPropertyName("grps")]
        public List<Translation> Translations { get; set; }
    }

    public class Head
    {
        [JsonPropertyName("entr")]
        public string FoundedWord { get; set; }

        [JsonPropertyName("morf")]
        public string Description { get; set; }
    }

    public class Translation
    {
        [JsonPropertyName("sens")]
        public List<Meaning> Meanings { get; set; }
    }

    public class Meaning
    {
        [JsonPropertyName("numb")]
        public string TranslateNumber { get; set; }

        [JsonPropertyName("trans")]
        public List<List<string>> Translations { get; set; }
    }

    public class SampleTranslation
    {
        [JsonPropertyName("samp2s")]
        public string Samp2s { get; set; }

        [JsonPropertyName("samp2t")]
        public string Samp2t { get; set; }
    }

    public class Relations
    {
        [JsonPropertyName("Slovní spojení")]
        public List<string> SlovniSpojeni { get; set; }

        [JsonPropertyName("Synonyma")]
        public List<string> Synonyms { get; set; }
    }

    public class ExampleSentence
    {
        [JsonPropertyName("samp2s")]
        public string OriginalSentence { get; set; }

        [JsonPropertyName("samp2t")]
        public string TranslatedSentence { get; set; }
    }

}
