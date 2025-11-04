using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum Symbol
{
    NONE,
    AXE,
    BOOTS,
    GEM,
    HOOK,
    HUMAN,
    SNOWMAN,
}

public class ServerRequest{}
public class ServerResponse{}
public class SpinRequest : ServerRequest
{
    public string path;
    public string userID;
    public int bet;
    public int lines;
}

public class SpinResult : ServerResponse
{
    public bool win;
    public string symbol;
    public int winLine;
    public int winAmount;
    public string[][] finalGrid;
}

public interface IServerAPI
{
    Task<SpinResult> GetSpinResult(SpinRequest request);
}

public sealed class FakeServerAPI : IServerAPI
{
    public async Task<SpinResult> GetSpinResult(SpinRequest request)
    {
        await Task.Delay(300);
        
        var result = new SpinResult
        {
            win = UnityEngine.Random.value > 0.5f,
            symbol = "GEM",
            winLine =  1,
            winAmount = 150,
            finalGrid = new []
            {
                new []{ "GEM", "GEM", "GEM", "GEM", "GEM" },
                new []{ "HUMAN", "SNOWMAN", "HUMAN", "AXE", "SNOWMAN" },
                new []{ "HOOK", "BOOTS", "HUMAN", "HOOK", "BOOTS" }
            }
        };

        return result;
    }
}
public sealed class ServerAPI : IServerAPI
{
    private readonly HttpClient http;

    public ServerAPI()
    {
        http = new HttpClient();
        http.BaseAddress = new Uri("https://myserver.com/api/");
    }
    
    public async Task<SpinResult> GetSpinResult(SpinRequest request)
    {
        var json = JsonUtility.ToJson(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await http.PostAsync(request.path, content);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        var result = JsonUtility.FromJson<SpinResult>(responseJson);

        return result;
    }
}