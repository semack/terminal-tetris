using System.Text.Json.Serialization;

namespace Terminal.Tetris.Models
{
    public class PlayerScoreItem
    {
        public string Player { get; set; }
        public short Level { get; set; }
        public int Score { get; set; }
        [JsonIgnore] public int Lines { get; set; }
        [JsonIgnore] public bool IsCurrentPlayer { get; set; }
    }
}