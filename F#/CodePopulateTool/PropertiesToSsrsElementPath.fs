open System
open System.Text.RegularExpressions

let input = ""

let matches = Regex.Matches(input, @"(?<=public )(\w*)[\?]? (\w*)")
for m in matches do
    let _type = m.Groups.[1].ToString()
    let _name = m.Groups.[2].ToString()
    let xType = match _type with
        | "string" -> ""
        | "int" -> "(Integer)"
        | "bool" -> "(Boolean)"
        | "double" -> "(Float)"
        | "DateTime" -> "(Date)"

    Console.Write(_name + xType + ", ")