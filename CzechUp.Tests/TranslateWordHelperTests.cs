using CzechUp.Helper;
using CzechUp.Helper.ApiHelper;

namespace CzechUp.Tests
{
    [TestClass]
    public sealed class TranslateWordHelperTests
    {
        [TestMethod]
        public async Task TranslateWord_WithoutDiacritics_SchouldReturnWithDiacritics()
        {
            // Arrange
            var czechWord = "kocka";
            using HttpClient client = new();

            // Act
            var result = await TranslateWordHelper.TranslateWord(client, czechWord, "ru");

            // Assert
            Assert.AreEqual("kočka", result.MainInfo.First().Head.FoundedWord);
            Assert.AreEqual("ко́шка", result.MainInfo.First().Translations.First().Meanings.First().Translations.First().Last());
        }
    }
}
