using SquashInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SquashInfo.Services
{
    public class SquashService : ISquashService
    {
        static readonly HttpClient client = new HttpClient();

        public async Task<string> GetSquashCourst(DateTime from, DateTime to)
        {
            var values = new Dictionary<string, string>
            {
                { "operacja", "ShowRezerwacjeTable" },
                { "action", "ShowRezerwacjeTable" },
                { "data", $"{from.ToString("yyyy-MM-dd")}" },
                { "obiekt_typ", "squash" },
                { "godz_od", $"{from.ToString("HH:mm")}" },
                { "godz_do", $"{to.ToString("HH:mm")}" }
            };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("http://hastalavista.pl/squash/klub/rezerwacje-2/", content);

            return await response.Content.ReadAsStringAsync();
        }

        public List<CourtDto> ConvertSquashResponse(string hastaResponse)
        {
            const string startText = "<tr  data-obie_id=\"1\">";
            const string endText = "32</td></tr>";

            var start = hastaResponse.IndexOf(startText);
            var end = hastaResponse.IndexOf(endText, start) + endText.Length;
            var length = end - start;
            var result = hastaResponse.Substring(start, length);

            List<CourtDto> squash = new List<CourtDto>();

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlDocument docHelp = new HtmlAgilityPack.HtmlDocument();

            doc.LoadHtml(result);

            foreach (var node in doc.DocumentNode.SelectNodes($"//tr"))
            {
                docHelp.LoadHtml(node.InnerHtml);
                string v = docHelp.DocumentNode.SelectSingleNode("td").InnerText;

                if (String.IsNullOrEmpty(v) || !Int32.TryParse(v, out int courtNumber))
                {
                    continue;
                }

                CourtDto court = new CourtDto() { Number = courtNumber };

                var inputNodes = docHelp.DocumentNode.SelectNodes("//td/input");

                if (inputNodes == null || !inputNodes.Any())
                {
                    continue;
                }

                List<FreeHoursDto> hours = inputNodes
                    .Select(i => new FreeHoursDto { From = DateTime.Parse(i.Attributes["data-godz_od"].Value), To = DateTime.Parse(i.Attributes["data-godz_do"].Value) }).ToList();

                court.Free = hours;
                squash.Add(court);
            }

            return squash;
        }
    }
}
