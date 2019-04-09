using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Installer
{
    public class DirectoryTools
    {
        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/f81bea37-26f5-44d8-bac4-bc534bbb03b4/c-how-to-check-file-folder-if-writable?forum=netfxbcl
        public static bool CanWriteToDirectory(string directoryPath) {
            if (string.IsNullOrEmpty(directoryPath)) return false;

            if (!Directory.Exists(directoryPath)) {
                try {
                    Directory.CreateDirectory(directoryPath);
                    return true;
                } catch {
                    return false;
                } finally {
                    if (Directory.Exists(directoryPath)) {
                        Directory.Delete(directoryPath);
                    }
                }
            }

            var testFile = Path.Combine(directoryPath, Path.GetRandomFileName());
            try {
                File.Create(testFile).Close();
                return true;
            } catch {
                return false;
            } finally {
                if (File.Exists(testFile)) {
                    File.Delete(testFile);
                }
            }
        }
    }
}
