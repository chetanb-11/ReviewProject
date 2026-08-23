using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewProject
{
    class Customer
    {
        public string Name { get; set; }

        public Customer(string name)
        {
            Name = name;
        }
    }

    abstract class Account
    {
        public string _accountNumber { get; protected set; }
        public double _balance { get; protected set; }
        public Customer customer { get; protected set; }    

        public Account(string accountNumber, string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accountNumber) || accountNumber.Length != 10)
                {
                    throw new ArgumentException("Enter a valid 10 digit account number.");
                }

                _accountNumber = accountNumber;
                customer = new Customer(name);
            } 
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine($"{ex.Message}");
            }
        }

        public virtual void depositAmount(string depositAmount)
        {
            try
            {
                int amount = int.Parse(depositAmount);
                if (amount <= 0)
                {
                    throw new ArgumentException("Enter a valid positive amount");
                }

                _balance = _balance + amount;
                Transactions.outputTransaction(_accountNumber, depositAmount, true);
            }
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine($"{ex.Message}");
            }
            catch (FormatException)
            {
                Console.Error.WriteLine("Please enter a valid numeric amount.");
            }
        }

        public virtual void withdrawAmount(string withdrawAmount)
        {
            try
            {
                int amount = int.Parse(withdrawAmount);
                if (amount <= 0)
                {
                    throw new ArgumentException("Enter a valid positive amount");
                }
                if (_balance - amount < 0)
                {
                    throw new ArgumentException("Insufficient balance in your account");
                }

                _balance = _balance - amount;
                Transactions.outputTransaction(_accountNumber, withdrawAmount, false);
            }
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine($"{ex.Message}");
            }
            catch (FormatException)
            {
                Console.Error.WriteLine("Please enter a valid numeric amount.");
            }
        }

        public virtual void displayAccountDetails()
        {
            Console.WriteLine($"Account Details:-");
            Console.WriteLine($"Account Holder: {customer.Name}");
            Console.WriteLine($"Account Number: {_accountNumber}");
            Console.WriteLine($"Current Balance: {_balance} INR");
        }
    }
     
    class SavingsAccount : Account
    {
        double _minBalance = 1000;

        public SavingsAccount(string name, string accountNumber, string balance) : base(accountNumber, name)
        {
            if (double.Parse(balance) < _minBalance)
            {
                throw new ArgumentException("Add minimum balance of 1000 INR to start a Savings Account");
            }
            _balance = double.Parse(balance);
        }

        public override void withdrawAmount(string withdrawAmount)
        {
            try
            {
                int amount = int.Parse(withdrawAmount);
                if (_balance - amount < _minBalance)
                {
                    Console.Error.WriteLine($"Cannot withdraw: Minimum balance of {_minBalance} INR must be maintained.");
                    return;
                }
                base.withdrawAmount(withdrawAmount);
            }
            catch (FormatException)
            {
                Console.Error.WriteLine("Please enter a valid numeric amount.");
            }
        }
    }

    class CurrentAccount : Account
    {
        double _minBalance = 5000;

        public CurrentAccount(string name, string accountNumber, string balance) : base(accountNumber, name)
        {
            if (double.Parse(balance) < _minBalance)
            {
                throw new ArgumentException("Add minimum balance of 5000 INR to start a Current Account");
            }
            _balance = double.Parse(balance);
        }

        // Current account display override
        public override void displayAccountDetails()
        {
            Console.WriteLine($"Current Account Details");
            Console.WriteLine($"Account Holder: {customer.Name}");
            Console.WriteLine($"Account Number: {_accountNumber}");
            Console.WriteLine($"Current Balance: {_balance} INR (Min Required: {_minBalance} INR)");
        }
    }

    static class Transactions
    {
        public static void outputTransaction(string accountNumber, string amount, bool deposit)
        {
            string transactionType = deposit ? "deposited into" : "withdrawn from";
            Console.WriteLine($"An amount of {amount} INR was {transactionType} account number {accountNumber}");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SAVINGS ACCOUNT");
            SavingsAccount cusAcc1 = new SavingsAccount("Sapna", "4567891321", "5000");
            cusAcc1.displayAccountDetails();

            Console.WriteLine("\nDepositing 20000 INR:");
            cusAcc1.depositAmount("20000");

            Console.WriteLine("\nWithdrawing 10000 INR:");
            cusAcc1.withdrawAmount("10000");

            Console.WriteLine("\nTrying to withdraw below minimum balance (15000 INR):");
            cusAcc1.withdrawAmount("15000");

            Console.WriteLine("\nFinal Savings Account Status:");
            cusAcc1.displayAccountDetails();

            Console.WriteLine("\nCURRENT ACCOUNT");
            CurrentAccount cusAcc2 = new CurrentAccount("Apex Corp", "9876543210", "10000");
            cusAcc2.displayAccountDetails();

            Console.WriteLine("\nDepositing 5000 INR:");
            cusAcc2.depositAmount("5000");

            Console.WriteLine("\nWithdrawing 8000 INR:");
            cusAcc2.withdrawAmount("8000");

            Console.WriteLine("\nFinal Current Account Status:");
            cusAcc2.displayAccountDetails();
        }
    }
}
