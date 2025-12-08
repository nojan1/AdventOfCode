#
#   Advent of code 2025 day 3
#

import fileinput

def findJoltageSumWithNumberLength(banks, length):
    result = 0

    for bank in banks:
        currentIndex = 0
        currentNumber = ""

        while len(currentNumber) < length:
            highestIndex = currentIndex
            for i in range(currentIndex, len(bank)):
                currentD = int(bank[i])
                if currentD > int(bank[highestIndex]):
                    highestIndex = i

                digitsLeft = length - len(currentNumber)
                if digitsLeft > len(bank) - i - 1:
                    currentIndex = highestIndex + 1
                    break

            currentNumber += bank[highestIndex]
        
        joltage = int(currentNumber)
        result += joltage

    return result

print("Running Advent of code 2025 day 3\n")

inputFile = fileinput.input(encoding="utf-8")
banks = [list(x.strip()) for x in inputFile]

step1Result = findJoltageSumWithNumberLength(banks, 2)
print(f"Step1: {step1Result}")

step2Result = findJoltageSumWithNumberLength(banks, 12)
print(f"Step2: {step2Result}")
