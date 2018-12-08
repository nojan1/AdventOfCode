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
    let rec findSequenceInternal (path:string) =
        let possibleTargets = 
            requirements |>
            Seq.filter (fun x -> not (path.Contains(x.Target)) && path.Contains(x.Requires)) |>
            Seq.sortBy (fun x -> x.Target) |>
            Seq.toList

        match possibleTargets with
        | [] -> path
        | nextTarget::tail -> findSequenceInternal (path + nextTarget.Target)                               

    let start = requirements |> Seq.find (fun x -> requirements |> Seq.forall (fun y -> x.Requires <> y.Target))
    findSequenceInternal start.Requires

let a = findSequence requirements



let testdata = ["Step C must be finished before step A can begin.";
"Step C must be finished before step F can begin.";
"Step A must be finished before step B can begin.";
"Step A must be finished before step D can begin.";
"Step B must be finished before step E can begin.";
"Step D must be finished before step E can begin.";
"Step F must be finished before step E can begin."]
let test = findSequence (testdata |> Seq.map parseRequirement)