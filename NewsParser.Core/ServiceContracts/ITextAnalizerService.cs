namespace NewsParser.Core.ServiceContracts
{
    public interface ITextAnalizerService
    {
        /// <summary>
        /// Получить коллекцию самых часто встречающихся слов в тексте
        /// </summary>
        /// <param name="sourceText">Исходный текст</param>
        /// <param name="count">Колличество слов</param>
        /// <returns>Словарь, содержащий слова и количество появлений в тексте</returns>
        public Dictionary<string, int>? GetTopWords(string sourceText, int count);
    }
}