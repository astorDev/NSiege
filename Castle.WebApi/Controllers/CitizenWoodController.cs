[Route(Uris.Citizens + "/{citizenId}/" + Uris.Wood)]
public class CitizenWoodController {
    public static readonly ConcurrentDictionary<string, Wood> Items = new(); 
    
    [HttpPost] // workaround while PatchAsJsonAsync doesn't exists in System.Net.Http.Json
    public async Task<Wood> Patch(string citizenId, [FromBody] WoodModification modification) {
        await Task.Delay(modification.Burn + modification.Craft);
        var wood = Items.GetOrAdd(citizenId, (c) => new Wood(0));
        if (modification.Burn > wood.Amount) throw new InvalidOperationException("Not enough wood");
        var resultWood = new Wood(wood.Amount - modification.Burn + modification.Craft);
        Items[citizenId] = resultWood;
        return resultWood;
    }
}