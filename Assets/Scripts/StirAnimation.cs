using UnityEngine;

public class StirAnimation : MonoBehaviour
{
    [SerializeField] private float angle = 12f;
    [SerializeField] private float speed = 3f;

    private Quaternion startRotation;

    private void Awake()
    {
        startRotation = transform.localRotation;
    }

    private void Update()
    {
        float rotationZ = Mathf.Sin(Time.time * speed) * angle;

        transform.localRotation =
            startRotation * Quaternion.Euler(0f, 0f, rotationZ);
    }
}