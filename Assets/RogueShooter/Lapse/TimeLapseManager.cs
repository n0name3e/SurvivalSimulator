using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLapseManager : MonoBehaviour
{
    public static TimeLapseManager Instance;
    public List<TimeLapse> timeLapses = new List<TimeLapse>();
    public int ongoingLapseCount = 0;

    public bool isRewinding = false;

    [SerializeField] private UnityEngine.UI.Image infoImage;
    [SerializeField] private UnityEngine.UI.Button lapseButton;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        infoImage.color = Color.green;
        
    }
    private void Update()
    {
        if (isRewinding && ongoingLapseCount == 0)
        {
            StopLapsing();
        }
    }
    public void RemoveLapse()
    {
        ongoingLapseCount--;
    }
    public void ToggleLapsing()
    {
        if (isRewinding)
        {
            StopLapsing();
            ongoingLapseCount = 0;
        }
        else
        {
            StartLapsing();
            ongoingLapseCount = timeLapses.Count;
        }
    }
    public void StartLapsing()
    {
        isRewinding = true;
        infoImage.color = Color.red;
        foreach (TimeLapse tl in timeLapses)
        {
            print(tl.gameObject.name + " startininininini");
            tl.StartLapsing();
        }
    }
    public void StopLapsing()
    {
        isRewinding = false;
        infoImage.color = Color.green;
        List<TimeLapse> tlNew = new List<TimeLapse>(timeLapses);
        foreach (TimeLapse tl in tlNew)
        {
            tl.StopLapsing();
        }
    }
    public void StopRecording()
    {
        foreach (TimeLapse tl in timeLapses)
        {
            tl.enabled = false;
        }
        gameObject.SetActive(false);
    }
}
