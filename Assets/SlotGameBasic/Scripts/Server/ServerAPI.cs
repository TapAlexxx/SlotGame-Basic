using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

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

        var winResult = UnityEngine.Random.value < 0.7f;
        var finalGrid = winResult
            ? SpinGrids.WinGrids[UnityEngine.Random.Range(0, SpinGrids.WinGrids.Length)]
            : SpinGrids.LoseGrids[UnityEngine.Random.Range(0, SpinGrids.LoseGrids.Length)];
        
        var result = new SpinResult
        {
            win = winResult,
            symbol = "GEM",
            winLine =  1,
            winAmount = 150,
            finalGrid = finalGrid
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

public static class SpinGrids
{
    public static string[][][] WinGrids = new[]
    {
        new[]
        {
            new[] { "GEM","GEM","GEM","GEM","GEM" },
            new[] { "HUMAN","AXE","SNOWMAN","HUMAN","HOOK" },
            new[] { "BOOTS","HOOK","HUMAN","BOOTS","AXE" }
        },
    
        new[]
        {
            new[] { "AXE","AXE","AXE","AXE","AXE" },
            new[] { "HOOK","HUMAN","GEM","SNOWMAN","HUMAN" },
            new[] { "BOOTS","HOOK","GEM","HUMAN","BOOTS" }
        },
    
        new[] 
        {
            new[] { "HUMAN","HUMAN","HUMAN","HUMAN","HUMAN" },
            new[] { "GEM","AXE","SNOWMAN","HOOK","AXE" },
            new[] { "BOOTS","SNOWMAN","HOOK","GEM","SNOWMAN" }
        },
    
        new[]
        {
            new[] { "SNOWMAN","SNOWMAN","SNOWMAN","SNOWMAN","SNOWMAN" },
            new[] { "HOOK","HUMAN","AXE","GEM","HOOK" },
            new[] { "BOOTS","AXE","GEM","HOOK","HUMAN" }
        },
    
        new[]
        {
            new[] { "HOOK","HOOK","HOOK","HOOK","HOOK" },
            new[] { "GEM","AXE","HUMAN","GEM","SNOWMAN" },
            new[] { "BOOTS","HUMAN","SNOWMAN","AXE","GEM" }
        },
    };

    public static string[][][] LoseGrids = new[]
    {
        new[]
        {
            new[] { "GEM","AXE","HUMAN","GEM","SNOWMAN" },
            new[] { "HOOK","HUMAN","GEM","HUMAN","AXE" },
            new[] { "SNOWMAN","AXE","HOOK","GEM","BOOTS" }
        },
    
        new[]
        {
            new[] { "AXE","GEM","AXE","HOOK","HUMAN" },
            new[] { "SNOWMAN","HOOK","SNOWMAN","GEM","AXE" },
            new[] { "GEM","HUMAN","BOOTS","SNOWMAN","GEM" }
        },
    
        new[]
        {
            new[] { "HOOK","SNOWMAN","GEM","HOOK","HUMAN" },
            new[] { "HUMAN","GEM","HUMAN","SNOWMAN","AXE" },
            new[] { "AXE","BOOTS","GEM","HUMAN","BOOTS" }
        },
    
        new[]
        {
            new[] { "BOOTS","HUMAN","GEM","BOOTS","HUMAN" },
            new[] { "SNOWMAN","HOOK","AXE","SNOWMAN","HOOK" },
            new[] { "GEM","AXE","SNOWMAN","AXE","GEM" }
        },
    
        new[] 
        {
            new[] { "HUMAN","GEM","SNOWMAN","AXE","GEM" },
            new[] { "AXE","BOOTS","HUMAN","HOOK","BOOTS" },
            new[] { "GEM","HOOK","AXE","SNOWMAN","HUMAN" }
        },
    };
}