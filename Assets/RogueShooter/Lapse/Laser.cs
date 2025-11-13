using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float timeLeft = 1f;
    private bool isDisabled = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f && !isDisabled)
        {
            GetComponentInChildren<Renderer>().enabled = false;
            isDisabled = true;
            //gameObject.SetActive(false);
        }
    }
}
