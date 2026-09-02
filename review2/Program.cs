using System.Text.RegularExpressions;
using review2;

List<string> trasactions = new();
trasactions.AddRange([
    "500 debited to 321654987321 using credit card",
    "5620 credited from 654321987321 ",
    "1,250 debited to 987654321098 using credit card at Amazon",
    "450 debited to 123456789012 using credit card at Netflix",
    "5000 credited from 654321987321 ",
    "3,200 debited to 456123789012 using credit card at MakeMyTrip",
    "85 credited from 654321987321 ",
    "6800 credited from 456987123789 ",
]);

FilterChain<string> filterChainTransactions = new();
filterChainTransactions.ApplyFilter(s => Regex.IsMatch(s, "credited", RegexOptions.IgnoreCase));
filterChainTransactions.ApplyFilter(s => Regex.IsMatch(s, "654321987321", RegexOptions.IgnoreCase));
foreach (var transaction in filterChainTransactions.Execute(trasactions))
{
    Console.WriteLine(transaction);
}



List<int> numbers = new();
numbers.AddRange([54, 21, 65, 87, 65, 98, 323, 21, 10, 50, 80, 67]);
FilterChain<int> filterChain = new();
filterChain.ApplyFilter(num => num > 50, "filter50");

foreach (var num in filterChain.Execute(numbers))
{
    Console.WriteLine(num);
}

// Console.ReadKey();