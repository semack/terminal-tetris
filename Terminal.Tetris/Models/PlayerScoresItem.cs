using System.Text.Json.Serialization;

namespace Terminal.Tetris.Models
{
    public class PlayerScoresItem
    {
        public int Lines { get; set; }
        public short Level { get; set; }
        public int Score { get; set; }
    }
}