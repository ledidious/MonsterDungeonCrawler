using System;
using System.Collections.Generic;
using GameLogic.MDC.Gamedata.PlayerType;

namespace GameLogic.MDC.Gamedata.LevelContent
{
    [Serializable]
    public class Level
    {
        protected const int WIDTH_DEFAULT = 20;
        protected const int HEIGHT_DEFAULT = 20;
        protected int _maxPlayer;
        private int _countMonster = 0;
        public Field[,] PlayingField;
        public List<Player> PlayerList = new List<Player>();
        public List<Field> TrapList = new List<Field>();
        private Boolean _keyOnField;
        public Dictionary<string, int[,]> StartingPoints = new Dictionary<string, int[,]>();

        public Boolean KeyOnField
        {
            get { return _keyOnField; }
            set => _keyOnField = value;
        }

        public Level(int maxPlayer)
        {
            _maxPlayer = maxPlayer;
            this.PlayingField = new Field[HEIGHT_DEFAULT, WIDTH_DEFAULT];
            this.StartingPoints = new Dictionary<string, int[,]>();
        }

        public Level(int maxPlayer, int levelSize)
        {
            _maxPlayer = maxPlayer;
            this.PlayingField = new Field[levelSize, levelSize];
            this.StartingPoints = new Dictionary<string, int[,]>();
        }

        /// <summary>
        /// Save the startingpositions for hero and monsters in startingpoints
        /// </summary>
        /// <param name="playerType">hero or monster</param>
        /// <param name="xPosition">x startingposition</param>
        /// <param name="yPosition">y startingposition</param>
        public void FillStartingPoint(string playerType, int xPosition, int yPosition)
        {
            if (playerType == "hero")
            {
                int[,] position = { { xPosition, yPosition } };
                this.StartingPoints.Add(playerType, position);
            }
            else if (playerType == "monster" && this._countMonster < this._maxPlayer - 1)
            {
                this._countMonster++;
                int[,] position = { { xPosition, yPosition } };
                this.StartingPoints.Add(playerType + this._countMonster, position);
            }
            else
            {
                throw new System.ArgumentOutOfRangeException("The maximum number of monsters is exceeded");
            }
        }

        /// <summary>
        /// Add a field to the level respectively to the playingfieldlist
        /// If the field is from fieldtype trap it will also added to the traplist
        /// </summary>
        /// <param name="field">Field to be added to the playingfieldlist</param>
        public void AddFieldToLevel(Field field)
        {
            PlayingField[field.XPosition, field.YPosition] = field;
            if (field.FieldType is Trap)
            {
                TrapList.Add(field);
            }
        }

        /// <summary>
        /// Add a player to the level respectively to the playerlist if the maximum number of players has not been reached
        /// </summary>
        /// <param name="player">Player to be added to the playerlist</param>
        public void AddPlayerToLevel(Player player)
        {
            if (PlayerList.Count <= _maxPlayer)
            {
                PlayerList.Add(player);
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

            for (int i = 0; i < PlayerList.Count; i++)
            {
                if (PlayerList[i].XPosition == xPosition && PlayerList[i].YPosition == yPosition)
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
