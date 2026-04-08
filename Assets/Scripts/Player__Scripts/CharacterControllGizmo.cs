using UnityEngine;
using System.Collections;

public class CharacterControllGizmo : MonoBehaviour
{

    void OnDrawGizmos()
    {
        CharacterController cc = GetComponent<CharacterController>();
        Gizmos.color = Color.yellow;

        if (cc.height > cc.radius * 2)
            Gizmos.DrawWireCube(transform.position, new Vector3(cc.radius * 2, cc.height, cc.radius * 2));
        else //if (cc.radius * 2 > cc.height || cc.radius * 2 == cc.height)
            Gizmos.DrawWireSphere(transform.position, cc.radius);
    }
}