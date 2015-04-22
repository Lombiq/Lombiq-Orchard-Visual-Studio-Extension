using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lombiq.CodeReviewer.Prototype.Models
{
    public interface ICreateCodeReviewCommentContext
    {
        int CodeLineNumber { get; set; }

        string Comment { get; set; }
    }
}
