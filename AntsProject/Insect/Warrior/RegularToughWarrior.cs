namespace AntsProject
{
    public class RegularToughWarrior : Warrior
    {
        public RegularToughWarrior() : base("обычный крепкий", 1, 0, 1, 1)
        {
        }

        public override void TakeDamage(int damage)
        {
            // Damage from enemy bites is halved
            base.TakeDamage(damage / 2);
        }

        public override void Attack(List<Insect> targets)
        {
            var target = targets.FirstOrDefault();
            if (target != null)
            {
                // One bite
                int actualDamage = Math.Max(0, Damage - target.Defense);
                target.TakeDamage(actualDamage);
            }
        }
    }
}