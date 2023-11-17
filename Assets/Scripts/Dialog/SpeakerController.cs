using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    [RequireComponent(typeof(Image))]
    public class SpeakerController : MonoBehaviour, ISpeakerController
    {
        private Image speaker;

        private void Start()
        {
            if (!TryGetComponent(out speaker))
            {
                throw new ArgumentNullException("speaker",
                    "Couldn't find IDialogWindowManager attached to the game object");
            }
        }

        public void UpdateSpeaker(Actor actor)
        {
            if (actor == null)
            {
                speaker.color = new Color(0, 0, 0, 0);
                return;
            }

            speaker.sprite = actor.portrait;
            speaker.color = Color.white;
        }
    }
}