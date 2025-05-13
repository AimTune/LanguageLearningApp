using Shared.Domain;

namespace LanguageLearningApp.Domain.Words.Events;

public class AnthonymAddedEvent(Guid wordId, Guid synonymId) : DomainEvent
{
    public Guid WordId { get; } = wordId;
    public Guid SynonymId { get; } = synonymId;
}