using System;
using Entity.DTA;

namespace Entity
{
    public class NumericalBet : Bet
    {
        #region Properties
        public int Number { get; }
        #endregion

        #region Constructor
        public NumericalBet(string user, DateTime date, double money, int number)
            : base(user, date, money)
        {
            this.Number = number;
        }
        #endregion

        #region Methods
        public override void CheckWinner(int randomWinnerNumber)
        {
            this.IsWinner = this.Number == randomWinnerNumber;
        }

        public override BetResult GetResult()
        {
            double moneyFactor = 5;

            return new BetResult()
            {
                BetType = "NumericalBet",
                Bet = this.Number.ToString(),
                MoneyBet = this.Money,
                MoneyWon = this.Money * moneyFactor,
                User = this.User
            };
        }
        #endregion
    }
}
