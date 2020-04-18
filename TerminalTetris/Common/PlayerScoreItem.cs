namespace TerminalTetris.Common
{
    public class PlayerScoreItem
    {
        public string Player { get; set; }
        public short Level { get; set; }
        public int Score { get; set; }
        public bool IsCurrentPlayer { get; set; }
    }
}