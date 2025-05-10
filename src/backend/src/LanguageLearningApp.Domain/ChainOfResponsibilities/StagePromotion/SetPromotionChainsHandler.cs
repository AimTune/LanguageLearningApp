using LanguageLearningApp.Domain.Abstractions;
using Shared.Domain;

namespace LanguageLearningApp.Domain.ChainOfResponsibilities.StagePromotion;

public class SetPromotionChainsHandler : ILanguageHandler<UserWordLearningProgress>
{

    private ILanguageHandler<UserWordLearningProgress>? _nextHandler;

    public virtual Result Handle(UserWordLearningProgress progress)
    {
        if (_nextHandler is not null)
        {
            return _nextHandler.Handle(progress);
        }

        return Result.Success();
    }

    public ILanguageHandler<UserWordLearningProgress> SetNext(ILanguageHandler<UserWordLearningProgress> handler)
    {
        _nextHandler = handler;
        return handler;
    }
}