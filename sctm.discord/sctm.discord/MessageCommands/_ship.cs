using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using sctm.connectors.rsi.models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sctm.discord
{
    public partial class MessageCommands : BaseCommandModule
    {
        [Command("ship")] // let's define this method as a command
        [Description("Get Ship(s) Information")] // this will be displayed to tell users what this command does when they invoke help
        [Aliases("ships")] // alternative names for the command
        public async Task Ship(CommandContext ctx, [Description("Search term")] params string[] args)
        {
            var _logAction = $"MessageCommands_Ping > {ctx.Message.Author.Id}";
            var _dateStarted = DateTime.Now;
            Log.Information("{logAction}: Command called", _logAction);


            await ctx.Message.CreateReactionAsync(_services.SearchEmoji);

            var _client = await _services.GetSCTMClient();
            if (_client == null) Log.Error("{logAction}: Unable to get SCTM Client", _logAction);
            else try
                {
                    string _searchTerm = null;
                    if (args != null && args.Any()) _searchTerm = string.Join(' ', args);
                    if (_searchTerm == null || _searchTerm.Length == 0) throw new Exception("No search term provided");

                    var _url = _config["SCTM:Urls:Ships"];
                    if (_searchTerm != null) _url += $"?search={_searchTerm}";

                    var _request = await _client.GetAsync(_url);
                    var _content = await _request.Content.ReadAsStringAsync();

                    if (_request.IsSuccessStatusCode)
                    {

                        //var _leaderboards = await _services.GetLeaderboards_Organization(ctx.Message.Channel.GuildId.ToString());

                        var _ships = JsonConvert.DeserializeObject<List<Ship>>(_content);

                        foreach (var ship in _ships)
                        {
                            string _leaderName = null;
                            string _leaderAvatarUrl = null;
                            ulong? _leaderProfit = null;
                            ulong? _leaderXP = null;
                            int? _leaderRecords = null;

                            //if (_leaderboards?.Results.Ships != null)
                            //{
                            //    var _shipXp = _leaderboards.Results.Ships.Where(i => i.Name.ToLower() == $"{ship.Manufacturer.KnownAs.ToLower()} {ship.ModelName.ToLower()}").FirstOrDefault();
                            //    if (_shipXp != null)
                            //    {
                            //        foreach (var team in _shipXp.Entrants)
                            //        {
                            //            var _topPlayer = team.TopPlayers.OrderByDescending(i => i.EarnedCredits).FirstOrDefault();
                            //            if (_topPlayer != null && (_leaderProfit == null || _topPlayer.EarnedCredits > _leaderProfit))
                            //            {
                            //                try
                            //                {
                            //                    var _player = await ctx.Guild.GetMemberAsync(ulong.Parse(_topPlayer.Name));
                            //                    _leaderName = _player.Username;
                            //                    _leaderProfit = _topPlayer.EarnedCredits;
                            //                    _leaderAvatarUrl = _player.AvatarUrl;
                            //                    _leaderXP = _topPlayer.Amount;
                            //                    _leaderRecords = _topPlayer.Entries;
                            //                }
                            //                catch (System.Exception)
                            //                {
                            //                    // logging
                            //                }
                            //            }
                            //        }
                            //    }

                            //}

                            var _embed = _embeds.Ship(ship, ctx.Client.CurrentUser);
                            await ctx.Message.RespondAsync(null, false, _embed);
                        }
                    }
                    else
                    {
                        Log.Error("{logAction}: unsuccessful API call: {@content}", _logAction, _content);
                        await ctx.Message.CreateReactionAsync(_services.FailEmoji);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "{logAction}: Error caught calling SCTM API: {message}", _logAction, ex.Message);
                }


            await ctx.Message.DeleteReactionsEmojiAsync(_services.SearchEmoji);
            var _duration = Math.Round((DateTime.Now - _dateStarted).TotalSeconds, 1);
            Log.Information("{logAction}: complete in {duration} secs", _logAction, _duration);
        }
    }
}
