#!/usr/bin/perl

use strict;
use v5.10;

sub CountVisible {
  my ($grid, $tree, $x0, $x1, $y0, $y1) = @_;
  my $xIncrement = $x0 < $x1 ? 1 : -1;
  my $yIncrement = $y0 < $y1 ? 1 : -1;
  my $count = 0;

  for(my $x = $x0; $x != ($x1 + $xIncrement); $x += $xIncrement) {
    for(my $y = $y0; $y != ($y1 + $yIncrement); $y += $yIncrement) {
      $count++;

      my $otherTree = @{$grid}[$y]->[$x];
      if($otherTree >= $tree) {
        return $count;
      }

    }
  }

  return $count;
}

chomp(my @lines = <>);
my @grid = map { [map {int} split //, $_] } @lines;

my $height = scalar @grid;
my $width = scalar @{@grid[0]};

my $highestScore = 0;

for(my $y = 1; $y < $height - 1; $y++){
  for(my $x = 1; $x < $width - 1; $x++) {
    my ($left, $up, $right, $down) = (
      CountVisible(\@grid, @grid[$y]->[$x], $x - 1, 0, $y, $y),
      CountVisible(\@grid, @grid[$y]->[$x], $x, $x, $y - 1, 0),
      CountVisible(\@grid, @grid[$y]->[$x], $x + 1, $width - 1, $y, $y),
      CountVisible(\@grid, @grid[$y]->[$x], $x, $x, $y + 1, $height - 1)
    ); 

    my $scenicScore = $left * $up * $right * $down;
    $highestScore = $scenicScore > $highestScore ? $scenicScore : $highestScore;
  }
}

print "B: $highestScore \n";