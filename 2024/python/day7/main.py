#
#   Advent of code 2024 day 7
#

from itertools import *
from functools import *
import fileinput, operator


class Equation(object):
    def __init__(self, line):
        targetRaw, termsPart = line.strip().split(":")

        self.target = int(targetRaw.strip())
        self.terms = [int(x) for x in termsPart.strip().split(" ")]

    def isValid(self, operatorList):
        allOperators = product(operatorList, repeat=len(self.terms) - 1)
        for operators in allOperators:
            result = self.terms[0]

            for i in range(1, len(self.terms)):
                result = operators[i - 1](result, self.terms[i])

            if self.target == result:
                return True

        return False


def calculate(equations, operatorList):
    return reduce(
        operator.add, [x.target for x in equations if x.isValid(operatorList)], 0
    )


def concatenate(a, b):
    return int(str(a) + str(b))


print("Running Advent of code 2024 day 7\n")

inputFile = fileinput.input(encoding="utf-8")
equations = [Equation(line) for line in inputFile]

step1Result = calculate(equations, [operator.add, operator.mul])
print(f"Step1: {step1Result}")

step2Result = calculate(equations, [operator.add, operator.mul, concatenate])
print(f"Step2: {step2Result}")
