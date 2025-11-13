using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text text;

    // Update is called once per frame
    void Update()
    {
        text.text = "FPS: " + (1f / Time.deltaTime);
    }
}
