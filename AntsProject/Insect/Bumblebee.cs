namespace AntsProject
{
    public class Bumblebee : SpecialInsect
    {
        public Bumblebee() : base("ленивый неуязвимый агрессивный неряшливый - Шмель", 24, 8, 8, 3)
        {
            IsInvulnerable = true;
            IsAggressive = true;
            CanTakeResources = false;
            Defense /= 2; // Defense is halved due to being неряшливый
        }

        public void Attack(List<Insect> enemies)
        {
            var targets = enemies.Take(TargetLimit).ToList();
            foreach (var target in targets)
            {
                for (int i = 0; i < 3; i++) // Наносит 3 укуса
                {
                    if (!target.IsInvulnerable)
                    {
                        int actualDamage = Math.Max(0, Damage - target.Defense);
                        target.TakeDamage(actualDamage);
                    }
                }
            }
        }
    }
}