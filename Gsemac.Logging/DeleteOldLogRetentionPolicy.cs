using System;
using System.Collections.Generic;
using System.Text;

namespace Gsemac.Logging {

    public class DeleteOldLogRetentionPolicy :
        ILogRetentionPolicy {

        // Public members

        public DeleteOldLogRetentionPolicy(TimeSpan deleteOlderThan) {

            this.deleteOlderThan = deleteOlderThan;

        }

        public void ExecutePolicy(string directoryPath, string searchPattern = "*") {

            if (System.IO.Directory.Exists(directoryPath)) {

                foreach (string filePath in System.IO.Directory.GetFiles(directoryPath, searchPattern)) {

                    DateTime creationTime = System.IO.File.GetCreationTime(filePath);

                    if (DateTime.Now - creationTime > deleteOlderThan)
                        System.IO.File.Delete(filePath);

                }

            }

        }

        // Private members

        private readonly TimeSpan deleteOlderThan;

    }

}