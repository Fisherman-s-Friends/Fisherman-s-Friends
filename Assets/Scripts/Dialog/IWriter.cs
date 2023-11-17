using System.Collections;

namespace Dialog
{
    public interface IWriter
    {
        void SkipLine();
        IEnumerator WriteByCharacter(string text);
    }
}