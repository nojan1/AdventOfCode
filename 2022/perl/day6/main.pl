#!/usr/bin/perl

use strict;
use v5.10;

use Array::Utils qw(unique);

sub Search {
  my ($input, $length) = @_; 

  my @buffer = ();

  for(my $i = 0; $i < length($input); $i++) {
    push @buffer, substr $input, $i, 1;
    shift @buffer if(scalar @buffer > $length);

    my @tmp = unique @buffer;
    if(scalar @tmp == $length) {
      return $i + 1;
    }
  }

  return -1;
}

my ($input) = <>;
chomp $input;

my $a = Search $input, 4;
print("A: $a \n");

my $b = Search $input, 14;
print("B: $b \n");