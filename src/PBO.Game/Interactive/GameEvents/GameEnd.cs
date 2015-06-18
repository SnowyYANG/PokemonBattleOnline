using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game.GameEvents
{
    [DataContract(Namespace = PBOMarks.JSON)]
    public class GameEnd : GameEvent
    {
        [DataMember(EmitDefaultValue = false)]
        public int Lose;

        public GameEnd(bool lose0, bool lose1)
        {
            if (lose1)
                if (lose0) Lose = 2;
                else Lose = 1;
        }

        protected override void Update()
        {
            int team0 = Game.Board.Teams[0].AliveCount;
            int team1 = Game.Board.Teams[1].AliveCount;
            if (Lose == 2)
            {
                AppendGameLog("GameResultTie", LogStyle.Center | LogStyle.Bold, 0, 1);
                if (team0 != 0) AppendGameLog("GameResult1", LogStyle.Center | LogStyle.Bold, team0, team1);
            }
            else
            {
                AppendGameLog(LogKeys.br, LogStyle.Center | LogStyle.Bold);
                AppendGameLog("GameResult0", LogStyle.Center | LogStyle.Bold, Lose == 0 ? 1 : 0);
                AppendGameLog("GameResult1", LogStyle.Center | LogStyle.Bold, team0, team1);
            }
            Game.EndGame();
        }
    }
}
