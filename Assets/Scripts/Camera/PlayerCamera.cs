using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static GameObject LocalPlayer;

    private const float OffsetZ = -10.0f;

    private void LateUpdate()
    {
        if (LocalPlayer)
        {
            transform.position = new Vector3(LocalPlayer.transform.position.x, LocalPlayer.transform.position.y, OffsetZ);
        }
    }
}
