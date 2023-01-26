using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoSharp
{
    /// <summary>
    /// Represents the statistics of the badge such as win percentage
    /// </summary>
    public class BadgeStatistics
    {
        public int pastDayAwardCount { get; set; }
        public int awardedCount { get; set; }
        public double winRatePercentage { get; set; }
    }

    /// <summary>
    /// Represents what game the badge was awarded from
    /// </summary>
    public class BadgeAwardingUniverse
    {
        public long id { get; set; }
        public string? name { get; set; }
        public long rootPlaceId { get; set; }
    }

    /// <summary>
    /// Roblox badge represented in JSON
    /// </summary>
    public class JSONBadge
    {
        public long id { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public string? displayName { get; set; }
        public string? displayDescription { get; set; }
        public bool enabled { get; set; }
        public long iconImageId { get; set; }
        public string? created { get; set; }
        public string? updated { get; set; }

        public BadgeStatistics? statistics { get; set; }
        public BadgeAwardingUniverse? awardingUniverse { get; set; }

    }

    /// <summary>
    /// Class that represents a Roblox badge
    /// </summary>
    public class Badge
    {
        public long id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string displayName { get; set; }
        public string displayDescription { get; set; }
        public bool enabled { get; set; }
        public long iconImageId { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }

        public BadgeStatistics? statistics { get; set; }
        public BadgeAwardingUniverse? awardingUniverse { get; set; }

        public BadgeAwarder? awarder { get; set; }
        public Badge(long id, string name, string description, string displayName, string displayDescription, bool enabled, long iconImageId, string created, string updated, BadgeStatistics stats, BadgeAwardingUniverse universe)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.displayName = displayName;
            this.displayDescription = displayDescription;
            this.enabled = enabled;
            this.iconImageId = iconImageId;
            this.created = DateTime.Parse(created, null, System.Globalization.DateTimeStyles.RoundtripKind);
            this.updated = DateTime.Parse(updated, null, System.Globalization.DateTimeStyles.RoundtripKind);
            this.statistics = stats;
            this.awardingUniverse = universe;
            this.awarder = null;
        }
        public Badge(long id, string name, string description, string displayName, string displayDescription, bool enabled, long iconImageId, string created, string updated, BadgeStatistics stats, BadgeAwarder universe)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.displayName = displayName;
            this.displayDescription = displayDescription;
            this.enabled = enabled;
            this.iconImageId = iconImageId;
            this.created = DateTime.Parse(created, null, System.Globalization.DateTimeStyles.RoundtripKind);
            this.updated = DateTime.Parse(updated, null, System.Globalization.DateTimeStyles.RoundtripKind);
            this.statistics = stats;
            this.awarder = universe;
        }
    }

    /// <summary>
    /// Represents Roblox's global badge metadata
    /// </summary>
    public class BadgeMetadata
    {
        public int badgeCreationPrice { get; set; }
        public int maxBadgeNameLength { get; set; }
        public int maxBadgeDescriptionLength { get; set; }
    }

    public class BadgeAwarder
    {
        public long id { get; set; }
        public string? type { get; set; }
    }

    /// <summary>
    /// Represents a badge in a user's inventory as JSON
    /// </summary>
    public class JSONUserBadge
    {
        public long id { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public string? displayName { get; set; }
        public string? displayDescription { get; set; }
        public bool enabled { get; set; }
        public long iconImageId { get; set; }
        public long displayIconImageId { get; set; }
        public BadgeAwarder? awarder { get; set; }
        public BadgeStatistics? statistics { get; set; }
        public string? created { get; set; }
        public string? updated { get; set; }
    }

    public class UserBadges
    {
        public string? previousPageCursor { get; set; }
        public string? nextPageCursor { get; set; }

        public List<JSONUserBadge>? data { get; set; }
    }

    public static class Badges
    {
        /// <summary>
        /// Gets a Roblox badge based on the ID passed in
        /// </summary>
        /// <param name="id">ID of the Badge</param>
        /// <returns> <see cref="Badge"/></returns>
        public static async Task<Badge> GetBadge(int id)
        {
            RequestHandler handler = new RequestHandler();

            string result = await handler.Get(string.Format("https://badges.roblox.com/v1/badges/{0}", id));

            JSONBadge? JSONBadge = JsonConvert.DeserializeObject<JSONBadge>(result);

            Badge badge = new Badge(JSONBadge.id, JSONBadge.name, JSONBadge.description, JSONBadge.displayName, JSONBadge.displayDescription, JSONBadge.enabled, JSONBadge.iconImageId, JSONBadge.created, JSONBadge.updated, JSONBadge.statistics, JSONBadge.awardingUniverse);
            return badge;
        }

        /// <summary>
        /// Gets the global badge metadata on Roblox
        /// </summary>
        /// <returns> <see cref="BadgeMetadata"/></returns>
        public static async Task<BadgeMetadata> GetMetadata()
        {
            RequestHandler handler = new RequestHandler();

            string result = await handler.Get("https://badges.roblox.com/v1/badges/metadata");

            return JsonConvert.DeserializeObject<BadgeMetadata>(result);
        }

        /// <summary>
        /// This would've been the function to get all badges in a game but I can't seem to figure out how the Roblox API works with this. Help would be greatly appreciated
        /// </summary>
        /// <param name="gameID">Game ID</param>
        /// <param name="limit"> Limit of results. Defaults to 10</param>
        /// <returns>Nothing right now</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static async Task<List<Badge>> GetBadgesByGame(int gameID, int limit = 10)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a user's badges
        /// </summary>
        /// <param name="userID">The ID of the user you wish to retrieve these from</param>
        /// <param name="limit">How many results you want. Defaults to 10</param>
        /// <returns>A list of <see cref="Badge"/></returns>
        public static async Task<List<Badge>> GetBadgesByUser(int userID, int limit = 10)
        {
            RequestHandler handler = new RequestHandler();

            string result = await handler.Get(string.Format("https://badges.roblox.com/v1/users/{0}/badges?limit={1}&sortOrder=Asc", userID, limit));

            UserBadges? badges = JsonConvert.DeserializeObject<UserBadges>(result);

            List<Badge> badgeList = new List<Badge>();

            foreach (JSONUserBadge badge in badges.data)
            {
                Badge badge_obj = new Badge(badge.id, badge.name, badge.description, badge.displayName, badge.displayDescription, badge.enabled, badge.iconImageId, badge.created, badge.updated, badge.statistics, badge.awarder);
                badgeList.Add(badge_obj);
            }

            return badgeList;
        }
    }
}
