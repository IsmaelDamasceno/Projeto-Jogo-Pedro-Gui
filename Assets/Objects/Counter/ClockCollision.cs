using UnityEngine;

public class ClockCollision : MonoBehaviour
{

    public static bool clockColected = false;

    void Start()
    {
        if (clockColected)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Counter.StartCounting();
        clockColected = true;
        Destroy(gameObject);
	}
}
