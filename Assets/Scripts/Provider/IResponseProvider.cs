using System.Threading.Tasks;

public interface IResponseProvider
{
    public Task<string> GetResponse(string input = null);
}
