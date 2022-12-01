#!/usr/bin/perl

sub get_ints {
  @ints = ();

  while(<>) {
    chomp;
    push(@ints, int($_));
  }

  return @ints;
}

print(scalar(get_ints()), "\n");
