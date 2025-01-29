namespace AntsProject
{
    public class SeniorWarrior : Warrior
    {
        public SeniorWarrior() : base("старший", 2, 1, 2, 1)
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