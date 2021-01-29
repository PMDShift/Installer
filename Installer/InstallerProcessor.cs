using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Installer
{
    public class InstallerProcessor : IDisposable
    {
        GitHubClient client;
        WebClient webClient;

        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        public InstallerProcessor() {
            this.client = new GitHubClient(new ProductHeaderValue("pmdshift-installer"));
            this.webClient = new WebClient();

            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
        }

        public async Task<InstallationPackage> DownloadPackage(InstallationComponent component) {
            string owner = "";
            string repository = "";
            string packageName = "";

            switch (component) {
                case InstallationComponent.Client: {
                        owner = "PMDShift";
                        repository = "CrowClient";
                        packageName = "PMDShift-C-Windows.zip";
                    }
                    break;
                case InstallationComponent.Editor: {
                        owner = "PMDShift";
                        repository = "CrowClient";
                        packageName = "PMDShift-Editors-Windows.zip";
                    }
                    break;
                case InstallationComponent.LegacyClient: {
                        owner = "PMDShift";
                        repository = "Client";
                        packageName = "PMDShift.zip";
                    }
                    break;
            }

            if (!string.IsNullOrEmpty(owner) && !string.IsNullOrEmpty(repository)) {
                var release = await client.Repository.Release.GetLatest(owner, repository);

                var assetPackage = release.Assets.Where(x => x.Name == packageName).FirstOrDefault();

                if (assetPackage != null) {
                    var packageTempFile = Path.GetTempFileName();

                    await webClient.DownloadFileTaskAsync(assetPackage.BrowserDownloadUrl, packageTempFile);

                    return new InstallationPackage(component, packageTempFile);
                }
            }

            return null;
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(e.ProgressPercentage));
        }

        public void Dispose() {
            webClient.Dispose();
        }
    }
}
