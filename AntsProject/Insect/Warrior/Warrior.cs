namespace AntsProject
{
    public abstract class Warrior : Insect
    {
        public int Damage { get; protected set; }
        public int TargetLimit { get; protected set; }

        protected Warrior(string type, int health, int defense, int damage, int targetLimit)
            : base(type, health, defense)
        {
            Damage = damage;
            TargetLimit = targetLimit;
        }

        public virtual void Attack(List<Insect> targets)
        {
            foreach (var target in targets.Take(TargetLimit))
            {
                // Implement attack logic
                var damage = Math.Max(0, Damage - target.Defense);
                // Apply damage to target
            }
        }
    }
}