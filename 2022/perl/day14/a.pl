#!/usr/bin/perl

use strict;
use v5.10;

use Data::Dumper;
use List::Util qw(min max);

my @rockCoords = ();
my %world = ();

sub dumpToFile {
  my $minX = min map {$_->[0]} @rockCoords;
  my $minY = 0;
  my $maxX = max map {$_->[0]} @rockCoords;
  my $maxY = max map {$_->[1]} @rockCoords;

  open(FH, ">", "out.txt") or die $!;
  for (my $y = $minY; $y <= $maxY; $y++) {
    for (my $x = $minX; $x <= $maxX; $x++) {
      my $key = "$x,$y";
      if($y == 0 && $x == 500) {
        print FH "+";
      }
      elsif(exists $world{$key}) {
        print FH $world{$key};
      }else {
        print FH ".";
      }
    }  
    print FH "\n";
  }
  close(FH);
}

while(<>){
  chomp;
  my @coordinates = split / -\> /;
  my @start = map {int} split /,/, pop @coordinates;

  while(@coordinates) {
    my @end = map {int} split /,/, pop @coordinates;

    my $travelX = $end[0] - $start[0];  
    my $travelY = $end[1] - $start[1];

    if($travelY == 0) {
      for(my $x = $start[0]; $x <= $end[0]; $x++) {
        push @rockCoords, [$x, $start[1]];

        my $key = "$x,$start[1]";
        $world{$key} = "#";
      }
    }else {
      my $increment = $travelY < 0 ? -1 : 1;
      for(my $y = $start[1]; $y != ($end[1] + $increment); $y += $increment) {
        push @rockCoords, [$start[0], $y];

        my $key = "$start[0],$y";
        $world{$key} = "#";
      }
    }

    @start = @end;
  }
}

my $triggerY = (max map {$_->[1]} @rockCoords) + 5;
my $numSands = 0;
while (1){
  my @sandPos = (500,0);
  
  sub tryPos {
    my ($sandPos, $incrementX, $incrementY) = @_;

    my $x = $sandPos->[0] + $incrementX;
    my $y = $sandPos->[1] + $incrementY;

    if($y >= $triggerY) {
      # Must be free falling....
      say "A: $numSands";
      dumpToFile();
      exit;
    }

    my $key = "$x,$y";
    
    say "trying new position $key, it contains: $world{$key}. Current $sandPos->[0],$sandPos->[1]";
    if(!exists $world{$key}){
      my $deleteKey = "$sandPos->[0],$sandPos->[1]";
      delete($world{$deleteKey});

      $sandPos->[0] = $x;
      $sandPos->[1] = $y;
      $world{$key} = "o";

      return 1;
    } 

    return 0;
  }

  while(1) {
    if(!tryPos(\@sandPos, 0, 1) and !tryPos(\@sandPos, -1, 1) and !tryPos(\@sandPos, 1, 1)) {
      # say "Sand $numSands settled";
      $numSands++;
      last;
    }

  }

  # last;
}

dumpToFile();
# say Dumper @rockCoords;

say "A: $numSands";