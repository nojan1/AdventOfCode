#!/usr/bin/perl

use strict;
use v5.10;

use Data::Dumper;

my %operations = (
  addx => sub {
    my ($cpuStateRef, $operand) = @_;
    $cpuStateRef->{X} += $operand;
  },
  noop => sub { }
);

my %cycleMap = (
  addx => 2,
  noop => 1,
);

my %cpuState = (
  X => 1
);

my $cycles = 0;
my $crtCycle = 0;

while (<>) {
  chomp;
  my ($opcode, $operand) = split / /;
  $operand = int $operand;

  my $subCycle = 0;
  while(++$subCycle <= $cycleMap{$opcode}) {
    $cycles++;

    print $crtCycle >= ($cpuState{X} - 1) && $crtCycle <= ($cpuState{X} + 1) ? "#" : ".";

    if(++$crtCycle == 40) {
      print "\n";
      $crtCycle = 0;
    }
  }

  $operations{$opcode}->(\%cpuState, $operand);
}

