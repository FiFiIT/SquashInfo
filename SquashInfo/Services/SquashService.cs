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

        public List<CourtDto> GetFreeSquashCourts(DateTime fromTime, DateTime toTime, TimeSpan requestedTime)
        {
            string hastResponse = GetSquashCourst(fromTime, toTime).Result;
            List<CourtDto> allCourts = ConvertSquashResponse(hastResponse);
            List<CourtDto> freeCourts = GetFreeCourts(allCourts, fromTime, toTime, requestedTime);

            return freeCourts;
        }

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

        private List<CourtDto> ConvertSquashResponse(string hastaResponse)
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
        private List<CourtDto> GetFreeCourts(List<CourtDto> FreeCourts, DateTime fromTime, DateTime toTime, TimeSpan requestedTime)
        {
            var result = FreeCourts.SelectMany(c => c.Free, (court, freeHours) => new { court, freeHours })
                .Where(courtAndHours => courtAndHours.freeHours.From >= fromTime && courtAndHours.freeHours.To <= toTime)
                .Select(courtAndHours =>
                    new
                    {
                        Number = courtAndHours.court.Number,
                        Free = new FreeHoursDto()
                        {
                            From = courtAndHours.freeHours.From,
                            To = courtAndHours.freeHours.To
                        }
                    }
                ).GroupBy(c => c.Number,
                    c => c.Free,
                    (groupKey, Free) => new
                    {
                        Number = groupKey,
                        Free
                    });

            if (result == null)
            {
                return null;
            }

            List<CourtDto> korty = new List<CourtDto>();
            CourtDto curCourt = null;

            foreach (var kort in result)
            {
                curCourt = new CourtDto() { Number = kort.Number, Free = new List<FreeHoursDto>() };
                FreeHoursDto prevTime = null;

                foreach (var curTime in kort.Free)
                {
                    if (prevTime is null)
                    {
                        prevTime = curTime;
                    }
                    else
                    {
                        if (curTime.From == prevTime.To)
                        {
                            prevTime.To = curTime.To;
                        }
                        else
                        {
                            if (prevTime.AvailableTime >= requestedTime)
                            {
                                curCourt.Free.Add(prevTime);
                            }

                            prevTime = curTime;
                        }
                    }
                }

                if (prevTime.AvailableTime >= requestedTime)
                {
                    curCourt.Free.Add(prevTime);
                }

                if (curCourt.Free.Count() > 0)
                {
                    korty.Add(curCourt);
                }
            }

            return korty;
        }
    }
}
