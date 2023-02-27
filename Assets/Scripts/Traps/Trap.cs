using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] int damage = 40;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject person = collision.gameObject;
        person.GetComponent<Entity>().TakeHit(damage);

        RepulsionPerson2D(person);
    }


    [SerializeField] float repulsionForceX = 15000f;
    [SerializeField] float repulsionForceY = 1000f;
    private void RepulsionPerson2D(GameObject Person)
    {
        if (Person.GetComponent<Entity>().isDead) return;

        Rigidbody2D personRigitbody2D = Person.GetComponent<Rigidbody2D>();
        bool DirectionLookToRight = Person.GetComponent<PlayerMoving>().faceRight;

        Vector2 repulsionVector = DirectionLookToRight ? Vector2.left : Vector2.right;

        personRigitbody2D?.AddForce(repulsionVector * repulsionForceX);
        personRigitbody2D?.AddForce(Vector2.up * repulsionForceY);
    }
}
