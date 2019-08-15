using System;
using MDC.Gamedata.PlayerType;
using MDC.Gamedata.LevelContent;

namespace MDC.Gamedata.LevelContent
{
    public class Field
    {
        private int _xPosition;
        private int _yPosition;

        public FieldType FieldType { get; set; }

        public Item Item 
        {
            get { return Item; }

            set
            {
                if (this.FieldType is Floor)
                {
                    Item = value;
                }
                else
                {
                    //items can only set on fieldtype floor
                    throw new System.ArgumentException();
                }

            }
        }


        public Player Reserve(){
            return null;
        }

        public int GetXPosition()
        {
            return _xPosition;
        }

          public int GetYPosition()
        {
            return _yPosition;
        }

       public Field(int xPosition, int yPosition, FieldType fieldType){
            this._xPosition = xPosition;
            this._yPosition = yPosition;
            this.FieldType = fieldType;
            Level.AddFieldToLevel(this);
        }
        
    }
}