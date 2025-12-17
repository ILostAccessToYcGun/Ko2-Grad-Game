using UnityEngine;

public class BookSlam : MonoBehaviour
{
    public float damage;
    public float rotation;
    public GameObject sprite;
    public GameObject Hitbox;

    private void Start()
    {
        rotation = Random.Range(-360.0f, 360.0f);
    }

    private void Update()
    {
        sprite.transform.Rotate(new Vector3(0, 0, rotation * 4.0f * Time.deltaTime));
    }

    public void SpawnDamage()
    {
        JCUSlamDamage slam = Instantiate(Hitbox, transform.position, Quaternion.identity).GetComponent<JCUSlamDamage>();
        slam.damage = damage;
        Destroy(gameObject);
    }


}
