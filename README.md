# PortfolioCalculator
Primilinary Calculator of a portfolio consisting of different evaluation techniques


You work for a company that manages two types of investments, equities and bonds.  All monetary values are in EUR. The (simplified) formulas for computing the value of these investments are
Equity Pricing Formula:
P=NQ
Where 
P = the price, 
N = the number of shares owned (may be fractional) and 
Q = the current stock price.

Bond Pricing Formula:
P=B(1+ I-R)^M
Where
P = the price
B = the principal balance of the bond
I = the interest rate, in % per annum 
R = the current market risk-free interest rate, in % per annum.  Assume this is 1%.
M = the maturity of the bond, in years
Leveraging object oriented programming concepts (polymorphism in particular), write a C#.NET program that will read a description of the investment portfolio from a text file, the path of which is supplied as the first command-line argument, compute the value of the portfolio, and output this value to the standard output.  
The format of the input will be one line per investment.  The line has space-separated tokens.  For equity investments the first token will be the character E, the second will be N and the third will be Q. For bond investments the first token will be the character B and the tokens following will be B I and M, in that order.  % values are represented as fractions (1.0 = 100%).  Decimal and thousand separators will follow the system’s regional settings.  The input will be terminated by an empty line.
Example input
E 40 8.5
B 100 0.04 5

Example output
455.93

For additional credit, revise your program to output a portfolio price for three interest rate scenarios, where R = 1%, 2% or 3% on three lines.  Only the revised program needs to be submitted.  
Example revised output
455.93
450.41
445.10
