using System;

namespace MDC.Gamedata.LevelContent
{
    public sealed class Key : Item  
    {
        //Singleton
        private Key()
        {

        }

        private static volatile Key instance = null; 

        public static Key getInstance()
        {   
            //Doublelock (threadsafety)
            if (instance == null)
                lock (m_lock) { if (instance == null) instance = new Key(); } 
                instance.IsVisible = false; 
            return instance;
        }

        private static Object m_lock = new Object(); 
       
        /// <summary>
        /// This method does not concern this class, because the duration is infinite
        /// </summary>
        /// <returns>True because the duration of key is infinite</returns>
        public override Boolean DecrementBoosterDuration()
        {
            return true; 
        }
    }
}