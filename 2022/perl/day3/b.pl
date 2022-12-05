#!/usr/bin/perl

use v5.10;

use strict;

use List::Util qw(reduce);
use Array::Utils qw(:all);

use Data::Dumper;

sub GetPriority {
  my ($x) = substr @_[0], 0, 1;
  
  if($x ge "a") {
    return ord($x) - ord("a") + 1;
  }

  return ord($x) - ord("A") + 27;
}

chomp(my @lines = <>);

my $sum = 0;

while (@lines) {
  my @partition = splice @lines, 0, 3;

  my @rugsackA = split //, @partition[0];
  my @rugsackB = split //, @partition[1];
  my @rugsackC = split //, @partition[2];

  my @commonAB = intersect @rugsackA, @rugsackB;
  my @common = unique intersect @commonAB, @rugsackC;

  $sum += GetPriority @common[0];
}

print "B: $sum\n";