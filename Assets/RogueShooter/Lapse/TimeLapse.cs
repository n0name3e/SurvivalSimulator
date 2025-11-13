using NeonEscape;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLapse : MonoBehaviour
{
    public float recordTime = 5f;
    public List<FrameData> recordedFrames = new List<FrameData>();

    private Animator animator;
    private Rigidbody rb;
    Renderer ren;

    private FrameData currentFrame;

    public bool isRewinding = false;
    public bool isRecording = true;

    public bool recordTransform = true;
    public bool recordAnimator = true;
    public bool destroyOnLapseEnd = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        ren = GetComponentInChildren<Renderer>();

        TimeLapseManager.Instance.timeLapses.Add(this);
    }

    private void FixedUpdate()
    {
        

        if (!isRewinding && isRecording)
        {
            RecordFrame();
        }
        else if (isRewinding)
        {
            PerformTimeLapse();
        }

    }
    private void LateUpdate()
    {
        if (!isRewinding)
        {
            return;
        }
        if (recordedFrames.Count <= 0)
        {
            StopLapsing();
            return;
        }
        if (currentFrame != null)
        {
            PerformVisualTimeLapse(currentFrame);
        }

    }
    public void ToggleLapse()
    {
        return;
        if (isRewinding)
        {
            isRewinding = false;
            StopLapsing();
        }
        else
        {
            isRewinding = true;
            StartLapsing();
        }
    }
    public void ToggleRecording()
    {
        isRecording = !isRecording;
    }
    private void RecordFrame()
    {
        for (int i = 0; i < recordedFrames.Count; i++)
        {
            if (recordedFrames[i].recordedTime + recordTime >= Time.time)
            {
                break;
            }
            
            recordedFrames.RemoveAt(i);
        }
        /*if (recordedFrames.Count >= 1 && recordedFrames[0].recordedTime + recordTime < Time.time)
        {
            recordedFrames.RemoveAt(0);
        }*/
        int animHash = 0;
        float animTime = 0;
        if (recordAnimator)
        {
            AnimatorStateInfo animState = animator.GetCurrentAnimatorStateInfo(0);
            animHash = animState.fullPathHash;
            animTime = animState.normalizedTime;
        }


        // 3. Add the new frame state to our history
        if (!recordTransform && !recordAnimator)
        {
            // If neither is being recorded, just record active state
            recordedFrames.Add(new FrameData(ren.enabled));
            return;
        }
        recordedFrames.Add(new FrameData(
            transform.position,
            transform.rotation,
            animHash,
            animTime,
            //gameObject.activeInHierarchy
            ren.enabled
        ));
    }

    public void RecordAction(ICommand action)
    {
        // Add this action to the *current frame*
        if (recordedFrames.Count > 0)
        {
            // Get the last frame we recorded and add this action to it
            FrameData currentFrame = recordedFrames[recordedFrames.Count - 1];
            currentFrame.actions.Add(action);
        }
    }

    public void PerformTimeLapse()
    {
        if (recordedFrames.Count <= 0)
        {
            StopLapsing();
            return;
        }

        FrameData currentFrame = recordedFrames[recordedFrames.Count - 1];
        this.currentFrame = currentFrame;
        recordedFrames.RemoveAt(recordedFrames.Count - 1);

        ren.enabled = currentFrame.isActive;
    }
    private void PerformVisualTimeLapse(FrameData currentFrame)
    {

        if (recordTransform)
        {
            transform.position = currentFrame.position;
            transform.rotation = currentFrame.rotation;
        }


        if (animator != null && recordAnimator)
        {
            animator.Play(currentFrame.animationStateHash, 0, currentFrame.animationNormalizedTime);
        }
    }
    private void PerformFixedTimeLapse(FrameData currentFrame)
    {
        //gameObject.SetActive(currentFrame.isActive);
        recordedFrames.RemoveAt(recordedFrames.Count - 1);
    }
    public void StartLapsing()
    {
        print("starting time lapse");
        isRewinding = true;
        if (rb != null) rb.isKinematic = true;
        if (animator != null) animator.speed = 0; // Pause animator
    }
    public void StopLapsing()
    {
        print("ending time lapse");
        isRewinding = false;
        TimeLapseManager.Instance.RemoveLapse();
        if (destroyOnLapseEnd)
        {
            Destroy(gameObject);
            TimeLapseManager.Instance.timeLapses.Remove(this);
            return;
        }
        if (rb != null) rb.isKinematic = false;
        if (animator != null) animator.speed = 1; // Resume animator
    }
}
