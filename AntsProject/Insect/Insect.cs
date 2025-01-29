namespace AntsProject
{
    public abstract class Insect
    {
        public string Type { get; protected set; }
        public int Health { get; protected set; }
        public int MaxHealth { get; protected set; }
        public int Defense { get; protected set; }
        public Queen Queen { get; set; }
        public bool IsAlive { get; protected set; }
        public bool IsInvulnerable { get; protected set; }

        protected Insect(string type, int health, int defense)
        {
            Type = type;
            Health = health;
            MaxHealth = health;
            Defense = defense;
            IsAlive = true;
            IsInvulnerable = false;
        }

        public virtual void TakeDamage(int damage)
        {
            if (!IsInvulnerable)
            {
                Health -= damage;
                if (Health <= 0)
                {
                    IsAlive = false;
                    Health = 0;
                }
            }
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Тип: {Type}");
            Console.WriteLine($"-- Параметры: здоровье={Health}, защита={Defense}");
            if (Queen != null)
                Console.WriteLine($"-- Королева \"{Queen.Name}\"");
            if (!IsAlive)
                Console.WriteLine("-- Статус: Мертв");
        }
    }
}