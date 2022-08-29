namespace Altairis.Services.StaticContent;
public interface IStaticContentStore {

    public Task<string> GetSource(string key);

}
