using ElectronAutoUpdateApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronAutoUpdateApi
{
  public interface ICache
  {
    Task<Dictionary<string, PlatformAssetInfo>> GetCacheAsync();
  }
}