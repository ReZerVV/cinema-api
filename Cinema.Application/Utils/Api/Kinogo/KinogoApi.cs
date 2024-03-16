using HtmlAgilityPack;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Cinema.Application.Utils.Api.Kinogo;

public class KinogoApi
{
    private string GetPlayerUrlFromKinogoUrl(string url)
    {
        HtmlWeb web = new HtmlWeb();
        var htmlDoc = web.Load(url);
        url = GetPlayer1HtmlNode(htmlDoc.DocumentNode)
            .GetAttributeValue("data-src", string.Empty);
        return $"https:{url}";
    }

    public (string, string)? GetKpIdAndDownloadUrlByUrl(string url)
    {
        try
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(GetPlayerUrlFromKinogoUrl(url));
            var kpId = htmlDoc.GetElementbyId("kinopoiskId").GetAttributeValue("value", null);
            var node = htmlDoc.GetElementbyId("fs");
            var json = JsonSerializer.Deserialize<Dictionary<string, string>>(
                node.GetAttributeValue("value", string.Empty));
            var bestLink = json.Values
                .SelectMany(value => value.Split(","))
                .OrderByDescending(value =>
                {
                    if (int.TryParse(Regex.Match(value, @"\[(\d+)p\]").Groups[1].Value, out int movieQuality))
                        if (movieQuality == 360)
                            return movieQuality;
                    return 0;
                })
                .Select(value => Regex.Replace(value, @"\[\d+p\]", "https:"))
                .First();
            return (kpId, bestLink);
        }
        catch
        {
            throw new CinemaError(
                Errors.CinemaErrorType.VALIDATION_ERROR,
                "Incorrect link, system failed to load data from kinogo.film server.");
        }
    }

    private HtmlNode? GetPlayer1HtmlNode(HtmlNode htmlNode)
    {
        if (htmlNode.InnerHtml == "Плеер 1")
            return htmlNode;
        foreach (var childHtmlNode in htmlNode.ChildNodes)
        {
            var result = GetPlayer1HtmlNode(childHtmlNode);
            if (result != null)
                return result;
        }
        return null;
    }
}
