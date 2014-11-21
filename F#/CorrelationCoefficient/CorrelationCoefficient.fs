open System
open System.Linq

let random = new Random()
let f = fun _ -> float(random.Next(0, 100)) / 100.0
let X = Array.init 10 f
let Y = Array.init 10 f

let correlationCoefficient (X: float[]) (Y: float[]) =
    if (X.Length <> Y.Length) then
        raise (ArgumentException("X should be as long as Y."))

    let meanX = Array.average X
    let meanY = Array.average Y

    let Cov X Y = Array.zip X Y |> Array.sumBy (fun (x, y) -> (x - meanX) * (y - meanY))
    let D = fun X -> X |> Array.sumBy (fun x -> ((x - meanX) * (x - meanX)))

    Cov X Y / Math.Sqrt(D(X) * D(Y))

Console.WriteLine(correlationCoefficient X Y)

0