using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] int damage = 40;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject person = collision.gameObject;
        person.GetComponent<Entity>().TakeHit(damage);

        RepelPerson2D(person);
    }


    [SerializeField] float repelForceX = 15000f;
    [SerializeField] float repelForceY = 1000f;
    private void RepelPerson2D(GameObject Person)
    {
        if (Person.GetComponent<Entity>().isDead) return;

        Rigidbody2D personRigitbody2D = Person.GetComponent<Rigidbody2D>();

        personRigitbody2D?.AddForce(Vector2.left * repelForceX);
        personRigitbody2D?.AddForce(Vector2.up * repelForceY);
    }
}
