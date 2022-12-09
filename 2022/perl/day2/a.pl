#!/usr/bin/perl

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
    my($opponent, $me) = split(" ", $_, 2);
    $score += GetScoreForPlayedMoved($me);

    if($opponent eq $me) {
      $score += 3;
    } elsif(
        ($opponent eq "C" && $me eq "A") || 
        ($opponent eq "B" && $me eq "C") || 
        ($opponent eq "A" && $me eq "B")) {

      $score += 6;
    }
  }
}

print("A: $score\n");

# print("B: $b\n")