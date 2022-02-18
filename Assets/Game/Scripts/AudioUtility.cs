using Game.Scripts.Managers;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Scripts
{
    public class AudioUtility
    {
        static AudioManager _audioManager;

        public enum AudioGroups
        {
            DamageTick,
            Impact,
            EnemyDetection,
            Pickup,
            HUDVictory,
            HUDObjective
        }

        public static void CreateSFX(AudioClip clip, Vector3 position, AudioGroups audioGroup, float spatialBlend,
            float rolloffDistanceMin = 1f)
        {
            var impactSfxInstance = new GameObject
            {
                transform =
                {
                    position = position
                }
            };
            AudioSource source = impactSfxInstance.AddComponent<AudioSource>();
            source.clip = clip;
            source.spatialBlend = spatialBlend;
            source.minDistance = rolloffDistanceMin;
            source.Play();

            source.outputAudioMixerGroup = GetAudioGroup(audioGroup);

            TimedSelfDestruct timedSelfDestruct = impactSfxInstance.AddComponent<TimedSelfDestruct>();
            timedSelfDestruct.lifeTime = clip.length;
        }

        public static AudioMixerGroup GetAudioGroup(AudioGroups group)
        {
            if (_audioManager == null)
                _audioManager = GameObject.FindObjectOfType<AudioManager>();

            var groups = _audioManager.FindMatchingGroups(group.ToString());

            if (groups.Length > 0)
                return groups[0];

            Debug.LogWarning("Didn't find audio group for " + group.ToString());
            return null;
        }

        public static void SetMasterVolume(float value)
        {
            if (_audioManager == null)
                _audioManager = GameObject.FindObjectOfType<AudioManager>();

            if (value <= 0)
                value = 0.001f;
            float valueInDb = Mathf.Log10(value) * 20;

            _audioManager.SetFloat("MasterVolume", valueInDb);
        }

        public static float GetMasterVolume()
        {
            if (_audioManager == null)
                _audioManager = GameObject.FindObjectOfType<AudioManager>();

            _audioManager.GetFloat("MasterVolume", out var valueInDb);
            return Mathf.Pow(10f, valueInDb / 20.0f);
        }
    }
}
