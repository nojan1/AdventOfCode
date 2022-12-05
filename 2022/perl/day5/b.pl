#!/usr/bin/perl

use strict;
use v5.10;

use Data::Dumper;
use List::Util qw(reduce);

chomp(my @lines = <>);

my ($dividerIndex) = grep { $lines[$_] eq "" } (0 .. @lines-1);
my @stackLine = split //, @lines[$dividerIndex - 1];
my $numStacks =  reduce { $a > $b ? $a : $b } @stackLine;

my @stacks = ();
push @stacks, [] while $numStacks--;

for(my $i = $dividerIndex - 2; $i >= 0; $i--) {
  my $lineLength = length(@lines[$i]);
  for(my $x = 1; $x < $lineLength; $x+=4) {
    my $crate = substr(@lines[$i], $x, 1); 
    next if($crate eq " ");

    my $arrayIndex = $x / 4; 
    push @{@stacks[$arrayIndex]}, $crate;
  }
}

for(my $i = $dividerIndex + 1; $i < scalar(@lines); $i++) {
  @lines[$i] =~ /move (\d*) from (\d*) to (\d*)/;
  my ($count, $from, $to) = (int($1), int($2), int($3));

  my @items = splice @{@stacks[$from - 1]}, -$count;
  push @{ @stacks[$to - 1] }, @items;
}

my $message = reduce { $a . ($b->[-1]) } "", @stacks;
print("B: $message \n");