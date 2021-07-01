using FSAPortfolio.Entities.Users;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Microsoft
{
    public static class GroupMembershipToRoleMap
    {
        private const string SettingKeyRolePattern = "GroupMemberMap.(?<role>.*)";

        /// <summary>
        /// Map: {ActiveDirectory Group ID} -> {List of Role objects}
        /// </summary>
        public static Dictionary<string, List<Role>> GroupMap { get; set; }

        /// <summary>
        /// App settings should a semicolon separated list of Active Directory object Ids with the form: <add key="GroupMemberMap.{portfolio}.{role}" value="{objectId};{objectId}"/>
        /// </summary>
        static GroupMembershipToRoleMap()
        {
            var mapSettings = ConfigurationManager.AppSettings.AllKeys
                .Select(k => new { Key = k, Match = Regex.Match(k, SettingKeyRolePattern) })
                .Where(s => s.Match.Success);

            GroupMap = new Dictionary<string, List<Role>>();

            foreach (var map in mapSettings)
            {
                List<Role> roles;
                var roleKey = map.Match.Groups["role"].Value; // e.g. roleKey = "ODD.Admin"
                var objectIds = ConfigurationManager.AppSettings[map.Key].Split(';');
                foreach (var objectId in objectIds)
                {
                    if (!GroupMap.TryGetValue(objectId, out roles))
                    {
                        roles = new List<Role>();
                        GroupMap[objectId] = roles;
                    }
                    roles.Add(new Role(roleKey));
                }
            }
        }
    }
}