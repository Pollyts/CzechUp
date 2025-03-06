using CzechUp.EF;
using CzechUp.EF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TranslationHepler;


class Program
{
    static async Task Main()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseNpgsql("");

        using (var db = new DatabaseContext(optionsBuilder.Options))
        {
            using HttpClient client = new();

            string apiKey = "";
            client.DefaultRequestHeaders.Add("Authorization", $"DeepL-Auth-Key {apiKey}");

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "words.txt");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found: " + filePath);
                return;
            }

            LanguageLevel? level = null;
            GeneralTopic? topic = null;
            Language languageRu = db.Languages.First(l => l.Name == "RU");
            Language languageEng = db.Languages.First(l => l.Name == "ENG");

            List<string> lines = new List<string>(File.ReadAllLines(filePath));

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i].Trim();
                if (line.StartsWith("!!!Level:"))
                {
                    var str = line.Replace("!!!Level:", "").Trim();
                    level = db.LanguageLevels.FirstOrDefault(x => x.Name == str);
                    if (level == null)
                    {
                        Console.WriteLine(string.Format("Language not found {0}", str));
                    }
                }
                else if (line.StartsWith("---Topic:"))
                {
                    var str = line.Replace("---Topic:", "").Trim();
                    topic = db.GeneralTopics.FirstOrDefault(x => x.Name == str);
                    if (topic == null)
                    {
                        Console.WriteLine(string.Format("Topic not found {0}", str));
                    }
                }
                else if (line.StartsWith("**") || line.Trim().StartsWith("CZ") || line.Trim().StartsWith("SZ"))
                {
                    continue; 
                }
                else if (line == "f" && i + 1 < lines.Count)
                {
                    lines[i + 1] = "f" + lines[i + 1].Trim(); 
                    lines.RemoveAt(i); 
                    i--; 
                }
                else if (!string.IsNullOrWhiteSpace(line))
                {                    
                    string[] words = line.Split('/');
                    foreach (var word in words)
                    {
                        //string cleanedWord = Regex.Replace(word, "\\(.*?\\)", "").Trim();
                        TranslateResult? resultRu = await TranslateWord(client, word.Trim(), "RU");
                        await Task.Delay(1000);
                        TranslateResult? resultEn = await TranslateWord(client, word.Trim(), "EN");

                        if (resultRu != null && resultRu.Translations.Length > 0)
                        {
                            string translatedTextRu = resultRu.Translations[0].Text;
                            db.GeneralWords.Add(new GeneralWord()
                            {
                                GeneralTopicId = topic!.Id,
                                LanguageId = languageRu.Id,
                                LanguageLevelId = level!.Id,
                                Original = word,
                                Translation = translatedTextRu                                
                            });
                        }
                        if (resultEn != null && resultEn.Translations.Length > 0)
                        {
                            string translatedTextEng = resultEn.Translations[0].Text;
                            db.GeneralWords.Add(new GeneralWord()
                            {
                                GeneralTopicId = topic!.Id,
                                LanguageId = languageEng.Id,
                                LanguageLevelId = level!.Id,
                                Original = word,
                                Translation = translatedTextEng
                            });

                            db.SaveChanges();
                        }
                    }

                    await Task.Delay(2000);
                }
            }
        }
    }

    static async Task<TranslateResult?> TranslateWord(HttpClient client, string word, string targerLang)
    {
        var requestBody = new
        {
            text = new[] { word },
            source_lang = "CS",
            target_lang = targerLang,

        };

        string json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("https://api-free.deepl.com/v2/translate", content);

        if (response.IsSuccessStatusCode)
        {
            string responseJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Перевод получился");
            return JsonSerializer.Deserialize<TranslateResult>(responseJson);
        }
        else
        {
            Console.WriteLine($"Ошибка при переводе '{word}': {response.StatusCode}");
            return null;
        }
    }
}