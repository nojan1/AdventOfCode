<?php
/*
    Advent of code 2023 day 2
*/

function parseInput($inputFile) {
    return array_reduce(file($inputFile), function($acc, $line){
        $parts = explode(":", $line);
        $games = explode("; ", trim($parts[1]));

        $counts = array_map(function($game){
            return array_reduce(explode(", ", $game), function($acc, $g){
                $gp = explode(" ", $g);
                $acc[$gp[1]] = intval($gp[0]);
                return $acc;
            }, array());
        }, $games);

        $acc[substr($parts[0], 5)] = $counts;
        return $acc;
    }, array());
}

function step1($inputFile) {
    $games = parseInput($inputFile);

    $possibleKeys = array_filter(array_keys($games), function($gameNum) use ($games){
        $game = $games[$gameNum];

        foreach($game as $round) {
           foreach($round as $color => $count) {
            if(($color == "red" && $count > 12) ||
               ($color == "green" && $count > 13) ||
               ($color == "blue" && $count > 14)) {
                return false;
               }
           }
        }

        return true;
    });

    return array_sum($possibleKeys);
}

function step2($inputFile) {
    $games = parseInput($inputFile);

    $powers = array_map(function($game){
        $lowest = array_reduce($game, function($acc, $round) {
            foreach($round as $color => $count) {
                if(!array_key_exists($color, $acc) || $count > $acc[$color]) {
                    $acc[$color] = $count;
                }
            }

            return $acc;
        }, array());
        
        return array_reduce(array_values($lowest), function($acc, $cur) {
            return $acc * $cur;
        }, 1);
    }, $games);

    return array_sum($powers);
}

echo "Running Advent of code 2023 day 2\n";

$step1Result = step1("example_input");
assert($step1Result == 8, "Step 1 returned $step1Result which was incorrect!\n");

$step2Result = step2("example_input");
assert($step2Result == 2286, "Step 2 returned $step2Result which was incorrect!\n");

$step1 = step1("input");
echo "Step1: {$step1}\n";

$step2 = step2("input");
echo "Step2: {$step2}\n";
