using Cinema.Application.Utils.Errors;
using HtmlAgilityPack;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Cinema.Application.Utils.Api.Kinogo;

public class KinogoApi
{
    private string GetPlayerUrlFromKinogoUrl(string url)
    {
        HtmlWeb web = new HtmlWeb();
        var htmlDoc = web.Load(url);
        var player = GetPlayer1HtmlNode(htmlDoc.DocumentNode);
        if (player == null)
            throw new CinemaError(
                CinemaErrorType.VALIDATION_ERROR,
                "Unable to download the movie from this link.");
        url = player.GetAttributeValue("data-src", string.Empty);
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
            var urls = json.Values
                .SelectMany(value => value.Split(","))
                .OrderByDescending(value =>
                {
                    if (int.TryParse(Regex.Match(value, @"\[(\d+)p\]").Groups[1].Value, out int movieQuality))
                        if (movieQuality == 360)
                            return movieQuality;
                    return 0;
                })
                .Select(value => Regex.Replace(value, @"\[\d+p\]", "https:"));
            foreach (var downloadUrl in urls)
            {
                var validDownloadUrl = GetValidUrl(downloadUrl);
                if (validDownloadUrl != null)
                    return (kpId, validDownloadUrl);
            }
            return null;
        }
        catch
        {
            throw new CinemaError(
                Errors.CinemaErrorType.VALIDATION_ERROR,
                "Incorrect link, system failed to load data from kinogo.film server.");
        }
    }

    private static string? GetValidUrl(string url)
    {
        try
        {
            if (url == null)
                return null;
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.AllowAutoRedirect = false;
            using var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;
            if (response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently)
                return GetValidUrl(response.Headers["Location"]);
            return url;
        }
        catch
        {
            return null;
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
