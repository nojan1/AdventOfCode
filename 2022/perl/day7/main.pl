#!/usr/bin/perl

use strict;
use v5.10;

use List::Util qw(min);

my @paths = ();
my @knownDirectories = ("/");
my @files = ();
my $inListOutput = 0;

while(<>) {
  chomp;
  
  if(/^\$(.*)/) {
    $inListOutput = 0;
    my ($x, $command, $parameter) = split / /, $1;
    
    if($command eq "ls") {
      $inListOutput = 1;
    }elsif($command eq "cd") {
      if($parameter eq "..") {
        pop @paths;
      }else{
        push @paths, $parameter;
      }
    }
  }elsif($inListOutput) {
    my ($derp, $filename) = split / /, $_;
    my $cwd = (join "/", @paths);

    if($derp ne "dir") {
      my $fileEntry = {
        NAME => $filename,
        PARENT => $cwd,
        SIZE => int($derp)
      };

      push @files, $fileEntry;
    }else {
      push @knownDirectories, $cwd . "/$filename";

      my $dirEntry = {
        NAME => $cwd . "/$filename",
        PARENT => $cwd,
        SIZE => -1
      };

      push @files, $dirEntry;
    } 
  }
}

sub SumDirectorySize {
  my ($files, $directory) = @_;
  my $size = 0;
  
  for my $entry (@{$files}) {
    if($entry->{PARENT} eq $directory) {
      if($entry->{SIZE} == -1) {
        $size += SumDirectorySize($files, $entry->{NAME});
      }else{
        $size += $entry->{SIZE};
      }
    }
  }

  return $size;
}


my @directorySizes = ();
my $dirSizeSum = 0;
my $rootSize = 0;
for my $dir (@knownDirectories) {
  my $size = SumDirectorySize \@files, $dir;
  if($size <= 100000) {
    $dirSizeSum += $size;
  }

  if($dir eq "/") {
    $rootSize = $size;
  }else{
    push @directorySizes, $size;
  }
}

print "A: $dirSizeSum \n";

my $requiredSpace = 30000000 - (70000000 - $rootSize);
my $minSize = min grep { $_ > $requiredSpace} @directorySizes;

print "B: $minSize \n"