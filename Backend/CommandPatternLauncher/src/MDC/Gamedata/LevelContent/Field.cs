using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata.LevelContent
{
    public class Field
    {
        private int XPosition;
        private int YPosition;

        public FieldType _fieldType { get; set; }

        public Player reserve(){
            return null;
        }

        public int getXPosition()
        {
            return XPosition;
        }

          public int getYPosition()
        {
            return YPosition;
        }

       public Field(int xPosition, int yPosition, FieldType fieldType){
            this.XPosition = xPosition;
            this.YPosition = yPosition;
            this._fieldType = fieldType;
            Level.AddFieldToLevel(this);
        }
        
    }
}