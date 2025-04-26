using CzechUp.EF;
using CzechUp.EF.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using CzechUp.Helper.ApiHelper;


class Program
{
    static async Task Main()
    {
        List<string> exceptionWords = new List<string>()
        {
        };

        using HttpClient wordTranslateClient = new();
        using HttpClient wordFormClient = new();
        using HttpClient translateSentenceClient = new();
        string apiKey = "";
        translateSentenceClient.DefaultRequestHeaders.Add("Authorization", $"DeepL-Auth-Key {apiKey}");

        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseNpgsql("");

        using (var db = new DatabaseContext(optionsBuilder.Options))
        {
            var words = db.GeneralOriginalWords.ToList();
            var languages = db.Languages.Where(l => l.Name != "CZ").ToList();

            foreach (var word in words)
            {
                foreach (var language in languages)
                {
                    try
                    {
                        string mainText = word.Word;
                        var mainWords = mainText.Split(' ');

                        var wordTranslationAreadyExist = db.GeneralWordTranslations.Any(t => t.GeneralOriginalWordGuid == word.Guid && t.LanguageGuid == language.Guid);
                        var wordFormsAreadyExist = db.GeneralWordForms.Any(t => t.GeneralOriginalWordGuid == word.Guid);

                        if (wordTranslationAreadyExist && wordFormsAreadyExist)
                        {
                            Console.WriteLine($"Translation and forms of word '{word.Word}' for language {language.Name} have already stored in database");
                            continue;
                        }

                        if (exceptionWords.Contains(mainText))
                        {
                            Console.WriteLine($"Skipping word '{word.Word}' for language '{language.Name}'");
                            continue;
                        }

                        Console.WriteLine($"Starting translation for word '{word.Word}', language {language.Name}");

                        if (mainWords.Count() == 1 || mainWords.Count() == 2 && (mainWords[1] == "se" || mainWords[1] == "si"))
                        {
                            //translate word
                            var translateWordResult = await TranslateWordHelper.TranslateWord(wordTranslateClient, mainText, language.Name.ToLower());

                            if (translateWordResult != null && translateWordResult.MainInfo.Count>0)
                            {
                                if (!wordTranslationAreadyExist)
                                {
                                    foreach (var meaning in translateWordResult.MainInfo.First().Translations.First().Meanings)
                                    {
                                        var translation = string.Join("", meaning.Translations.First());
                                        db.GeneralWordTranslations.Add(new GeneralWordTranslation()
                                        {
                                            GeneralOriginalWordGuid = word.Guid,
                                            LanguageGuid = language.Guid,
                                            Translation = Regex.Replace(translation, "\\<.*?>", "")
                                        });
                                    }
                                    foreach (var translationExample in translateWordResult.ExampleSentences)
                                    {
                                        db.GeneralWordExamples.Add(new GeneralWordExample()
                                        {
                                            GeneralOriginalWordGuid = word.Guid,
                                            OriginalExample = Regex.Replace(translationExample.OriginalSentence, "\\<.*?>", ""),
                                            TranslatedExample = translationExample.TranslatedSentence,
                                            LanguageGuid = language.Guid
                                        });
                                    }
                                }
                                //find word forms
                                if (!wordFormsAreadyExist)
                                {
                                    var foundedWord = translateWordResult.MainInfo.First().Head.FoundedWord;
                                    //remove se/si
                                    if (foundedWord.Split(' ').Count() == 2)
                                    {
                                        foundedWord = foundedWord.Split(' ').First();
                                    }
                                    var forms = await WordFormHelper.GetWordForms(wordFormClient, foundedWord);

                                    foreach (var form in forms)
                                    {
                                        db.GeneralWordForms.Add(new GeneralWordForm()
                                        {
                                            WordNumber = 1,
                                            GeneralOriginalWordGuid = word.Guid,
                                            Tag = form.Tag,
                                            WordForm = form.Word,
                                        });
                                    }
                                }
                                
                            }
                            else
                            {
                                if (!wordTranslationAreadyExist)
                                {
                                    //слово по какой-то причине отсутствует в seznam slovnik (например, множ число или слово 18+)
                                    var translateSentenceResult = await TranslateSentenceHelper.TranslateSentence(translateSentenceClient, mainText, language.Name);
                                                                    
                                    var generalWordTranslation = new GeneralWordTranslation()
                                    {
                                        GeneralOriginalWordGuid = word.Guid,
                                        LanguageGuid = language.Guid,
                                        Translation = translateSentenceResult.TranslationTexts.First().Text
                                    };

                                    db.Add(generalWordTranslation);
                                }

                                //find word forms
                                if(!wordFormsAreadyExist)
                                {
                                    var forms = await WordFormHelper.GetWordForms(wordFormClient, mainText);

                                    foreach (var form in forms)
                                    {
                                        db.GeneralWordForms.Add(new GeneralWordForm()
                                        {
                                            WordNumber = 1,
                                            GeneralOriginalWordGuid = word.Guid,
                                            Tag = form.Tag,
                                            WordForm = form.Word,
                                        });
                                    }
                                }                                
                            }                            
                        }
                        else
                        {
                            //translate all sentence
                            if(!wordTranslationAreadyExist)
                            {
                                var translateSentenceResult = await TranslateSentenceHelper.TranslateSentence(translateSentenceClient, mainText, language.Name);

                                var generalWordTranslation = new GeneralWordTranslation()
                                {
                                    GeneralOriginalWordGuid = word.Guid,
                                    LanguageGuid = language.Guid,
                                    Translation = translateSentenceResult.TranslationTexts.First().Text
                                };

                                db.Add(generalWordTranslation);
                            }

                            if (!wordFormsAreadyExist)
                            {
                                if (char.IsUpper(mainText[0]))
                                {
                                    continue;
                                }
                                //translate each word to find word form for every word
                                int wordNumber = 1;
                                List<WordPart> parts = new List<WordPart>();
                                foreach (var mainWord in mainWords)
                                {
                                    var translateResult = await TranslateWordHelper.TranslateWord(wordTranslateClient, mainWord, language.Name.ToLower());

                                    parts.Add(new WordPart()
                                    {
                                        Word = translateResult.MainInfo.First().Head.FoundedWord,
                                        WordDescription = translateResult.MainInfo.First().Head.Description,
                                        WordNumber = wordNumber
                                    });

                                    wordNumber++;
                                }

                                parts = parts.OrderBy(p => p.WordNumber).ToList();

                                if (parts.Count >= 2 && (parts[0].WordDescription == "přídavné jméno" && parts[1].WordDescription.Contains("podstatné jméno") || (parts[1].WordDescription == "přídavné jméno" && parts[0].WordDescription.Contains("podstatné jméno")))) {
                                    var firstWordForms = await WordFormHelper.GetWordForms(wordFormClient, parts[0].Word);
                                    foreach (var form in firstWordForms)
                                    {
                                        db.GeneralWordForms.Add(new GeneralWordForm()
                                        {
                                            WordNumber = 1,
                                            GeneralOriginalWordGuid = word.Guid,
                                            Tag = form.Tag,
                                            WordForm = form.Word,
                                        });
                                    }
                                    await Task.Delay(5000);
                                    var secondWordForms = await WordFormHelper.GetWordForms(wordFormClient, parts[1].Word);
                                    if (parts.Count == 3)
                                    {
                                        foreach (var form in secondWordForms)
                                        {
                                            db.GeneralWordForms.Add(new GeneralWordForm()
                                            {
                                                WordNumber = 2,
                                                GeneralOriginalWordGuid = word.Guid,
                                                Tag = form.Tag,
                                                WordForm = form.Word + ' ' + parts[2].Word,
                                            });
                                        }
                                    }
                                    else
                                    {
                                        foreach (var form in secondWordForms)
                                        {
                                            db.GeneralWordForms.Add(new GeneralWordForm()
                                            {
                                                WordNumber = 2,
                                                GeneralOriginalWordGuid = word.Guid,
                                                Tag = form.Tag,
                                                WordForm = form.Word,
                                            });
                                        }
                                    }
                                    
                                }
                                else if (parts[0].WordDescription.Contains("sloveso") || parts[0].WordDescription.Contains("podstatné jméno") && parts.Count == 2)
                                {
                                    var firstWordForms = await WordFormHelper.GetWordForms(wordFormClient, parts[0].Word);
                                    foreach (var form in firstWordForms)
                                    {
                                        db.GeneralWordForms.Add(new GeneralWordForm()
                                        {
                                            WordNumber = 1,
                                            GeneralOriginalWordGuid = word.Guid,
                                            Tag = form.Tag,
                                            WordForm = form.Word + ' ' + parts[1].Word,
                                        });
                                    }
                                }

                            }         
                        }

                        Console.WriteLine($"The translation for word '{word.Word}' ({language.Name}) was added  to database, waiting for save");
                        await db.SaveChangesAsync();

                        await Task.Delay(5000);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("ERROR! Word {0}, language {1}. {2}", word.Word, language.Name, ex.ToString()));
                        string logFilePath = "error_log.txt";
                        File.AppendAllText(logFilePath, DateTime.Now + " - " + string.Format("ERROR! Word {0}, language {1}. {2}", word.Word, language.Name, ex.ToString()) + Environment.NewLine);
                        await Task.Delay(60000);
                    }
                    finally
                    {
                        Console.WriteLine($"The translation process for the word '{word.Word}' ({language.Name}) was finished");
                    }
                }
            }

        }
    }
}

public class WordPart
{
    public string Word { get; set; }
    public string WordDescription { get; set; }
    public int WordNumber { get; set; }
}