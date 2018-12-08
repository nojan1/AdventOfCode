
let distance (x1,y1) (x2,y2) =
    (abs (x1-x2)) + (abs (y1-y2))

let coordinates = 
    System.IO.File.ReadAllLines("2018/Day6/input.txt") |>
    Array.map (fun l -> 
        let parts = l.Split ','
        (int (parts.[0].Trim()), int (parts.[1].Trim()))
    )

let minX = coordinates |> Seq.minBy fst |> fst
let maxX = coordinates |> Seq.maxBy fst |> fst
let minY = coordinates |> Seq.minBy snd |> snd
let maxY = coordinates |> Seq.maxBy snd |> snd
let finiteCoordinates = 
    coordinates |>
    Seq.filter (fun (x,y) -> x > minX && x < maxX && y > minY && y < maxY)

let ownerMap =
    seq { for x in minX..maxX do for y in minY..maxY do yield (x,y) } |>
    Seq.map (fun (x,y) ->
        let distances = 
            coordinates |> 
            Seq.filter (fun (x2,y2) -> x <> x2 && y <> y2) |> 
            Seq.map (fun (x2,y2) -> ((x2,y2), distance (x,y) (x2,y2))) |>
            Seq.sortBy snd |>
            Seq.toList

        if (snd distances.[0]) = (snd distances.[1]) then
            (0, 0)
        else
            fst distances.[0]              
    ) |>
    Seq.toList

let maxSize = 
    ownerMap |>
    List.countBy (id) |>
    List.maxBy snd |>
    snd

//6631 to high so is 5372
let distances = 
    coordinates |>
    Seq.map (fun (x1,y1) ->
        let friends = [
            for (x2,y2) in coordinates do
                if x1 <> x2 && y1 <> y2 then
                    yield distance (x1,y1) (x2,y2)    
        ]

        ((x1, y1), friends)
    )
