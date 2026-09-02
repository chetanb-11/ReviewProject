using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace review2;

public class FilterChain<T>
{
    private Dictionary<string, Func<T, bool>> _filters = new();
    private Queue<string> _executionOrder = new();

    private Stack<string> _historyOfFilters = new();


    public List<string> FindCreditTransactions(IEnumerable<string>? list) 
    {  return list
            .Where(item => !string.IsNullOrEmpty(item) && Regex.IsMatch(item!, "credited", RegexOptions.IgnoreCase))
            .ToList();
            // ApplyFilter(FindCreditTransactions(list), "FindCreditTrasaction");
    }
    public List<string> FindDebitTransactions(IEnumerable<string>? list)
    { return list
            .Where(item => !string.IsNullOrEmpty(item) && Regex.IsMatch(item!, "debited", RegexOptions.IgnoreCase))
            .ToList();
            // ApplyFilter(FindDebitTransactions(list), "FindCreditTrasaction");
    }

    public string ApplyFilter(Func<T, bool> filter, string? name = null)
    {
        if (filter is null) throw new ArgumentNullException("Filter not found");
    
        var key = string.IsNullOrWhiteSpace(name) ? $"{_filters.Count + 1}" : name;
        if (!_filters.TryAdd(key, filter)) throw new ArgumentException($"A filter with name '{key}' already exists.", nameof(name));
    
        // _filters[key] = filter;
        _executionOrder.Enqueue(key);
        _historyOfFilters.Push(key);
    
        return key;
    }

    public void Undo()
    {
        if (_historyOfFilters.Count == 0) return;

        var last = _historyOfFilters.Pop();
        if (!_filters.Remove(last))
        {
            throw new Exception("filter doesn't exist");
        }
    }

    public IEnumerable<T> Execute(IEnumerable<T> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        var result = source;
        foreach (var name in _executionOrder)
        {
            if (_filters.TryGetValue(name, out var f))
            {
                result = result.Where(item => f(item));
            }
        }

        return result.ToList();
    }
    // clear funciton
}
