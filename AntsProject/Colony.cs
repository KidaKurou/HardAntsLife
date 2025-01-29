namespace AntsProject
{
    public class Colony
    {
        public string Name { get; private set; }
        public Queen Queen { get; private set; }
        public Dictionary<string, int> Resources { get; private set; }
        public List<Worker> Workers { get; private set; }
        public List<Warrior> Warriors { get; private set; }
        public List<Insect> SpecialInsects { get; private set; }
        public bool IgnoreNegativeEffects { get; set; }
        public bool IsDestroyed { get; private set; }
        private readonly Random random = new Random();

        public Colony(string name, Queen queen)
        {
            Name = name;
            Queen = queen;
            Queen.SetColony(this);
            Resources = new Dictionary<string, int>();
            Workers = new List<Worker>();
            Warriors = new List<Warrior>();
            SpecialInsects = new List<Insect>();
            IgnoreNegativeEffects = false;
            IsDestroyed = false;
        }

        public void AddWorker(Worker worker)
        {
            worker.Queen = Queen;
            Workers.Add(worker);
        }

        public void AddWarrior(Warrior warrior)
        {
            warrior.Queen = Queen;
            Warriors.Add(warrior);
        }

        public void AddSpecialInsect(Insect insect)
        {
            insect.Queen = Queen;
            SpecialInsects.Add(insect);
            if (insect is Dragonfly)
            {
                var dragonfly = insect as Dragonfly;
                dragonfly?.ApplyColonyEffect(this);
            }
        }

        public void DisplayInfo()
        {
            Console.WriteLine("\n---------------------------------");
            if (IsDestroyed)
            {
                Console.WriteLine($"Колония {Name} уничтожена");
                return;
            }

            Console.WriteLine($"\nКолония \"{Name}\"");
            Queen.DisplayInfo();
            Console.WriteLine($"Популяция: рабочих={Workers.Count}, воинов={Warriors.Count}, особенных={SpecialInsects.Count}");
            Console.WriteLine($"Всего: {Workers.Count + Warriors.Count + SpecialInsects.Count}");

            if (Resources.Any())
            {
                Console.WriteLine($"-- Ресурсы: {string.Join(", ", Resources.Select(r => $"{r.Key}={r.Value}"))}");
            }

            if (Workers.Any())
            {
                Console.WriteLine("<<<<<<<<<<<<< Работники:");
                var workerGroups = Workers.GroupBy(w => w.Type);
                foreach (var group in workerGroups)
                {
                    group.First().DisplayInfo();
                    Console.WriteLine($"-- Количество: {group.Count()}");
                }
            }

            if (Warriors.Any())
            {
                Console.WriteLine("<<<<<<<<<<<<< Воины:");
                var warriorGroups = Warriors.GroupBy(w => w.Type);
                foreach (var group in warriorGroups)
                {
                    group.First().DisplayInfo();
                    Console.WriteLine($"-- Количество: {group.Count()}");
                }
            }

            if (SpecialInsects.Any())
            {
                Console.WriteLine("<<<<<<<<<<<<< Особенные:");
                foreach (var insect in SpecialInsects)
                {
                    insect.DisplayInfo();
                }
            }
            Console.WriteLine("---------------------------------");
        }

        public void Destroy()
        {
            IsDestroyed = true;
            Workers.Clear();
            Warriors.Clear();
            SpecialInsects.Clear();
            Resources.Clear();
        }

        public void ProcessDay(List<ResourcePile> piles, List<Colony> enemies)
        {
            if (IsDestroyed) return;

            // Обработка дня королевы
            Queen.ProcessDay();

            // Распределение муравьев по кучам для сбора ресурсов
            var availablePiles = piles.Where(p => !p.IsExhausted).ToList();
            if (availablePiles.Any())
            {
                foreach (var worker in Workers.Where(w => w.IsAlive))
                {
                    var randomPile = availablePiles[random.Next(availablePiles.Count)];
                    var gathered = worker.GatherResources(randomPile.Resources, enemies);

                    foreach (var resource in gathered)
                    {
                        if (!Resources.ContainsKey(resource.Type))
                        {
                            Resources[resource.Type] = 0;
                        }
                        Resources[resource.Type] += resource.Amount;
                    }

                    Console.WriteLine($"{worker.Type} Рабочий из колони {Name} собрал {string.Join(", ", gathered.Select(r => $"{r.Amount} {r.Type}"))} из кучи {randomPile.Index}");
                }
            }

            // Атаки воинов
            var allEnemyInsects = enemies
                .Where(e => !e.IsDestroyed)
                .SelectMany(e =>
                    e.Workers.Cast<Insect>()
                        .Concat(e.Warriors)
                        .Concat(e.SpecialInsects)
                        .Where(i => i.IsAlive)
                ).ToList();

            foreach (var warrior in Warriors.Where(w => w.IsAlive))
            {
                // Выбираем цели в зависимости от типа воина и их уязвимости
                var validTargets = allEnemyInsects;

                // Для охотников выбираем только неуязвимые цели
                if (warrior.Type.Contains("охотник"))
                {
                    validTargets = allEnemyInsects.Where(i => i.IsInvulnerable).ToList();
                }

                if (validTargets.Any())
                {
                    warrior.Attack(validTargets);
                }
            }

            // Атаки особых насекомых
            foreach (var special in SpecialInsects.Where(s => s.IsAlive))
            {
                if (special.Type.Contains("агрессивный"))
                {
                    special.TakeDamage(0); // Активация особой способности через переопределенный метод TakeDamage
                }
            }

            // Потери
            Console.WriteLine($"\nПотери в колонии {Name}:");
            Console.WriteLine($"-- Рабочих: {Workers.Where(w => !w.IsAlive).Count()}");
            Console.WriteLine($"-- Воинов: {Warriors.Where(w => !w.IsAlive).Count()}");
            Console.WriteLine($"-- Особенных: {SpecialInsects.Where(s => !s.IsAlive).Count()}");

            // Удаление мертвых насекомых
            Workers.RemoveAll(w => !w.IsAlive);
            Warriors.RemoveAll(w => !w.IsAlive);
            SpecialInsects.RemoveAll(s => !s.IsAlive);

            // Проверка на уничтожение колонии
            if (!Workers.Any() && !Warriors.Any() && !SpecialInsects.Any())
            {
                Destroy();
            }
        }
    }
}