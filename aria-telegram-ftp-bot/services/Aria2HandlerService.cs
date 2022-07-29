using Aria2NET;
using Newtonsoft.Json;
using Aria2NET.Exceptions;

namespace AriaTelegramFtpBot.Services;

public class Aria2HandlerService
{
    private readonly Aria2NetClient _aria2NetClient;
    private string test_gid;
    private string DefaultDownloadDir;
    public Aria2HandlerService()
    {
        DefaultDownloadDir = Environment.GetEnvironmentVariable("DOWNLOAD_DIR");
        string port = Environment.GetEnvironmentVariable("ARIA2_PORT");
        string secret = Environment.GetEnvironmentVariable("ARIA2_SECRET");
        _aria2NetClient = new Aria2NetClient($"http://localhost:{port}/jsonrpc", secret);
    }
    public async Task AddQueue(string link)
    {
        string mid = Guid.NewGuid().ToString();
        string result = await _aria2NetClient.AddUriAsync(new List<string>{ "magnet:?xt=urn:btih:86226b41985a245ac0c9a8946e1e6d6643ff64f3"}, new Dictionary<string, object>{
            { "dir", $"{DefaultDownloadDir}/{mid}"}
        }, 0);
        Console.WriteLine(result);
        test_gid = result;
    }
    public async Task GetStatus()
    {
        GlobalStatResult result = await _aria2NetClient.GetGlobalStatAsync();        
        Console.WriteLine($"{test_gid}, Global Stat===> DownloadSpeed: {result.DownloadSpeed}, NumActive: {result.NumActive}, NumStopped: {result.NumStopped}, NumWaiting: {result.NumWaiting}");

        IList<DownloadStatusResult> dsrlist = await _aria2NetClient.TellAllAsync();
        foreach(var dsr in dsrlist )
        {
            // Console.WriteLine($"이름:{dsr.Bittorrent?.Info?.Name}, 상태:{dsr.Status}, 받은크기/전체크기: {dsr.CompletedLength}/{dsr.TotalLength}, 다운로드속도: {dsr.DownloadSpeed}, 에러:{dsr.ErrorCode?.ToString()}/{dsr.ErrorMessage?.ToString()}");
            Console.WriteLine($"이름:{dsr.Bittorrent?.Info?.Name}({dsr.Gid}), 상태:{dsr.Status}, followed: {dsr.Following?.ToString()} 에러:{dsr.ErrorCode?.ToString()}/{dsr.ErrorMessage?.ToString()}");
            
        }
        // DownloadStatusResult dsr = await _aria2NetClient.TellStatusAsync(test_gid);
        // Console.WriteLine($"이름:{dsr.Bittorrent?.Info?.Name}, 상태:{dsr.Status}, 받은크기/전체크기: {dsr.CompletedLength}/{dsr.TotalLength}, 다운로드속도: {dsr.DownloadSpeed}, 에러:{dsr.ErrorCode?.ToString()}/{dsr.ErrorMessage?.ToString()}");


        // IList<FileResult> fr = await _aria2NetClient.GetFilesAsync(test_gid);
        // foreach( FileResult f in fr)
        // {

        //     Console.Write($"{f.CompletedLength}, {f.Index}, {f.Length}, {f.Path}, {f.Selected},");
        //     foreach( UriResult u in f.Uris)
        //     {
        //         Console.Write($"{u.Uri} - {u.Status},");
        //     }
        //     Console.WriteLine("");
        // }
        

    }

    public async Task Shutdown()
    {
        await _aria2NetClient.ShutdownAsync();
    }

}