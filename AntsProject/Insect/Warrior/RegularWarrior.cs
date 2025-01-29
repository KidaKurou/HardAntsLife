namespace AntsProject
{
    public class RegularWarrior : Warrior
    {
        public RegularWarrior() : base("обычный", 1, 0, 1, 1)
        {
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