namespace DentalApp.Shared.Helpers;

public static class MarkdownHelper
{
    public static string ToMarkdownTable<T>(IEnumerable<T> items, params string[] headers)
        => "| " + string.Join(" | ", headers) + " |\n|" + string.Join("|", headers.Select(_ => "---")) + "|\n" +
           string.Join("\n", items.Select(item => "| " + string.Join(" | ", typeof(T).GetProperties().Select(p => p.GetValue(item)?.ToString() ?? "")) + " |"));
}