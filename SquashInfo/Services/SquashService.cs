using SquashInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SquashInfo.Services
{
    public class SquashService : ISquashService
    {
        static readonly HttpClient client = new HttpClient();


        public List<CourtDto> GetFreeSquashCourts(ReservationRequest request)
        {
            string hastResponse = GetSquashCourst(request.FromTime, request.ToTime, request.Type).Result;
            List<CourtDto> allCourts = ConvertSquashResponse(hastResponse, request.StartDate, request.Type);
            allCourts = allCourts.Where(c => !request.Exclude.Contains(c.Number)).ToList();
            List<CourtDto> freeCourts = GetFreeCourts(allCourts, request.FromTime, request.ToTime, request.Duration);

            return freeCourts;
        }

        public HttpResponseMessage RezerwujTest(BookRequestDto book)
        {
            var baseAddress = new Uri("http://hastalavista.pl");
            var cookieContainer = new CookieContainer();
            HttpResponseMessage response = null;

            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer, UseCookies = true })
            {
                using (HttpClient client = new HttpClient(handler) { BaseAddress = baseAddress })
                {
                    //Let's visit the homepage to set initial cookie values
                    Task.Run(async () => response = await client.GetAsync("/")).GetAwaiter().GetResult(); //200

                    string urlToPost = "http://hastalavista.pl/wp-login.php";

                    var postData = new List<KeyValuePair<string, string>>();
                    postData.Add(new KeyValuePair<string, string>("log", $"{book.Login}"));
                    postData.Add(new KeyValuePair<string, string>("pwd", $"{book.Password}"));
                    postData.Add(new KeyValuePair<string, string>("rememberme", "forever"));
                    postData.Add(new KeyValuePair<string, string>("wp-submit", "Zaloguj się"));
                    postData.Add(new KeyValuePair<string, string>("redirect_to", "http://hastalavista.pl/squash/klub/rezerwacje-2/"));

                    HttpContent stringContent = new FormUrlEncodedContent(postData);

                    client.DefaultRequestHeaders.Add("Host", "hastalavista.pl");
                    client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                    //client.DefaultRequestHeaders.Add("Content-Length", 100);
                    client.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
                    client.DefaultRequestHeaders.Add("Accept", "*/*");
                    client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                    client.DefaultRequestHeaders.Add("Origin", "http://hastalavista.pl/");
                    client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.75 Safari/537.36");
                    //client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                    client.DefaultRequestHeaders.Add("Referer", "http://hastalavista.pl/squash/klub/rezerwacje-2/");
                    client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                    client.DefaultRequestHeaders.Add("Accept-Language", "pl-PL,pl;q=0.9,en-US;q=0.8,en;q=0.7");

                    //cookieContainer.Add(baseAddress, new Cookie("wordpress_528dc539b665da98547be13913aae6de", "TYBOFIL%7C1570542788%7CVZf7s9o3JagQvf6AOeDwZpHnpz9oX2TeaI6LJbB6I39%7C2a0e4ce86e9ce64ec7b3a78f2f99e63e8630e0e892536cbfba1f5d78fb54e445"));
                    //cookieContainer.Add(baseAddress, new Cookie("wordpress_logged_in_528dc539b665da98547be13913aae6de", "TYBOFIL%7C1570542788%7CVZf7s9o3JagQvf6AOeDwZpHnpz9oX2TeaI6LJbB6I39%7C6f18ba1806f44050db704f5da057b9b370d806e59f16115b162f84b8e1ef16cb"));
                    cookieContainer.Add(baseAddress, new Cookie("viewed_cookie_policy", "yes"));

                    //Receiving 200 response for the nextline, though it returns a 302 in a browser environment
                    Task.Run(async () => response = await client.PostAsync(urlToPost, stringContent)).GetAwaiter().GetResult();

                    if(response.StatusCode != HttpStatusCode.OK)
                    {
                        return response;
                    }

                    //401 response for the next line
                    urlToPost = "http://hastalavista.pl/wp-admin/admin-ajax.php";

                    postData = new List<KeyValuePair<string, string>>();
                    postData.Add(new KeyValuePair<string, string>("action", "RezerwujWybraneZapisz"));
                    postData.Add(new KeyValuePair<string, string>("data", $"{book.Data.ToString("yyyy-MM-dd")}"));
                    postData.Add(new KeyValuePair<string, string>("REZ[]", $"{book.Rez[0]}"));
                    postData.Add(new KeyValuePair<string, string>("REZ[]", $"{book.Rez[1]}"));

                    stringContent = new FormUrlEncodedContent(postData);

                    Task.Run(async () => response = await client.PostAsync(urlToPost, stringContent)).GetAwaiter().GetResult();
                }
            }

            return response;
        }

        public async Task<string> Rezerwuj(BookRequestDto book)
        {
            var values = new Dictionary<string, string>
            {
                { "action", "RezerwujWybraneZapisz" },
                { "data", $"{book.Data.ToString("yyyy-MM-dd")}" },
                { "REZ[]", $"{book.Rez[0]}" }
            };

            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync("http://hastalavista.pl/wp-admin/admin-ajax.php", content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> RezerwujPotwierdz(BookRequestDto book)
        {
            var values = new Dictionary<string, string>
            {
                { "action", "Rezerwacje4Datepicker" },
                { "klie_nick", "tyborowskif" },
                { "data_od", "" },
                { "data_do", "" }
            };

            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync("http://hastalavista.pl/wp-admin/admin-ajax.php", content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> GetSquashCourst(DateTime from, DateTime to, string type)
        {
            string startHour;
            string endHour;

            if(to.Hour < 15) {
                startHour = "06:00";
                endHour = "15:00";
            }else if(to.Hour < 20)
            {
                startHour = "11:00";
                endHour = "20:00";
            }
            else
            {
                startHour = "15:00";
                endHour = "00:00";
            }

            var values = new Dictionary<string, string>
            {
                { "operacja", "ShowRezerwacjeTable" },
                { "action", "ShowRezerwacjeTable" },
                { "data", $"{from.ToString("yyyy-MM-dd")}" },
                { "obiekt_typ", $"{type}" },
                { "godz_od", $"{startHour}" },
                { "godz_do", $"{endHour}" }
            };

            var content = new FormUrlEncodedContent(values);
            //var response = await client.PostAsync("http://hastalavista.pl/squash/klub/rezerwacje-2/", content);
            var response = await client.PostAsync("http://hastalavista.pl/wp-admin/admin-ajax.php", content);

            return await response.Content.ReadAsStringAsync();
        }
        private List<CourtDto> ConvertSquashResponse(string hastaResponse, DateTime StartDate, string type)
        {
            string startText = "<tr  data-obie_id=\"1\">";
            string endText = "32</td></tr>";
            if(type == "badminton")
            {
                startText = "<tr  data-obie_id=\"30\">";
                endText = "10</td></tr>"; //there is less courts on badminton
            }

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

                string obiekt = node.Attributes["data-obie_id"].Value;
                if (String.IsNullOrEmpty(obiekt) || !Int32.TryParse(obiekt, out int obiektId))
                {
                    continue;
                }

                CourtDto court = new CourtDto() { Number = courtNumber, ObkietId = obiektId };

                var inputNodes = docHelp.DocumentNode.SelectNodes("//td/input");

                if (inputNodes == null || !inputNodes.Any())
                {
                    continue;
                }

                List<FreeHoursDto> hours = inputNodes
                    .Select(i => new FreeHoursDto { From = StartDate + TimeSpan.Parse(i.Attributes["data-godz_od"].Value), To = StartDate + TimeSpan.Parse(i.Attributes["data-godz_do"].Value) }).ToList();

                court.Free = hours;
                squash.Add(court);
            }

            return squash;
        }
        public List<CourtDto> GroupFreeCourts(List<CourtDto> FreeCourts, DateTime fromTime, DateTime toTime, TimeSpan duration)
        {
            return GetFreeCourts(FreeCourts, fromTime, toTime, duration);
        }
        private List<CourtDto> GetFreeCourts(List<CourtDto> FreeCourts, DateTime fromTime, DateTime toTime, TimeSpan duration)
        {
            var result = FreeCourts.SelectMany(c => c.Free, (court, freeHours) => new { court, freeHours })
                .Where(courtAndHours => courtAndHours.freeHours.From >= fromTime && courtAndHours.freeHours.To <= toTime)
                .Select(courtAndHours =>
                    new
                    {
                        Number = courtAndHours.court.Number + "_" + courtAndHours.court.ObkietId,
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
            string num, obiekt;
            int index;

            foreach (var kort in result)
            {
                index = kort.Number.IndexOf("_");
                num = kort.Number.Substring(0, index);
                obiekt = kort.Number.Substring(index + 1);

                curCourt = new CourtDto() { Number = Int32.Parse(num), ObkietId = Int32.Parse(obiekt), Free = new List<FreeHoursDto>() };
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
                            if (prevTime.AvailableTime >= duration)
                            {
                                curCourt.Free.Add(prevTime);
                            }

                            prevTime = curTime;
                        }
                    }
                }

                if (prevTime.AvailableTime >= duration)
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
