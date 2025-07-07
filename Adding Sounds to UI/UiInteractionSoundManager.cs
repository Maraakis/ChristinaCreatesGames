using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ChristinaCreatesGames.UI
{
    public class UiInteractionSoundsManager : MonoBehaviour
    {
        [Serializable]
        public class SoundFeedback
        {
            public InteractionSoundType soundType;
            public AudioClip soundClip;
        }
        
        [Header("Feedback list")]
        public List<SoundFeedback> soundFeedbacks = new List<SoundFeedback>();
        
        [Header("Audio Setup")]
        public AudioSource audioSource;
        
        
        
        private Dictionary<InteractionSoundType, AudioClip> _soundFeedbacksDictionary = new Dictionary<InteractionSoundType, AudioClip>();
        
        private void Reset()
        {
            audioSource = GetComponent<AudioSource>();
            
            foreach (InteractionSoundType soundType in Enum.GetValues(typeof(InteractionSoundType)))
            {
                soundFeedbacks.Add(new SoundFeedback { soundType = soundType, soundClip = null });
            }
        }

        private void Start()
        {
            foreach (var entry in soundFeedbacks)
            {
                _soundFeedbacksDictionary.Add(entry.soundType, entry.soundClip);
            }
        }

        public void PlaySound(InteractionSoundType soundType, Object senderObject)
        {
            if (!_soundFeedbacksDictionary.TryGetValue(soundType, out var soundClip))
            {
                Debug.LogWarning($"Sound for {soundType} not found!", senderObject);
                return;
            }
            
            if (soundClip == null)
            {
                Debug.Log($"SoundClip for {soundType} is null!", senderObject);
                return;
            }
            
            if (soundType == InteractionSoundType.Unspecified)
                Debug.Log($"{senderObject} plays an unspecified sound.", senderObject);
            
            if (audioSource != null)
                audioSource.PlayOneShot(soundClip);
        }
    }
}
