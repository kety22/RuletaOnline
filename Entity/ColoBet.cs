using Entity.DTA;
using System;
namespace Entity
{
    public class ColoBet  : Bet
    {
        #region Properties
        public string Color { get; }
        #endregion
        #region Constructor
        public ColoBet(string user, DateTime date, double money, string color)
            : base(user, date, money)
        {
            this.Color = color;
        }
        #endregion
        #region Methods
        public override void CheckWinner(int randomWinnerNumber)
        {
            bool isEvenNumber = randomWinnerNumber % 2 == 0;
            bool isEvenColor = Color.Equals("ROJO");
            this.IsWinner = (isEvenNumber && isEvenColor) || (!isEvenNumber && !isEvenColor);
        }
        public override BetResult GetResult()
        {
            double moneyFactor = 1.8;

            return new BetResult()
            {
                BetType = "ColoBet",
                Bet = this.Color,
                MoneyBet = this.Money,
                MoneyWon = this.Money * moneyFactor,
                User = this.User
            };
        }
        #endregion
    }
}
