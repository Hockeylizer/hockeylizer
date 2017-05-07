using Hangfire.Dashboard;
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

    public class HangfireAuth : IDashboardAuthorizationFilter
	{
		public bool Authorize(DashboardContext context)
		{
			var httpContext = context.GetHttpContext();

			// Allow all authenticated users to see the Dashboard (potentially dangerous).
			return httpContext.User.Identity.IsAuthenticated;
		}
	}
}
