using System;

class Programm
{
    static void Main()
    {
        // определяем умения обоих игроков
        Skill[] skills = new Skill[]
        {
            new Skill(name: "Удар",
                      energyCost: 10,
                      isSelfCast: false,
                      healthChange: 20,
                      staminaChange: 0),

            new Skill(name: "Сильный удар",
                      energyCost: 40,
                      isSelfCast: false,
                      healthChange: 40,
                      staminaChange: 0),

            new Skill(name: "Попить",
                      energyCost: 0,
                      isSelfCast: true,
                      healthChange: 0,
                      staminaChange: 20),

            new Skill(name: "Поесть",
                      energyCost: 20,
                      isSelfCast: true,
                      healthChange: 25,
                      staminaChange: 0),
        };

        Player player1 = new Player("Игрок", skills);
        Player player2 = new Player("Вирус", skills);

        Play(player1, player2);
    }

    static void Play(Player p1, Player p2)
    {
        bool isGameOver = false;
        while (!isGameOver)
        {
            Console.Clear();
            // отображение статов и скиллов
            PrintStat(p1, p2);

            // определение победы или поражения
            if (IsGameOver(p1, p2)) break;

            // Получение действий от игрока
            PlayerTurn(p1, p2, false);
            // Описание работы скиллов игрока
            //LogAction(p1, p2);

            // Получение действий от противника
            PlayerTurn(p2, p1, true);
            // Описание работы скиллов противника
            //LogAction(p2, p1);

            Console.Write("===Продолжить (ENTER)===");
            Console.ReadLine();
        }

    }

    /// <summary>
    /// Ход игрока: выбирает умение из списка.
    /// Если стоит флаг isRandom, то умение выбираеся случайно программой
    /// (симулирует ИИ противника).
    /// </summary>
    /// <param name="activePlayer">Игрок, который делает ход</param>
    /// <param name="opponent">Игрок оппонент</param>
    /// <param name="isAi">Флаг случайного выбора умения</param>
    private static void PlayerTurn(Player activePlayer, Player opponent, bool isAi)
    {
        int skillCount = activePlayer.Skills.Length;

        int num = isAi ?
            GetAIAction(activePlayer) :
            GetPlayerAction(skillCount);

        activePlayer.UseSkill(
            activePlayer.Skills[num],
            opponent
        );
    }

    private static int GetAIAction(Player player)
    {
        int skillCount = player.Skills.Length;
        Random rnd = new Random();
        if (player.Stamina <= 10)
        {
            // use stamina recovery skill
            for (int i = 0; i < skillCount; i++)
            {
                if (player.Skills[i].IsSelfCast
                    && player.Skills[i].EnergyCost == 0)
                {
                    return i;
                }
            }

        }
        else if (player.Health <= 10)
        {
            // use heal
            for (int i = 0; i < skillCount; i++)
            {
                if (player.Skills[i].IsSelfCast
                    && player.Skills[i].HealthChange > 0)
                {
                    return i;
                }
            }
        }
        return rnd.Next(0, skillCount);
    }

    /// <summary>
    /// Считывает дейстиве от игрока из командной строки
    /// </summary>
    /// <param name="skillsCount">Число умений игрока</param>
    /// <returns></returns>
    private static int GetPlayerAction(int skillsCount)
    {
        Console.Write($"Выберите действие (1 - {skillsCount}): ");
        // проверяем корректность ввода
        bool success = int.TryParse(Console.ReadLine(), out int num)
            && num <= skillsCount
            && num > 0;
        // продолжаем запроашивать число, пока не введём правильное
        while (!success)
        {
            Console.WriteLine("Введено неверное число");
            Console.Write($"Выберите действие (1 - {skillsCount}): ");
            success = int.TryParse(Console.ReadLine(), out num)
                        && num <= skillsCount
                        && num > 0;
        }
        // для маппинга на массив с началом в 0, а игрок вводит числа от 1
        return num - 1;
    }

    /// <summary>
    /// Проверяем здоровье игроков. Если хотя бы у одного из игроков здоровье
    /// опустилось до 0 или ниже, то игра закончена.
    /// </summary>
    /// <param name="p1">Игрок 1</param>
    /// <param name="p2">Игрок 2</param>
    /// <returns>true если здоровье хотя бы одного из огроково стало 0 или меньше</returns>
    private static bool IsGameOver(Player p1, Player p2)
    {
        bool player1Lose = p1.Health <= 0;
        bool player2Lose = p2.Health <= 0;

        if (player1Lose && player2Lose)
        {
            Console.WriteLine("Ничья!");
        }
        else if (player1Lose)
        {
            Console.WriteLine($"{p2.Name} победил!");
        }
        else if (player2Lose)
        {
            Console.WriteLine($"{p1.Name} победил!");
        }

        return player1Lose || player2Lose;
    }

    /// <summary>
    /// Выводим жизни и энергию игроков, а также их умения.
    /// </summary>
    /// <param name="p1">Игрок 1</param>
    /// <param name="p2">Игрок 2</param>
    static void PrintStat(Player p1, Player p2)
    {
        Console.WriteLine($"Жизни {p1.Name}: {p1.Health}        Жизни {p2.Name}: {p2.Health}");
        Console.WriteLine($"Энергия {p1.Name}: {p1.Stamina}     Энергия {p2.Name}: {p2.Stamina}");
        Console.WriteLine();
        for (int i = 0; i < p1.Skills.Length; i++)
        {
            Console.WriteLine($"{i + 1}) {p1.Skills[i].GetNiceDescription()}");
        }
        Console.WriteLine();
    }
}

/// <summary>
/// Класс игрока.
/// </summary>
class Player
{
    /// <summary>
    /// Имя игрока
    /// </summary>
    readonly string name;

    /// <summary>
    /// Здороввье игрока
    /// </summary>
    int health = 100;

    /// <summary>
    /// Энергия игрока
    /// </summary>
    int stamina = 100;

    /// <summary>
    /// Умения игрока
    /// </summary>
    readonly Skill[] skills;

    public int Health { get => health; set => health = value; }
    public int Stamina { get => stamina; set => stamina = value; }
    internal Skill[] Skills { get => (Skill[])skills.Clone(); }
    public string Name { get => name; }

    public Player(string name, Skill[] skills)
    {
        this.name = name;
        this.skills = skills;
    }

    /// <summary>
    /// Проверяет сколько энергии стоит умение.
    /// Если энергрии не хватает, то игрок пропускает ход.
    /// Иначе уменьшает энергрию игрока на стоимость применения умения
    /// </summary>
    /// <param name="skill">Применяемое умение</param>
    /// <param name="opponent">Игрок оппонента</param>
    public void UseSkill(Skill skill, Player opponent)
    {
        Console.WriteLine();
        if (this.Stamina < skill.EnergyCost)
        {
            // если недостаточно энергии, то ход пропускается и выводим сообщение
            Console.WriteLine($"{this.Name} недостаточно энергии, чтобы " +
                $"применить навык {skill.Name}. Ход пропущен.");
            return;
        }
        else
        {
            // уменьшаем энергрию за примененеия навыка
            Console.WriteLine($"{this.Name} применил навык {skill.Name}");
            this.Stamina -= skill.EnergyCost;
        }

        // если навык для восполнения своих статов, то применяем на текущего игрока
        if (skill.IsSelfCast)
        {
            skill.Apply(this);
        }
        else // иначе на оппонента
        {
            skill.Apply(opponent);
        }
        Console.WriteLine();
    }
}

/// <summary>
/// Класс умения
/// </summary>
class Skill
{
    /// <summary>
    /// Название умения
    /// </summary>
    readonly string name;

    /// <summary>
    /// Затраты энергии игрока на использование умения
    /// </summary>
    readonly int energyCost = 0;

    /// <summary>
    /// Флаг определяет, является ли умение вспомогательным (применяется на себя)
    /// или на атакующим (применяется на оппонента).
    /// </summary>
    readonly bool isSelfCast = false;

    /// <summary>
    /// Изменения здоровья умением. Если умение вспомогательное, то значение
    /// прибавляется к здоровью. Если умение атакующее, то значение равно урону
    /// для здоровья опонента
    /// </summary>
    readonly int healthChange = 0;

    /// <summary>
    /// Изменение энергии умением. Если умение вспомогательное6 то значение
    /// прибавляется к энергрии. Если умение атакующее, то значение равно урону
    /// энергии опонента
    /// </summary>
    readonly int staminaChange = 0;

    public Skill(string name, int energyCost, bool isSelfCast, int healthChange, int staminaChange)
    {
        this.name = name;
        this.energyCost = energyCost;
        this.isSelfCast = isSelfCast;
        this.healthChange = healthChange;
        this.staminaChange = staminaChange;
    }

    public string Name { get => name; }
    public int EnergyCost { get => energyCost; }
    public bool IsSelfCast { get => isSelfCast; }
    public int HealthChange { get => healthChange; }
    public int StaminaChange { get => staminaChange; }

    /// <summary>
    /// Применяет умение к игроку. Если у умения стоит флаг isSelfCast, то
    /// умение восстанавливает жизни/энергию у игрока. Иначе снижает.
    /// </summary>
    /// <param name="player">Игрок к которому применяется умение</param>
    public void Apply(Player player)
    {
        if (isSelfCast)
        {
            IncreaseHealthIfNeed(player);
            IncreaseStaminaIfNeed(player);
        }
        else
        {
            DecreaseHealthIfNeed(player);
            DecreaseStaminaIfNeed(player);
        }
    }

    /// <summary>
    /// Уменьшает энергию игрока, если значение StaminaChange умения больше 0
    /// </summary>
    /// <param name="player">Игрок, которому уменьшается энергия</param>
    private void DecreaseStaminaIfNeed(Player player)
    {
        if (this.StaminaChange != 0)
        {
            player.Stamina -= this.StaminaChange;
            Console.WriteLine($"Энергия {player.Name} " +
                $"уменьшилась на: -{this.StaminaChange}");
        }
    }

    /// <summary>
    /// Уменьшает здоровье игрока, если значение HealthChange умения больше 0
    /// </summary>
    /// <param name="player">Игрок, которому уменьшается здоровье</param>
    private void DecreaseHealthIfNeed(Player player)
    {
        if (this.HealthChange != 0)
        {
            player.Health -= this.HealthChange;
            Console.WriteLine($"Здоровье {player.Name} " +
                $"уменьшилось на: -{this.HealthChange}");
        }
    }

    /// <summary>
    /// Увеличивает энергию игрока, если значение StaminaChange больше 0
    /// </summary>
    /// <param name="player">Игрок, которому увеличивают здоровье</param>
    private void IncreaseStaminaIfNeed(Player player)
    {
        if (this.StaminaChange != 0)
        {
            player.Stamina += this.StaminaChange;
            Console.WriteLine($"Энергия {player.Name} " +
                $"увеличилась на: +{this.StaminaChange}");
        }
    }

    /// <summary>
    /// Увеличивает здоровье игрока, если значение HealthChange больше 0
    /// </summary>
    /// <param name="player">Игрок, которому увеличивается здоровье</param>
    private void IncreaseHealthIfNeed(Player player)
    {
        if (this.HealthChange != 0)
        {
            player.Health += this.HealthChange;
            Console.WriteLine($"Здоровье {player.Name} " +
                $"увеличилось на: +{this.HealthChange}");
        }
    }

    /// <summary>
    /// Возвращает строку с описанием умения для вывода
    /// </summary>
    public string GetNiceDescription()
    {
        string res = this.Name + "(";
        if (isSelfCast)
        {
            if (this.healthChange > 0) res += $"+{this.healthChange} здоровья, ";
            if (this.staminaChange > 0) res += $"+{this.staminaChange} энергии";
            if (this.energyCost > 0) res += $"-{this.energyCost} энергии";
        }
        else
        {
            if (this.healthChange > 0) res += $"{this.healthChange} урона здоровью, ";
            if (this.staminaChange > 0) res += $"{this.staminaChange} урона энергии, ";
            if (this.energyCost > 0) res += $"-{this.energyCost} энергии";
        }
        res += ")";
        return res;
    }
}