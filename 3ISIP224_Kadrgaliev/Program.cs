using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace _3ISIP224_Kadrgaliev
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            ConsoleGame game = new ConsoleGame(new RandomSource());
            game.Run();
        }
    }
    public interface IRandomSource
    {
        int Next(int minInclusive, int maxExclusive);
        bool Chance(double probability);
    }
    public sealed class RandomSource : IRandomSource
    {
        private readonly Random random = new Random();
        public int Next(int minInclusive, int maxExclusive) { return random.Next(minInclusive, maxExclusive); }
        public bool Chance(double probability) { return random.NextDouble() < probability; }
    }
    public enum ItemRarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }
    public static class RarityRules
    {
        public static int GetDropChance(ItemRarity rarity)
        {
            switch (rarity)
            {
                case ItemRarity.Common: return 60;
                case ItemRarity.Rare: return 25;
                case ItemRarity.Epic: return 10;
                case ItemRarity.Legendary: return 5;
                default: throw new ArgumentOutOfRangeException(nameof(rarity));
            }
        }
        public static string GetName(ItemRarity rarity)
        {
            switch (rarity)
            {
                case ItemRarity.Common: return "Обычный";
                case ItemRarity.Rare: return "Редкий";
                case ItemRarity.Epic: return "Эпический";
                case ItemRarity.Legendary: return "Легендарный";
                default: return "Неизвестный";
            }
        }
    }
    public abstract class Equipment
    {
        public string Name { get; private set; }
        public int Value { get; private set; }
        public ItemRarity Rarity { get; private set; }
        protected Equipment(string name, int value, ItemRarity rarity)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Название не должно быть пустым.");
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
            if (!Enum.IsDefined(typeof(ItemRarity), rarity)) throw new ArgumentOutOfRangeException(nameof(rarity));
            Name = name;
            Value = value;
            Rarity = rarity;
        }
    }
    public sealed class Weapon : Equipment
    {
        public int Damage { get { return Value; } }
        public Weapon(string name, int damage, ItemRarity rarity = ItemRarity.Common)
            : base(name, damage, rarity) { }
    }
    public sealed class Armor : Equipment
    {
        public int Defense { get { return Value; } }
        public Armor(string name,int defense, ItemRarity rarity = ItemRarity.Common)
            : base(name, defense, rarity) { }
    }
    public static class ItemCatalog
    {
        public static List<Weapon> CreateWeapons()
        {
            return new List<Weapon>
            {
                new Weapon("Ржавый кинжал", 12, ItemRarity.Common),
                new Weapon("Стальной меч", 18, ItemRarity.Rare),
                new Weapon("Боевой топор", 22, ItemRarity.Epic),
                new Weapon("Рунный клинок", 28, ItemRarity.Legendary)
            };
        }
        public static List<Armor> CreateArmors()
        {
            return new List<Armor>
            {
                new Armor("Потрёпанная куртка", 4, ItemRarity.Common),
                new Armor("Кольчуга", 7, ItemRarity.Rare),
                new Armor("Стальные латы", 10, ItemRarity.Epic),
                new Armor("Рунный доспех", 12, ItemRarity.Legendary)
            };
        }
    }
    public abstract class Character
    {
        public string Name { get; private set; }
        public int MaxHealth { get; private set; }
        public int Health { get; private set; }
        public bool IsAlive { get { return Health > 0; } }
        public abstract int Attack { get; }
        public abstract int Defense { get; }
        protected Character(string name, int maxHealth)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Имя не должно быть пустым.");
            if (maxHealth <= 0) throw new ArgumentOutOfRangeException(nameof(maxHealth));
            Name = name;
            MaxHealth = maxHealth;
            Health = maxHealth;
        }
        public int TakeDamage(int damage)
        {
            if (damage < 0) throw new ArgumentOutOfRangeException(nameof(damage));
            int actualDamage = Math.Min(Health, damage);
            Health -= actualDamage;
            return actualDamage;
        }
        protected void RestoreHealth() { Health = MaxHealth; }
    }
    public sealed class Player : Character
    {
        public Weapon Weapon { get; private set; }
        public Armor Armor { get; private set; }
        public override int Attack { get { return Weapon.Damage; } }
        public override int Defense { get { return Armor.Defense; } }
        public Player()
            : this(100, new Weapon("Старый меч", 16), new Armor("Кожаный жилет", 5)) { }
        public Player(int health, Weapon weapon, Armor armor)
            : base("Искатель приключений", 100)
        {
            if (health < 0 || health > MaxHealth) throw new ArgumentOutOfRangeException(nameof(health));
            Equip(weapon);
            Equip(armor);
            TakeDamage(MaxHealth - health);
        }
        public void Equip(Weapon weapon)
        {
            if (weapon == null) throw new ArgumentNullException(nameof(weapon));
            Weapon = weapon;
        }
        public void Equip(Armor armor)
        {
            if (armor == null) throw new ArgumentNullException(nameof(armor));
            Armor = armor;
        }
        public int HealFully()
        {
            int restored = MaxHealth - Health;
            RestoreHealth();
            return restored;
        }
    }
    public enum EnemyRace
    {
        Goblin,
        Skeleton,
        Mage
    }
    public sealed class EnemyAttack
    {
        public int Damage { get; private set; }
        public bool IsCritical { get; private set; }
        public bool IgnoresArmor { get; private set; }
        public bool Freezes { get; private set; }
        public EnemyAttack(int damage, bool critical, bool ignoresArmor, bool freezes)
        {
            Damage = damage;
            IsCritical = critical;
            IgnoresArmor = ignoresArmor;
            Freezes = freezes;
        }
    }
    public abstract class Enemy : Character
    {
        private readonly int attack;
        private readonly int defense;
        public EnemyRace Race { get; private set; }
        public bool IsBoss { get; private set; }
        public override int Attack { get { return attack; } }
        public override int Defense { get { return defense; } }
        public virtual double CriticalChance { get { return 0; } }
        public virtual double FreezeChance { get { return 0; } }
        public virtual bool IgnoresArmor { get { return false; } }
        protected Enemy(string name, EnemyRace race, int health, int attack, int defense, bool isBoss)
            : base(name, health)
        {
            if (attack < 0 || defense < 0)
            {
                throw new ArgumentException("Характеристики не могут быть отрицательными.");
            }
            Race = race;
            IsBoss = isBoss;
            this.attack = attack;
            this.defense = defense;
        }
        public EnemyAttack CreateAttack(IRandomSource random)
        {
            bool critical = CriticalChance > 0 && random.Chance(CriticalChance);
            bool freezes = FreezeChance > 0 && random.Chance(FreezeChance);
            return new EnemyAttack(critical ? Attack * 2 : Attack, critical, IgnoresArmor, freezes);
        }
        protected static int Scale(int value, double multiplier)
        {
            return (int)Math.Round(value * multiplier, MidpointRounding.AwayFromZero);
        }
    }
    public class Goblin : Enemy
    {
        public const int BaseHealth = 30;
        public const int BaseAttack = 10;
        public const int BaseDefense = 3;
        public const double BaseCriticalChance = 0.20;
        public override double CriticalChance { get { return BaseCriticalChance; } }
        public Goblin() : this("Гоблин", 1, 1, 1, false) { }
        protected Goblin(string name,  double hp, double attack, double defense, bool boss)
            : base(name, EnemyRace.Goblin, Scale(BaseHealth, hp), Scale(BaseAttack, attack), Scale(BaseDefense, defense), boss) { }
    }
    public class Skeleton : Enemy
    {
        public const int BaseHealth = 38;
        public const int BaseAttack = 11;
        public const int BaseDefense = 4;
        public override bool IgnoresArmor { get { return true; } }
        public Skeleton() : this("Скелет", 1, 1, 1, false) { }
        protected Skeleton(string name, double hp, double attack, double defense, bool boss)
            : base(name, EnemyRace.Skeleton, Scale(BaseHealth, hp), Scale(BaseAttack, attack), Scale(BaseDefense, defense), boss) {}
    }
    public class Mage : Enemy
    {
        public const int BaseHealth = 26;
        public const int BaseAttack = 12;
        public const int BaseDefense = 2;
        public const double BaseFreezeChance = 0.20;
        public override double FreezeChance { get { return BaseFreezeChance; } }
        public Mage() : this("Маг", 1, 1, 1, false) { }
        protected Mage(string name, double hp, double attack, double defense, bool boss)
            : base(name, EnemyRace.Mage, Scale(BaseHealth, hp), Scale(BaseAttack, attack), Scale(BaseDefense, defense), boss) { }
    }
    public sealed class Vvg : Goblin
    {
        public override double CriticalChance { get { return BaseCriticalChance + 0.10; } }
        public Vvg() : base("ВВГ", 2.0, 1.5, 1.2, true) { }
    }
    public sealed class Kovalsky : Skeleton
    {
        public Kovalsky() : base("Ковальский", 2.5, 1.3, 1.4, true) { }
    }
    public sealed class Archmage : Mage
    {
        public override double FreezeChance { get { return BaseFreezeChance + 0.10; } }
        public Archmage() : base("Архимаг C++", 1.8, 1.6, 1.1, true) { }
    }
    public sealed class Pestov : Skeleton
    {
        public override double FreezeChance { get { return Mage.BaseFreezeChance + 0.15; } }
        public Pestov() : base("Пестов С--", 1.3, 1.8, 0.6, true) { }
    }
    public sealed class EnemyFactory
    {
        private readonly IRandomSource random;
        private readonly List<Func<Enemy>> regular =
            new List<Func<Enemy>>
            {
                () => new Goblin(),
                () => new Skeleton(),
                () => new Mage()
            };
        private readonly List<Func<Enemy>> bosses =
            new List<Func<Enemy>>
            {
                () => new Vvg(),
                () => new Kovalsky(),
                () => new Archmage(),
                () => new Pestov()
            };
        public EnemyFactory(IRandomSource random) { this.random = random;}
        public Enemy CreateRegular() { return regular[random.Next(0, regular.Count)](); }
        public Enemy CreateBoss() { return bosses[random.Next(0, bosses.Count)](); }
    }
    public sealed class AdventureTurn
    {
        public long Number { get; private set; }
        public Enemy Enemy { get; private set; }
        public bool IsChest { get { return Enemy == null; } }
        public AdventureTurn(long number, Enemy enemy)
        {
            Number = number;
            Enemy = enemy;
        }
    }
    public sealed class Adventure
    {
        private readonly IRandomSource random;
        private readonly EnemyFactory enemies;
        public long Turns { get; private set; }
        public Adventure(IRandomSource random, long completedTurns = 0)
        {
            if (completedTurns < 0 || completedTurns >= long.MaxValue) throw new ArgumentOutOfRangeException(nameof(completedTurns));
            this.random = random;
            enemies = new EnemyFactory(random);
            Turns = completedTurns;
        }
        public AdventureTurn NextTurn()
        {
            Turns++;
            if (Turns % 10 == 0)
            {
                return new AdventureTurn(Turns, enemies.CreateBoss());
            }
            return new AdventureTurn(Turns, random.Chance(0.5) ? null : enemies.CreateRegular());
        }
    }
    public abstract class Loot { }
    public sealed class HealingPotion : Loot { }
    public sealed class EquipmentLoot : Loot
    {
        public Equipment Equipment { get; private set; }
        public EquipmentLoot(Equipment equipment)
        {
            Equipment = equipment;
        }
    }
    public sealed class LootFactory
    {
        private readonly IRandomSource random;
        private readonly List<Weapon> weapons = ItemCatalog.CreateWeapons();
        private readonly List<Armor> armors = ItemCatalog.CreateArmors();
        public LootFactory(IRandomSource random)
        {
            this.random = random;
        }
        public Loot Create()
        {
            switch (random.Next(0, 3))
            {
                case 0: return new HealingPotion();
                case 1: return new EquipmentLoot(Pick(weapons));
                default: return new EquipmentLoot(Pick(armors));
            }
        }
        private T Pick<T>(List<T> items) where T : Equipment
        {
            T[] catalog = items.OrderBy(item => item.Rarity).ToArray();
            int totalChance = catalog.Sum(item => RarityRules.GetDropChance(item.Rarity));
            int roll = random.Next(0, totalChance);
            int currentChance = 0;
            foreach (T item in catalog)
            {
                currentChance += RarityRules.GetDropChance(item.Rarity);
                if (roll < currentChance) return item;
            }
            return catalog[catalog.Length - 1];
        }
    }
    public enum PlayerAction
    {
        Attack,
        Defend
    }
    public sealed class RoundResult
    {
        public bool PlayerSkipped { get; private set; }
        public bool PlayerAttacked { get; private set; }
        public int DamageToEnemy { get; private set; }
        public EnemyAttack EnemyAttack { get; private set; }
        public int DamageToPlayer { get; private set; }
        public bool Dodged { get; private set; }
        public int Blocked { get; private set; }
        public bool PlayerFrozen { get; private set; }
        public RoundResult(bool skipped, bool attacked, int damageToEnemy, EnemyAttack enemyAttack, int damageToPlayer, bool dodged, int blocked, bool frozen)
        {
            PlayerSkipped = skipped;
            PlayerAttacked = attacked;
            DamageToEnemy = damageToEnemy;
            EnemyAttack = enemyAttack;
            DamageToPlayer = damageToPlayer;
            Dodged = dodged;
            Blocked = blocked;
            PlayerFrozen = frozen;
        }
    }
    public sealed class Battle
    {
        private readonly Player player;
        private readonly Enemy enemy;
        private readonly IRandomSource random;
        public bool PlayerFrozen { get; private set; }
        public bool IsOver { get { return !player.IsAlive || !enemy.IsAlive; } }
        public Battle(Player player, Enemy enemy, IRandomSource random)
        {
            this.player = player;
            this.enemy = enemy;
            this.random = random;
        }
        public RoundResult PlayRound(PlayerAction action)
        {
            if (IsOver) throw new InvalidOperationException("Бой уже завершён.");
            if (!Enum.IsDefined(typeof(PlayerAction), action)) throw new ArgumentOutOfRangeException(nameof(action));
            bool skipped = PlayerFrozen;
            PlayerFrozen = false;
            bool defending = !skipped && action == PlayerAction.Defend;
            bool attacked = !skipped && action == PlayerAction.Attack;
            int outgoing = attacked ? enemy.TakeDamage(Math.Max(1, player.Attack - enemy.Defense)) : 0;
            if (!enemy.IsAlive)
            {
                return new RoundResult(skipped, attacked, outgoing, null, 0, false, 0, false);
            }
            EnemyAttack attack = enemy.CreateAttack(random);
            bool dodged = defending && random.Chance(0.40);
            int blocked = 0;
            int incoming = 0;
            if (!dodged)
            {
                int armor = attack.IgnoresArmor ? 0 : player.Defense;
                if (defending && !attack.IgnoresArmor)
                {
                    blocked = (int)(player.Defense * (random.Next(70, 101) / 100.0));
                }
                incoming = player.TakeDamage(Math.Max(0, attack.Damage - armor - blocked));
                PlayerFrozen = attack.Freezes && incoming > 0 && player.IsAlive;
            }
            return new RoundResult(skipped, attacked, outgoing, attack, incoming, dodged, blocked, PlayerFrozen);
        }
    }
    public sealed class GameSession
    {
        public Player Player { get; private set; }
        public Adventure Adventure { get; private set; }
        public long Victories { get; private set; }
        public GameSession(IRandomSource random)
            : this(random, new Player(), 0, 0) { }
        public GameSession(IRandomSource random, Player player, long turns, long victories)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            if (turns < 0 || victories < 0 || victories > turns)
            {
                throw new ArgumentException("Некорректные счётчики приключения.");
            }
            Player = player;
            Adventure = new Adventure(random, turns);
            Victories = victories;
        }
        public void RecordVictory()
        {
            Victories++;
        }
    }
    public sealed class SavedEquipment
    {
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }
        [JsonProperty(Required = Required.Always)]
        public int Value { get; set; }
        [JsonProperty(Required = Required.Always)]
        public ItemRarity Rarity { get; set; }
        public static SavedEquipment FromItem(Equipment item)
        {
            return new SavedEquipment
            {
                Name = item.Name,
                Value = item.Value,
                Rarity = item.Rarity
            };
        }
    }
    public sealed class SaveData
    {
        [JsonProperty(Required = Required.Always)]
        public int Version { get; set; }
        [JsonProperty(Required = Required.Always)]
        public int Health { get; set; }
        [JsonProperty(Required = Required.Always)]
        public long Turns { get; set; }
        [JsonProperty(Required = Required.Always)]
        public long Victories { get; set; }
        [JsonProperty(Required = Required.Always)]
        public SavedEquipment Weapon { get; set; }
        [JsonProperty(Required = Required.Always)]
        public SavedEquipment Armor { get; set; }
    }
    public sealed class SaveService
    {
        private readonly string path;
        private readonly JsonSerializerSettings settings =
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None,
                MissingMemberHandling = MissingMemberHandling.Error,
                CheckAdditionalContent = true,
                MaxDepth = 16,
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter
                    {
                        AllowIntegerValues = false
                    }
                }
            };
        public SaveService(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Не задан путь сохранения.");
            this.path = path;
        }
        public bool TrySave(GameSession session, out string error)
        {
            error = null;
            SaveData data = new SaveData
            {
                Version = 1,
                Health = session.Player.Health,
                Turns = session.Adventure.Turns,
                Victories = session.Victories,
                Weapon = SavedEquipment.FromItem(session.Player.Weapon),
                Armor = SavedEquipment.FromItem(session.Player.Armor)
            };
            if (!Validate(data, out error)) return false;
            string temporaryPath = path + ".tmp";
            try
            {
                string json = JsonConvert.SerializeObject(data, Formatting.Indented, settings);
                System.IO.File.WriteAllText(temporaryPath, json, Encoding.UTF8);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Replace(temporaryPath, path, null);
                }
                else
                {
                    System.IO.File.Move(temporaryPath, path);
                }
                return true;
            }
            catch (Exception exception)
                when (IsStorageError(exception))
            {
                error = "Не удалось записать сохранение. " + "Проверьте доступ к папке игры.";
                return false;
            }
            finally
            {
                try
                {
                    System.IO.File.Delete(temporaryPath);
                }
                catch (Exception exception)
                    when (IsStorageError(exception))
                {
                }
            }
        }
        public bool TryLoad(IRandomSource random, out GameSession session, out string error)
        {
            session = null;
            error = null;
            try
            {
                if (!System.IO.File.Exists(path))
                {
                    error = "Сохранение не найдено. " + "Начните новую игру.";
                    return false;
                }
                if (new System.IO.FileInfo(path).Length > 65536)
                {
                    error = "Файл сохранения слишком большой.";
                    return false;
                }
                string json = System.IO.File.ReadAllText(path, Encoding.UTF8);
                SaveData data = JsonConvert.DeserializeObject<SaveData>(json, settings);
                if (!Validate(data, out error))
                    return false;
                if (data.Health == 0)
                {
                    error ="Это приключение закончено: персонаж погиб. " + "Начните новую игру.";
                    return false;
                }

                Player player =
                    new Player(data.Health,new Weapon(data.Weapon.Name, data.Weapon.Value, data.Weapon.Rarity),
                    new Armor(data.Armor.Name, data.Armor.Value, data.Armor.Rarity));
                session = new GameSession(random, player, data.Turns, data.Victories);
                return true;
            }
            catch (JsonException)
            {
                error = "Сохранение повреждено " + "или имеет неизвестный формат.";
                return false;
            }
            catch (Exception exception)
                when (IsStorageError(exception))
            {
                error = "Не удалось прочитать сохранение. " + "Проверьте доступ к файлу.";
                return false;
            }
        }
        private static bool Validate(SaveData data, out string error)
        {
            error = "В сохранении обнаружены некорректные данные.";
            if (data == null || data.Version != 1 || data.Health < 0 || data.Health > 100 || data.Turns < 0 || data.Turns >= long.MaxValue || data.Victories < 0 || data.Victories > data.Turns)
            {
                return false;
            }
            List<Weapon> weapons = ItemCatalog.CreateWeapons();
            weapons.Add(new Weapon("Старый меч", 16));
            List<Armor> armors = ItemCatalog.CreateArmors();
            armors.Add(new Armor("Кожаный жилет", 5));
            if (!Matches(data.Weapon, weapons) || !Matches(data.Armor, armors))
            {
                return false;
            }
            error = null;
            return true;
        }
        private static bool Matches<T>(SavedEquipment saved, List<T> catalog) where T : Equipment
        {
            return saved != null && catalog.Any(item => item.Name == saved.Name && item.Value == saved.Value && item.Rarity == saved.Rarity);
        }
        private static bool IsStorageError(Exception exception)
        {
            return exception is System.IO.IOException || exception is UnauthorizedAccessException || exception is System.Security.SecurityException || exception is NotSupportedException || exception is ArgumentException;
        }
    }
    internal static class ConsoleUi
    {
        public static int? ReadChoice(int maxChoice)
        {
            while (true)
            {
                Console.Write("Ваш выбор: ");
                string input = Console.ReadLine();
                if (input == null) return null;
                int choice;
                if (int.TryParse(input, out choice) && choice >= 0 && choice <= maxChoice)
                {
                    return choice;
                }
                Console.WriteLine("Введите целое число от 0 до {0}.", maxChoice);
            }
        }
        public static void ShowPlayer(Player player)
        {
            Console.WriteLine("HP: {0}/{1} | Атака: {2} | Защита: {3}", player.Health, player.MaxHealth, player.Attack, player.Defense);
            Console.WriteLine("Оружие: " + Describe(player.Weapon));
            Console.WriteLine("Доспех: " + Describe(player.Armor));
        }
        public static string Describe(Equipment equipment)
        {
            string rarity = " [" + RarityRules.GetName(equipment.Rarity) + "]";
            if (equipment is Weapon)
            {
                return equipment.Name + rarity + " (атака " + equipment.Value + ")";
            }
            return equipment.Name + rarity + " (защита " + equipment.Value + ")";
        }
        public static void ShowEnemy(Enemy enemy)
        {
            string race = "Маг";
            if (enemy.Race == EnemyRace.Goblin) race = "Гоблин";
            else if (enemy.Race == EnemyRace.Skeleton) race = "Скелет";
            Console.WriteLine("{0}: {1} | Раса: {2}", enemy.IsBoss ? "БОСС" : "Враг", enemy.Name, race);
            Console.WriteLine("HP: {0}/{1} | Атака: {2} | Защита: {3}", enemy.Health, enemy.MaxHealth, enemy.Attack, enemy.Defense);
            if (enemy.CriticalChance > 0)
            {
                Console.WriteLine("Критический удар: {0:P0}, двойной урон.", enemy.CriticalChance);
            }
            if (enemy.IgnoresArmor)
            {
                Console.WriteLine("Игнорирует броню и блок. " + "От атаки можно уклониться.");
            }
            if (enemy.FreezeChance > 0)
            {
                Console.WriteLine("Заморозка: {0:P0}, пропуск следующего действия.", enemy.FreezeChance);
            }
        }
    }
    internal sealed class ConsoleGame
    {
        private readonly IRandomSource random;
        private readonly LootFactory loot;
        private readonly SaveService saves;
        public ConsoleGame(IRandomSource random)
        {
            this.random = random;
            loot = new LootFactory(random);
            saves = new SaveService(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"save.json"));
        }
        public void Run()
        {
            Console.WriteLine("=== ПОДЗЕМЕЛЬЕ ===");
            Console.WriteLine("Сундук или враг: 50/50. " + "Каждый 10-й ход — случайный босс.");
            Console.WriteLine("Зелья восстанавливают всё HP. " + "При HP = 0 вы погибаете.");
            while (true)
            {
                Console.WriteLine("\n1 — Новая игра (заменит сохранение)\n" + "2 — Загрузить прогресс\n" + "0 — Выход");
                int? choice = ConsoleUi.ReadChoice(2);
                if (!choice.HasValue || choice.Value == 0) break;
                GameSession session;
                if (choice.Value == 1)
                {
                    session = new GameSession(random);
                    SaveProgress(session, false);
                }
                else
                {
                    string error;
                    if (!saves.TryLoad(random, out session, out error))
                    {
                        Console.WriteLine(error);
                        continue;
                    }
                    Console.WriteLine("Прогресс загружен. Завершено ходов: {0}.", session.Adventure.Turns);
                }
                if (!PlayAdventure(session)) break;
            }
            Console.WriteLine("Игра завершена.");
        }
        private bool SaveProgress(GameSession session, bool showSuccess)
        {
            string error;
            if (!saves.TrySave(session, out error))
            {
                Console.WriteLine(error);
                return false;
            }
            if (showSuccess) Console.WriteLine("Прогресс сохранён.");
            return true;
        }
        private bool PlayAdventure(GameSession session)
        {
            Player player = session.Player;
            Adventure adventure = session.Adventure;
            while (player.IsAlive)
            {
                Console.WriteLine("\n----------------------------------------");
                ConsoleUi.ShowPlayer(player);
                Console.WriteLine("Пройдено ходов: {0}. Побед: {1}.", adventure.Turns, session.Victories);
                Console.WriteLine("1 — Следующий ход\n" + "2 — Сохранить прогресс\n" + "0 — Сохранить и выйти");
                int? choice = ConsoleUi.ReadChoice(2);
                if (!choice.HasValue || choice.Value == 0)
                {
                    SaveProgress(session, true);
                    return false;
                }
                if (choice.Value == 2)
                {
                    SaveProgress(session, true);
                    continue;
                }
                AdventureTurn turn = adventure.NextTurn();
                Console.WriteLine("\n=== ХОД {0} ===", turn.Number);
                if (turn.IsChest)
                {
                    if (!OpenChest(player)) return false;
                }
                else
                {
                    if (!Fight(player, turn.Enemy)) return false;
                    if (player.IsAlive) session.RecordVictory();
                }
                SaveProgress(session, false);
            }
            Console.WriteLine("\nВы погибли. Ходов: {0}. Побеждено врагов: {1}.", adventure.Turns, session.Victories);
            return true;
        }
        private bool Fight(Player player, Enemy enemy)
        {
            ConsoleUi.ShowEnemy(enemy);
            Battle battle = new Battle(player, enemy, random);
            while (!battle.IsOver)
            {
                Console.WriteLine("\nВаше HP: {0}/{1} | {2}: {3}/{4}", player.Health, player.MaxHealth, enemy.Name, enemy.Health, enemy.MaxHealth);
                PlayerAction action = PlayerAction.Attack;
                if (battle.PlayerFrozen)
                {
                    Console.WriteLine("Вы заморожены и пропускаете действие!");
                }
                else
                {
                    Console.WriteLine("1 — Атака\n" + "2 — Защита (40% уклонение; " + "иначе блок 70–100% защиты)\n" + "0 — Выйти (останется сохранение до боя)");
                    int? choice = ConsoleUi.ReadChoice(2);
                    if (!choice.HasValue || choice.Value == 0) return false;
                    action = choice.Value == 1 ? PlayerAction.Attack : PlayerAction.Defend;
                }
                RoundResult result = battle.PlayRound(action);
                if (result.PlayerAttacked)
                {
                    Console.WriteLine("Вы нанесли {0} урона.", result.DamageToEnemy);
                }
                else if (!result.PlayerSkipped)
                {
                    Console.WriteLine("Вы приготовились защищаться.");
                }
                if (result.EnemyAttack != null)
                {
                    if (result.Dodged)
                    {
                        Console.WriteLine("Вы полностью уклонились от атаки!");
                    }
                    else
                    {
                        if (result.EnemyAttack.IsCritical)
                        {
                            Console.WriteLine("Критический удар врага!");
                        }
                        if (result.Blocked > 0)
                        {
                            Console.WriteLine("Дополнительный блок: {0} ед.", result.Blocked);
                        }
                        Console.WriteLine("{0} нанёс {1} урона.", enemy.Name, result.DamageToPlayer);
                        if (result.PlayerFrozen)
                        {
                            Console.WriteLine("На вас наложена заморозка!");
                        }
                    }
                }
            }
            if (player.IsAlive)
            {
                Console.WriteLine("Победа! {0} повержен. Осталось HP: {1}/{2}.", enemy.Name, player.Health, player.MaxHealth);
            }
            return true;
        }
        private bool OpenChest(Player player)
        {
            Console.WriteLine("Вы нашли сундук!");
            Loot foundLoot = loot.Create();
            if (foundLoot is HealingPotion)
            {
                int restored = player.HealFully();
                Console.WriteLine("Лечебное зелье: восстановлено {0} HP. " + "Здоровье: {1}/{2}.", restored, player.Health, player.MaxHealth);
                return true;
            }
            Equipment found = ((EquipmentLoot)foundLoot).Equipment;
            Equipment current = found is Weapon ? (Equipment)player.Weapon : player.Armor;
            Console.WriteLine("Новый предмет: " + ConsoleUi.Describe(found));
            Console.WriteLine("Сейчас надето: " + ConsoleUi.Describe(current));
            Console.WriteLine("Изменение характеристики: {0:+0;-0;0}", found.Value - current.Value);
            Console.WriteLine("1 — Взять и заменить текущий предмет\n" + "0 — Выбросить находку");
            int? choice = ConsoleUi.ReadChoice(1);
            if (!choice.HasValue) return false;
            if (choice.Value == 1)
            {
                if (found is Weapon) player.Equip((Weapon)found);
                else player.Equip((Armor)found);
                Console.WriteLine( "Новый предмет экипирован.");
            }
            else
            {
                Console.WriteLine("Предмет выброшен.");
            }
            return true;
        }
    }
}