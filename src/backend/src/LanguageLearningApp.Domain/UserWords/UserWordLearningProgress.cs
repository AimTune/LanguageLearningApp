using LanguageLearningApp.Domain.UserWords.ChainOfResponsibilities.StagePromotion.Rules;
using LanguageLearningApp.Domain.UserWords.Events;
using Shared.Domain;

namespace LanguageLearningApp.Domain.UserWords;

public class UserWordLearningProgress : Entity<Guid>
{
    public Guid UserId { get; private set; }
    public Guid WordId { get; private set; }
    public LearningStage Stage { get; private set; } = LearningStage.Daily;
    public DateTime LastReviewedAt { get; private set; } = DateTime.UtcNow;
    public int CorrectAnswersInARow { get; private set; } = 0;
    public bool IsMastered => Stage == LearningStage.Yearly && CorrectAnswersInARow >= 5;

    private UserWordLearningProgress() { }

    public static UserWordLearningProgress Create(Guid wordId, Guid userId)
    {
        var progress = new UserWordLearningProgress
        {
            UserId = userId,
            WordId = wordId
        };

        progress.Raise(new UserStartedLearningWordDomainEvent(userId, wordId));

        return progress;
    }

    public Result Review(bool answeredCorrectly)
    {
        LastReviewedAt = DateTime.UtcNow;

        if (answeredCorrectly)
        {
            CorrectAnswersInARow++;
            Raise(new WordAnsweredCorrectlyDomainEvent(UserId, WordId, Stage));

            var result = TryPromoteStage();
            if (IsMastered)
            {
                Raise(new WordMasteredDomainEvent(UserId, WordId));
            }

            return result;
        }
        else
        {
            CorrectAnswersInARow = 0;
            DemoteStage();
            Raise(new WordAnsweredIncorrectlyDomainEvent(UserId, WordId));
            return Result.Success("Incorrect answer. Stage reset to Daily.");
        }
    }

    private void DemoteStage()
    {
        LearningStage previousStage = Stage;

        Stage = Stage switch
        {
            LearningStage.Yearly => LearningStage.Monthly,
            LearningStage.Monthly => LearningStage.Weekly,
            LearningStage.Weekly => LearningStage.Daily,
            LearningStage.Daily => LearningStage.Daily, // Minimum seviye
            _ => Stage
        };

        Raise(new WordStageDemotedDomainEvent(UserId, WordId, previousStage, Stage)); ;
    }

    public void PromoteTo(LearningStage newStage)
    {
        var previous = Stage;
        Stage = newStage;
        CorrectAnswersInARow = 0;

        Raise(new WordStagePromotedDomainEvent(UserId, WordId, newStage));
    }

    private Result TryPromoteStage()
    {
        var dailyToWeekly = new DailyToWeeklyPromotionHandler();

        dailyToWeekly
            .SetNext(new WeeklyToMonthlyPromotionHandler())
            .SetNext(new MonthlyToYearlyPromotionHandler())
            .SetNext(new YearlyToLearntPromotionHandler());

        var result = dailyToWeekly.Handle(this);

        if (result.IsFailure) {
        
        }

        return Result.Success("Correct answer. Not enough streak for promotion.");
    }
}