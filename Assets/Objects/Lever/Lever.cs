using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IAttackable
{

    [Header("Animation")]
    [SerializeField] private AnimationCurve activationCurve;
    [SerializeField] private float animationTime;

    [Header("Connections")]
    [SerializeField] private List<GameObject> triggers;

    private float initialAngle;
    private float finalAngle;
    private Transform leverTrs;

    private bool activated = false;

    void Start()
    {
        leverTrs = Utils.SearchObjectWithComponent<Transform>(transform, "Lever Top");
		initialAngle = leverTrs.eulerAngles.z;
        finalAngle = initialAngle * -1f;
    }

    IEnumerator PlayAnimation()
    {
        activated = true;

		float time = 0f;
        while (time < animationTime) {
            time += Time.deltaTime;
            time = Mathf.Clamp(time, 0f, animationTime);
            float percent = time / animationTime;
            float point = activationCurve.Evaluate(percent);
            float rotation = Mathf.Lerp(initialAngle, finalAngle, point);

            leverTrs.rotation = Quaternion.Euler(0f, 0f, rotation);
			yield return null;
		}
		leverTrs.Rotate(0f, finalAngle, 0f, Space.World);
    }

    public void SufferDamage(int damage, Transform attackTransform = null, Vector2 direction = default, float force = 1, float torqueIntensity = 1)
    {
        if (!activated)
        {
			StartCoroutine(PlayAnimation());
            foreach(GameObject itrGameObj in triggers)
            {
                if (itrGameObj.TryGetComponent(out ILeverTrigger trigger))
                {
                    trigger.Trigger();
                }
                else
                {
                    Debug.LogError($"game object: {itrGameObj.name} doesn't implement the ILeverTrigger interface");
                }
            }
		}
    }
}
