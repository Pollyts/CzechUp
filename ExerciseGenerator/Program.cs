using CzechUp.EF;
using CzechUp.EF.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;

class Program
{
    static async Task Main()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseNpgsql("");

        await GenerateFromAI(optionsBuilder.Options);
        await GenerateWithDb(optionsBuilder.Options);
    }

    static async Task GenerateFromAI(DbContextOptions<DatabaseContext> options)
    {
        string apiKey = "";
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";          

        using (var db = new DatabaseContext(options))
        {
            var languages = db.Languages.Where(l => l.Name != "CZ").ToList();
            var words = db.GeneralOriginalWords.Include(w => w.LanguageLevel).Include(w => w.GeneralTopics).ToList();
            foreach (var word in words)
            {
                var existWordInDb = db.GeneralExercises.Where(e => e.GeneralOriginalWordGuid == word.Guid).Any();
                if (existWordInDb)
                {
                    continue;
                }
                foreach (var topic in word.GeneralTopics)
                {
                    Console.WriteLine(string.Format("Starting GenerateInsertWordInRightForm The word {0} topic {1}", word.Word, topic.Name));
                    var exercises = await GenerateInsertWordInRightForm(word, topic, url);
                    if (exercises == null)
                    {
                        Console.WriteLine(string.Format("Error in GenerateInsertWordInRightForm. The word {0} topic {1}.", word.Word, topic.Name));
                        continue;
                    }
                    foreach (var ex in exercises)
                    {
                        db.GeneralExercises.Add(new GeneralExercise()
                        {
                            LanguageLevelGuid = word.LanguageLevelGuid,
                            Question = ex.Question,
                            Answer = ex.Answer,
                            GeneralOriginalWordGuid = word.Guid,
                            GeneralTopicGuid = topic.Guid,
                            ExerciseType = ExerciseType.InsertWordInRightForm,
                            AnswerOptions = ""
                        });
                    }

                    Console.WriteLine(string.Format("Finishing GenerateInsertWordInRightForm The word {0} topic {1}", word.Word, topic.Name));
                    await Task.Delay(5000);

                    Console.WriteLine(string.Format("Starting CreateSentence The word {0} topic {1}", word.Word, topic.Name));
                    foreach (var language in languages)
                    {
                        exercises = await CreateSentence(word, topic, language, url);

                        if (exercises == null)
                        {
                            Console.WriteLine(string.Format("Error in CreateSentence. The word {0} topic {1} lang {2}.", word.Word, topic.Name, language.Name));
                            continue;
                        }

                        foreach (var ex in exercises)
                        {
                            db.GeneralExercises.Add(new GeneralExercise()
                            {
                                LanguageLevelGuid = word.LanguageLevelGuid,
                                Question = ex.Question,
                                Answer = ex.Answer,
                                GeneralOriginalWordGuid = word.Guid,
                                GeneralTopicGuid = topic.Guid,
                                ExerciseType = ExerciseType.CreateSentence,
                                AnswerOptions = ""
                            });
                        }

                        await Task.Delay(5000);
                    }

                    Console.WriteLine(string.Format("Finishing CreateSentence The word {0} topic {1}", word.Word, topic.Name));
                    Console.WriteLine(string.Format("Starting GenerateText The word {0} topic {1}", word.Word, topic.Name));

                    var exercise = await GenerateText(word, topic, url);

                    if (exercise == null)
                    {
                        Console.WriteLine(string.Format("Error in CreateSentence. The word {0} topic {1}", word.Word, topic.Name));
                        continue;
                    }

                    db.GeneralExercises.Add(new GeneralExercise()
                    {
                        LanguageLevelGuid = word.LanguageLevelGuid,
                        Question = exercise.Question,
                        Answer = exercise.Answer,
                        GeneralOriginalWordGuid = word.Guid,
                        GeneralTopicGuid = topic.Guid,
                        ExerciseType = ExerciseType.InsertWordToText,
                        AnswerOptions = string.Join(';', exercise.Answer.Split(", "))
                    });

                    await Task.Delay(5000);

                    Console.WriteLine(string.Format("Finishing GenerateText The word {0} topic {1}", word.Word, topic.Name));

                    db.SaveChanges();
                    Console.WriteLine(string.Format("The word {0} topic {1} was saved to database", word.Word, topic.Name));

                }                
            }
        }
    }

    static async Task<List<ApiExercise>> GenerateInsertWordInRightForm(GeneralOriginalWord word, GeneralTopic topic, string url)
    {
        using HttpClient client = new HttpClient();
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
                        "Я изучаю чешский язык на уровень {0}. Сгенерируй мне json из 3 элементов. Каждый элемент имеет поля Question и Answer. В поле Question сгенерируй мне предложение на чешском языке со словом {1} (тема {2}) в любой его форме. В самом предложении вместо слова {3} будет пропуск. В поле Answer напиши слово {4} в правильной форме.",
                        word.LanguageLevel.Name,
                        word.Word,
                        topic.Name,
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

        try
        {
            string cleanedJson = Regex.Replace(apiResponse.Candidates.FirstOrDefault().Content.Parts.FirstOrDefault().Text, @"^```json\s*|\s*```$", "", RegexOptions.Multiline);

            List<ApiExercise> exercises = JsonSerializer.Deserialize<List<ApiExercise>>(cleanedJson);
            return exercises;
        }
        catch (Exception ex)
        {
            Console.WriteLine(string.Format("Error in GenerateInsertWordInRightForm. The request {0} with response {1}. Exception {2}", content.ToString(), result.ToString(), ex.Message));
        }
        return null;

        
    }

    static async Task<List<ApiExercise>> CreateSentence(GeneralOriginalWord word, GeneralTopic topic, Language language, string url)
    {      


        using HttpClient client = new HttpClient();
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
                        "Я изучаю чешский язык на уровень {0}. Сгенерируй мне json из 3 элементов. Каждый элемент имеет поле Answer и Question.В поле Answer сгенерируй мне предложение на чешском языке со словом {1} (тема {2}) в любой его форме. в поле Question переведи мне это предложение на язык {3}",
                        word.LanguageLevel.Name,
                        word.Word,
                        topic.Name,
                        language.Name
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
        var response = await client.PostAsync(url, content);
        var result = await response.Content.ReadAsStringAsync();

        try
        {
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(result);


            var cleanedJson = Regex.Replace(apiResponse.Candidates.FirstOrDefault().Content.Parts.FirstOrDefault().Text, @"^```json\s*|\s*```$", "", RegexOptions.Multiline);

            var exercises = JsonSerializer.Deserialize<List<ApiExercise>>(cleanedJson);
            return exercises;
        }
        catch (Exception ex) {
            Console.WriteLine(string.Format("Error in CreateSentence. The request {0} with response {1}. Exception {2}", content.ToString(), result.ToString(), ex.Message));
        }
        return null;
    }

    static async Task<ApiExercise> GenerateText(GeneralOriginalWord word, GeneralTopic topic, string url)
    {
        using HttpClient client = new HttpClient();
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
                        "Я изучаю чешский язык на уровень {0}. Сгенерируй мне json, который имеет полe Text. В поле Text сгенерируй мне связный логичный текст, который состоит из 6-8 предложений и обязательно содержит слово {1} (смысл по теме {2})",
                        word.LanguageLevel.Name,
                        word.Word,
                        topic.Name
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
        var response = await client.PostAsync(url, content);
        var result = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<ApiResponse>(result);
        string text = "";

        try
        {
            var cleanedJson = Regex.Replace(apiResponse.Candidates.FirstOrDefault().Content.Parts.FirstOrDefault().Text, @"^```json\s*|\s*```$", "", RegexOptions.Multiline);

            text = JsonSerializer.Deserialize<ApiText>(cleanedJson).Text;
        }
        catch (Exception ex)
        {
            Console.WriteLine(string.Format("Error in GenerateText. The request {0} with response {1}. Exception {2}", content.ToString(), result.ToString(), ex.Message));
            return null;
        }

        // Разбиваем текст на слова
        var wordsInOrder = Regex.Matches(text, @"\b\w+\b")
                               .Cast<Match>()
                               .Select(m => m.Value)
                               .Where(w => w.Length > 2)
                               .ToList();

        var uniqueWords = new List<string>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var wordInOrder in wordsInOrder)
        {
            if (seen.Add(wordInOrder))
            {
                uniqueWords.Add(wordInOrder);
            }
        }

        // Выбираем 10-15 уникальных случайных слов
        var random = new Random();
        int countToReplace = Math.Min(uniqueWords.Count, random.Next(10, 16));
        var selectedWords = uniqueWords.OrderBy(x => random.Next())
                                       .Take(countToReplace)
                                       .OrderBy(w => text.IndexOf(w, StringComparison.OrdinalIgnoreCase)) // сохранить порядок в тексте
                                       .ToList();

        selectedWords = selectedWords
            .OrderBy(w => GetWordIndex(text, w))
            .ToList();

        int GetWordIndex(string text, string word)
        {
            var match = Regex.Match(text, $@"\b{Regex.Escape(word)}\b", RegexOptions.IgnoreCase);
            return match.Success ? match.Index : int.MaxValue;  // Возвращаем индекс первого вхождения слова, или максимальное значение, если слово не найдено
        }

        // Создаем копию текста с заменой слов на "_______"
        var replacedWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        string maskedText = Regex.Replace(text, @"\b\w+\b", match =>
        {
            string word = match.Value;
            if (selectedWords.Contains(word, StringComparer.OrdinalIgnoreCase) && !replacedWords.Contains(word))
            {
                replacedWords.Add(word);
                return "_______";
            }
            return word;
        }, RegexOptions.IgnoreCase);

        // Ответы
        string answers = string.Join(", ", selectedWords);

        return new ApiExercise()
        {
            Question = maskedText,
            Answer = answers
        };
    }

    static async Task GenerateWithDb(DbContextOptions<DatabaseContext> options)
    {
        using (var db = new DatabaseContext(options))
        {
            var topics = db.GeneralTopics.ToList();
            var languages = db.Languages.Where(l=>l.Name!="CZ").ToList();
            foreach (var topic in topics)
            {
                var words = db.GeneralOriginalWords.Include(w => w.LanguageLevel).Include(w => w.GeneralTopics).ToList();
                foreach (var word in words)
                {
                    var answerOptions = words
                        .Where(w => w.LanguageLevelGuid == word.LanguageLevelGuid && w.Guid != word.Guid)
                        .OrderBy(_ => Guid.NewGuid())
                        .Select(o=>o.Word)
                        .Take(8)
                        .ToList();
                    
                    foreach(var language in languages)
                    {
                        var translatedWord = db.GeneralWordTranslations.Where(t => t.GeneralOriginalWordGuid == word.Guid && t.LanguageGuid == language.Guid).Select(t=>t.Translation).ToList();

                        db.Add<GeneralExercise>(new GeneralExercise()
                        {
                            Answer = word.Word,
                            AnswerOptions = string.Join(";", answerOptions),
                            ExerciseType = ExerciseType.MatchingWordAndItsTranslate,
                            GeneralOriginalWordGuid = word.Guid,
                            GeneralTopicGuid = topic.Guid,
                            LanguageLevelGuid = word.LanguageLevelGuid,
                            Question = string.Join(";", translatedWord),
                            
                        });

                        db.Add<GeneralExercise>(new GeneralExercise()
                        {
                            Answer = word.Word,
                            AnswerOptions = "",
                            ExerciseType = ExerciseType.WriteCzechWord,
                            GeneralOriginalWordGuid = word.Guid,
                            GeneralTopicGuid = topic.Guid,
                            LanguageLevelGuid = word.LanguageLevelGuid,
                            Question = string.Join(";", translatedWord),

                        });
                    }                    
                }
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

public class ApiText
{
    public string Text { get; set; }
}
