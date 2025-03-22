using System.Text.Json;
using System.Text.Json.Serialization;

namespace CzechUp.Helper.ApiHelper
{
    public static class TranslateWordHelper
    {
        //https://slovnik.seznam.cz/api/slovnik?dictionary=ru&query=syn

        //
        public static async Task TranslateWord(HttpClient client, string word, string targetLang)
        {
            HttpResponseMessage response = await client.GetAsync(string.Format("https://slovnik.seznam.cz/api/slovnik?dictionary={0}&query={1}", targetLang, word));

            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<TranslateWordResponse>(responseJson);
            }
            else
            {
                throw new Exception();
            }
        }
    }

    public class TranslateWordResponse
    {
        [JsonPropertyName("translate")]
        public List<Translation> Translate { get; set; }

        [JsonPropertyName("sound")]
        public string Sound { get; set; }

        [JsonPropertyName("relations")]
        public Relations Relations { get; set; }

        [JsonPropertyName("ftx_samp")]
        public List<ExampleSentence> FtxSamp { get; set; }
    }

    public class Translation
    {
        [JsonPropertyName("head")]
        public Head Head { get; set; }

        [JsonPropertyName("grps")]
        public List<Group> Grps { get; set; }
    }

    public class Head
    {
        [JsonPropertyName("entr")]
        public string Entr { get; set; }
    }

    public class Group
    {
        [JsonPropertyName("sens")]
        public List<Sense> Sens { get; set; }
    }

    public class Sense
    {
        [JsonPropertyName("numb")]
        public string Numb { get; set; }

        [JsonPropertyName("trans")]
        public List<List<string>> Trans { get; set; }

        [JsonPropertyName("samp2")]
        public List<SampleTranslation> Samp2 { get; set; }
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
    }

    public class ExampleSentence
    {
        [JsonPropertyName("samp2s")]
        public string Samp2s { get; set; }

        [JsonPropertyName("samp2t")]
        public string Samp2t { get; set; }
    }

}
