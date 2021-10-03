﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace ElectronAutoUpdateApi
{
  public class GithubConfig : IGithubConfig
  {
    private readonly string m_ProductHeader;
    private readonly string m_Owner;
    private readonly string m_Repository;
    private readonly string m_Token;

    public GithubConfig(ILogger<GithubConfig> logger, IConfiguration configuration)
    {
      try
      {
        m_ProductHeader = configuration["Github:ProductHeader"];
        m_Owner = configuration["Github:Owner"];
        m_Repository = configuration["Github:Repository"];
        m_Token = configuration["APIKey"];

        PropertyInfo[] properties = GetType().GetProperties();
        foreach (PropertyInfo property in properties)
        {
          var propertyValue = property.GetValue(this).ToString();
          if (string.IsNullOrEmpty(propertyValue))
          {
            throw new Exception($"Required configuration property {property.Name} was not set.");
          }
        }
      }
      catch (Exception e)
      {
        throw new Exception($"Failed to initialize Github config.{Environment.NewLine}{e}");
      }

      logger.LogInformation($"Github configuration owner: {m_Owner} & repository: {m_Repository}");
    }

    public string ProductHeader => m_ProductHeader;

    public string Owner => m_Owner;

    public string Repository => m_Repository;

    public string Token => m_Token;
  }
}
