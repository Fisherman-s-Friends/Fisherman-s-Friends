using System;
using System.Collections.Generic;

namespace Dialog
{
    public interface IChoiceRenderer
    {
        void RenderChoices(IEnumerable<Choice> choices, Action<Choice> callback);
    }
}