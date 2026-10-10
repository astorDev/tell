using System.Collections.ObjectModel;

namespace Tell;

public class RuleCollection(IReadOnlyDictionary<string, Rule> rules)
    : ReadOnlyDictionary<string, Rule>(rules.ToDictionary(entry => entry.Key, entry => entry.Value))
{
    public Rule this[Rule rule] => this[rule.Name];
}
