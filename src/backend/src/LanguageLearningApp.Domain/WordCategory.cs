using Shared.Domain;

namespace LanguageLearningApp.Domain;

public class WordCategory : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;
}
