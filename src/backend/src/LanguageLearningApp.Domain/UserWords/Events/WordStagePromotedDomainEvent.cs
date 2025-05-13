using Shared.Domain;

namespace LanguageLearningApp.Domain.UserWords.Events;

public class WordStagePromotedDomainEvent(Guid userId, Guid wordId, LearningStage newStage) : DomainEvent
{
    public Guid UserId { get; } = userId;
    public Guid WordId { get; } = wordId;
    public LearningStage NewStage { get; } = newStage;
}