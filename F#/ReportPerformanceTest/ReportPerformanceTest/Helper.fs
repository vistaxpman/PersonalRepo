module Helper

open System
open System.IO
open System.Reflection

let private path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
let private file = DateTime.Now.ToString("yyyy-MM-dd")
let fileName = Path.Combine(path, file)