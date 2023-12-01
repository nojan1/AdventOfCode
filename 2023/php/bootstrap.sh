#!/bin/bash

AOC=/opt/homebrew/bin/aoc
YEAR=2023

[ -z "$1" ] && echo "Usage: $0 <day>" && exit 1

path="./day$1"
[ -d "$path" ] && echo "Day folder $1 already exists, can't bootstrap" && exit 1

mkdir $path
cd $path
$AOC -y $YEAR -d $1 download

cat > main.php <<EOF
<?php
/*
    Advent of code $YEAR day $1
*/

function step1(\$inputFile) {
    //Implementation for step 1 goes here

    return "";
}

function step2(\$inputFile) {
    //Implementation for step 2 goes here

    return "";
}

echo "Running Advent of code $YEAR day $1\n";

//\$step1Result = step1("example_input");
//assert(\$step1Result == "", "Step 1 returned \$step1Result which was incorrect!\n");

//\$step2Result = step2("example_input");
//assert(\$step2Result == "", "Step 2 returned \$step2Result which was incorrect!\n");

\$step1 = step1("input");
echo "Step1: {\$step1}\n";

\$step2 = step2("input");
echo "Step2: {\$step2}\n";
EOF

echo "Successfully bootstrapped day $1 for $YEAR"
echo "To start coding:"
echo "  cd $path"
echo "  php main.php"