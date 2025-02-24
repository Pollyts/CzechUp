using CzechUp.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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

            string[] wordsToTranslate = { "Hello", "World", "Test", "Example", "Computer", "Programming", "Language", "Coffee", "Sun", "Moon" };
            string outputFilePath = "translations.txt";

            // Очищаем файл перед записью
            File.WriteAllText(outputFilePath, string.Empty);

            foreach (string word in wordsToTranslate)
            {
                TranslateResult? result = await TranslateWord(client, word);

                if (result != null && result.Translations.Length > 0)
                {
                    string translatedText = result.Translations[0].Text;
                    string detectedLanguage = result.Translations[0].DetectedSourceLanguage;

                    string outputLine = $"{word} -> {translatedText} (Detected: {detectedLanguage})";
                    Console.WriteLine(outputLine);

                    // Добавляем строку в файл
                    await File.AppendAllTextAsync(outputFilePath, outputLine + Environment.NewLine);
                }
            }
            Console.WriteLine($"Результаты перевода сохранены в {outputFilePath}");
        }
    }

    static async Task<TranslateResult?> TranslateWord(HttpClient client, string word)
    {
        var requestBody = new
        {
            text = new[] { word },
            target_lang = "DE"
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