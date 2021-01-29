using System;
using System.Collections.Generic;

namespace SMARTII.Domain.IO
{
    public class FileProcessContext
    {
        public List<string> Paths { get; set; } = new List<string>();
    }

    public class FileProcessInvoker
    {
        public FileProcessInvoker(Action<FileProcessContext> action)
        {
            var context = new FileProcessContext();

            try
            {
                action(context);
            }
            catch (Exception ex)
            {
                ClearLegacy(context);

                throw ex;
            }
        }

        private void ClearLegacy(FileProcessContext context)
        {
            foreach (var path in context.Paths)
            {
                FileUtility.DeleteFile(path);
            }
        }
    }
}