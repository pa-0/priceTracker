using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

public static class program2
{
    public static void ProcessUrl(string url, out string productName, out string siteName, out float price)
    {
        var task = ProcessUrlAsync(url);
        task.Wait();

        var result = task.Result;

        productName = result.ProductName;
        siteName = result.SiteName;
        price = result.Price;
    }

    private static async Task<(string ProductName, string SiteName, float Price)> ProcessUrlAsync(string url)
    {
        string productName = string.Empty;
        string siteName = string.Empty;
        float price = 0;

        try
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var pageContents = await response.Content.ReadAsStringAsync();
            var document = new HtmlDocument();
            document.LoadHtml(pageContents);

            if (url.Contains("teknosa"))
            {
                var nameNode = document.DocumentNode.SelectSingleNode("//h1[@class='pdp-title']/span[@class='replaceName']");
                if (nameNode == null)
                    throw new Exception("Product name node not found for Teknosa.");

                productName = nameNode.InnerText.Trim();

                var priceNode = document.DocumentNode.SelectSingleNode("//div[@class ='prd-prc2']/span[@class ='prc prc-third']");
                if (priceNode == null)
                    throw new Exception("Price node not found for Teknosa.");

                price = ParsePriceTeknosa(priceNode.InnerText.Trim().Replace("TL", "").Trim());
                siteName = "Teknosa";
            }
            else if (url.Contains("mediamarkt"))
            {
                var nameNode = document.DocumentNode.SelectSingleNode("//div[@id='product-details']//div[@class='details']/h1");
                if (nameNode == null)
                    throw new Exception("Product name node not found for MediaMarkt.");

                productName = nameNode.InnerText.Trim();

                var priceNode = document.DocumentNode.SelectSingleNode("//div[@id='product-details']//div[@class='price-sidebar']//meta[@itemprop='price']");
                if (priceNode == null)
                    throw new Exception("Price node not found for MediaMarkt.");

                string priceString = priceNode.GetAttributeValue("content", "").Trim().Replace(",", ".");
                if (!float.TryParse(priceString, NumberStyles.Float, CultureInfo.InvariantCulture, out price))
                {
                    throw new FormatException($"Invalid price format: {priceString}");
                }

                price = (float)Math.Round(price, 2);
                siteName = "MediaMarkt";
            }
            else
            {
                throw new Exception("Unsupported site.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return (productName, siteName, price);
    }

    private static float ParsePriceTeknosa(string priceString)
    {
        priceString = priceString.Replace(" ", "");

        if (priceString.Contains(".") && priceString.Contains(","))
        {
            int lastCommaIndex = priceString.LastIndexOf(',');

            string integerPart = priceString.Substring(0, lastCommaIndex).Replace(".", "");
            string decimalPart = priceString.Substring(lastCommaIndex).Replace(",", ".");

            priceString = integerPart + decimalPart;
        }
        else
        {
            priceString = priceString.Replace(".", "").Replace(",", ".");
        }

        if (float.TryParse(priceString, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
        {
            result = (float)Math.Round(result, 2);
            return result;
        }
        else
        {
            throw new FormatException($"Invalid price format: {priceString}");
        }
    }

}
