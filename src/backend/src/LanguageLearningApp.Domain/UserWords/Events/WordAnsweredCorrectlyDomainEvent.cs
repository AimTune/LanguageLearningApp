using Shared.Domain;

namespace LanguageLearningApp.Domain.UserWords.Events;

public class WordAnsweredCorrectlyDomainEvent(Guid userId, Guid wordId, LearningStage currentStage) : DomainEvent
{
    public Guid UserId { get; } = userId;
    public Guid WordId { get; } = wordId;
    public LearningStage CurrentStage { get; } = currentStage;
}