using UnityEngine;

public class BookSlam : MonoBehaviour
{
    public float damage;
    public float rotation;
    public GameObject sprite;
    public JCUSlamDamage Hitbox;

    private void Start()
    {
        rotation = Random.Range(-360.0f, 360.0f);
    }

    private void Update()
    {
        sprite.transform.Rotate(new Vector3(0, 0, rotation * Time.deltaTime));
    }

    public void SpawnDamage()
    {
        JCUSlamDamage slam = Instantiate(Hitbox).GetComponent<JCUSlamDamage>();
        slam.damage = damage;
    }


}
