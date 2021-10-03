using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Linq;

namespace ElectronAutoUpdateApi.Controllers
{
  [Route("api/")]
  [ApiController]
  public class OverviewController : ControllerBase
  {
    private readonly ILogger<OverviewController> m_Logger;
    private readonly ICache m_Cache;

    public OverviewController(ILogger<OverviewController> logger, ICache cache)
    {
      m_Logger = logger;
      m_Cache = cache;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
      try
      {
        var releaseCache = await m_Cache.GetCacheAsync();
        var latestCacheObjects = releaseCache
          .Where(x => x.Value.Assets.Count > 0)
          .Select(platform => new
          {
            Platform = platform.Key,
            Version = platform.Value.Version.ToString(),
            Assets = platform.Value.Assets.Select(asset => asset.Name)
          });

        var asJson = JsonSerializer.Serialize(latestCacheObjects, options: new JsonSerializerOptions { WriteIndented = true });

        return Ok(asJson);
      }
      catch (Exception e)
      {
        return BadRequest($"There was an error retrieving an overview.{Environment.NewLine}{e}");
      }
    }
  }
}