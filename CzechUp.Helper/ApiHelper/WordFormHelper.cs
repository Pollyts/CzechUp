using System.Text.Json;
using System.Text.Json.Serialization;

namespace CzechUp.Helper.ApiHelper
{
    public static class WordFormHelper
    {
        public static async Task<List<WordForm>> GetWordForms (HttpClient client, string word)
        {
            Console.WriteLine("Start scrapping from https://nlp.fi.muni.cz/languageservices/service.py");
            try
            {
                HttpResponseMessage response = await client.GetAsync(string.Format("https://nlp.fi.muni.cz/languageservices/service.py?call=gen&lang=cs&output=json&lemma={0}", word));

                if (response.IsSuccessStatusCode)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();
                    var responseContent = JsonSerializer.Deserialize<WordFormsResponse>(responseJson);
                    if (responseContent != null && responseContent.Forms!=null)
                        return responseContent.Forms;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }

                return new List<WordForm>();
            }
            catch (Exception ex) 
            {
                throw new Exception(string.Format("Error while getting forms of word '{0}'. {1}", word, ex.Message));
            }
            
        }
    }

    public class WordFormsResponse
    {
        [JsonPropertyName("forms")]
        public List<WordForm> Forms { get; set; }
    }

    public class WordForm
    {
        [JsonPropertyName("tag")]
        public string Tag { get; set; }

        [JsonPropertyName("word")]
        public string Word { get; set; }
    }    
}
