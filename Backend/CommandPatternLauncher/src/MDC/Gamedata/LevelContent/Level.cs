using System;
using System.Collections;
using MDC.Gamedata.LevelContent;

namespace MDC.Gamedata.LevelContent
{
    public class Level
    {
        protected const int WIDTH = 20; 
        protected const int HIGHT = 20;
         
        public static Field[,] playingField = new Field[WIDTH, HIGHT];

        public static void AddFieldToLevel(Field field) => playingField[field.XPosition, field.YPosition] = field;

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