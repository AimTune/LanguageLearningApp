using Shared.Domain;

namespace LanguageLearningApp.Domain.ChainOfResponsibilities.StagePromotion.Rules;

public class DailyToWeeklyPromotionHandler : SetPromotionChainsHandler
{
    public override Result Handle(UserWordLearningProgress progress)
    {
        if (progress.Stage == LearningStage.Daily && progress.CorrectAnswersInARow >= 7)
        {
            progress.PromoteTo(LearningStage.Weekly);
            return Result.Success();
        }

        return base.Handle(progress);
    }
}