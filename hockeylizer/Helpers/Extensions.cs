using hockeylizer.Data;
using System;

namespace hockeylizer.Helpers
{
    public static class Extensions
    {
        public static Guid GetAvailableTeamId(this ApplicationDbContext db)
        {
            var teamId = new Guid();

            while (db.AppTeams.Find(teamId) != null)
            {
                teamId = new Guid();
            }

            return teamId;
        }
    }
}
