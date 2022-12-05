#!/usr/bin/perl

use strict;
use v5.10;
use Data::Dumper;

use Array::Utils qw(:all);

my $countFullOverlap = 0;
my $countPartialOverlap = 0;

while (<>) {
  chomp;
  /(\d*)-(\d*),(\d*)-(\d*)/i;
  my ($a1, $a2, $b1, $b2) = (int($1), int($2), int($3),int($4));

  $countFullOverlap += 1 if(($a1 >= $b1 && $a2 <= $b2) || ($b1 >= $a1 && $b2 <= $a2));
  $countPartialOverlap += 1 if(($a1 >= $b1 && $a1 <= $b2) || ($a2 >= $b1 && $a2 <= $b2) || ($b1 >= $a1 && $b1 <= $a2) || ($b2 >= $a1 && $b2 <= $a2));
}

print("A: $countFullOverlap\n");
print("B: $countPartialOverlap\n");
