using Shared.Domain;

namespace LanguageLearningApp.Domain.UserWords.ChainOfResponsibilities.StagePromotion.Rules;

public class DailyToWeeklyPromotionHandler : SetPromotionChainsHandler
{
    public override Result Handle(UserWordLearningProgress progress)
    {
        if (progress.Stage == LearningStage.Daily && progress.CorrectAnswersInARow >= 7)
        {
            progress.PromoteTo(LearningStage.Daily);
            return Result.Success();
        }

        return base.Handle(progress);
    }
}