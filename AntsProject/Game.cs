namespace AntsProject
{
    public class Game
    {
        private List<Colony> _colonies;
        private List<ResourcePile> _resourcePiles;
        private int _daysUntilDrought;
        private LegendaryMythicalAnt _legendaryAnt;
        private static Game _instance;
        private Random Random;

        public static Game Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Game();
                return _instance;
            }
        }

        private Game()
        {
            _colonies = new List<Colony>();
            _resourcePiles = new List<ResourcePile>();
            _daysUntilDrought = 15;
            _legendaryAnt = new LegendaryMythicalAnt();
            Random = new Random();
        }

        public static void AddNewColony(Colony colony)
        {
            Instance._colonies.Add(colony);
        }

        public void Initialize()
        {
            // Инициализация куч ресурсов
            _resourcePiles.Add(new ResourcePile(1, new Dictionary<string, int>
            {
                { "веточка", 39 }, { "листик", 35 }, { "росинка", 34 }
            }));
            _resourcePiles.Add(new ResourcePile(2, new Dictionary<string, int>
            {
                { "веточка", 40 }, { "росинка", 45 }
            }));
            _resourcePiles.Add(new ResourcePile(3, new Dictionary<string, int>
            {
                { "веточка", 32 }, { "листик", 30 }, { "камушек", 44 }, { "росинка", 27 }
            }));
            _resourcePiles.Add(new ResourcePile(4, new Dictionary<string, int>
            {
                { "веточка", 37 }, { "листик", 21 }, { "росинка", 35 }
            }));
            _resourcePiles.Add(new ResourcePile(5, new Dictionary<string, int>
            {
                { "веточка", 28 }
            }));

            InitializeRedColony();
            InitializeGingerColony();
        }

        private void InitializeRedColony()
        {
            var queen = new Queen("Маргрете", 22, 7, 20, 2, 4, 4);
            var colony = new Colony("красные", queen);

            // Добавление рабочих
            for (int i = 0; i < 4; i++)
                colony.AddWorker(new EliteWorker());
            for (int i = 0; i < 4; i++)
                colony.AddWorker(new AdvancedWorker());
            for (int i = 0; i < 5; i++)
                colony.AddWorker(new AdvancedPickpocketWorker());

            // Добавление воинов
            for (int i = 0; i < 3; i++)
                colony.AddWarrior(new LegendaryWarrior());
            for (int i = 0; i < 2; i++)
                colony.AddWarrior(new SeniorWarrior());
            for (int i = 0; i < 3; i++)
                colony.AddWarrior(new RegularToughWarrior());

            // Добавление особого насекомого
            colony.AddSpecialInsect(new Dragonfly());

            _colonies.Add(colony);
        }

        private void InitializeGingerColony()
        {
            var queen = new Queen("Екатерина", 29, 6, 28, 3, 5, 4);
            var colony = new Colony("рыжие", queen);

            // Добавление рабочих
            for (int i = 0; i < 6; i++)
                colony.AddWorker(new LegendaryWorker());
            for (int i = 0; i < 6; i++)
                colony.AddWorker(new AdvancedWorker());
            for (int i = 0; i < 6; i++)
                colony.AddWorker(new AdvancedForemanWorker());

            // Добавление воинов
            for (int i = 0; i < 3; i++)
                colony.AddWarrior(new RegularWarrior());
            for (int i = 0; i < 3; i++)
                colony.AddWarrior(new LegendaryWarrior());
            for (int i = 0; i < 3; i++)
                colony.AddWarrior(new AdvancedHunterWarrior());

            // Добавление особого насекомого
            colony.AddSpecialInsect(new Bumblebee());

            _colonies.Add(colony);
        }

        public void RunSimulation()
        {
            Console.WriteLine("Начало симуляции\n");

            for (int day = 1; day <= _daysUntilDrought && _colonies.Count > 0; day++)
            {
                Console.WriteLine($"\nДень {day} (до засухи осталось {_daysUntilDrought - day} дней):\n");

                // Обработка легендарного муравья в первый день
                if (day == 1)
                {
                    _legendaryAnt.ApplyEffect(_colonies);
                }

                // Обработка дня для каждой колонии
                var activeColonies = _colonies.Where(c => !c.IsDestroyed).ToList();
                foreach (var colony in activeColonies)
                {
                    var enemies = activeColonies.Where(c => c != colony).ToList();
                    colony.ProcessDay(_resourcePiles, enemies);
                }

                // Вывод статуса
                DisplayStatus();

                // Проверка на окончание симуляции
                if (_colonies.All(c => c.IsDestroyed))
                {
                    Console.WriteLine("\nВсе колонии уничтожены!");
                    break;
                }

                // Проверка истощения куч
                if (_resourcePiles.All(p => p.IsExhausted))
                {
                    Console.WriteLine("\nВсе кучи истощены!");
                    break;
                }
            }

            // Определение победителя
            if (_colonies.Any(c => !c.IsDestroyed))
            {
                var winner = _colonies
                    .Where(c => !c.IsDestroyed)
                    .OrderByDescending(c => c.Resources.Sum(r => r.Value))
                    .First();
                Console.WriteLine($"\nПобедила колония {winner.Name}!");
            }

            Console.WriteLine($"\nСимуляция завершена. {(_daysUntilDrought > 0 ? "Наступила засуха!" : "Все колонии уничтожены!")}");
        }

        private void DisplayStatus()
        {
            foreach (var colony in _colonies)
            {
                colony.DisplayInfo();
            }

            Console.WriteLine("\nСтатус куч:");
            foreach (var pile in _resourcePiles)
            {
                pile.DisplayStatus();
            }
        }
    }
}