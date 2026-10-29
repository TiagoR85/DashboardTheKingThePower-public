using DashboardTheKingThePower.Models;

namespace DashboardTheKingThePower.Services;

public static class FaqFilter
{
    public static List<FaqItem> Filter(List<FaqItem> items, string query)
    {
        var q = query?.Trim() ?? "";
        if (q.Length == 0) return items;
        return items
            .Where(i => i.Question.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                        i.Answer.Contains(q, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}
