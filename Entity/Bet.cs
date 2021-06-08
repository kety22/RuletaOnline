using Entity.DTA;
using System;

namespace Entity
{
    public abstract class Bet
    {
        #region Properties
        public string User { get; }
        public DateTime Date { get; }
        public double Money { get; }
        public bool IsWinner { get; set; }
        #endregion
        #region Constructor
        public Bet(string user, DateTime date, double money)
        {
            this.User = user;
            this.Date = date;
            this.Money = money;
        }

        public abstract void CheckWinner(int randomWinnerNumber);
        public abstract BetResult GetResult();
        #endregion
    }
}
