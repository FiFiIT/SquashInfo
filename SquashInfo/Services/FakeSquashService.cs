using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SquashInfo.Models;

namespace SquashInfo.Services
{
    public class FakeSquashService : ISquashService
    {
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

        public Task<string> GetSquashCourst(DateTime from, DateTime to)
        {
            StreamReader myFile = new StreamReader(@"D:\GitHub\SquashInfo\SquashInfo\Services\sampleResponse.txt");
            return Task.Run(() => { return myFile.ReadToEnd(); });
        }

        List<CourtDto> ISquashService.GetFreeSquashCourts(DateTime fromTime, DateTime toTime, TimeSpan requestedTime)
        {
            throw new NotImplementedException();
        }
    }
}
