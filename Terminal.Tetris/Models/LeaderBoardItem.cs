using System.Text.Json.Serialization;

namespace Terminal.Tetris.Models
{
    public class LeaderBoardItem
    {
        public string Player { get; set; }
        public short Level { get; set; }
        public int Score { get; set; }
        [JsonIgnore] public bool IsCurrentPlayer { get; set; }
    }
}