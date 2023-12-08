<?php
/*
    Advent of code 2023 day 8
*/

function gcd($a, $b)
{
    if ($b == 0)
    return $a;
    return gcd($b, $a % $b);
}


function lcmofn($arr, $n)
{
    $ans = $arr[0];

    for ($i = 1; $i < $n; $i++)
        $ans = ((($arr[$i] * $ans)) / (gcd($arr[$i], $ans)));

    return $ans;
} 

function parse($inputFile) {
    $lines = array_map("trim", file($inputFile));
    
    $instructions = str_split($lines[0]);
    $navpoints = array();

    for($i = 2; $i < count($lines); $i++) {
        list($location, $destinations) = explode(" = ", $lines[$i]);
        list($l, $r) = explode(", ", substr($destinations, 1, 8));

        $navpoints[$location] = array(
            "L" => $l,
            "R" => $r
        );
    }

    return array($instructions, $navpoints);
}

function follow($current, $instructions, $navpoints, $isEnd) {
    $numSteps = 0;

    while(true) {
        $turn = $instructions[$numSteps++ % count($instructions)];
        $current = $navpoints[$current][$turn];

        if($isEnd($current)) break;
    }

    return $numSteps;
}

function step1($inputFile) {
    list($instructions, $navpoints) = parse($inputFile);
    return follow("AAA", $instructions, $navpoints, fn($x) => $x == "ZZZ");
}

function step2($inputFile) {
    list($instructions, $navpoints) = parse($inputFile);
    $currents = array_values(array_filter(array_keys($navpoints), fn($x) => str_ends_with($x, "A")));
    $steps = array_map(fn($x) => follow($x, $instructions, $navpoints, fn($x) => str_ends_with($x, "Z")), $currents); 

    return lcmofn($steps, count($steps));
}

echo "Running Advent of code 2023 day 8\n";

$step1Result = step1("example_input");
assert($step1Result == 6, "Step 1 returned $step1Result which was incorrect!\n");

$step2Result = step2("example_input2");
assert($step2Result == 6, "Step 2 returned $step2Result which was incorrect!\n");

$step1 = step1("input");
echo "Step1: {$step1}\n";

$step2 = step2("input");
echo "Step2: {$step2}\n";
