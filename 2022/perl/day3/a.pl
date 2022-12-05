#!/usr/bin/perl

use strict;

use Array::Utils qw(:all);

sub GetPriority {
  my ($x) = substr $_, 0, 1;
  
  if($x ge "a") {
    return ord($x) - ord("a") + 1;
  }

  return ord($x) - ord("A") + 27;
}

my $sum = 0;

while (<>) {
  chomp;

  my $firstHalf = substr $_, 0, length($_) / 2;
  my $secondHalf = substr $_, length($_) / 2;

  my @firstArray = split(//, $firstHalf); 
  my @secondArray = split(//, $secondHalf); 

  my @diff = intersect @firstArray, @secondArray;

  my @priorities = map GetPriority, unique(@diff);

  $sum += @priorities[0];
}

print("A: $sum\n");
