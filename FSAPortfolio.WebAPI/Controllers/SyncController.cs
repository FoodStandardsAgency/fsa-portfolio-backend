using FSAPortfolio.Entites;
using FSAPortfolio.Entites.Users;
using FSAPortfolio.PostgreSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace FSAPortfolio.WebAPI.Controllers
{
    public class SyncController : ApiController
    {
        // GET: api/Sync
        public string Get()
        {
            return "Success!";
        }

        // GET: api/Sync/SyncUsers
        [AcceptVerbs("GET")]
        public string SyncUsers()
        {
            List<string> messages = new List<string>() { "Syncing users...</br>" };

            using (var source = new MigratePortfolioContext())
            using (var dest = new PortfolioContext())
            {
                var sourceAccessGroups = source.users.Select(u => u.access_group)
                    .Distinct()
                    .Select(ag => new AccessGroup() { Name = ag.ToString() })
                    .ToList();

                // Ensure we have all access groups
                var accessGroupLookup = dest.AccessGroups.ToDictionary(ag => ag.Name);
                foreach (var sourceAccessGroup in sourceAccessGroups)
                {
                    var destAccessGroup = dest.AccessGroups.SingleOrDefault(dag => dag.Name == sourceAccessGroup.Name);
                    if(destAccessGroup == null)
                    {
                        destAccessGroup = sourceAccessGroup;
                        dest.AccessGroups.Add(destAccessGroup);
                        accessGroupLookup[sourceAccessGroup.Name] = destAccessGroup;
                        messages.Add($"Added access group {destAccessGroup.Name}");
                    }
                }

                // Sync the users
                foreach(var sourceUser in source.users)
                {
                    var destUser = dest.Users.SingleOrDefault(u => u.UserName == sourceUser.username);
                    if(destUser == null)
                    {
                        destUser = new User() { 
                            UserName = sourceUser.username,
                            PasswordHash = sourceUser.pass_hash,
                            Timestamp = sourceUser.timestamp,
                            AccessGroup = accessGroupLookup[sourceUser.access_group.ToString()]
                        };
                        dest.Users.Add(destUser);
                        messages.Add($"Added user {destUser.UserName}");
                    }
                }

                dest.SaveChanges();
                messages.Add("Sync users complete.");
            }

            return string.Join("\n", messages);
        }


    }
}
