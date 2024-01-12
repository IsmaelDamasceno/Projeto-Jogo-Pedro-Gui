using Player;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DownDashState : BaseState
{

    [SerializeField] private float gravityForce;
    [SerializeField] private float initialForce;
    [SerializeField] private GameObject shockWavePrefab; 

    public override void Enter()
    {
        PlayerCore.rb.gravityScale = 0f;
        PlayerCore.rb.velocity = Vector2.down * initialForce;
	}

    public override void Exit()
    {
        PlayerCore.rb.gravityScale = PlayerCore.startGravScale;
	}

    public override void FixedStep()
    {

    }

    public override void Step()
    {
        if (PlayerCore.rectGroundDetection.Check())
        {
            for(int i = 0; i <= 1; i++) {
				GameObject obj = Instantiate(shockWavePrefab, transform.position, Quaternion.identity);
                obj.GetComponent<ShockWave>().direction = (int)(Mathf.Sign(i - 1f));
            }
            CameraMovement.ShakeIt(3f, 0.05f);
            machine.ChangeState("Move");
        }
    }

#if UNITY_EDITOR 
    private void OnDrawGizmosSelected()
    {
        if (PlayerCore.boxCollider == null)
        {
            return;
        }

        PlayerCore.rectGroundDetection.DebugDraw(Color.blue);
    }
#endif
}
