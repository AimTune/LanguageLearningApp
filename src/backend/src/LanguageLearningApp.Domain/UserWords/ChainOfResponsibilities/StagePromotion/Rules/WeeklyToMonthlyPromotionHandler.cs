using Shared.Domain;

namespace LanguageLearningApp.Domain.UserWords.ChainOfResponsibilities.StagePromotion.Rules;

public class WeeklyToMonthlyPromotionHandler : SetPromotionChainsHandler
{
    public override Result Handle(UserWordLearningProgress progress)
    {
        if (progress.Stage == LearningStage.Weekly && progress.CorrectAnswersInARow !>= 3)
        {
            progress.PromoteTo(LearningStage.Daily);
            return Result.Success();
        }

        return base.Handle(progress);
    }
}