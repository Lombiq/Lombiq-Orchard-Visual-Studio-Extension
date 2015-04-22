using System;
using System.Collections.Generic;

namespace Lombiq.CodeReviewer.Prototype.Models
{
    interface ICodeReviewComment
    {
        int Id { get; }

        string Author { get; }

        DateTime CreatedUtc { get; }

        string Comment { get; }

        string CodeFileName { get; }

        int CodeLineNumber { get; }

        IEnumerable<ICodeReviewComment> Answers { get; }
    }
}
