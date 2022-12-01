#!/usr/bin/perl

$last = -1;
$count = 0;

while (<>) {
  chomp;

  if ($last > 0 && $_ > $last) {
    $count++;
  }

  $last = int($_);
}

print("Got count of: $count\n");
