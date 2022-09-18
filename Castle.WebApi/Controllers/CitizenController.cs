[Route(Uris.Citizens)]
public class CitizenController {
    public static readonly BlockingCollection<Citizen> Items = new();
    private static int indexer = 0;

    [HttpPost]
    public Citizen Post([FromBody] CitizenCandidate candidate){
        var index = Interlocked.Increment(ref indexer);
        var citizen = new Citizen(index.ToString(), candidate.Name);
        Items.Add(citizen);
        return citizen;
    }
}