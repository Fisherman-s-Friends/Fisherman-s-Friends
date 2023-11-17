using System.Collections;
using System.Collections.Generic;

namespace Dialog
{
    public interface IChoiceController
    {
        Choice SelectedChoice { get; }

        void AskChoices(Dialogue dialogue);

        IEnumerator WaitForSelection();
    }
}