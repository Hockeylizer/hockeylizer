using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace hockeylizer.Models
{
    public class PlayerVideo
    {
        private PlayerVideo() { }

        public PlayerVideo(string path, int playerId)
        {
            this.VideoPath = path;
            this.PlayerId = playerId;
        }

        [Key]
        public int VideoId { get; set; }
        public string VideoPath { get; set; }

        [ForeignKey("Player")]
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }
    }
}
