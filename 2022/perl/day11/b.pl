#!/usr/bin/perl

use strict;
use v5.10;

use List::Util qw(reduce);
use Data::Dumper;
use integer;

my %operations = (
  "+" => sub { return @_[0] + @_[1] },
  "-" => sub { return @_[0] - @_[1] },
  "*" => sub { return @_[0] * @_[1] },
  "/" => sub { return @_[0] / @_[1] },
);

my @monkeys = ();
chomp(my @lines = <>);

while(scalar @lines) { 
  my $line = shift @lines;
  $line =~ /Monkey (\d*):/; 
  my $monkeyNumber = int $1;

  $line = shift @lines;
  $line =~ /Starting items: (.*?)$/; 
  my @startingItems = map {int} split /,/, $1;

  $line = shift @lines;
  $line =~ /new = (\d|\w+) (.) (\d|\w+)$/;
  my @operation = ($1, $2, $3);  

  $line = shift @lines;
  $line =~ /divisible by (\d*)$/; 
  my $divisibleBy = int($1); 

  $line = shift @lines;
  $line =~ /monkey (\d*)$/; 
  my $throwIfTrue = int($1); 

  $line = shift @lines;
  $line =~ /monkey (\d*)$/; 
  my $throwIfFalse = int($1);  

  shift @lines; #Blank line

  my %monkey = (
    NUMBER => $monkeyNumber,
    ITEMS => \@startingItems,
    OPERATION => \@operation,
    DIVISIBLE => $divisibleBy,
    THROWTRUE => $throwIfTrue,
    THROWFALSE => $throwIfFalse
  );

  push @monkeys, \%monkey;
}

my @timesInspected = (0)x(scalar @monkeys);

for(my $round = 0; $round < 10000; $round++) {
  for my $monkey (@monkeys) {
    while(my $item = shift @{$monkey->{ITEMS}}){
      @timesInspected[$monkey->{NUMBER}]++;

      my $operand = $monkey->{OPERATION}->[2] eq "old" ? $item : int( $monkey->{OPERATION}->[2]);
      $item = %operations{$monkey->{OPERATION}->[1]}->($item, $operand);

      my $throwTo = $item % $monkey->{DIVISIBLE} == 0 ? $monkey->{THROWTRUE} : $monkey->{THROWFALSE};
      push @{%monkeys[$throwTo]->{ITEMS}}, $item;
      # say "Throwing $item to monkey $throwTo";
    }
  }

  say "After round ", $round + 1;
  for my $monkey (@monkeys) {
    say "Monkey $monkey->{NUMBER}: ", join ", ", @{$monkey->{ITEMS}}; 
  } 
  say "";
}

say "Inspection amounts ", join ", ", @timesInspected;

my $monkeyBusinessLevel = reduce {$a * $b} 1, (sort {$b <=> $a} @timesInspected)[0,1];
say "A: $monkeyBusinessLevel";