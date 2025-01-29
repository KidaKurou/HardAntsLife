namespace AntsProject
{
    public class AdvancedHunterWarrior : Warrior
    {
        public AdvancedHunterWarrior() : base("продвинутый охотник", 6, 2, 4, 2)
        {
        }

        public override void Attack(List<Insect> targets)
        {
            // Only attack invulnerable targets
            var invulnerableTargets = targets
                .Where(t => t is SpecialInsect s && s.IsInvulnerable)
                .Take(TargetLimit)
                .ToList();

            foreach (var target in invulnerableTargets)
            {
                // One bite per target, can damage invulnerable insects
                int actualDamage = Math.Max(0, Damage - target.Defense);
                target.TakeDamage(actualDamage);
            }
        }
    }
}