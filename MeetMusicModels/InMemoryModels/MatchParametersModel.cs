using Newtonsoft.Json;

namespace MeetMusicModels.InMemoryModels
{
    public class MatchParametersModel
    {
        [JsonProperty("gender")]
        public int Gender { get; set; }
    }
}
