using LanguageLearningApp.Domain.ChainOfResponsibilities.StagePromotion.Rules;
using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageLearningApp.Domain;

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
            .SetNext(new WeeklyToMonthlyPromotionHandler());
            // .SetNext(new MonthlyToYearlyPromotionHandler());

        var result = dailyToWeekly.Handle(this);

        if (result.IsFailure) {
        
        }

        return Result.Success("Correct answer. Not enough streak for promotion.");
    }
}

public enum LearningStage
{
    Daily,
    Weekly,
    Monthly,
    Yearly
}

public class UserStartedLearningWordDomainEvent : DomainEvent
{
    public Guid UserId { get; }
    public Guid WordId { get; }

    public UserStartedLearningWordDomainEvent(Guid userId, Guid wordId)
    {
        UserId = userId;
        WordId = wordId;
    }
}

public class WordAnsweredCorrectlyDomainEvent : DomainEvent
{
    public Guid UserId { get; }
    public Guid WordId { get; }
    public LearningStage CurrentStage { get; }

    public WordAnsweredCorrectlyDomainEvent(Guid userId, Guid wordId, LearningStage currentStage)
    {
        UserId = userId;
        WordId = wordId;
        CurrentStage = currentStage;
    }
}

public class WordAnsweredIncorrectlyDomainEvent : DomainEvent
{
    public Guid UserId { get; }
    public Guid WordId { get; }

    public WordAnsweredIncorrectlyDomainEvent(Guid userId, Guid wordId)
    {
        UserId = userId;
        WordId = wordId;
    }
}


public class WordStagePromotedDomainEvent : DomainEvent
{
    public Guid UserId { get; }
    public Guid WordId { get; }
    public LearningStage NewStage { get; }

    public WordStagePromotedDomainEvent(Guid userId, Guid wordId, LearningStage newStage)
    {
        UserId = userId;
        WordId = wordId;
        NewStage = newStage;
    }
}


public class WordMasteredDomainEvent : DomainEvent
{
    public Guid UserId { get; }
    public Guid WordId { get; }

    public WordMasteredDomainEvent(Guid userId, Guid wordId)
    {
        UserId = userId;
        WordId = wordId;
    }
}

public class WordStageDemotedDomainEvent : DomainEvent
{
    public Guid UserId { get; }
    public Guid WordId { get; }
    public LearningStage PreviousStage { get; }
    public LearningStage NewStage { get; }

    public WordStageDemotedDomainEvent(Guid userId, Guid wordId, LearningStage previousStage, LearningStage newStage)
    {
        UserId = userId;
        WordId = wordId;
        PreviousStage = previousStage;
        NewStage = newStage;
    }
}
