using Business.Interfaces;
using Data.Repositories.Interfaces;
using Entity;
using Entity.DTA;
using Entity.NUM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{
    public class RuletaBusiness : IRuletaBusiness
    {
        #region Cons
        private readonly int MIN_NUMERICAL_BET = 0;
        private readonly int MAX_NUMERICAL_BET = 36;
        private readonly string[] ALLOWED_COLOR_BET = new string[] { "ROJO", "NEGRO" };
        private readonly double MIN_MONEY_BET = 1;
        private readonly double MAX_MONEY_BET = 10000;
        #endregion
        #region Properties
        private readonly IRuletaRepository _repository;
        #endregion
        #region Constructor
        public RuletaBusiness(IRuletaRepository repository)
        {
            this._repository = repository;
        }
        #endregion
        #region Methods
        public async Task<string> CreateRouletteAsync()
        {
            Roulette roulette = new Roulette(GenerateId())
            {
                OpeningDate = DateTime.UtcNow,
                Bets = new List<Bet>(),
                Status = RouletteStatus.Created
            };
            roulette = await _repository.UpdateRoulette(roulette);

            return roulette.Id;
        }

        public async Task<string> OpenRouletteByIdAsync(string id)
        {
            var roulette = await _repository.GetRouletteById(id);
            if (roulette == null)
                throw new Exception("La Ruleta no existe");
            if (roulette.Status == RouletteStatus.Opened)
                throw new Exception("La Ruleta ya se encuentra abierta");
            if (roulette.Status == RouletteStatus.Closed)
                throw new Exception("La Ruleta ya se encuentra cerrada");
            roulette.Status = RouletteStatus.Opened;
            await _repository.UpdateRoulette(roulette);

            return "La ruleta ha sido abierta correctamente";
        }

        public async Task<string> CreateBet(BetRequest betRequest)
        {
            ValidateInputBet(betRequest);
            Bet bet;
            if (betRequest.Type == BetType.Numerical)
                bet = new NumericalBet(betRequest.User, DateTime.UtcNow, betRequest.Money, int.Parse(betRequest.Bet));
            else
                bet = new ColoBet(betRequest.User, DateTime.UtcNow, betRequest.Money, betRequest.Bet);
            var roulette = await _repository.GetRouletteById(betRequest.RouletteId);
            if (roulette == null)
                throw new Exception(string.Format("La Ruleta con id {0} no existe", betRequest.RouletteId));
            if (roulette.Status != RouletteStatus.Opened)
                throw new Exception("No se puede realizar la apuesta, la Ruleta no ha sido abierta");
            roulette.Bets.Add(bet);
            await _repository.UpdateRoulette(roulette);

            return "La apuesta se guardo correctamente";
        }

        public async Task<RouletteResult> ClosedRouletteById(string id)
        {
            var roulette = await _repository.GetRouletteById(id);
            if (roulette == null)
                throw new Exception("La Ruleta no existe");
            if (roulette.Status == RouletteStatus.Closed)
                throw new Exception("La Ruleta ya se encuentra cerrada");
            int randomNumber = GenerateRandomWinnerNumber();
            roulette.WinnerNumber = randomNumber;
            roulette.ClosingDate = DateTime.UtcNow;
            roulette.Status = RouletteStatus.Closed;
            foreach (var bet in roulette.Bets)
                bet.CheckWinner(randomNumber);
            await _repository.UpdateRoulette(roulette);

            return GetResultFromRoulette(roulette);
        }

        public async Task<List<RouletteDetail>> GetRoulettes()
        {
            var roulettes = await _repository.GetRoulettes();

            return roulettes
                .Select(g => new RouletteDetail { Id = g.Id, Status = ((RouletteStatus)g.Status).ToString() }).ToList();
        }

        private RouletteResult GetResultFromRoulette(Roulette roulette)
        {
            RouletteResult rouletteResult = new RouletteResult();
            rouletteResult.WinnerNumber = roulette.WinnerNumber;
            rouletteResult.Id = roulette.Id;
            rouletteResult.OpeningDate = string.Concat(roulette.OpeningDate.ToString("s"), "Z");
            rouletteResult.ClosingDate = string.Concat(roulette.ClosingDate.ToString("s"), "Z");
            foreach (var bet in roulette.Bets.Where(r => r.IsWinner))
                rouletteResult.WinningBets.Add(bet.GetResult());
            rouletteResult.TotalWinners = rouletteResult.WinningBets.Count;

            return rouletteResult;
        }

        private void ValidateInputBet(BetRequest betRequest)
        {
            if (!Enum.IsDefined(typeof(BetType), betRequest.Type))
                throw new Exception("No existe el tipo de apuesta. Los tipos de apuesta validos son 1: Numerico y 2: Color");
            if (betRequest.Money < MIN_MONEY_BET || betRequest.Money > MAX_MONEY_BET)
                throw new Exception(string.Format
                    ("El rango valido para el valor de la apuesta es de {0} a {1} dolares", MIN_MONEY_BET, MAX_MONEY_BET));
            if (betRequest.Type == BetType.Numerical)
            {
                if (!int.TryParse(betRequest.Bet, out int betNumber))
                    throw new Exception("El valor de la apuesta numerica no es valido");

                if (betNumber < MIN_NUMERICAL_BET || betNumber > MAX_NUMERICAL_BET)
                    throw new Exception(string.Format
                        ("El rango valido para apuesta numerica es de {0} a {1}", MIN_NUMERICAL_BET, MAX_NUMERICAL_BET));
            }
            else
            {
                if (!ALLOWED_COLOR_BET.Any(x => x == betRequest.Bet.ToUpper()))
                    throw new Exception(string.Format
                        ("El color de la apuesta no es valido. Los colores validos son: {0}", string.Join(",", ALLOWED_COLOR_BET)));
            }
        }

        private int GenerateRandomWinnerNumber()
        {
            Random random = new Random();

            return random.Next(0, 36);
        }

        private string GenerateId()
        {
            return Guid.NewGuid().ToString("N");
        }
        #endregion
    }
}
