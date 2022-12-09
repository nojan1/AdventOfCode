#!/usr/bin/perl

use strict;
use v5.10;

use Data::Dumper;

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
my @headPosition = (0,4);
my @tailPosition = (0,4);

while (<>) {
  chomp;
  my ($direction, $steps) = split / /;
  $steps = int $steps;

  say "Error $direction not in map" if !exists $directionMap{$direction};

  while($steps--) {
    @headPosition[0] += $directionMap{$direction}[0]; 
    @headPosition[1] += $directionMap{$direction}[1]; 

    my $diffX = $headPosition[0] - $tailPosition[0];
    my $diffY = $headPosition[1] - $tailPosition[1];

    if(abs($diffX) > 1 && $diffY == 0) {
      @tailPosition[0] += clamp $diffX;
    } elsif(abs($diffY) > 1 && $diffX == 0) {
      @tailPosition[1] += clamp $diffY; 
    } elsif (abs($diffX) > 1 || abs($diffY) > 1) {
      @tailPosition[0] += clamp $diffX;
      @tailPosition[1] += clamp $diffY; 
    } 

    my $positionHash = join ",", @tailPosition;
    $positionsVisited{$positionHash} = ();
  }
}

my $numberOfPositionsVisited = scalar keys %positionsVisited;
say "A: $numberOfPositionsVisited";