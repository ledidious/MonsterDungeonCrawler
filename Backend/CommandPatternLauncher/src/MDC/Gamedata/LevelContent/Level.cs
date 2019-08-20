using System;
using System.Collections;
using System.Collections.Generic; 
using MDC.Gamedata.LevelContent;
using MDC.Gamedata.PlayerType; 


namespace MDC.Gamedata.LevelContent
{
    public class Level
    {
        protected const int WIDTH = 20; 
        protected const int HIGHT = 20;
        private const int MAX_CLIENTS = 4;
         
        public static Field[,] playingField = new Field[WIDTH, HIGHT];

        public static List<Player> playerList = new List<Player>(); 
    
        public static void AddFieldToLevel(Field field) => playingField[field.XPosition, field.YPosition] = field;

        public static void AddPlayerToLevel(Player player)
        {
                if (playerList.Count <= MAX_CLIENTS)
                {
                    playerList.Add(player);     
                }
                else
                {
                    //too many player
                }
        }

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