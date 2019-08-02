using System;

namespace MDC.Gamedata.Level
{
    class Level
    {
        private int _width{get; set;}
        private int _heigth{get; set;}
        Field[][] field;
        enum landscape
        {
            desert,
            iceland,
            forest,
            dungeon,
            island,
            asia
        }
    }
}