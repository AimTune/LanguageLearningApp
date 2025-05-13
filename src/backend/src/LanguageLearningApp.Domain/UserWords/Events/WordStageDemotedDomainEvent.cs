using Shared.Domain;

namespace LanguageLearningApp.Domain.UserWords.Events;

public class WordStageDemotedDomainEvent(Guid userId, Guid wordId, LearningStage previousStage, LearningStage newStage) : DomainEvent
{
    public Guid UserId { get; } = userId;
    public Guid WordId { get; } = wordId;
    public LearningStage PreviousStage { get; } = previousStage;
    public LearningStage NewStage { get; } = newStage;
}