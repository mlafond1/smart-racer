using UnityEngine;

public class Brick : MonoBehaviour
{
    Vector3 originalPosition;
    Quaternion originalRotation;
    Rigidbody rb;
    private void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        rb = gameObject.GetComponent<Rigidbody>();
        Freeze(true);
    }

    public void ResetOriginalState()
    {
        transform.SetPositionAndRotation(originalPosition, originalRotation);
        Freeze(true);
    }

    public void Freeze(bool freezed)
    {
        if (freezed)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;
        }
    }
}