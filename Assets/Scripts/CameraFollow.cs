using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CameraFollow : MonoBehaviour
{
    List<PlayerController> players;
    //GameObject indicator;

    public float falloff = 1;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("num players: " + ReInput.players.playerCount);

        players = new List<PlayerController>();
        PlayerController[] objs = FindObjectsOfType<PlayerController>();
        //GameObject[] objs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (PlayerController obj in objs)
        {
            players.Add(obj);
            Debug.Log("adding player: "+ obj.name);
        }
        //indicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //indicator.transform.localScale = Vector3.one * .1f;
        //indicator.name = "indicator";
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraPosition();
    }

    Bounds GetPlayerBounds()
    {
        Bounds bounds = new Bounds(players[0].transform.position, Vector3.zero);
        for (int i=1; i<players.Count; i++)
        {
            bounds.Encapsulate(players[i].transform.position);
        }
        return bounds;
    }

    /// <summary>
    /// Update to track the player
    /// </summary>
    void UpdateCameraPosition()
    {
        float yoff = .01f;
        Vector3 campos = transform.position;
        Vector3 groundPos = campos - transform.forward / transform.forward.y * transform.position.y; //cam fwd proj to ground
        //Debug.DrawLine(transform.position, groundPos);
        groundPos.y = yoff;
        //indicator.transform.position = groundPos;

        Bounds players = GetPlayerBounds();
        Vector3 groundTarget = players.center;
        groundTarget.y = yoff;
        float targetDist = new Vector3(groundTarget.x, 0, groundTarget.z).magnitude;
        float playerDist = new Vector3(players.center.x, 0, players.center.z).magnitude;
        if (targetDist > falloff)
        {
            targetDist = falloff;
            groundTarget = groundTarget.normalized * targetDist;
        }
        else
        {
            float easeAmount = (Mathf.Clamp(Mathf.Cos(targetDist / falloff * Mathf.PI / 2), 0, 1));
            //Debug.Log("Ease amount: " + easeAmount);
            targetDist = easeAmount * playerDist + (1-easeAmount) * falloff;
            groundTarget = groundTarget.normalized * targetDist;
        }
        //Debug.Log("east dist: " + easedist);
        
        //shift camera so camera center points to groundTarget
        Vector3 diff = groundTarget;
        diff.y = yoff;
        diff -= new Vector3(groundPos.x, yoff, groundPos.z);
        
        transform.position += diff;
    }
}
