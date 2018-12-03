#load "common.fs"

let doesNotOverlap (s1:Day3.Claim) (s2:Day3.Claim) =
    s1.Left > (s2.Left + s2.Width) ||
    (s1.Left + s1.Width) < s2.Left ||
    s1.Top > (s2.Top + s2.Height) ||
    (s1.Top + s1.Height) < s2.Top

let nonOverlapping =
    Day3.claims |>
    Seq.filter (fun s1 -> Day3.claims |> Seq.forall (fun s2 -> s1 = s2 || doesNotOverlap s1 s2)) |>
    Seq.head
   
printf "The correct ClaimId is %s\n" nonOverlapping.Id