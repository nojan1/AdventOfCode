//#load "../testutils.fs"

let solveA (numElves: int) lastWorth = 
    let rec placeMarble (circle: int list) marbles currentMarbleIndex (players: Map<int,int>) currentPlayerIndex =
        match marbles with
        | [] -> players
        | toPlace::restMarbles -> 
            let nextPlayer = (currentPlayerIndex + 1) % players.Count
            
            if toPlace % 23 = 0 then
                let removalIndex = (currentMarbleIndex - 7) % circle.Length
                let newPlayerScore = players.[currentPlayerIndex] + toPlace + circle.[removalIndex]
                let newCircle = circle.[0..removalIndex - 1] @ [toPlace] @ circle.[removalIndex - 1..circle.Length - 1]
                let newPlayers = Map.add currentPlayerIndex newPlayerScore players

                placeMarble newCircle restMarbles removalIndex newPlayers nextPlayer                    
            else
                let (newCircle, newCurrent) =
                    if circle.Length = 1 then
                        (circle @ [toPlace], 1)
                    else
                        let placeAtIndex = (currentMarbleIndex + 2) % circle.Length
                        printf "%i\n" placeAtIndex
                        (circle.[0..placeAtIndex - 1] @ [toPlace] @ circle.[placeAtIndex - 1..circle.Length - 1], placeAtIndex)
                
                placeMarble newCircle restMarbles newCurrent players nextPlayer
                    
    let players = [for i in 0..numElves -> (i,0)] |> Map.ofList
    placeMarble [0] [1..lastWorth] 0 players 0 

//Testing.assertEqual 8317 (solveA 10 1618)
//let solution = solveA 428 70825

solveA 10 1618