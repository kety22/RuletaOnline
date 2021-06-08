using System.Collections.Generic;

namespace Entity.DTA
{
    public class RouletteResult
    {
        #region Properties
        public string Id { get; set; }
        public int WinnerNumber { get; set; }
        public int TotalWinners { get; set; }
        public string OpeningDate { get; set; }
        public string ClosingDate { get; set; }
        public List<BetResult> WinningBets { get; set; }
        #endregion

        #region Constructor
        public RouletteResult()
        {
            WinningBets = new List<BetResult>();
        }
        #endregion
    }
}
