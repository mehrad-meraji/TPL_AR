﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vuforia;

public class openLinkScript : MonoBehaviour, ITrackableEventHandler, IVirtualButtonEventHandler
{
    VirtualButtonBehaviour[] virtualButtonBehaviours;
    string vbName;
    float currCountdownValue;
    bool startTimer;
    bool multipleTargetsFound;
    bool canTriggerPostMultipleTargetsLink;
    string[] videoTargetsArray = new string[] { "nailbiters_video_trigger", "headspace_trigger", "fantasy_trigger", "true_story_trigger", "own_voices_trigger", "science_fiction_trigger", "resist_trigger", "comics_trigger", "turning_points_trigger", "poetry_trigger"};
    string[] linkTargetsArray = new string[] { "nailbiters_books", "headspace_books", "fantasy_books", "true_story_books", "own_voices_books", "science_fiction_books", "resist_books", "comics_books", "turning_points_books", "poetry_books" };

    // Update is called once per frame
    void Update () {
        // Get the Vuforia StateManager
        StateManager sm = TrackerManager.Instance.GetStateManager ();

        IList<TrackableBehaviour> activeTrackablesList = (IList<TrackableBehaviour>)sm.GetActiveTrackableBehaviours();
        //Debug.Log("Number of active trackables:");
        //Debug.Log(activeTrackablesList.Count);
        if (activeTrackablesList.Count >= 2)
        {
            startTimer = false;
            multipleTargetsFound = true;
            canTriggerPostMultipleTargetsLink = true;
        }
        else
        {
            multipleTargetsFound = false;
        }

        // Query the StateManager to retrieve the list of
        // currently 'active' trackables 
        //(i.e. the ones currently being tracked by Vuforia)
        //Iterate through the list of active trackables
        //Debug.Log ("List of trackables currently active (tracked): ");
        IEnumerable<TrackableBehaviour> activeTrackables = sm.GetActiveTrackableBehaviours ();
        foreach (TrackableBehaviour tb in activeTrackables) {
            //Debug.Log("Target found!");
            //Debug.Log(tb.TrackableName);
            if (linkTargetsArray.Any(tb.TrackableName.Contains) && multipleTargetsFound == false)
            {
                //Debug.Log("Link target found!");
                //start countdown timer when only the link target is visible again
                startTimer = true;
                if (canTriggerPostMultipleTargetsLink)
                {
                    TriggerLinkOpening(tb.TrackableName);
                }
                canTriggerPostMultipleTargetsLink = false;
            }
        }
    }
    
    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {

    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {

    }

    public IEnumerator StartCountdown(string name, float countdownValue = 2)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0 && startTimer == true)
        {
            Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
            Debug.Log(startTimer);
        }
        if (startTimer)
        {
            switch (name)
            {
                case "library_card":
                    Application.OpenURL("https://account.torontopubliclibrary.ca");
                    break;
                case "adult_library_card_1":
                    Application.OpenURL("https://account.torontopubliclibrary.ca");
                    break;
                case "adult_library_card_2":
                    Application.OpenURL("https://account.torontopubliclibrary.ca");
                    break;
                case "adult_library_card_3":
                    Application.OpenURL("https://account.torontopubliclibrary.ca");
                    break;
                case "nailbiters_books":
                    Application.OpenURL("https://www.torontopubliclibrary.ca/teens/the-list.jsp?category=nailbiters");
                    break;
                case "headspace_books":
                    Application.OpenURL("https://www.torontopubliclibrary.ca/teens/the-list.jsp?category=headspace");
                    break;
                case "fantasy_books":
                    Application.OpenURL("https://www.torontopubliclibrary.ca/teens/the-list.jsp?category=fantasy");
                    break;
                case "true_story_books":
                    Application.OpenURL("https://www.torontopubliclibrary.ca/teens/the-list.jsp?category=true_story");
                    break;
                case "own_voices_books":
                    Application.OpenURL("https://www.torontopubliclibrary.ca/teens/the-list.jsp?category=ownvoices");
                    break;
                case "science_fiction_books":
                    Application.OpenURL("https://www.torontopubliclibrary.ca/teens/the-list.jsp?category=science_fiction");
                    break;
                case "resist_books":
                    Application.OpenURL("https://www.torontopubliclibrary.ca/teens/the-list.jsp?category=resist");
                    break;
                case "comics_books":
                    Application.OpenURL("https://www.torontopubliclibrary.ca/teens/the-list.jsp?category=comics_and_manga");
                    break;
                case "turning_points_books":
                    Application.OpenURL("https://www.torontopubliclibrary.ca/teens/the-list.jsp?category=turning_points");
                    break;
                case "poetry_books":
                    Application.OpenURL("https://www.torontopubliclibrary.ca/teens/the-list.jsp?category=poetry");
                    break;
                default:
                    Debug.Log("No link source detected...");
                    break;
            }
        }
    }

    public void EndCountdown()
    {
        currCountdownValue = 3;
        startTimer = false;
    }

    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);

        //Register with the virtual buttons TrackableBehaviour
        virtualButtonBehaviours = GetComponentsInChildren<VirtualButtonBehaviour>();

        for (int i = 0; i < virtualButtonBehaviours.Length; i++)
            virtualButtonBehaviours[i].RegisterEventHandler(this);
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;

        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound()
    {
        Debug.Log("***************************");
        Debug.Log("On tracking found starting...");
        Debug.Log("***************************");
        startTimer = true;
        StartCoroutine(StartCountdown(mTrackableBehaviour.TrackableName, 2));
        
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;
    }


    protected virtual void OnTrackingLost()
    {
        Debug.Log("********************");
        Debug.Log("On tracking lost...");
        Debug.Log("*********************");
        EndCountdown();


        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;
    }

    protected virtual void TriggerLinkOpening(string name)
    {
        //StartCoroutine(StartCountdown(name, 3));
        OnTrackingLost();
    }

    #endregion // PROTECTED_METHODS
}
