using System;
using GameLogic.MDC.Gamedata.PlayerType;

namespace GameLogic.MDC.Gamedata.LevelContent
{
    public interface FieldType
    {
        Boolean CanBeAccessed();
        void Effects(Player player);
        void OnNextRound();
        Boolean IsHidden();
    }
}
