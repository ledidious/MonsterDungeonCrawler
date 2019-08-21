using System;
using System.Collections;
using System.Collections.Generic; 
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

        public static int GetMax_Clients()
        { return MAX_CLIENTS; }

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

        public static Boolean FieldBlockedByPlayer(int xPosition, int yPosition)
        {
            Boolean FieldIsBlocked = false; 

            for (int i = 0; i < playerList.Count; i++)
            {
                if (playerList[i].XPosition == xPosition && playerList[i].YPosition == yPosition)
                {
                    FieldIsBlocked = true; 
                }
                else
                {
                    //field is not blocked
                }
            }

            return FieldIsBlocked;
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