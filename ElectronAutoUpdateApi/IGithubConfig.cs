namespace ElectronAutoUpdateApi
{
  public interface IGithubConfig
  {
    public string ProductHeader { get; }

    public string Owner { get; }

    public string Repository { get; }

    public string Token { get; }
  }
}
