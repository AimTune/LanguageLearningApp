using Shared.Domain;

namespace LanguageLearningApp.Domain.UserWords.Events;

public class WordAnsweredIncorrectlyDomainEvent(Guid userId, Guid wordId) : DomainEvent
{
    public Guid UserId { get; } = userId;
    public Guid WordId { get; } = wordId;
}