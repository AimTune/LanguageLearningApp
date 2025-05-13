using Shared.Domain;

namespace LanguageLearningApp.Domain.Words.Events;

public class ExampleSentenceAddedEvent(Guid wordId, string exampleSentence) : DomainEvent
{
    public Guid WordId { get; } = wordId;
    public string ExampleSentence { get; } = exampleSentence;
}