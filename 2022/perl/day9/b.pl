#!/usr/bin/perl

use strict;
use v5.10;

sub clamp {
  my $i = shift(@_);
  if($i < -1) { return -1 ; }
  if($i > 1) { return 1; }
  return $i;
}

my %directionMap = (
  "L" => [-1, 0],
  "U" => [0,- 1],
  "R" => [1, 0],
  "D" => [0, 1]
);

my %positionsVisited = ();
my @positions = ([13,15],[13,15],[13,15],[13,15],[13,15],[13,15],[13,15],[13,15],[13,15],[13,15]);

while (<>) {
  chomp;
  my ($direction, $steps) = split / /;
  $steps = int $steps;

  say "Error $direction not in map" unless exists $directionMap{$direction};

  while($steps--) {
    @positions[0]->[0] += $directionMap{$direction}[0]; 
    @positions[0]->[1] += $directionMap{$direction}[1]; 

    for(my $i = 1; $i < 10; $i++) {
      my $diffX = $positions[$i - 1][0] - $positions[$i][0];
      my $diffY = $positions[$i - 1][1] - $positions[$i][1];

      if(abs($diffX) > 1 && $diffY == 0) {
        @positions[$i]->[0] += clamp $diffX;
      } elsif(abs($diffY) > 1 && $diffX == 0) {
        @positions[$i]->[1] += clamp $diffY; 
      } elsif (abs($diffX) > 1 || abs($diffY) > 1) {
        @positions[$i]->[0] += clamp $diffX;
        @positions[$i]->[1] += clamp $diffY; 
      } 
    }

  
    my $positionHash = join ",", @{$positions[9]};
    $positionsVisited{$positionHash} = ();
  }
}

my $numberOfPositionsVisited = scalar keys %positionsVisited;
say "B: $numberOfPositionsVisited";