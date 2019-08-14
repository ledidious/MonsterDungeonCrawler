using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata.LevelContent
{
    public class Field
    {
        private int XPosition;
        private int YPosition;

        public FieldType _fieldType { get; set; }

        public Item _item { get; set; }

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

        public Item Item
        {
            get { return _item; }

            set
            {
                if (this._fieldType is Floor)
                {
                    _item = value;
                    Level.itemList.Add(_item);
                }
                else
                {
                    //items can only set on fieldtype floor
                    throw new System.ArgumentException();
                }
            }
        }

       public Field(int xPosition, int yPosition, FieldType fieldType){
            this.XPosition = xPosition;
            this.YPosition = yPosition;
            this._fieldType = fieldType;
            Level.AddFieldToLevel(this);
        }
        
    }
}