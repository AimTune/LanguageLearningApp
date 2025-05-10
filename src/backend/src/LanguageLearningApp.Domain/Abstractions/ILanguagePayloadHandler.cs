using Shared.Domain;

namespace LanguageLearningApp.Domain.Abstractions;

public interface ILanguageHandler<TIn, TOut>
{
    public ILanguageHandler<TIn, TOut> SetNext(ILanguageHandler<TIn, TOut> handler);
    public Result<TOut> Handle(TIn payload);
}

public interface ILanguageHandler<TIn>
{
    public ILanguageHandler<TIn> SetNext(ILanguageHandler<TIn> handler);
    public Result Handle(TIn payload);
}