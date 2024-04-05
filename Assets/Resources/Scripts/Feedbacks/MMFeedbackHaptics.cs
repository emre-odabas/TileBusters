using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using GameCore.Managers;
using GameCore.Core;

#if NICEVIBRATIONS_INSTALLED
using MoreMountains.NiceVibrations;
#endif

namespace MoreMountains.FeedbacksForThirdParty
{
    [AddComponentMenu("")]
    [FeedbackPath("Haptics")]
    [FeedbackHelp("This feedback lets you trigger haptic feedbacks through the Nice Vibrations asset, available on the Unity Asset Store. You'll need to own that asset and have it " +
        "in your project for this to work.")]
    public class MMFeedbackHaptics : MMFeedback
    {
#if NICEVIBRATIONS_INSTALLED
        [Header("Haptics")]
        public HapticTypes HapticType = HapticTypes.None;
#endif

        protected override void CustomPlayFeedback(Vector3 position, float attenuation = 1.0f)
        {
           
            if (Active && SettingsManager.Instance.m_IsHapticOn)
            {
#if NICEVIBRATIONS_INSTALLED
                MMVibrationManager.Haptic(HapticType);
#if UNITY_EDITOR
                AudioClip _clip = null;
                switch (HapticType)
                {
                    case HapticTypes.Selection:
                        _clip = ProjectSettings.Instance.m_SelectionSound;
                        break;
                    case HapticTypes.Success:
                        _clip = ProjectSettings.Instance.m_SuccessSound;
                        break;
                    case HapticTypes.Warning:
                        _clip = ProjectSettings.Instance.m_WarningSound;
                        break;
                    case HapticTypes.Failure:
                        _clip = ProjectSettings.Instance.m_FailSound;
                        break;
                    case HapticTypes.LightImpact:
                        _clip = ProjectSettings.Instance.m_LightSound;
                        break;
                    case HapticTypes.MediumImpact:
                        _clip = ProjectSettings.Instance.m_MediumSound;
                        break;
                    case HapticTypes.HeavyImpact:
                        _clip = ProjectSettings.Instance.m_HeavySound;
                        break;
                    case HapticTypes.RigidImpact:
                        _clip = ProjectSettings.Instance.m_RigidSound;
                        break;
                    case HapticTypes.SoftImpact:
                        _clip = ProjectSettings.Instance.m_SoftSound;
                        break;
                    case HapticTypes.None:
                        break;
                }
                if (_clip != null)
                {
                    GameObject _object = new GameObject();
                    _object.AddComponent<AudioSource>();
                    _object.GetComponent<AudioSource>().clip = _clip;
                    _object.GetComponent<AudioSource>().Play();
                    Destroy(_object,1);
                }
#endif
#endif
            }
        }
    }
}
