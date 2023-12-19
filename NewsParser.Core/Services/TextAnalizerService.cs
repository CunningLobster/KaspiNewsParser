using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using NewsParser.Core.ServiceContracts;
using StopWord;

namespace NewsParser.Core.Services
{
    public class TextAnalizerService : ITextAnalizerService
    {
        private readonly ILogger<TextAnalizerService> _logger;

        public TextAnalizerService(ILogger<TextAnalizerService> logger)
        {
            _logger = logger;

        }

        public Dictionary<string, int>? GetTopWords(string sourceText, int count)
        {
            _logger.LogInformation("Run Method {0} from {1}", nameof(GetTopWords), nameof(TextAnalizerService));

            //Regex для токенизации по пробелам и знакам препинания
            Regex regex = new Regex(@"[\s\p{P}]");

            //Создание коллекции стоп-слов
            string[] stopWordsRu = StopWords.GetStopWords("ru");
            string[] stopWordsEn = StopWords.GetStopWords("ru");
            HashSet<string> stopWords = stopWordsEn.Union(stopWordsRu).ToHashSet();

            string[] tokens = regex.Split(sourceText);

            //Исключить стоп-слова из коллекции
            List<string> filteredWords = tokens
            .Where(w => !stopWords.Contains(w.ToLower()) && !string.IsNullOrWhiteSpace(w))
            .ToList();

            //Сгруппировать слова в словарь из самых популярных слов
            Dictionary<string, int> topTenWords = filteredWords
            .GroupBy(w => w.ToLower())
            .OrderByDescending(g => g.Count())
            .Take(count)
            .ToDictionary(g => g.Key, g => g.Count());

            return topTenWords;
        }
    }
}