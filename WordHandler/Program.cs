using CzechUp.EF;
using CzechUp.EF.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;


class Program
{
    static async Task Main()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseNpgsql("");
        int levelCounter = 0;
        int totalCounter = 0;

        using (var db = new DatabaseContext(optionsBuilder.Options))
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "WordsFromSource1.txt");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found: " + filePath);
                return;
            }

            LanguageLevel? level = null;
            GeneralTopic? topic = null;
            Language languageRu = db.Languages.First(l => l.Name == "RU");
            Language languageEng = db.Languages.First(l => l.Name == "EN");

            List<string> lines = new List<string>(File.ReadAllLines(filePath));

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i].Trim();
                if (line.StartsWith("!!!Level:"))
                {
                    var str = line.Replace("!!!Level:", "").Trim();
                    if (level != null)
                    {
                        Console.WriteLine(string.Format("{0} words was written for the level {1} from the first source", levelCounter, level.Name));
                        levelCounter = 0;
                    }
                    level = db.LanguageLevels.FirstOrDefault(x => x.Name == str);
                    if (level == null)
                    {
                        Console.WriteLine(string.Format("Language not found {0}", str));
                        throw new Exception();
                    }                    
                }
                else if (line.StartsWith("---Topic:"))
                {
                    var str = line.Replace("---Topic:", "").Trim();
                    topic = db.GeneralTopics.FirstOrDefault(x => x.Name == str);
                    if (topic == null)
                    {
                        Console.WriteLine(string.Format("Topic not found {0}", str));
                        throw new Exception();
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
                        Match match = Regex.Match(word, @"^(.*?)\s*\((.*?)\)\s*$");
                        db.GeneralOriginalWords.Add(new GeneralOriginalWord()
                        {
                            GeneralTopicGuid = topic!.Guid,
                            LanguageLevelGuid = level!.Guid,
                            Word = match.Success? match.Groups[1].Value.Trim(): word.Trim()
                        });
                        levelCounter++;
                        totalCounter++;
                    }
                }
            }

            Console.WriteLine(string.Format("{0} words was written for the level {1} from the first source", levelCounter, level.Name));
            Console.WriteLine(string.Format("totalWords from the first source: {0}", totalCounter));
            db.SaveChanges();


        }
    }
}