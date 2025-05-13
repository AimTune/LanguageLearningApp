using Shared.Domain;

namespace LanguageLearningApp.Domain.Words;

public class Pronunciation // :ValueObject
{
    public string BritishPronunciation { get; private set; } = string.Empty;
    public string AmericanPronunciation { get; private set; } = string.Empty;

    protected Pronunciation() { }

    public Result SetBritishPronunciation(string britishPronunciation)
    {
        BritishPronunciation = britishPronunciation;

        return Result.Success();
    }

    public Result SetAmericanPronunciation(string americanPronunciation)
    {
        AmericanPronunciation = americanPronunciation;

        return Result.Success();
    }

    protected  IEnumerable<object> GetEqualityComponents() //override
    {
        yield return BritishPronunciation;
        yield return AmericanPronunciation;
    }
}
