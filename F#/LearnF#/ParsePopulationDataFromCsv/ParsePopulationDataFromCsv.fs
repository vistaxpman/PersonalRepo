open System
open System.IO

let ConvertDataRow(csvLine:string) =
    let cells = List.ofSeq(csvLine.Split(','))
    match cells with
    | title::number::_ ->
        let parsedNumber = Int32.Parse(number)
        (title, parsedNumber)
    | _ ->
        failwith "Incorrect data format"

let rec ProcessLines(lines) =
    match lines with
    | [] ->
        []
    | head::tail ->
        let parsedLine = ConvertDataRow(head)
        let restLines = ProcessLines(tail)
        parsedLine::restLines

let rec CalculateSum(rows) =
    match rows with
    | [] ->
        0
    | (_, value)::tail ->
        value + CalculateSum(tail)

[<EntryPoint>]
let Main(args) =
    let lines = List.ofSeq(File.ReadAllLines(args.[0]))
    let data = ProcessLines(lines)
    let sum = float(CalculateSum(data))
    for (title, value) in data do
        let percentage = float(value) / sum * 100.0
        Console.WriteLine("{0, -18}, {1, 8}, ({2})%", title, value, percentage)
    0
