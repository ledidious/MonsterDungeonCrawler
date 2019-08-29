using System;
using System.Collections.Generic; 
using MDC.Gamedata.PlayerType; 


namespace MDC.Gamedata.LevelContent
{
    [Serializable]
    public class Level
    {
        protected const int WIDTH = 20; 
        protected const int HIGHT = 20;
        protected int _maxPlayer; 
        public Field[,] playingField = new Field[WIDTH, HIGHT];
        public List<Player> playerList = new List<Player>();
        public List<Field> trapList = new List<Field>(); 
        private Boolean _keyOnField; 

        public Boolean KeyOnField
        {
            get { return _keyOnField; }
            set => _keyOnField = value; 
        }

        public Level(int maxPlayer)
        {
            _maxPlayer = maxPlayer; 
        }

        /// <summary>
        /// Add a field to the level respectively to the playingfieldlist
        /// If the field is from fieldtype trap it will also added to the traplist
        /// </summary>
        /// <param name="field">Field to be added to the playingfieldlist</param>
        public void AddFieldToLevel(Field field)
        {
            playingField[field.XPosition, field.YPosition] = field;
            if (field.FieldType is Trap)
            {
                trapList.Add(field);  
            }
        }

        /// <summary>
        /// Add a player to the level respectively to the playerlist if the maximum number of players has not been reached
        /// </summary>
        /// <param name="player">Player to be added to the playerlist</param>
        public void AddPlayerToLevel(Player player)
        {
                if (playerList.Count <= _maxPlayer)
                {
                    playerList.Add(player);     
                }
                else
                {
                    //Too many player
                }
        }

        /// <summary>
        /// Verified if a field is blocked by another player
        /// </summary>
        /// <param name="xPosition">X-Positon of the targetfield</param>
        /// <param name="yPosition">Y-Position of the targetfield</param>
        /// <returns>Returns true if the field is blocked and false if the field is not blocked by another player</returns>
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
                    //Field is not blocked
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