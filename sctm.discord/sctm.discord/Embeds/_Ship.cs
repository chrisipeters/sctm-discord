﻿using DSharpPlus.Entities;
using sctm.connectors.rsi.models;
using System;

namespace sctm.discord
{
    public partial class Embeds
    {
        public DiscordEmbed Ship(Ship ship, DiscordUser bot)
        {
            string _timeAgo = "";
            if ((DateTime.Now - ship.LastRead).TotalMinutes < 1) _timeAgo = "a few seconds ago";
            else if ((DateTime.Now - ship.LastRead).TotalMinutes >= 1 && (DateTime.Now - ship.LastRead).TotalMinutes <= 60) _timeAgo = Math.Round((DateTime.Now - ship.LastRead).TotalMinutes, 0) + " minutes ago";
            else _timeAgo = Math.Round((DateTime.Now - ship.LastRead).TotalHours, 0) + " hours ago";

            var _minCrew = 1;
            int.TryParse(ship.CrewMin.ToString(), out _minCrew);

            var _maxCrew = 1;
            int.TryParse(ship.CrewMax.ToString(), out _maxCrew);

            var _ret = new DiscordEmbedBuilder
            {
                Author = new DiscordEmbedBuilder.EmbedAuthor { IconUrl = ship.Manufacturer.LogoPath, Name = ship.Manufacturer.Name },
                Title = $"{ship.ModelName} - *[{ship.State}]*",
                Description = $"{ship.Description}  [RSI Link]({ship.RSILink})",
                ImageUrl = ship.ThumbnailPath,
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = ship.Manufacturer.LogoPath
                },
                Color = (ship.State.ToLower() == "flight ready") ? DiscordColor.Green : DiscordColor.Red,
                Footer = new DiscordEmbedBuilder.EmbedFooter { Text = $"Star Citizen Tools by SC TradeMasters - Read from RSI {_timeAgo}", IconUrl = bot.AvatarUrl }
            }
            .AddField($"{ship.Size}: {_minCrew}-{_maxCrew} crew", ((ship.Focus == null || ship.Focus.Trim().Length == 0) ? "n/a" : ship.Focus), true)
            .AddField($"Cargo Capacity: {ship.CargoCapacity ?? 0}", $"{ship.Length ?? 0}x{ship.Beam ?? 0}x{ship.Height ?? 0} LBH", true)
            ;


            return _ret;
        }
    }
}
