using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteracting : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 3f))
            {
                Chest chest = hit.collider.GetComponent<Chest>();
                if (chest != null)
                {
                    GameManager.Instance.OpenChest(chest);
                }
            }
        }
    }
}
