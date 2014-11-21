open System
open System.Text.RegularExpressions

let input = ""

let matches = Regex.Matches(input, @"(?<=public \w*[\?]? )\w*")
for m in matches do
    let name = m.ToString()
    Console.WriteLine(String.Format("{0} = trade.{0},", name))