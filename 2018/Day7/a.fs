type Requirement = {Target: string; Requires: string} 
let parseRequirement (row: string) =
    let parts = row.Split ' '
    
    { 
        Target = parts.[7].Trim() 
        Requires = parts.[1].Trim() 
    }

let requirements = 
    System.IO.File.ReadAllLines "2018/Day7/input.txt" |>
    Array.map parseRequirement

let findSequence (requirements: Requirement[]) =
    let rec findSequenceInternal (path:string) ending =
        let possibleTargets = 
            requirements |>
            Seq.filter (fun x -> x.Target <> ending && not (path.Contains(x.Target)) && path.Contains(x.Requires)) |>
            Seq.distinctBy (fun x -> x.Target) |>
            Seq.sortBy (fun x -> x.Target) |>
            Seq.toList

        match possibleTargets with
        | [] -> path
        //(| nextTarget::tail -> findSequenceInternal (path + nextTarget.Target) ending                             
        | _ -> possibleTargets |> Seq.fold (fun acc elem -> findSequenceInternal (acc + elem.Target) ending) path

    let start = requirements |> Seq.find (fun x -> requirements |> Seq.forall (fun y -> x.Requires <> y.Target))
    let ending = requirements |> Seq.find (fun x -> requirements |> Seq.forall (fun y -> y.Requires <> x.Target))
    (findSequenceInternal start.Requires ending.Target) + ending.Target
let a = findSequence requirements



let testdata = [|"Step C must be finished before step A can begin.";
"Step C must be finished before step F can begin.";
"Step A must be finished before step B can begin.";
"Step A must be finished before step D can begin.";
"Step B must be finished before step E can begin.";
"Step D must be finished before step E can begin.";
"Step F must be finished before step E can begin."|]
let test = findSequence (testdata |> Array.map parseRequirement)