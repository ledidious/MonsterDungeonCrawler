using System;
namespace GameLogic.MDC.Gamedata.LevelContent
{
    [Serializable]
    public sealed class Key : Item
    {
        //Singleton
        private Key()
        {

        }

        private static volatile Key instance = null;

        /// <summary>
        /// Creates only one instance of a key object and set isvisible to false
        /// </summary>
        /// <returns>Key object</returns>
        public static Key GetInstance()
        {
            //Doublelock (threadsafety)
            if (instance == null)
                lock (m_lock) { if (instance == null) instance = new Key(); }
            //At the beginning the key is invisible
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
