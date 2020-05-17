namespace Terminal.Tetris.Definitions
{
    public static class Constants
    {
        public const int ScreenWidth = 80;
        public const int ScreenHeight = 24;
        public const int MaxTopPlayers = 20;
        public const int LevelSpeedMultiplier = 50;
        public const int GlassWidth = 10;
        public const int GlassHeight = 20;
        public const int LinesNextLevelSwitch = 50;
        public const int GlassDeltaX = 27;
        public const int GlassDeltaY = 1;
        public const string Box = "▌";
        public const string BlockBox = "▌▌";
        public const string EmptyBox = " .";
        public const string GlassItem = "<! . . . . . . . . . .!>";
        public const string GlassBottom1 = "<!====================!>";
        public const string GlassBottom2 = @"  \/\/\/\/\/\/\/\/\/\/  ";
        public const string CurrentPlayer = "**";
    }
}