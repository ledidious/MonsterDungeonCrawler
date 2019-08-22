using System;
using MDC.Gamedata.PlayerType;
using MDC.Gamedata.LevelContent;

namespace MDC.Gamedata.LevelContent
{
    public class Field
    {
        private int _xPosition;
        private int _yPosition;

        private FieldType _fieldType;

        public FieldType FieldType
        {
            get { return _fieldType; }

            set { _fieldType = value; }
        }

        private Item _item; 

        public Item Item
        {
            get { return _item; }

            set 
            {
                if (FieldType is Floor)
                {
                    _item = value; 
                }
                else
                {
                    throw new System.ArgumentException();
                } 
            }
        }

        public Player Reserve(){
            return null;
        }

        public int XPosition
        {
            get { return _xPosition; }

            set { _xPosition = value; }
        }

          public int YPosition
        {
            get { return _yPosition; }

            set { _yPosition = value; }
        }



       public Field(int xPosition, int yPosition, FieldType fieldType){
            this._xPosition = xPosition;
            this._yPosition = yPosition;
            this.FieldType = fieldType;

            Level.AddFieldToLevel(this);

            if (fieldType is Trap)
            {
                Level.AddTrapToList(this); 
            }
        }
        
    }
}