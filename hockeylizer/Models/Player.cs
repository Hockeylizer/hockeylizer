using System.Collections.Generic;

namespace hockeylizer.Models
{
    public class Player
    {
        public Player()
        {
            this.Videos = new HashSet<PlayerVideo>();
        }

        public Player(string name)
        {
            this.Name = name;
            this.Videos = new HashSet<PlayerVideo>();
        }

        public string RetrieveContainerName()
        {
            return this.Name.ToLower() + "-" + this.PlayerId;
        }

        public int PlayerId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PlayerVideo> Videos { get; set; }
    }
}
