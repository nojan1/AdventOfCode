
let parsePoint (row:string) = 
    let splitPair (pair:string) =
        let parts = pair.Split ','
        printf "%A" parts
        (int (parts.[0].Trim()), int (parts.[1].Trim()))

    let pairs = System.Text.RegularExpressions.Regex.Matches(row, "<(.*?)>")
    (splitPair pairs.[0].Groups.[1].Value, splitPair pairs.[1].Groups.[1].Value)

let update points = 
    points |>
    Array.map (fun (position, velocity) -> 
        ((fst position + fst velocity, snd position + snd velocity), velocity)    
    )

let printPoints points numUpdates =
    let minX = points |> Seq.map fst |> Seq.minBy fst |> fst
    let minY = points |> Seq.map fst |> Seq.minBy snd |> snd
    let maxX = points |> Seq.map fst |> Seq.maxBy fst |> fst
    let maxY = points |> Seq.map fst |> Seq.maxBy snd |> snd

    let coordinates =
        points |>
        Array.map fst |>
        Array.sort    

    for y in minY..maxY do
        for x in minX..maxX do
            if Array.contains (x,y) coordinates then
                System.Console.Write("#")
            else
                System.Console.Write(" ")
        
        System.Console.WriteLine();
    System.Console.WriteLine(numUpdates.ToString())


let rec solve points numUpdates = 
    let updatedPoints = update points
    
    let width = (updatedPoints |> Seq.maxBy (fun (pos, _) -> fst pos) |> fst |> fst) - 
                (updatedPoints |> Seq.minBy (fun (pos, _) -> fst pos)|> fst |> fst)

    //I'm guessing normal terminal width might apply here
    if width < 80 then
        printPoints updatedPoints numUpdates

        let key = System.Console.ReadKey()
        if key.Key <> System.ConsoleKey.Escape then
            solve updatedPoints (numUpdates + 1)
        else
            numUpdates
    else
        solve updatedPoints (numUpdates + 1)

let points = System.IO.File.ReadAllLines("2018/Day10/input.txt") |> Array.map parsePoint

let numUpdates = solve points 1

