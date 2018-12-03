module Day3 

type Claim = {Id: string; Left: int; Top: int; Width: int; Height: int}
let buildClaim (row: string) =
    let m = System.Text.RegularExpressions.Regex.Match(row, @"#(\d*) @ (\d*),(\d*): (\d*)x(\d*)")

    { 
        Id = m.Groups.[1].Value; 
        Left = int m.Groups.[2].Value;
        Top = int m.Groups.[3].Value;
        Width = int m.Groups.[4].Value;
        Height = int m.Groups.[5].Value; 
    }

let claims = Array.map buildClaim (System.IO.File.ReadAllLines("2018/Day3/input.txt"))