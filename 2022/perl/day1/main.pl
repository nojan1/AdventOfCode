#!/usr/bin/perl

use List::Util qw(max);

@maxes = ();
$current = 0;

while (<>) {
  chomp;

  if($_ == "") {
    push @maxes, $current;
    $current = 0;
  } else {
    $current += int($_);
  }
}

$current += int($_);
push @maxes, $current;

@sortedMaxes = sort {$b <=> $a} @maxes;

print("A: @sortedMaxes[0]\n");

$b = @sortedMaxes[0] + @sortedMaxes[1] + @sortedMaxes[2];
print("B: $b\n")