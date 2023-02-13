using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementManager : MovementManager
{
    Ray pointScreen;
    RaycastHit cameraRayImpact;

   public void PlayerRotate(LayerMask aimingLayer)
   {
       pointScreen = Camera.main.ScreenPointToRay(Input.mousePosition);
       Debug.DrawRay(pointScreen.origin, pointScreen.direction * 30, Color.white);
   
       if (Physics.Raycast(pointScreen, out cameraRayImpact, 50, aimingLayer))
       {
           Vector3 lookDirection = cameraRayImpact.point - transform.position;
           lookDirection.y = transform.position.y;
   
           Rotate(lookDirection);
       }
   }

}
