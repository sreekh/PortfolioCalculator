using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioCalculator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args != null && args.Length != 1)
                throw new ArgumentException("Path to input file not specified as argument.");
            string inputFilePath = args[0];
            List<Investment> providedInvestments = new List<Investment>();
            if (File.Exists(inputFilePath))
            {
                List<string> fileLines = File.ReadAllLines(inputFilePath).ToList();
                InvestmentFactoryProvider provider = new InvestmentFactoryProvider();
                fileLines.ForEach(investmentInput => providedInvestments.Add(provider.GetInvestmentFactory(Utilities.GetParametersFromInputString(investmentInput)).GetInvestment()));
                StringBuilder resultAccumulator = new StringBuilder();
                resultAccumulator.AppendLine(providedInvestments.Sum(i => i.Price).ToString());
                resultAccumulator.AppendLine(providedInvestments.Sum(i => i.CalculateDynamicPrice(new string[] { "0.02" })).ToString());
                resultAccumulator.AppendLine(providedInvestments.Sum(i => i.CalculateDynamicPrice(new string[] { "0.03" })).ToString());
                Console.Write(resultAccumulator.ToString());
                Console.ReadLine();

            }
        }
    }
    public static class Constants
    {
        public const char InputLineDelimiter = ' ';
    }
    public abstract class Investment
    {
        public double Price { get { return EvaluatePrice(); } }

        protected abstract double EvaluatePrice();
        public string[] Parameters { get; set; }
        public abstract double CalculateDynamicPrice(string[] criteria);
    }
    public class BondInvestment : Investment
    {
        public BondInvestment()
        {
            RiskFreeInterestRatePerAnnum = 0.01;
        }
        public double PrincipalBalance { get; set; }
        public double InterestRatePerAnnum { get; set; }
        public double MaturityInYears { get; set; }
        public double RiskFreeInterestRatePerAnnum { get; set; }

        protected override double EvaluatePrice()
        {
            return PrincipalBalance * (Math.Pow((1 + (InterestRatePerAnnum - RiskFreeInterestRatePerAnnum)), MaturityInYears));
        }
        public override double CalculateDynamicPrice(string[] criteria)
        {
            if (criteria != null && criteria.Length == 1)
                this.RiskFreeInterestRatePerAnnum = Convert.ToDouble(criteria[0]);
            return EvaluatePrice();
        }
    }
    public class EquityInvestment : Investment
    {
        public double CurrentStockPrice { get; set; }
        public double NumberOfShares { get; set; }
        protected override double EvaluatePrice()
        {
            return CurrentStockPrice * NumberOfShares;
        }
        public override double CalculateDynamicPrice(string[] criteria)
        {
            return EvaluatePrice();
        }
    }
    public class InvestmentFactoryProvider
    {
        public InvestmentFactory GetInvestmentFactory(string[] parameters)
        {
            InvestmentFactory result;
            if (parameters != null && parameters.Length > 0)
            {
                switch (parameters[0])
                {
                    case "E":
                        result = new EquityInvestmentFactory(parameters);
                        break;
                    case "B":
                        result = new BondInvestmentFactory(parameters);
                        break;
                    default:
                        throw new ArgumentException("Unknown Investment Type.");
                }
            }
            else
                throw new ArgumentException("Invalid arguments for Portfolio Calculator.");
            return result;
        }
    }
    public abstract class InvestmentFactory
    {
        public InvestmentFactory(string[] parameters)
        { this.Parameters = parameters; }
        public string[] Parameters { get; set; }
        public abstract Investment GetInvestment();
    }
    public class BondInvestmentFactory : InvestmentFactory
    {
        public BondInvestmentFactory(string[] parameters)
            : base(parameters) { }
        public override Investment GetInvestment()
        {
            //parameters - B B I M
            BondInvestment investment = new BondInvestment();
            if (Parameters != null && Parameters.Length != 4 && Parameters.ToList().Any(i => i == null))
                throw new ArgumentException("Invalid Arguments for Bond Investment");
            investment.PrincipalBalance = Convert.ToInt64(Parameters[1]);
            investment.InterestRatePerAnnum = Convert.ToDouble(Parameters[2]);
            investment.MaturityInYears = Convert.ToInt64(Parameters[3]);
            return investment;
        }
    }
    public class EquityInvestmentFactory : InvestmentFactory
    {
        public EquityInvestmentFactory(string[] parameters)
            : base(parameters) { }
        public override Investment GetInvestment()
        {
            //params - E N Q
            EquityInvestment investment = new EquityInvestment();
            if (Parameters != null && Parameters.Length != 3 && Parameters.ToList().Any(i => i == null))
                throw new ArgumentException("Invalid Arguments for Equity Investment");

            investment.NumberOfShares = Convert.ToDouble(Parameters[1]);
            investment.CurrentStockPrice = Convert.ToDouble(Parameters[2]);
            return investment;
        }
    }

    public static class Utilities
    {
        public static string[] GetParametersFromInputString(string input)
        {
            return input.Split(Constants.InputLineDelimiter);
        }
    }
}
