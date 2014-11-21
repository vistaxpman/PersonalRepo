open System
open System.Net

let AsyncRequestWeb (requestUri: Uri) =
    async {
        let webRequest = WebRequest.Create(requestUri)
        use! webResponse = webRequest.AsyncGetResponse()
        Console.WriteLine("Content length: {0}", webResponse.ContentLength)
    }

[<EntryPoint>]
let Main(args) =
    let _, uri = Uri.TryCreate("http://Wintellect.com/", UriKind.Absolute);
    AsyncRequestWeb uri |> Async.RunSynchronously

    Console.ReadLine() |> ignore;
    0
