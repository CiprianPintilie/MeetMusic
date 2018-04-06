using MeetMusicModels.Models;

namespace MeetMusicModels.InMemoryModels
{
    public class MatchModel
    {
        public User User { get; set; }
        public double MatchScore { get; set; }
    }
}
