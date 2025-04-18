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
            "had"
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
                                    var forms = await WordFormHelper.GetWordForms(wordFormClient, translateWordResult.MainInfo.First().Head.FoundedWord);

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
                                //translate each word to find word form for every word
                                int wordNumber = 1;
                                foreach (var mainWord in mainWords)
                                {
                                    var translateResult = await TranslateWordHelper.TranslateWord(wordTranslateClient, mainWord, language.Name.ToLower());
                                    //find word forms for every word
                                    var forms = await WordFormHelper.GetWordForms(wordFormClient, translateResult.MainInfo.First().Head.FoundedWord);

                                    foreach (var form in forms)
                                    {
                                        db.GeneralWordForms.Add(new GeneralWordForm()
                                        {
                                            WordNumber = wordNumber,
                                            GeneralOriginalWordGuid = word.Guid,
                                            Tag = form.Tag,
                                            WordForm = form.Word,
                                        });
                                    }

                                    wordNumber++;
                                    await Task.Delay(5000);
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