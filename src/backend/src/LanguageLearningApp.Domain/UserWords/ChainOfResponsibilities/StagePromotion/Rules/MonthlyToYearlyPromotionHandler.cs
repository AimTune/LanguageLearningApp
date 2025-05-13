using Shared.Domain;

namespace LanguageLearningApp.Domain.UserWords.ChainOfResponsibilities.StagePromotion.Rules;

public class MonthlyToYearlyPromotionHandler : SetPromotionChainsHandler
{
    public override Result Handle(UserWordLearningProgress progress)
    {
        if (progress.Stage == LearningStage.Monthly && progress.CorrectAnswersInARow !>= 4)
        {
            progress.PromoteTo(LearningStage.Weekly);
            return Result.Success();
        }

        return base.Handle(progress);
    }
}