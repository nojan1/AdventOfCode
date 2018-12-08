type Node = {Children: Node list; Metadata: int[]; Length: int}

let parseNodes (data: int[]) =
    let rec parseNodeInternal subIndex : Node =
        let numChildren = data.[subIndex]
        let numMetadata = data.[subIndex + 1]
        
        let children = List.unfold (fun state -> 
            if fst state >= numChildren then 
                None
            else
                let subNode = parseNodeInternal (snd state)
                Some(subNode, ((fst state) + 1, (snd state) + subNode.Length)) ) (0, subIndex + 2)

        let childrenLength = children |> Seq.sumBy (fun x -> x.Length)
        let metadataOffset = subIndex + childrenLength + 2 
        
        {
            Children = children
            Metadata = data.[metadataOffset..metadataOffset + numMetadata - 1]
            Length = 2 + childrenLength + numMetadata
        }

    parseNodeInternal 0

let rec sumMetadata (node:Node) =
    Seq.sum node.Metadata + (Seq.sumBy sumMetadata node.Children)

let rec calculateValueForNode (node: Node) = 
    match node.Children with
    | [] -> Seq.sum node.Metadata
    |_ -> node.Metadata |> 
            Seq.filter (fun x -> x > 0 && x <= node.Children.Length) |>
            Seq.sumBy (fun i -> calculateValueForNode node.Children.[i - 1])

let data = System.IO.File.ReadAllText("2018/Day8/input.txt").Split ' ' |> Array.map int
let rootNode = parseNodes data
let a = sumMetadata rootNode
let b = calculateValueForNode rootNode