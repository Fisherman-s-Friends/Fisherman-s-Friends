namespace Dialog
{
    public interface IDialogWindowManager
    {
        void ToggleDialogWindow(bool visible);
        void ShowDialogBox();
        void ShowChoicesBox();
    }
}