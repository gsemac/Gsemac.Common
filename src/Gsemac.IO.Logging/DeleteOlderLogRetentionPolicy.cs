﻿using System;

namespace Gsemac.IO.Logging {

    public class DeleteOlderLogRetentionPolicy :
        ILogRetentionPolicy {

        // Public members

        public DeleteOlderLogRetentionPolicy(TimeSpan deleteOlderThan) {

            this.deleteOlderThan = deleteOlderThan;

        }

        public void ExecutePolicy(string directoryPath, string searchPattern) {

            if (string.IsNullOrEmpty(searchPattern))
                return;

            TimeSpan timeSinceLastExecution = DateTimeOffset.Now - lastExecutionTime;

            if (timeSinceLastExecution > deleteOlderThan && System.IO.Directory.Exists(directoryPath)) {

                lastExecutionTime = DateTimeOffset.Now;

                foreach (string filePath in System.IO.Directory.GetFiles(directoryPath, searchPattern)) {

                    DateTime creationTime = System.IO.File.GetCreationTime(filePath);

                    if (DateTime.Now - creationTime > deleteOlderThan)
                        System.IO.File.Delete(filePath);

                }

            }

        }

        // Private members

        private readonly TimeSpan deleteOlderThan;
        private DateTimeOffset lastExecutionTime = DateTimeOffset.MinValue;

    }

}