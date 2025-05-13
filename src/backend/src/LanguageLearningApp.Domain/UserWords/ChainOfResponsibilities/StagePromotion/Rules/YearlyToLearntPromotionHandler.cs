using Shared.Domain;

namespace LanguageLearningApp.Domain.UserWords.ChainOfResponsibilities.StagePromotion.Rules;

public class YearlyToLearntPromotionHandler : SetPromotionChainsHandler
{
    public override Result Handle(UserWordLearningProgress progress)
    {
        if (progress.Stage == LearningStage.Yearly && progress.CorrectAnswersInARow !>= 1)
        {
            progress.PromoteTo(LearningStage.Monthly);
            return Result.Success();
        }

        return base.Handle(progress);
    }
}