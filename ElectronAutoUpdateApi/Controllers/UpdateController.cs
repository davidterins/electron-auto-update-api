using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using ElectronAutoUpdateApi.Helpers;
using ElectronAutoUpdateApi.Models;
using NuGet.Versioning;
using System.Linq;

namespace ElectronAutoUpdateApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UpdateController : ControllerBase
  {
    private readonly ILogger<UpdateController> m_Logger;
    private readonly ICache m_Cache;
    public UpdateController(ILogger<UpdateController> logger, ICache cache)
    {
      m_Logger = logger;
      m_Cache = cache;
    }

    [HttpGet("{platform}/{version}/{file}")]
    public async Task<IActionResult> GetUpdateAsync(string platform, string version, string file)
    {
      try
      {
        m_Logger.LogInformation($"Requesting file: {file}");

        var requestedFile = file;
        var clientVersion = new SemanticVersionConverter().ConvertFromInvariantString(version) as SemanticVersion;
        var clientPlatform = Aliases.GetPlatform(platform);

        Dictionary<string, PlatformAssetInfo> latestReleaseAssets = await m_Cache.GetCacheAsync();

        PlatformAssetInfo platformSpecificAssetsFromLatestRelease = latestReleaseAssets[clientPlatform];

        if (clientVersion == platformSpecificAssetsFromLatestRelease.Version)
        {
          throw new Exception($"Client version is up to date with latest version for platform: {clientPlatform}.");
        }
        if (!platformSpecificAssetsFromLatestRelease.Assets.Any())
        {
          throw new Exception($"Latest version do not include any assets for {clientPlatform}.");
        }

        var requestedAsset = platformSpecificAssetsFromLatestRelease.Assets.Where(asset => asset.Name == requestedFile).FirstOrDefault();

        if (requestedAsset == null)
        {
          return NotFound($"Could not find asset {requestedFile} in latest release.");
        }

        return Redirect(requestedAsset.Url);
      }
      catch (Exception e)
      {
        m_Logger.LogError($"Failed to get update.{Environment.NewLine}{e}");
        return BadRequest($"Failed to get update.{Environment.NewLine}{e}");
      }
    }
  }
}
