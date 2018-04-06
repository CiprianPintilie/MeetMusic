using Newtonsoft.Json;

namespace MeetMusicModels.InMemoryModels
{
    public class MusicFamilyUpdateModel
    {
        [JsonProperty("genre")]
        public string GenreName { get; set; }
        [JsonProperty("family")]
        public string FamilyName { get; set; }
    }
}