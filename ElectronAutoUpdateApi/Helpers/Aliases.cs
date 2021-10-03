using ElectronAutoUpdateApi.Constants;
using System;
using System.Collections.Generic;

namespace ElectronAutoUpdateApi.Helpers
{
  public static class Aliases
  {
    private static Dictionary<string, List<string>> m_Aliases = new Dictionary<string, List<string>>
    {
      {Platform.DARWIN, new List<string>{"mac", "macos", "osx", "darwin"} },
      {Platform.WINDOWS, new List<string>{"win32", "windows", "win", "latest.yml", "exe"} },
      {Platform.DEBIAN, new List<string>{"debian"} },
      {Platform.FEDORA, new List<string>{"fedora"} },
      {Platform.APPIMAGE, new List<string>{"appimage"} },
      {Platform.DMG, new List<string>{"dmg"} }
    };

    public static string GetPlatform(string platFormName)
    {
      foreach (var platformName in m_Aliases.Keys)
      {
        foreach (var platformAlias in m_Aliases[platformName])
        {
          if (platFormName.Contains(platformAlias, StringComparison.OrdinalIgnoreCase))
          {
            return platformName;
          }
        }
      }

      throw new Exception($"Could not determine a platform based on: {platFormName}");
    }
  }
}
