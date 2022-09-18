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
        var delaySecs = new Random().NextDouble() * 3.5;
        await Task.Delay(TimeSpan.FromSeconds(delaySecs));
        if (DateTime.Now.Millisecond % 5 == 0) throw new InvalidOperationException("Cannot use about now");

        return new(
            Description : "Castle webapi",
            Version : this.GetType().Assembly.GetName().Version!.ToString(),
            Environment : this.Environment.EnvironmentName
        );
    }
}