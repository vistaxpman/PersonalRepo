open System
open System.Collections.Generic

let rec Factorial1(x) =
    printfn "x is %d" x
    if x <= 0 then
        1
    else
        x * Factorial1(x - 1)

let Factorial2(x) =
    let rec TailFactorial(x, result) = 
        printfn "x is %d" x
        if x <= 0 then
            result
        else
            let result = x * result
            TailFactorial(x - 1, result)
    TailFactorial(x, 1)

let memoize(f) =
    let cache = new Dictionary<_, _>()
    (
        fun x ->
            match cache.TryGetValue(x) with
            | true, v -> v
            | _ ->
                let v = f(x)
                cache.Add(x, v)
                v
    )

let FactorialMemWrong = memoize Factorial1

let rec FactorialMem = memoize (fun x ->
    printfn "x is %d" x
    if x <= 0 then
        1
    else
        x * FactorialMem(x - 1)
    )

printfn "Recursion"
printfn "%d" (Factorial1(10))
printfn "TailRecursion"
printfn "%d" (Factorial2(10))
printfn "WrongMemRecursion"
printfn "%d" (FactorialMemWrong(10))
printfn "%d" (FactorialMemWrong(11))
printfn "CorrectMemRecursion"
printfn "%d" (FactorialMem(10))
printfn "%d" (FactorialMem(11))

Console.ReadKey() |> ignore