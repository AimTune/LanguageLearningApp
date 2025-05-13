using Shared.Domain;

namespace LanguageLearningApp.Domain.Words.Events;

public class WordCreatedEvent(Guid wordId) : DomainEvent
{
    public Guid WordId { get; } = wordId;
}