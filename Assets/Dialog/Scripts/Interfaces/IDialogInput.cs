using System.Collections;

namespace Dialog
{
    public interface IDialogInput
    {
        bool GetInput();
        IEnumerator WaitForInput();
    }
}