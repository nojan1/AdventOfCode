#!/usr/bin/perl

use strict;
use v5.30;

use Data::Dumper;
use List::Util qw(min);

sub h {
    my ($grid, $width, $height, $x, $y) = @_;
    return 0;
}

sub d {
    return 1;

    my ($grid, $current, $neighbor) = @_;

    my $currentChar = $grid->[$current->[1]]->[$current->[0]];
    my $neighborChar = $grid->[$neighbor->[1]]->[$neighbor->[0]]; 

    my $currentValue = $currentChar eq "S" ? ord("a") : ($currentChar eq "E" ? ord("z") : ord($currentChar));
    my $neighborValue = $neighborChar eq "S" ? ord("a") : ($neighborChar eq  "E" ? ord("z") : ord($neighborChar));

    my $heightDiff = $currentValue - $neighborValue;
    # say $currentChar, " ", $neighborChar, " ", $currentValue, " ", $neighborValue, " ", $heightDiff;
    return 9999999, if($heightDiff > 1);

    return $heightDiff;
}

sub getNeighbors {
    my ($grid, $width, $height, $x, $y) = @_;

    sub isValid {
        my ($testX, $testY) = @_;

        my $currentChar = $grid->[$y]->[$x];
        my $neighborChar = $grid->[$testY]->[$testX]; 

        my $currentValue = $currentChar eq "S" ? ord("a") : ($currentChar eq "E" ? ord("z") : ord($currentChar));
        my $neighborValue = $neighborChar eq "S" ? ord("a") : ($neighborChar eq  "E" ? ord("z") : ord($neighborChar));

        my $heightDiff = $currentValue - $neighborValue;
        return $heightDiff <= 1;
    }

    my @neighbors = ();

    push @neighbors, [$x - 1, $y] if ($x > 0 && isValid($x - 1, $y));
    push @neighbors, [$x, $y - 1] if ($y > 0 && isValid($x, $y - 1));
    push @neighbors, [$x + 1, $y] if ($x < $width - 1 && isValid($x + 1, $y));
    push @neighbors, [$x, $y + 1] if ($y < $height - 1 && isValid($x, $y + 1)); 

    return \@neighbors;
}

sub FindPathAStar {
   my ($grid, $width, $height, $goalX, $goalY, $startX, $startY) = @_;

    my @openSet = ([$startX, $startY]);
    my %cameFrom = ();

    my %gScore = (
        "$startX,$startY" => 0
    );

    my %fScore = (
        "$startX,$startY" => h($grid, $width, $height, $startX, $startY)
    );        

    while (scalar @openSet) {
        my $current = pop @openSet;
        if($current->[0] == $goalX && $current->[1] == $goalY) {
            return values %cameFrom;
        }

        my $currentKey = "$current->[0],$current->[1]";

        my $neighbors = getNeighbors($grid, $width, $height, $current->[0], $current->[1]);
        for my $neighbor (@$neighbors) {
            my $neighborKey =  "$neighbor->[0],$neighbor->[1]";
            my $tentative_gScore = $gScore{$currentKey} + d($grid, $current, $neighbor);

            say "$neighborKey: $tentative_gScore";

            if(!exists $gScore{$neighborKey} || $tentative_gScore < $gScore{$neighborKey}) {
                $cameFrom{$neighborKey} = $current;
                $gScore{$neighborKey} = $tentative_gScore;
                $fScore{$neighborKey} = $tentative_gScore + h($neighbor);

                push @openSet, $neighbor unless grep {$_->[0] == $neighbor->[0] && $_->[1] == $neighbor->[1]} @openSet;
            }
        }
    }

    return undef;
}

# sub FindPath {
#     my ($grid, $width, $height, $goal, $paths, $visited, $lastChar, $lastDirection, $x, $y) = @_;

#     my $positionString = "$x,$y";

#     my $currentHeight = ord(@{$grid}[$y]->[$x]);
#     if($positionString eq $goal) {
#         $currentHeight = ord("z");
#     }elsif(@{$grid}[$y]->[$x] eq "S") {
#         $currentHeight = ord("a");
#     }

#     my $heightDiff = $currentHeight - ord($lastChar ne "S" ? $lastChar : "a");
#     # say "Got heightdiff $heightDiff, $lastChar @{$grid}[$y]->[$x]";
#     if($heightDiff > 1) {
#         return;
#     }

#     if(grep {$_ eq $positionString} @{$visited}) {
#         # say "Already visitied $positionString";
#         return;
#     }

#     if($positionString eq $goal) {
#         push @$paths, $visited;
#         say "Found the goal";
#         return;
#     }

#     push @$visited, $positionString;

#     FindPath($grid, $width, $height, $goal, $paths, [@$visited], @{$grid}[$y]->[$x], 0, $x - 1, $y) if ($x > 0 && $lastDirection != 2);
#     FindPath($grid, $width, $height, $goal, $paths, [@$visited], @{$grid}[$y]->[$x], 1, $x, $y - 1) if ($y > 0 && $lastDirection != 3);
#     FindPath($grid, $width, $height, $goal, $paths, [@$visited], @{$grid}[$y]->[$x], 2, $x + 1, $y) if ($x < $width - 1 && $lastDirection != 0);
#     FindPath($grid, $width, $height, $goal, $paths, [@$visited], @{$grid}[$y]->[$x], 3, $x, $y + 1) if ($y < $height - 1 && $lastDirection != 1);
# }

chomp(my @lines = <>);
my @grid = map { [split //, $_] } @lines;

my $height = scalar @grid;
my $width = scalar @{@grid[0]};

my @start = ();
my @goal = ();
# my $goal = "";

for(my $y = 0; $y < $height; $y++){
    for(my $x = 0; $x < $width; $x++) {
        if(@grid[$y]->[$x] eq "S") {
            @start = ($x, $y);
        }elsif(@grid[$y]->[$x] eq "E") {
            # $goal = "$x,$y";
            @goal = ($x, $y);
        }
    }
}

my @paths = ();
my @visited = ();

# FindPath(\@grid, $width, $height, $goal, \@paths, \@visited, "a", -1, $start[0], $start[1]);
my @path = FindPathAStar(\@grid, $width, $height, $goal[0], $goal[1], $start[0], $start[1]);

# say Dumper @paths;
# my $numSteps = min map {scalar @$_ }@paths;
# say Dumper @path;
my $numSteps = (scalar @path) - 1;

say "A: $numSteps";