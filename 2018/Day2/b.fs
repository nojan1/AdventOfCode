let data = System.IO.File.ReadAllLines("2018/Day2/input.txt")

let findCommons (x:string,y:string) =
    let commons = [for i in 0..x.Length-1 do if x.[i] = y.[i] then yield x.[i]]
    (commons, x, y)

let pairs = seq {for x in data do for y in data do if x <> y then yield x, y}
let bestCommons, left, right = pairs |> Seq.map findCommons |> Seq.sortByDescending (fun (c,_,_) -> c.Length) |> Seq.head
printf "%s, %s => %s" left right (new System.String(Array.ofList(bestCommons)))
