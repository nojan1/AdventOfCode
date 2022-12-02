#!/usr/bin/perl

use strict;

my $score = 0;

sub GetScoreForPlayedMoved {
  my ($move) = @_;
  my %scoremap = (
    "A" => 1,
    "B" => 2,
    "C" => 3
  );
  
  return $scoremap{$move};
}

while (<>) {
  chomp;

  if($_  ne "") {
    $_ =~ tr/XYZ/ABC/;
    my($opponent, $desired_outcome) = split(" ", $_, 2);

    if($desired_outcome eq "A") {
      # Loose
      my %loosemap = (
        "A" => "C",
        "B" => "A",
        "C" => "B"
      );

      $score += GetScoreForPlayedMoved($loosemap{$opponent});
    } elsif ($desired_outcome eq "B") {
      # Draw
      $score += GetScoreForPlayedMoved($opponent); 
      $score += 3;
    } else {
      # Win
      $score += 6;

      my %winmap = (
        "A" => "B",
        "B" => "C",
        "C" => "A"
      );

      $score += GetScoreForPlayedMoved($winmap{$opponent});
    }   
  }
}

print("B: $score\n");