using Shared.Domain;

namespace LanguageLearningApp.Domain;

public class Word : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public Pronunciation Pronunciation { get; private set; } = default!;

    private List<WordType> _wordTypes { get; set; } = [];
    public IReadOnlyCollection<WordType> WordTypes => _wordTypes;

    private List<WordCategory> _wordCategories { get; set; } = [];
    public IReadOnlyCollection<WordCategory> WordCategories => _wordCategories;

    private List<Word> _synonyms { get; set; } = [];
    public IReadOnlyCollection<Word> Synonyms => _synonyms;

    private List<Word> _anthonyms { get; set; } = [];
    public IReadOnlyCollection<Word> Anthonyms => _anthonyms;

    private List<string> _examplesSentences { get; set; } = [];
    public IReadOnlyCollection<string> ExamplesSentences => _examplesSentences;

    public static Word Create(string name, string definition)
    {
        Word word = new()
        {
            Name = name,
            Definition = definition
        };

        //word.Raise(new WordCreatedEvent());

        return word;
    }

    public Result AddSynonym(Word synonym)
    {
        //if (_synonyms.Any(w => w.Id == synonym.Id))
        //    return Result.Failure("This synonym already exists.");

        _synonyms.Add(synonym);
        //Raise(new SynonymAddedEvent());

        return Result.Success();
    }

    public Result AddAnthonym(Word synonym)
    {
        _anthonyms.Add(synonym);
        //Raise(new AnthonymAddedEvent());

        return Result.Success();
    }

    public Result AddExampleSentence(string sentence)
    {
        _examplesSentences.Add(sentence);
        //Raise(new ExampleSentenceAddedEvent());
        // Business kurallarıyla bu eventten sonra yeni cümle eklendi görmek ister misin falan diyeceğiz

        return Result.Success();
    }
}
