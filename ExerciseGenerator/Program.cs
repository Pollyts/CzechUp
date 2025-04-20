using CzechUp.EF;
using CzechUp.EF.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;

class Program
{
    //InsertWord, //Заполнение пропусков подходящими словами
    //CreateSentence, //Составление предложений с новыми словами
    //InsertWordInRightWorm, //Подстановка форм слов (например, спряжение глаголов)
    //SelectAnswer, //Выбрать правильный ответ из предложенных
    //WriteAnswer, //Заполнить пропуск в вопросе/ввести ответ на вопрос
    //ChooseCategory, //Выбрать категорию

    static async Task Main()
    {
        //InsertWord + WriteAnswer

        string apiKey = "";
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";
        using HttpClient client = new HttpClient();

        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseNpgsql("");

        using (var db = new DatabaseContext(optionsBuilder.Options))
        {
            var words = db.GeneralOriginalWords.Include(w=>w.LanguageLevel).Include(w=>w.GeneralTopic).ToList();
            foreach (var word in words)
            {
                var requestBody = new
                {
                    contents = new[]
                                    {
                                    new
                                        {
                                        parts = new[]
            {
                new
                {
                    text = string.Format(
                        "Я изучаю чешский язык на уровень {0}. Сгенерируй мне json из 5 элементов. Каждый элемент имеет поля Question и Answer. В поле Question сгенерируй мне предложение на чешском языке со словом {1} (тема {2}) в любой его форме. В самом предложении вместо слова {3} будет пропуск. В поле Answer напиши слово {4} в правильной форме.",
                        word.LanguageLevel.Name,
                        word.Word,
                        word.GeneralTopic.Name,
                        word.Word,
                        word.Word
                    )
                }
            }

        }
    }
                };


                // Сериализуем корректно
                var json = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();
                ApiResponse apiResponse = JsonSerializer.Deserialize<ApiResponse>(result);

                string cleanedJson = Regex.Replace(apiResponse.Candidates.FirstOrDefault().Content.Parts.FirstOrDefault().Text, @"^```json\s*|\s*```$", "", RegexOptions.Multiline);

                List<ApiExercise> exercises = JsonSerializer.Deserialize<List<ApiExercise>>(cleanedJson);

                foreach (var ex in exercises)
                {
                    db.GeneralExercises.Add(new GeneralExercise()
                    {
                        Question = ex.Question,
                        Answer = ex.Answer,
                        GeneralOriginalWordGuid = word.Guid,
                        GeneralTopicGuid = word.GeneralTopicGuid,
                        ExerciseType = ExerciseType.InsertWordInRightForm,
                        AnswerOptions = ""
                    });
                }

                db.SaveChanges();
            }
        }
    }
}

public class ApiResponse
{
    [JsonPropertyName("candidates")]
    public List<Candidate> Candidates { get; set; }

    [JsonPropertyName("usageMetadata")]
    public UsageMetadata UsageMetadata { get; set; }

    [JsonPropertyName("modelVersion")]
    public string ModelVersion { get; set; }
}

public class Candidate
{
    [JsonPropertyName("content")]
    public Content Content { get; set; }

    [JsonPropertyName("finishReason")]
    public string FinishReason { get; set; }

    [JsonPropertyName("avgLogprobs")]
    public double AvgLogprobs { get; set; }
}

public class Content
{
    [JsonPropertyName("parts")]
    public List<Part> Parts { get; set; }

    [JsonPropertyName("role")]
    public string Role { get; set; }
}

public class Part
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
}

public class UsageMetadata
{
    [JsonPropertyName("promptTokenCount")]
    public int PromptTokenCount { get; set; }

    [JsonPropertyName("candidatesTokenCount")]
    public int CandidatesTokenCount { get; set; }

    [JsonPropertyName("totalTokenCount")]
    public int TotalTokenCount { get; set; }

    [JsonPropertyName("promptTokensDetails")]
    public List<TokenDetail> PromptTokensDetails { get; set; }

    [JsonPropertyName("candidatesTokensDetails")]
    public List<TokenDetail> CandidatesTokensDetails { get; set; }
}

public class TokenDetail
{
    [JsonPropertyName("modality")]
    public string Modality { get; set; }

    [JsonPropertyName("tokenCount")]
    public int TokenCount { get; set; }
}

public class ApiExercise
{
    public string Question { get; set; }
    public string Answer { get; set; }
}
