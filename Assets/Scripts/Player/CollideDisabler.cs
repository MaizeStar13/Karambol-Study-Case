using UnityEngine;

public class CollideDisabler : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
/*            Destroy(col.gameObject);*/
            col.gameObject.SetActive(false);
        }
    }
}
