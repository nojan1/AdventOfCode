#
#   Advent of code 2024 day 5
#

from itertools import *
from functools import *
import fileinput, sys


class Rules(object):
    def __init__(self, inputFile):
        self.pagesToPrint = []
        self.dependencies = {}
        self.reverseDependencies = {}
        self.rules = []

        for line in inputFile:
            if line.strip() == "":
                return

            page, dependent = line.strip().split("|")
            if page in self.dependencies:
                self.dependencies[page].append(dependent)
            else:
                self.dependencies[page] = [dependent]

            if dependent in self.reverseDependencies:
                self.reverseDependencies[dependent].append(page)
            else:
                self.reverseDependencies[dependent] = [page]

            self.rules.append((page, dependent))

    def isCorrectOrder(self, pages):
        for i in range(len(pages)):
            if i > 0 and pages[i] in self.reverseDependencies:
                for before in pages[0:i]:
                    if not before in self.reverseDependencies[pages[i]]:
                        return False

            if i < len(pages) - 1 and pages[i] in self.dependencies:
                for after in pages[i + 1 :]:
                    if not after in self.dependencies[pages[i]]:
                        return False

        return True

    def hasRule(self, rule):
        return rule in self.rules


def parseInput(inputFile):
    rules = Rules(inputFile)

    pagesToPrint = []
    for line in inputFile:
        pagesToPrint.append([x.strip() for x in line.split(",")])

    return (rules, pagesToPrint)


def splitCorrect(rules, pagesToPrint):
    correct = []
    notCorrect = []

    for pages in pagesToPrint:
        if rules.isCorrectOrder(pages):
            correct.append(pages)
        else:
            notCorrect.append(pages)

    return (correct, notCorrect)


def doSum(pages):
    return reduce(
        lambda acc, cur: acc + int(cur[int(len(cur) / 2)]),
        pages,
        0,
    )


def step2(notCorrect, rules):
    def ruleCmp(a, b):
        if rules.hasRule((a, b)):
            return -1

        return 0

    for pages in notCorrect:
        pages.sort(key=cmp_to_key(ruleCmp))

    return doSum(notCorrect)


print("Running Advent of code 2024 day 5\n")

inputFile = fileinput.input(encoding="utf-8")
rules, pagesToPrint = parseInput(inputFile)
correct, notCorrect = splitCorrect(rules, pagesToPrint)

step1Result = doSum(correct)
print(f"Step1: {step1Result}")

step2Result = step2(notCorrect, rules)
print(f"Step2: {step2Result}")
