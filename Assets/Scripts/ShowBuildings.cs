using System.Collections;
using System.Numerics;
using System.Xml.Serialization;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ShowBuildings : MonoBehaviour
{

    public Transform window;
    public float moveSpeed;

    [HideInInspector] private bool isMoving;
    private float sinTime;
    private UnityEngine.Vector3 current;
    private UnityEngine.Vector3 target;
    private bool isOpen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isMoving = false;
        isOpen = false;
        current = window.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildingsWindow()
    {
        if (!isMoving)
        {
            if (!isOpen)
            {
                target = new UnityEngine.Vector3(0, -80, 0);
                isOpen = true;
            }
            else
            {
                target = new UnityEngine.Vector3(0, -1200, 0);
                isOpen = false;
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
