using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _3ISIP224_Kadrgaliev
{
    public class Account
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    internal interface IAccountService
    {
        string Register(string userName, string password, string confirmation);
        Account Login(string userName, string password);
    }
    internal class Core : IAccountService
    {
        public static ShopDatabaseEntities Context = new ShopDatabaseEntities();
        public static readonly string AccountPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Account.json");
        private List<Account> ReadAccounts()
        {
            if (!File.Exists(AccountPath)) return new List<Account>();
            try
            {
                var accounts = JsonConvert.DeserializeObject<List<Account>>(File.ReadAllText(AccountPath));
                if (accounts == null)
                {
                    throw new InvalidDataException("В Account.json должен быть список аккаунтов.");
                }
                return accounts;
            }
            catch (JsonException)
            {
                throw new InvalidDataException("Файл Account.json повреждён. Его содержимое сохранено.");
            }
        }
        private void SaveAccounts(List<Account> accounts)
        {
            string temporaryPath = AccountPath + ".tmp";
            string json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
            File.WriteAllText(temporaryPath, json);
            if (File.Exists(AccountPath))
            {
                File.Replace(temporaryPath, AccountPath, AccountPath + ".bak", true);
            }
            else
            {
                File.Move(temporaryPath, AccountPath);
            }
        }
        public string Register(string userName, string password, string confirmation)
        {
            userName = (userName ?? "").Trim();
            if (userName.Length == 0 || userName.Length > 50) return "Ник должен содержать от 1 до 50 символов.";
            if (!Regex.IsMatch(userName, @"\A[A-Za-z]+\z")) return "В нике разрешены только латинские буквы A-Z и a-z.";
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8 || password.Length > 128) return "Пароль должен содержать от 8 до 128 символов.";
            if (password != confirmation) return "Пароли не совпадают.";
            var accounts = ReadAccounts();
            if (accounts.Any(account => string.Equals(account.UserName, userName, StringComparison.OrdinalIgnoreCase))) return "Этот ник уже занят.";
            accounts.Add(new Account
            {
                UserName = userName,
                Password = password
            });
            SaveAccounts(accounts);
            return "Регистрация завершена!";
        }
        public Account Login(string userName, string password)
        {
            userName = (userName ?? "").Trim();
            if (userName.Length == 0 || string.IsNullOrWhiteSpace(password)) return null;
            return ReadAccounts().FirstOrDefault(account => string.Equals(account.UserName, userName, StringComparison.OrdinalIgnoreCase) && account.Password == password);
        }
    }
}