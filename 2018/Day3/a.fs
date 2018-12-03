#load "common.fs"

let usages = 
    Day3.claims |>
    Array.collect (fun c -> [|
        for x in c.Left..(c.Left + c.Width - 1) do
            for y in c.Top..(c.Top + c.Height - 1) do
                yield x,y
    |]) |>
    Array.groupBy (id)

let numCollisions = usages |> Array.filter (fun (k,d) -> d.Length > 1) |> Array.length

printf "There are %i overlapping squareinches\n" numCollisions