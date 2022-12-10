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
my $valuesSum = 0;

while (<>) {
  chomp;
  my ($opcode, $operand) = split / /;
  $operand = int $operand;

  say "Error $opcode not in map" unless exists $cycleMap{$opcode};
  my $subCycle = 0;
  while(++$subCycle <= $cycleMap{$opcode}) {
    $cycles++;
   
    if($cycles == 20 || $cycles == 60 || $cycles == 100 || $cycles == 140 || $cycles == 180 || $cycles == 220) {
      say "Cycle: $cycles, X is: $cpuState{X}";
      $valuesSum += $cycles * $cpuState{X}; 
    }
  }

  $operations{$opcode}->(\%cpuState, $operand);
}

say "A: $valuesSum";

