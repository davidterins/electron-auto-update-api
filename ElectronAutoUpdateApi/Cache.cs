using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronAutoUpdateApi.Constants;
using ElectronAutoUpdateApi.Helpers;
using ElectronAutoUpdateApi.Models;
using Microsoft.Extensions.Logging;
using Octokit;
using NuGet.Versioning;

namespace ElectronAutoUpdateApi
{
  public class Cache : ICache
  {
    private readonly Dictionary<string, PlatformAssetInfo> m_CachedAssets;
    private readonly int m_RefreshMinuteInterval = 15;
    private readonly ILogger<Cache> m_Logger;
    private readonly IGithubConfig m_githubConfig;
    private Release m_Latest;
    private DateTime m_LastUpdatedTime;

    public Cache(ILogger<Cache> logger, IGithubConfig githubConfig)
    {
      m_Logger = logger;
      m_githubConfig = githubConfig;

      m_CachedAssets = new Dictionary<string, PlatformAssetInfo>
      {
        { Platform.DARWIN, new PlatformAssetInfo() },
        { Platform.WINDOWS, new PlatformAssetInfo() },
        { Platform.DEBIAN, new PlatformAssetInfo()},
        { Platform.FEDORA, new PlatformAssetInfo() },
        { Platform.APPIMAGE, new PlatformAssetInfo() },
        { Platform.DMG, new PlatformAssetInfo() },
      };
    }

    public async Task<Dictionary<string, PlatformAssetInfo>> GetCacheAsync()
    {
      if (IsOutDated() || m_Latest == null)
      {
        await RefreshLatest();
      }

      return m_CachedAssets;
    }

    private async Task<bool> RefreshLatest()
    {
      GitHubClient client = new GitHubClient(new ProductHeaderValue(m_githubConfig.ProductHeader));

      if (m_githubConfig.Private)
      {
        client.Credentials = new Credentials(m_githubConfig.Token);
      }

      Release latestFetch = await client.Repository.Release.GetLatest(m_githubConfig.Owner, m_githubConfig.Repository);

      if (latestFetch.Prerelease || latestFetch.Draft || !latestFetch.Assets.Any())
      {
        m_Logger.LogInformation($"Latest GitHub version is either PreRelease, Draft or Empty.");
      }

      if (m_Latest != null && m_Latest.TagName == latestFetch.TagName)
      {
        m_LastUpdatedTime = DateTime.Now;
        m_Logger.LogInformation(
          $"Cached version [{m_Latest.TagName}] is the same as latest [{latestFetch.TagName}].");
        return false;
      }

      // Try parse the tag with nuget versioning semantics.
      var versionString = latestFetch.TagName;

      if (versionString.StartsWith("v"))
      {
        versionString = latestFetch.TagName.Substring(1);
      }

      SemanticVersion latestVersion = new SemanticVersionConverter().ConvertFromInvariantString(versionString) as SemanticVersion;

      m_Logger.LogInformation(
         $"Updating cache with newer version [{latestFetch.TagName}].");
      m_Latest = latestFetch;

      foreach (ReleaseAsset asset in m_Latest.Assets)
      {
        var platform = Aliases.GetPlatform(asset.Name);

        if (latestVersion > m_CachedAssets[platform].Version)
        {
          m_CachedAssets[platform].Version = latestVersion;
          m_CachedAssets[platform].Assets.Clear();
        }

        m_CachedAssets[platform].Assets.Add(asset);

        // NUPKG stuff - skip this for now...
        if (asset.Name == "RELEASES") { }
      }

      m_LastUpdatedTime = DateTime.Now;
      m_Logger.LogInformation($"Cached newer version [{latestFetch.TagName}].");
      return false;
    }

    private bool IsOutDated()
    {
      var elapsedMinutesSinceSinceLastFetch = (DateTime.Now - m_LastUpdatedTime).TotalMinutes;

      return elapsedMinutesSinceSinceLastFetch >= m_RefreshMinuteInterval;
    }
  }
}
