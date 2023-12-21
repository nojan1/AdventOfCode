<?php
/*
    Advent of code 2023 day 15
*/

function parse($inputFile) {
    return explode(",", trim(file($inputFile)[0]));
}

function do_hash($str) {
    return array_reduce(str_split($str), fn($acc, $cur) => (($acc + ord($cur)) * 17) % 256, 0);
}

function step1($inputFile) {
    $vals = array_map("do_hash", parse($inputFile));
    return array_sum($vals);
}

function step2($inputFile) {
    $instructions = parse($inputFile);
    $boxes = array_fill(0, 256, array());
    
    foreach($instructions as $instruction) {
        preg_match("#(\w*)(.)(\d*)#", $instruction, $matches);
        list($_, $label, $op, $lens) = $matches;
        $boxNum = do_hash($label);

        if($op == "-") {
            $boxes[$boxNum] = array_values(array_filter($boxes[$boxNum], fn($x) => $x[0] != $label));
        }else{
            $wasFound = false;
            for ($i=0; $i < count($boxes[$boxNum]); $i++) { 
                if($boxes[$boxNum][$i][0] == $label) {
                    $boxes[$boxNum][$i][1] = intval($lens);
                    $wasFound = true;
                }
            }

            if(!$wasFound) {
                $boxes[$boxNum][] = array($label, intval($lens));
            }
        }
    }

    $score = 0;
    for ($boxNum=0; $boxNum < count($boxes); $boxNum++) { 
        for($i=0;$i < count($boxes[$boxNum]); $i++) {
            $score += ($boxNum + 1) * ($i + 1) * $boxes[$boxNum][$i][1];
        }
    }

    return $score;
}

echo "Running Advent of code 2023 day 15\n";

$step1Result = step1("example_input");
assert($step1Result == 1320, "Step 1 returned $step1Result which was incorrect!\n");

$step2Result = step2("example_input");
assert($step2Result == 145, "Step 2 returned $step2Result which was incorrect!\n");

$step1 = step1("input");
echo "Step1: {$step1}\n";

$step2 = step2("input");
echo "Step2: {$step2}\n";
