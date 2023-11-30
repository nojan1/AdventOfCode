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

\$inputfile="input";

function step1() {
    //Implementation for step 1 goes here

    return "";
}

function step2() {
    //Implementation for step 2 goes here

    return "";
}

echo "Running Advent of code $YEAR day $1";

//assert(step1() == "")
//assert(step2() == "")

EOF

echo "Successfully bootstrapped day $1 for $YEAR"
echo "To start coding:"
echo "  cd $path"
echo "  php main.php"