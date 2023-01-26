using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace RoSharp
{
    /// <summary>
    /// Class to represent the data returned from the Roblox Users API
    /// </summary>
    public class User
    {
        public string? description { get; set; }
        public string? created { get; set; }
        public bool isBanned { get; set; }
        public string? externalAppDisplayName { get; set; }
        public bool hasVerifiedBadge { get; set; }
        public int id { get; set; }
        public string? name { get; set; }
        public string? displayName { get; set; }
    }
    // INTERNAL USE ONLY
    class Username
    {
        public string? name { get; set; }
    }

    // INTERNAL USE ONLY
    class UsernameHistory
    {
        public string? previousPageCursor { get; set; }
        public string? nextPageCursor { get; set; }
        public List<Username>? data { get; set; }
    }
    /// <summary>
    /// Object in the data List of SearchResults <see cref="SearchResults"/>
    /// </summary>
    public class UserResult
    {
        public List<string>? previousUsernames { get; set; }
        public bool hasVerifiedbadge { get; set; }
        public int id { get; set; }
        public string? name { get; set; }
        public string? displayName { get; set; }
    }
    /// <summary>
    /// Class used to represent the results from the users/search API
    /// </summary>
    public class SearchResults
    {
        public string? previousPageCursor { get; set; }
        public string? nextPageCursor { get; set; }
        public List<UserResult>? data { get; set; }

    }
    
    public static class Users
    {
        /// <summary>
        /// Gets a user model based on the given ID
        /// </summary>
        /// <param name="id">The UserID To Find</param>
        /// <returns>A User Class</returns>
        public static async Task<User?> GetUser(int id)
        {
            RequestHandler handler = new RequestHandler();

            string result = await handler.Get(string.Format("https://users.roblox.com/v1/users/{0}", id));

            return JsonConvert.DeserializeObject<User>(result);
        }

        /// <summary>
        /// Gets the past usernames of a given user
        /// </summary>
        /// <param name="id">The ID of the account you wish to get the data from</param>
        /// <returns></returns>
        public static async Task<List<string>> GetUsernameHistory(int id)
        {
            RequestHandler handler = new RequestHandler();

            string result = await handler.Get(string.Format("https://users.roblox.com/v1/users/{0}/username-history?limit=10&sortOrder=Asc", id));

            UsernameHistory? usernameHistory = JsonConvert.DeserializeObject<UsernameHistory>(result);

            List<string> pastUsernamesList = new List<string>();
            
            foreach (Username user in usernameHistory.data)
            {
                pastUsernamesList.Add(user.name);
            }

            return pastUsernamesList;
        }
        /// <summary>
        /// Searches for users based on keyword similar to Roblox's search bar
        /// </summary>
        /// <param name="keyword">The search term</param>
        /// <param name="limit">How many results you want to show. Defaults to 10</param>
        /// <returns>A list of the Users</returns>
        public static async Task<List<UserResult>> SearchForUsers(string keyword, int limit = 10)
        {
            RequestHandler handler = new RequestHandler();

            string result = await handler.Get(string.Format("https://users.roblox.com/v1/users/search/?keyword={0}&limit={1}", keyword, limit));


            SearchResults? results = JsonConvert.DeserializeObject<SearchResults>(result);

            return results.data;
        }
    }

}
