import re

def parse_input(data):
    rules = {}
    words = []
    parse_mode = 0

    for line in data.split("\n"):
        if parse_mode == 0:
            if line == "":
                parse_mode = 1
                continue

            number, rest = line.split(":")
            rules[number.strip()] = rest.strip()
        else:
            words.append(line)

    return rules, words

def combine_rules(rules):
    def resolve(rule_number):
        rule_string = ""
        has_unmatched_left_paranthesis = False
        for part in rules[rule_number].split(" "):
            if part.isnumeric():
                rule_string += resolve(part)
            elif part == "|":
                rule_string = f"({rule_string})|("
                has_unmatched_left_paranthesis = True
            else:
                rule_string += part.replace('"','')

        return f"{rule_string})" if has_unmatched_left_paranthesis else rule_string

    return f"^{resolve('0')}$"

def part1(data):
    rules, words = parse_input(data)
    combined_rule = combine_rules(rules)
    matching_words = list(x for x in map(lambda x: re.match(combined_rule, x), words) if x != None)
    print(combined_rule)
    print(matching_words)
    return len(matching_words)

def part2(data):
    return None

tests = [
    ("19_test1", 2, part1),
    # ("<input file name minus txt>", <expected result>, part1),
]
