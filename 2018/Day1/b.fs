let numbers = System.IO.File.ReadAllLines("2018/Day1/input.txt") |> Array.map int

let rec findFirstRepeat curValue nextIndex seen =
    let newValue = curValue + numbers.[nextIndex]
    if Set.contains newValue seen then
        newValue
    else
        findFirstRepeat newValue ((nextIndex+1) % numbers.Length) (Set.add newValue seen)

let b = findFirstRepeat 0 0 Set.empty
printf "%i" b