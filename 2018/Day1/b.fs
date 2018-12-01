let numbers = System.IO.File.ReadAllLines("2018/Day1/input.txt") |> Array.map int

let rec findFirstRepeat curValue nextIndex seen =
    let newValue = curValue + numbers.[nextIndex]
    if Seq.contains newValue seen then
        newValue
    else
        findFirstRepeat newValue ((nextIndex+1) % numbers.Length) (newValue::seen)

let b = findFirstRepeat 0 0 []
printf "%i" b