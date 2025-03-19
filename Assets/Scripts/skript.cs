using System.Collections;
using System.Numerics;
using System.Xml.Serialization;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public Transform window;
    public float moveSpeed;

    [HideInInspector] private bool isMoving;
    private float sinTime;
    private UnityEngine.Vector3 current;
    private UnityEngine.Vector3 target;


    private void Start()
    {
        current = window.position;
        isMoving = false;
    }

    private void Update()
    {
        
    }

    public void SwitchWindow(int windowIndex)
    {
        if (!isMoving)
        {
            switch (windowIndex)
            {
                case 0:
                    target = new UnityEngine.Vector3(0, 0, 0);
                    break;
                case 1:
                    target = new UnityEngine.Vector3(2560, 0, 0);
                    break;
                case 2:
                    target = new UnityEngine.Vector3(-2560, 0, 0);
                    break;
            }
            StartCoroutine(MoveWindow());
        }
    }

    public float evaluate(float x)
    {
        return 0.5f * Mathf.Sin(x - Mathf.PI / 2f) + 0.5f;
    }

    private IEnumerator MoveWindow()
    {
        isMoving = true;
        sinTime = 0f; // Reset sinTime at the start of the movement
        current = window.position;

        while (sinTime < Mathf.PI)
        {
            sinTime += Time.deltaTime * moveSpeed;
            sinTime = Mathf.Clamp(sinTime, 0, Mathf.PI);
            float t = evaluate(sinTime); // Use your evaluate function
            window.position = UnityEngine.Vector3.Lerp(current, target, t);
            yield return null; // Wait until the next frame
        }

        window.position = target; // Ensure the final position is set exactly
        isMoving = false;
    }
}
