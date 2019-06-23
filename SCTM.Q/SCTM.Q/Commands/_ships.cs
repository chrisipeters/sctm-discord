using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using System.Net.Http;
using Newtonsoft.Json;
using SCTM.Q.Models;
using System.Linq;
using System.Net;
using Discord;

namespace SCTM.Q.Commands
{
    public partial class Commands : ModuleBase
    {
        [Command("ships")]
        public async Task Process_Ships([Remainder]string args = "")
        {
            if (args == null || args.Trim().Length == 0) return;

            var msg = await ReplyAsync("Searching for ships...");

            var _searchTerms = args.ToLower().Trim().Split(' ').ToList();

            var result = await _http.GetAsync($"data/ships");
            var content = await result.Content.ReadAsStringAsync();
            var _ships = JsonConvert.DeserializeObject<List<Ship>>(content);

            var _shipsRet = new List<Ship>();
            foreach (var item in _searchTerms)
            {
                _shipsRet.AddRange(_ships.Where(i => i.MatchingStrings.Select(o => o.Value.ToLower()).Contains(item) || i.Manufacturer.MatchingStrings.Select(o => o.Value.ToLower()).Contains(item)));
            }

            if (_shipsRet.Any())
            {
                foreach (var ship in _shipsRet)
                {
                    var _random = new Random();

                    #region image check

                    bool _imageExists;
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(ship.ThumbnailPath);
                        request.Method = "HEAD";
                        request.GetResponse();
                        _imageExists = true;
                    }
                    catch
                    {
                        _imageExists = false;
                    }

                    #endregion



                    var _ret = new EmbedBuilder();
                    _ret.WithTitle($"{ship.Manufacturer.KnownAs} {ship.Model}");
                    _ret.WithFooter(new EmbedFooterBuilder
                    {
                        IconUrl = (_imageExists) ? ship.ThumbnailPath : null,
                        Text = $"{_random.Next(10, 1000)} Org members have the {ship.Model}"
                    });
                    if (_imageExists) _ret.WithThumbnailUrl(ship.ThumbnailPath);
                    _ret.WithColor((ship.GameState.Name.ToLower() == "flight ready") ? Color.Green : Color.LightGrey);
                    _ret.Fields = new List<EmbedFieldBuilder>
                            {
                                new EmbedFieldBuilder{
                                    IsInline = true,
                                    Name = "Status",
                                    Value = (ship.GameState != null) ? ship.GameState.Name : "Unknown"
                                },
                                new EmbedFieldBuilder{
                                    IsInline = true,
                                    Name = "SCU",
                                    Value = ship.CargoCapacity ?? 0
                                },
                                new EmbedFieldBuilder
                                {
                                    IsInline = false,
                                    Name = "Description",
                                    Value = ship.Description ?? ""
                                }
                            };

                    try { await msg.DeleteAsync(); } catch { }
                    await Context.Message.Channel.SendMessageAsync("", false, _ret.Build());
                }
            }
        }
    }
}
