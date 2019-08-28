using System;
using System.Collections.Generic; 
using MDC.Gamedata.PlayerType; 


namespace MDC.Gamedata.LevelContent
{
    public class Level
    {
        protected const int WIDTH = 20; 
        protected const int HIGHT = 20;

        protected int _maxPlayer; 

        public Field[,] playingField = new Field[WIDTH, HIGHT];

        public List<Player> playerList = new List<Player>();

        public List<Field> trapList = new List<Field>(); 

        public Level(int maxPlayer)
        {
            _maxPlayer = maxPlayer; 
        }


        public void AddFieldToLevel(Field field)
        {
            playingField[field.XPosition, field.YPosition] = field;
            if (field.FieldType is Trap)
            {
                trapList.Add(field); 
            }
        }

        public void AddTrapToList(Field field) => trapList.Add(field); 

        public void AddPlayerToLevel(Player player)
        {
                if (playerList.Count <= _maxPlayer)
                {
                    playerList.Add(player);     
                }
                else
                {
                    //too many player
                }
        }

        public Boolean FieldBlockedByPlayer(int xPosition, int yPosition)
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