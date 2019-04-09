using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Installer.Steps
{
    public class ExtractStep : AbstractInstallationStep
    {
        public override async Task<StepResult> Execute(InstallationState state) {
            if (!Directory.Exists(state.TargetDirectoryPath)) {
                Directory.CreateDirectory(state.TargetDirectoryPath);
            }

            using (var fileStream = new FileStream(state.Package.Path, FileMode.Open)) {
                using (var zipArchive = new ZipArchive(fileStream)) {
                    foreach (var entry in zipArchive.Entries) {
                        if (!(entry.FullName.EndsWith("/") && string.IsNullOrEmpty(entry.Name))) {
                            state.UpdateStatus($"Extracting: {entry.Name}");

                            string fullEntryPath = Path.Combine(state.TargetDirectoryPath, entry.FullName);

                            if (!Directory.Exists(Path.GetDirectoryName(fullEntryPath))) {
                                Directory.CreateDirectory(Path.GetDirectoryName(fullEntryPath));
                            }

                            using (var entryFileSteam = new FileStream(fullEntryPath, FileMode.Create)) {
                                using (var entryStream = entry.Open()) {
                                    await entryStream.CopyToAsync(entryFileSteam);
                                }
                            }
                        }
                    }
                }
            }

            return StepResult.Success;
        }
    }
}
