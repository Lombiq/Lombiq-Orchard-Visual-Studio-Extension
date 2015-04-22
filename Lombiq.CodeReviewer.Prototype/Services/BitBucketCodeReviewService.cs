using Lombiq.CodeReviewer.Prototype.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lombiq.CodeReviewer.Prototype.Services
{
    public interface IBitBucketCodeReviewService
    {
        void AddComment();

        void RemoveComment();

        IEnumerable<ICodeReviewComment> GetComments();
    }


    public class BitBucketCodeReviewService
    {
    }
}
