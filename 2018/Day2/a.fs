let data = System.IO.File.ReadAllLines("2018/Day2/input.txt")

let getCounts (str: string) =
    let letterGroupings = 
        str |>
        Seq.groupBy (id)

    let hasThree = letterGroupings |> Seq.exists (fun (c,x) -> Seq.length x = 3)
    let hasTwo = letterGroupings |> Seq.exists (fun (c,x) -> Seq.length x = 2)    

    (hasThree, hasTwo)

let counts = 
    data |> 
    Array.map getCounts

let numThree = counts |> Seq.sumBy (fun (th,tw) -> if th then 1 else 0)
let numTwo = counts |> Seq.sumBy (fun (th,tw) -> if tw then 1 else 0) 
let a = numThree * numTwo