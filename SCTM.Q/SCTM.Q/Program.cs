using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using SCTM.Q.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SCTM.Q
{
    class Program
    {
        private DiscordSocketClient _client;

        public static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;

            // Remember to keep token private or to read it from an 
            // external source! In this case, we are reading the token 
            // from an environment variable. If you do not know how to set-up
            // environment variables, you may find more information on the 
            // Internet or by using other methods such as reading from 
            // a configuration.
            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("SCTM_DiscordToken"));
            await _client.StartAsync();


            _client.MessageReceived += MessageReceived;

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Content == "!ping")
            {
                await message.Channel.SendMessageAsync("Pong!");
            }

            if (message.Content.Trim().ToLower().StartsWith("!ships"))
            {
                var _commandArray = message.Content.Trim().Split(' ');
                if (_commandArray.Length > 1)
                {
                    HttpClient _client = new HttpClient();
                    var result = await _client.GetAsync($"https://localhost:44325/data/ships");
                    var content = await result.Content.ReadAsStringAsync();
                    var _ships = JsonConvert.DeserializeObject<List<Ship>>(content);

                    if (_commandArray.Length == 2)
                    {
                        // assume manufacturer or model passed
                        _ships = _ships.Where(i =>
                        (i.MatchingStrings.Select(o => o.Value.ToLower()).Contains(_commandArray[1].ToLower().Trim())) || i.Model.ToLower().Contains(_commandArray[1].ToLower())
                        || (i.Manufacturer.MatchingStrings.Select(o => o.Value.ToLower()).Contains(_commandArray[1].ToLower().Trim()))
                        ).ToList();


                    }
                    else if (_commandArray.Length == 3)
                    {
                        // assume manufacturer and model passed
                        _ships = _ships.Where(i =>
                        i.MatchingStrings.Select(o => o.Value.ToLower()).Contains(_commandArray[2].ToLower().Trim())
                        || i.Manufacturer.MatchingStrings.Select(o => o.Value.ToLower()).Contains(_commandArray[1].ToLower().Trim())
                        ).ToList();
                    }

                    if (_ships.Any())
                    {
                        foreach (var ship in _ships)
                        {
                            var _random = new Random();

                            #region image check

                            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(ship.ThumbnailPath);
                            request.Method = "HEAD";

                            bool _imageExists;
                            try
                            {
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

                            await message.Channel.SendMessageAsync("", false, _ret.Build());
                        }
                    }

                }
            }

            if (message.Content.Trim().ToLower().StartsWith("!hello"))
            {
                string _code = null;
                try { _code = message.Content.Trim().Split(' ')[1]; } catch { }

                if (_code == null) return;

                var _userId = message.Author.Id;
                var _username = message.Author.Username;
                var _disc = message.Author.Discriminator;

                var _ret = new EmbedBuilder();
                _ret.WithTitle($"Welcome {_username}");
                _ret.Fields = new List<EmbedFieldBuilder>
                    {
                        new EmbedFieldBuilder{
                            IsInline = true,
                            Name = "Id",
                            Value = _userId
                        },
                        new EmbedFieldBuilder{
                            IsInline = true,
                            Name = "Username",
                            Value = _username
                        },
                        new EmbedFieldBuilder
                        {
                            IsInline = false,
                            Name = "Description",
                            Value = _disc
                        }
                    };

                await message.Channel.SendMessageAsync("", false, _ret.Build());
            }
        }
    }

    public class whoIsResult
    {
        public bool IsRegistered { get; set; }
        public string RegistrationStatus { get; set; }
        public bool RSIExists { get; set; }
        public string ThumbnailPath { get; set; }
        public string Message { get; set; }
    }
}
