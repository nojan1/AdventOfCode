
let rec reducePolymer (polymer:string) =
    let pairIndex = 
        Seq.pairwise polymer |>
        Seq.tryFindIndex (fun (a,b) -> abs (int a - int b) = 32)

    match pairIndex with
    | None -> polymer.Trim()
    | Some i -> reducePolymer (String.concat "" [polymer.Substring(0, i); polymer.Substring(i + 2)])

let stripPolymer polymer unit =
    System.Text.RegularExpressions.Regex.Replace(polymer, (string unit), "", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

let rawPolymer = System.IO.File.ReadAllText("2018/Day5/input.txt") 
let polymer = reducePolymer rawPolymer
let a = Seq.length polymer

let b =
    seq {'a'..'z'} |>
    Seq.map (fun c -> reducePolymer (stripPolymer polymer c)) |>
    Seq.map Seq.length |>
    Seq.min