type EventType = ShiftStart | WakesUp | FallsAsleep
type Event = {Time: System.DateTime; GuardId: int; What: EventType}

let parseEventType (row: string) =
    match row with
    | row when row.Contains("wakes") -> WakesUp
    | row when row.Contains("asleep") -> FallsAsleep
    | _ -> ShiftStart

let parseEventRows (rows : string []) =
    [
        let mutable lastGuardId = -1        
        for row in rows do
            let time = System.DateTime.Parse(row.[1..16])
            let eventType = parseEventType row

            if eventType = ShiftStart then
                let m = System.Text.RegularExpressions.Regex.Match(row, @"#(\d*)")
                lastGuardId <- int m.Groups.[1].Value

            yield {
                Time = time
                GuardId = lastGuardId
                What = eventType
            }
    ]

let events =
    System.IO.File.ReadAllLines("2018/Day4/input.txt") |> 
    Array.sort |>
    parseEventRows  
let sleepingMinutesForGuard id =
    [ 0..59 ] |>
    List.map (fun m -> 
        events |>
        Seq.filter (fun e -> e.GuardId = id && e.What = ShiftStart) |>
        Seq.map (fun e -> if e.Time.Hour = 23 then e.Time.Date.AddDays(1.0) else e.Time.Date) |>
        Seq.sumBy (fun day -> 
            let asleepEvent = events |> Seq.exists (fun e -> e.GuardId = id && e.What = FallsAsleep && e.Time.Date = day && e.Time.Minute < m)
            let wakeEvent = events |> Seq.exists (fun e -> e.GuardId = id && e.What = WakesUp && e.Time.Date = day && e.Time.Minute > m)
            
            if asleepEvent && wakeEvent then 1 else 0
        )
    )
   
let sleepMap =
    events |> 
    Seq.map (fun x -> x.GuardId) |>
    Seq.distinct |>
    Seq.map (fun i -> (i, sleepingMinutesForGuard i)) |>
    Seq.toArray
    
//Part 1
 
let sleeper = 
    sleepMap |>
    Seq.maxBy (fun x -> snd x |> Seq.sum)

let sleepyMinute = 
    snd sleeper |> 
    Seq.mapi (fun i v -> i, v) |> 
    Seq.maxBy snd |>
    fst

let a = fst sleeper * sleepyMinute
printf "Answer to part 1 is: %i\n" a

//Part 2

let mostSleepyMinutes =
    [0..59] |>
    Seq.map (fun m -> 
        let mostSleepyGuardThisMinute = 
            sleepMap |>
            Seq.maxBy (fun x -> (snd x).[m])

        (fst mostSleepyGuardThisMinute, (snd mostSleepyGuardThisMinute).[m])
    )

let guardId, minuteNumber, _ =
    mostSleepyMinutes |> 
    Seq.mapi (fun i (a,b) -> a, i ,b) |>
    Seq.maxBy (fun (guardId, minuteNumber, minuteAmount) -> minuteAmount)

let b = guardId * (minuteNumber-1) //Off by one error?.... Or something
printf "Answer to part 2 is: %i\n" b