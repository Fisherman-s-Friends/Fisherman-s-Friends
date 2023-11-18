using System.Collections;

namespace Dialog
{
    public interface IDialogueController
    {
        /// <summary>
        /// Start a dialogue
        /// </summary>
        /// <param name="dialogue">Dialogue to start</param>
        /// <returns></returns>
        IEnumerator StartDialog(Dialogue dialogue);
    }
}