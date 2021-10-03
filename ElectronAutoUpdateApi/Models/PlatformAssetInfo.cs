using NuGet.Versioning;
using Octokit;
using System.Collections.Generic;

namespace ElectronAutoUpdateApi.Models
{
  public class PlatformAssetInfo
  {
    public SemanticVersion Version { get; set; }

    public List<ReleaseAsset> Assets { get; set; } = new List<ReleaseAsset>();
  }
}