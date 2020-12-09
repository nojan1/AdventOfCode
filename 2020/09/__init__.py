from itertools import combinations

PREAMBLE_LENGTH = 25

def is_valid(preamble, digit):
    return any(x for x in combinations(preamble, 2) if x[0] + x[1] == digit)

def find_bad_number(digits):
    for i,digit in enumerate(digits):
        if i < PREAMBLE_LENGTH:
            continue

        if not is_valid(digits[i-PREAMBLE_LENGTH:i], digit):
            return digit

    return None

def part1(data):
    digits = [int(x) for x in data.split("\n")]
    return find_bad_number(digits)

def part2(data):
    digits = [int(x) for x in data.split("\n")]
    bad_number = find_bad_number(digits)

    def find_set(index):
        history = [digits[index]]
        while True:
            index += 1
            history.append(digits[index])
            current_sum = sum(history)

            if current_sum == bad_number:
                return (len(history), min(history), max(history))
            elif current_sum > bad_number:
                return (0,0,0)

    for i,_ in enumerate(digits):
        count, lower, higher = find_set(i)

        if count >= 2:
            return lower + higher

    return None


tests = [
    ("09_test1", 127, part1),
    ("09_test1", 62, part2),
    # ("<input file name minus txt>", <expected result>, part1),
]
