
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

let ownerMap =
    seq { for x in minX..maxX do for y in minY..maxY do yield (x,y) } |>
    Seq.filter (fun (x,y) -> not (Array.contains (x,y) coordinates)) |>
    Seq.map (fun (x,y) ->
        let distances =
            coordinates |>
            Seq.map (fun (x2,y2) -> ((x2,y2), distance (x,y) (x2,y2))) |>
            Seq.sortBy snd |>
            Seq.toList

        if (snd distances.[0]) = (snd distances.[1]) then
            (0, 0)
        else
            fst distances.[0]
    ) |>
    Seq.toList

let a =
    ownerMap |>
    List.countBy (id) |>
    List.maxBy snd |>
    snd |>
    (+) 1 //Include the coordinate itself

let b = 
    seq { for x in minX..maxX do for y in minY..maxY do yield (x,y) } |>
    Seq.filter (fun (x,y) ->
        let totalDistance = 
            coordinates |>
            Seq.map (fun (x2,y2) -> distance (x,y) (x2,y2)) |>
            Seq.sum

        totalDistance < 10000
    ) |>
    Seq.length