using UnityEngine;
using System;
using System.Collections.Generic;

public class AnimationEventSystem : MonoBehaviour
{
    public static AnimationEventSystem instance;
    private Dictionary<string, Action> animationEndEvents = new Dictionary<string, Action>();
    private Dictionary<string, Action> animationStartEvents = new Dictionary<string, Action>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void RegisterEndEvent(string animationName, Action callback)
    {
        if (!animationEndEvents.ContainsKey(animationName))
            animationEndEvents[animationName] = callback;
        else
            animationEndEvents[animationName] += callback;
    }

    public void UnregisterEndEvent(string animationName, Action callback)
    {
        if (animationEndEvents.ContainsKey(animationName))
            animationEndEvents[animationName] -= callback;
    }

    // Animation Event'ten çağrılacak
    public void OnAnimationEnd(string animationName)
    {
        if (animationEndEvents.ContainsKey(animationName))
            animationEndEvents[animationName]?.Invoke();
    }
}