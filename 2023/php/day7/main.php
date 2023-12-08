<?php
/*
    Advent of code 2023 day 7

245926187
246572053
246835012
*/

function getJokerHandType($hand, $counts, $actual_counts, $useJokerRule) {
    if(!$useJokerRule) return 0;

    $numJokers = $counts["J"] ?? 0;
        
    // if($hand == "KTJJT") {
    //     echo $numJokers, PHP_EOL;
    //     print_r($counts);
    //     print_r($actual_counts);
    // }

    if($numJokers >= 4 || ($numJokers == 3 && @$actual_counts[2] == 1) || ($numJokers == 2 && @$actual_counts[3] == 1) || ($numJokers == 1 && @$actual_counts[4] == 1)) return 7;
    if($numJokers == 3 || ($numJokers == 1 && @$actual_counts[3] == 1) || ($numJokers == 2 && @$actual_counts[2] >= 1)) return 6;
    if(($numJokers >= 1 && @$actual_counts[3] == 1) || ($numJokers == 1 && @$actual_counts[2] >= 2)) return 5; 
    if(($numJokers == 2 && @$actual_counts[1] >= 1) || ($numJokers == 1 && @$actual_counts[2] >= 1)) return 4;
    if($numJokers > 1 && @$actual_counts[2] >= 1) return 3;
    if($numJokers > 1) return 2;
}

function getHandType($hand, $useJokerRule) {
    $counts = array_count_values(str_split($hand));
    $actual_counts = array_count_values(array_values($counts));

    if(@$actual_counts[5] == 1) return max(7, getJokerHandType($hand, $counts, $actual_counts, $useJokerRule));
    if(@$actual_counts[4] == 1) return max(6, getJokerHandType($hand, $counts, $actual_counts, $useJokerRule));
    if(@$actual_counts[3] == 1 && @$actual_counts[2] == 1) return max(5, getJokerHandType($hand, $counts, $actual_counts, $useJokerRule));
    if(@$actual_counts[3] == 1) return max(4, getJokerHandType($hand, $counts, $actual_counts, $useJokerRule));
    if(@$actual_counts[2] == 2) return max(3, getJokerHandType($hand, $counts, $actual_counts, $useJokerRule));
    if(@$actual_counts[2] == 1) return max(2, getJokerHandType($hand, $counts, $actual_counts, $useJokerRule));
    if(@$actual_counts[1] == 5) return max(1, getJokerHandType($hand, $counts, $actual_counts, $useJokerRule));

    echo "I should never get here!", PHP_EOL;
    return 0;
}

function parse($inputFile, $useJokerRule) {
    return array_map(function($line) use ($useJokerRule){
        list($hand, $bid) = explode(" ", trim($line));
        $handType = getHandType($hand, $useJokerRule);

        // if($useJokerRule)
        //     echo $hand, " => ", $handType , PHP_EOL;

        return array(
            "type" => $handType,
            "bid" => intval($bid),
            "hand" => $hand
        );
    }, file($inputFile));
}

function makeCompareCard($label) {
    return function ($a, $b) use ($label) {
        if($a["type"] != $b["type"]) return $a["type"] <=> $b["type"];

        for($i = 0; $i < strlen($a["hand"]); $i++) {
            $aLabel = strpos($label, $a["hand"][$i]);
            $bLabel = strpos($label, $b["hand"][$i]);

            if($aLabel != $bLabel) return $aLabel <=> $bLabel;
        }

        return 0;
    };
}

function step1($inputFile) {
    $data = parse($inputFile, false);
    usort($data, makeCompareCard("23456789TJQKA"));

    $winnings = array_map(fn($i) => 
        $data[$i]["bid"] * ($i + 1)
    , array_keys($data));
    
    return array_sum($winnings);
}

function step2($inputFile) {
    $data = parse($inputFile, true);
    usort($data, makeCompareCard("J23456789TQKA"));

    $winnings = array_map(fn($i) => 
        $data[$i]["bid"] * ($i + 1)
    , array_keys($data));
    
    return array_sum($winnings);
}

echo "Running Advent of code 2023 day 7\n";

$step1Result = step1("example_input");
assert($step1Result == 6440, "Step 1 returned $step1Result which was incorrect!\n");

$step2Result = step2("example_input");
assert($step2Result == 5905, "Step 2 returned $step2Result which was incorrect!\n");

$step1 = step1("input");
echo "Step1: {$step1}\n";

$step2 = step2("input");
echo "Step2: {$step2}\n";
