from functools import reduce
from itertools import combinations

def get_jolts(data):
    jolts = [int(x) for x in data.split("\n")]
    jolts.append(0)
    jolts.append(max(jolts) + 3)
    jolts.sort()
    return jolts

def part1(data):
    def find_differences(jolts):
        differences = dict()

        for i in range(1, len(jolts)):
            difference = jolts[i] - jolts[i - 1]

            if difference in differences:
                differences[difference] += 1
            else:
                differences[difference] = 1

        return differences

    jolts = get_jolts(data)
    differences = find_differences(jolts)
    return differences[1] * differences[3]

def part2(data):
    def find_count(nums):
        current = 0
        consecutiveOnes = 0
        total = 1
        for num in nums:
            diff = num - current
            if diff == 1:
                consecutiveOnes += 1
            
            if diff == 3:
                if consecutiveOnes == 2:
                    total *= 2
                elif consecutiveOnes == 3:
                    total *= 4
                elif consecutiveOnes == 4:
                    total *= 7

                consecutiveOnes = 0

            current = num

        return total

    jolts = get_jolts(data)
    return find_count(jolts)

tests = [
    ("10_test1", 220, part1),
    ("10_test2", 8, part2),
    ("10_test1", 19208, part2),
    # ("<input file name minus txt>", <expected result>, part1),
]
