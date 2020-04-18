using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TerminalTetris.Common
{
    public class PlayerScoreItem
    {
        public string Player { get; set; }
        public short Level { get; set; }
        public int Score { get; set; }
        [JsonIgnore]
        public bool IsCurrentPlayer { get; set; }
    }
}