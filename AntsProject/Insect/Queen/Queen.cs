namespace AntsProject
{
    public class Queen : Insect
    {
        public string Name { get; private set; }
        public int Damage { get; private set; }
        public int MinGrowthCycle { get; private set; }
        public int MaxGrowthCycle { get; private set; }
        public int MaxQueens { get; private set; }
        public int LarvaeCount => _larvae.Count;
        public int NewLarvaeCount { get; private set; }
        public int NewlyGrownCount { get; private set; }

        private List<LarvaInfo> _larvae;
        private int _queensCreated;
        private readonly Random _random;
        private Colony _colony;

        // Структура для хранения информации о личинках
        private class LarvaInfo
        {
            public InsectType Type { get; set; }
            public int DaysToGrow { get; set; }
        }

        private enum InsectType
        {
            Worker,
            Warrior,
            Queen
        }

        public Queen(string name, int health, int defense, int damage, int minGrowthCycle, int maxGrowthCycle, int maxQueens) 
            : base("Queen", health, defense)
        {
            Name = name;
            Damage = damage;
            MinGrowthCycle = minGrowthCycle;
            MaxGrowthCycle = maxGrowthCycle;
            MaxQueens = maxQueens;
            _queensCreated = 0;
            _larvae = new List<LarvaInfo>();
            _random = new Random();
        }

        public void SetColony(Colony colony)
        {
            _colony = colony;
        }

        // Метод обработки ежедневной активности королевы
        public void ProcessDay()
        {
            NewLarvaeCount = 0;
            NewlyGrownCount = 0;

            // Обработка роста существующих личинок
            ProcessLarvaeGrowth();

            // Откладка новых личинок, только если нет растущих
            if (_larvae.Count == 0)
            {
                LayNewLarvae();
            }
        }

        private void ProcessLarvaeGrowth()
        {
            for (int i = _larvae.Count - 1; i >= 0; i--)
            {
                var larva = _larvae[i];
                larva.DaysToGrow--;

                if (larva.DaysToGrow <= 0)
                {
                    // Личинка выросла
                    CreateNewInsect(larva.Type);
                    _larvae.RemoveAt(i);
                    NewlyGrownCount++;
                }
            }
        }

        private void LayNewLarvae()
        {
            int larvaeCount = _random.Next(3, 7); // Случайное количество личинок
            for (int i = 0; i < larvaeCount; i++)
            {
                InsectType type;
                int typeRoll = _random.Next(100);

                // Определение типа новой личинки
                if (typeRoll < 60) // 60% шанс
                {
                    type = InsectType.Worker;
                }
                else if (typeRoll < 95 || _queensCreated >= MaxQueens) // 35% шанс
                {
                    type = InsectType.Warrior;
                }
                else // 5% шанс на королеву, если лимит не достигнут
                {
                    type = InsectType.Queen;
                }

                // Добавление новой личинки
                _larvae.Add(new LarvaInfo
                {
                    Type = type,
                    DaysToGrow = _random.Next(MinGrowthCycle, MaxGrowthCycle + 1)
                });
                NewLarvaeCount++;
            }
        }

        private void CreateNewInsect(InsectType type)
        {
            if (_colony == null) return;

            switch (type)
            {
                case InsectType.Worker:
                    CreateWorker();
                    break;
                case InsectType.Warrior:
                    CreateWarrior();
                    break;
                case InsectType.Queen:
                    CreateNewQueen();
                    break;
            }
        }

        private void CreateWorker()
        {
            // Случайный выбор типа рабочего согласно билету
            Worker newWorker = _random.Next(3) switch
            {
                0 => new EliteWorker(),
                1 => new AdvancedWorker(),
                _ => new AdvancedPickpocketWorker()
            };
            
            newWorker.Queen = this;
            _colony.AddWorker(newWorker);
        }

        private void CreateWarrior()
        {
            // Случайный выбор типа воина согласно билету
            Warrior newWarrior = _random.Next(3) switch
            {
                0 => new LegendaryWarrior(),
                1 => new SeniorWarrior(),
                _ => new RegularToughWarrior()
            };
            
            newWarrior.Queen = this;
            _colony.AddWarrior(newWarrior);
        }

        private void CreateNewQueen()
        {
            if (_queensCreated < MaxQueens)
            {
                // Создание новой королевы с похожими параметрами
                var newQueen = new Queen(
                    $"Princess_{Name}_{_queensCreated + 1}",
                    Health - _random.Next(2, 5), // Немного меньше здоровья
                    Defense - _random.Next(1, 3), // Немного меньше защиты
                    Damage - _random.Next(2, 5),  // Немного меньше урона
                    MinGrowthCycle,
                    MaxGrowthCycle,
                    MaxQueens
                );
                _queensCreated++;

                // Попытка основать новую колонию (50% шанс)
                if (_random.Next(2) == 0)
                {
                    var newColony = new Colony($"{_colony.Name}_New_{_queensCreated}", newQueen);
                    Game.AddNewColony(newColony); // Предполагается, что есть статический метод в Game
                }
            }
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Королева «{Name}»:");
            Console.WriteLine($"-- Параметры: здоровье={Health}, защита={Defense}, урон={Damage}");
            Console.WriteLine($"-- Цикл роста личинок: {MinGrowthCycle}-{MaxGrowthCycle} дней");
            Console.WriteLine($"-- Создано королев: {_queensCreated}/{MaxQueens}");
            Console.WriteLine($"-- Текущих личинок: {LarvaeCount}");
        }
    }
}