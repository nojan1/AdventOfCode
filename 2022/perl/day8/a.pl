#!/usr/bin/perl

use strict;
use v5.10;

use Data::Dumper;

sub CheckVisible {
  my ($grid, $tree, $x0, $x1, $y0, $y1) = @_;
  my $xIncrement = $x0 < $x1 ? 1 : -1;
  my $yIncrement = $y0 < $y1 ? 1 : -1;

  for(my $x = $x0; $x != ($x1 + $xIncrement); $ x += $xIncrement) {
    for(my $y = $y0; $y != ($y1 + $yIncrement); $y += $yIncrement) {
      if(@{$grid}[$y]->[$x] >= $tree) {
        return 0;
      }
    }
  }

  return 1;
}

chomp(my @lines = <>);
my @grid = map { [map {int} split //, $_] } @lines;

my $height = scalar @grid;
my $width = scalar @{@grid[0]};

my $count = ($height * 2) + ($width * 2) - 4;

for(my $y = 1; $y < $height - 1; $y++){
  for(my $x = 1; $x < $width - 1; $x++) {
    $count++ if (
      CheckVisible(\@grid, @grid[$y]->[$x], 0, $x - 1, $y, $y) ||
      CheckVisible(\@grid, @grid[$y]->[$x], $x, $x, 0, $y - 1) ||
      CheckVisible(\@grid, @grid[$y]->[$x], $x + 1, $width - 1, $y, $y) ||
      CheckVisible(\@grid, @grid[$y]->[$x], $x, $x, $y + 1, $height - 1)
    );
  }
}

print "A: $count \n";