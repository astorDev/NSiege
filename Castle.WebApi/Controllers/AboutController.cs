[Route(Uris.About)]
public class AboutController
{
    public IHostEnvironment Environment { get; }
    public AboutController(IHostEnvironment environment) {
        this.Environment = environment;
    }

    [HttpGet]
    public async Task<About> GetAbout()
    {
        return new(
            Description : "Castle webapi",
            Version : this.GetType().Assembly.GetName().Version!.ToString(),
            Environment : this.Environment.EnvironmentName
        );
    }
}