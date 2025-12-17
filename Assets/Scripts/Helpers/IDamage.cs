using UnityEngine;

public interface IDamage
{
    public void TakeDamage(float damage, Vector2 attackPos, int evoId = 0);
    public void TakeDamage(float damage, int evoId = 0);
}
