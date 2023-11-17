using System.Collections;

namespace Dialog
{
    public interface IDialogueController
    {
        IEnumerator StartDialog(Dialogue dialogue);
    }
}