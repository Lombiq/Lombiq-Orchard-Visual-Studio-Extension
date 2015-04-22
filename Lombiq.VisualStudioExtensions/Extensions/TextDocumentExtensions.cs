//using EnvDTE;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Lombiq.VisualStudioExtensions.Extensions
//{
//    public static class TextDocumentExtensions
//    {
//        public static EditPoint FindLineByCriteria(this TextDocument document, Func<string, bool> criteria)
//        {
//            var currentEditPoint = document.StartPoint as EditPoint;
//            var index = 0;

//            while (currentEditPoint != null)
//            {
//                var line = document.Selection.GotoLine(index);
//            }

//            return null;
//        }
//    }
//}
