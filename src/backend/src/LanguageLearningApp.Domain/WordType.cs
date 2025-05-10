using Shared.Domain;

namespace LanguageLearningApp.Domain;

public class WordType : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;
}
