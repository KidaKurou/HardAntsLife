namespace AntsProject
{
    public class LegendaryWarrior : Warrior
    {
        public LegendaryWarrior() : base("легендарный", 10, 6, 6, 3)
        {
        }

        public override void Attack(List<Insect> targets)
        {
            foreach (var target in targets.Take(TargetLimit))
            {
                // One bite per target
                int actualDamage = Math.Max(0, Damage - target.Defense);
                target.TakeDamage(actualDamage);
            }
        }
    }
}