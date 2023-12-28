using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Pickup : MonoBehaviour
{

    [SerializeField] private float pickupRadius;
    [SerializeField] private LayerMask pickupMask;

    public static float PickupRadius { get => instance.pickupRadius; set => instance.pickupRadius = value; }
    public static Pickup instance;

    private static Collider2D hovering;

#if UNITY_EDITOR
    private bool hover = false;
    private Vector2 colliderPos;
#endif

	void Start()
    {
        if (instance == null)
        {
            instance = this;
            
        }
        else
        {
            Debug.LogError($"Duas instâncias de Pickup encontradas, deletando {gameObject.name}");
            Destroy(gameObject);
        }
    }

    private Collider2D GetNearestCollider(Collider2D[] colliderArray)
    {
        Collider2D nearestCollider = null;
        float leastDistance = float.MaxValue;
        foreach(Collider2D collider in colliderArray)
        {
            float distance = Vector2.Distance(transform.position, collider.transform.position);
			if (distance < leastDistance)
            {
                nearestCollider = collider;
                leastDistance = distance;
			}
        }
        return nearestCollider;
    }

    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRadius, pickupMask);
        if (colliders.Length > 0)
        {
			Collider2D collider = GetNearestCollider(colliders);
            
            #if UNITY_EDITOR
			hover = true;
			colliderPos = collider.transform.position;
            #endif

			if (hovering != null)
			{
				hovering.GetComponent<Pickable>().hover = false;
			}
			hovering = collider;
			hovering.GetComponent<Pickable>().hover = true;
		}
		else
        {
            if (hovering != null)
            {
                hovering.GetComponent<Pickable>().hover = false;
                hovering = null;

                #if UNITY_EDITOR
				hover = false;
                #endif
			}
		}
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
        if (hover)
        {
			Gizmos.color = Color.white;
			Gizmos.DrawWireSphere(colliderPos, pickupRadius);
		}
	}
#endif
}
