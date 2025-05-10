using Shared.Domain;

namespace LanguageLearningApp.Domain.ChainOfResponsibilities.StagePromotion.Rules;

public class WeeklyToMonthlyPromotionHandler : SetPromotionChainsHandler
{
    public override Result Handle(UserWordLearningProgress progress)
    {
        if (progress.Stage == LearningStage.Weekly && progress.CorrectAnswersInARow >= 3)
        {
            progress.PromoteTo(LearningStage.Monthly);
            return Result.Success();
        }

        return base.Handle(progress);
    }
}