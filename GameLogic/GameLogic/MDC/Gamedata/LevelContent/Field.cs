using System;
namespace GameLogic.MDC.Gamedata.LevelContent
{
    [Serializable]
    public class Field
    {
        private int _xPosition;
        private int _yPosition;
        private FieldType _fieldType;
        private Item _item;

        public FieldType FieldType
        {
            get { return _fieldType; }
            set => _fieldType = value;
        }

        /// <summary>
        /// Property for getting and setting the item
        /// Set the item only if the fieldtype is a floor, because a wall or trap can't contain an item
        /// </summary>
        /// <value>Has to be a Item</value>
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
                    throw new System.ArgumentException("Only fieldtype floor can contain an item");
                }
            }
        }

        public int XPosition
        {
            get { return _xPosition; }
            set => _xPosition = value;
        }

        public int YPosition
        {
            get { return _yPosition; }
            set => _yPosition = value;
        }

        public Field(int xPosition, int yPosition, FieldType fieldType)
        {
            this._xPosition = xPosition;
            this._yPosition = yPosition;
            this.FieldType = fieldType;
        }
    }
}
