using UnityEngine;
using System.Collections.Generic;

public class CombatTrailController : MonoBehaviour
{
    [System.Serializable]
    public class TrailConfig
    {
        public TrailRenderer trail;
        public AttackType attackType;
        [Tooltip("Trail efektinin animasyonda başlayacağı normalized zaman (0-1 arası)")]
        public float startTime = 0.2f;
        [Tooltip("Trail efektinin animasyonda biteceği normalized zaman (0-1 arası)")]
        public float endTime = 0.8f;
    }

    public enum AttackType
    {
        RightHook,
        LeftHook,
        Uppercut,
        AirKick,
        RightPunch,
        LeftPunch,
        RightKick,
        LeftKick
    }

    [SerializeField]
    private TrailConfig[] trailConfigs;

    private Dictionary<string, TrailConfig> trailConfigMap;
    private Animator animator;

    private void Awake()
    {
        Initialize();
        SetupAutomaticTrailEvents();
    }

    private void Initialize()
    {
        animator = GetComponent<Animator>();
        trailConfigMap = new Dictionary<string, TrailConfig>();

        // Trail config map'i oluştur
        foreach (var config in trailConfigs)
        {
            string[] possibleAnimNames = GetPossibleAnimationNames(config.attackType);
            foreach (var animName in possibleAnimNames)
            {
                trailConfigMap[animName.ToLower()] = config;
            }
        }

        // Başlangıçta tüm trail'leri kapat
        DisableAllTrails();
    }

    private string[] GetPossibleAnimationNames(AttackType attackType)
    {
        // Her attack type için olası animasyon isimlerini döndür
        switch (attackType)
        {
            case AttackType.RightHook:
                return new[] { "righthook", "hookright", "right_hook" };
            case AttackType.LeftHook:
                return new[] { "lefthook", "hookleft", "left_hook" };
            case AttackType.Uppercut:
                return new[] { "uppercut", "upper_cut" };
            case AttackType.AirKick:
                return new[] { "airkick", "air_kick" };
            case AttackType.RightPunch:
                return new[] { "rightpunch", "punchright", "right_punch" };
            case AttackType.LeftPunch:
                return new[] { "leftpunch", "punchleft", "left_punch" };
            case AttackType.RightKick:
                return new[] { "rightkick", "kickright", "right_kick" };
            case AttackType.LeftKick:
                return new[] { "leftkick", "kickleft", "left_kick" };
            default:
                return new string[] { };
        }
    }

    private void SetupAutomaticTrailEvents()
    {
        if (animator == null) return;

        var controller = animator.runtimeAnimatorController;
        foreach (var clip in controller.animationClips)
        {
            TrailConfig matchingConfig = FindMatchingTrailConfig(clip.name);
            if (matchingConfig != null)
            {
                SetupEventsForClip(clip, matchingConfig);
            }
        }
    }

    private TrailConfig FindMatchingTrailConfig(string clipName)
    {
        string lowerClipName = clipName.ToLower();
        if (trailConfigMap.ContainsKey(lowerClipName))
        {
            return trailConfigMap[lowerClipName];
        }
        return null;
    }

    private void SetupEventsForClip(AnimationClip clip, TrailConfig config)
    {
        // Mevcut eventleri temizle
        var events = new List<AnimationEvent>();
        foreach (var evt in clip.events)
        {
            if (!evt.functionName.Contains("Trail"))
            {
                events.Add(evt);
            }
        }

        // Trail başlangıç eventi
        var startEvent = new AnimationEvent
        {
            functionName = "EnableTrail",
            stringParameter = config.attackType.ToString(),
            time = clip.length * config.startTime
        };
        events.Add(startEvent);

        // Trail bitiş eventi
        var endEvent = new AnimationEvent
        {
            functionName = "DisableTrail",
            stringParameter = config.attackType.ToString(),
            time = clip.length * config.endTime
        };
        events.Add(endEvent);

        // Eventleri clip'e uygula
        clip.events = events.ToArray();
    }

    public void EnableTrail(string attackType)
    {
        foreach (var config in trailConfigs)
        {
            if (config.attackType.ToString() == attackType)
            {
                config.trail.enabled = true;
                break;
            }
        }
    }

    public void DisableTrail(string attackType)
    {
        foreach (var config in trailConfigs)
        {
            if (config.attackType.ToString() == attackType)
            {
                config.trail.enabled = false;
                break;
            }
        }
    }

    private void DisableAllTrails()
    {
        foreach (var config in trailConfigs)
        {
            if (config.trail != null)
                config.trail.enabled = false;
        }
    }

#if UNITY_EDITOR
    // Editor'da trail zamanlamalarını test etmek için
    private void OnValidate()
    {
        if (Application.isPlaying) return;
        foreach (var config in trailConfigs)
        {
            config.startTime = Mathf.Clamp(config.startTime, 0f, 1f);
            config.endTime = Mathf.Clamp(config.endTime, config.startTime, 1f);
        }
    }
#endif
}