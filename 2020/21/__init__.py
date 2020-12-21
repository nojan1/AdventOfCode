from functools import reduce

def parse_input(data):
    def parse_line(line):
        line = line.replace("contains ", "").replace(",","")
        paranthesis_index = line.index("(")
        return (line[0:paranthesis_index-1].split(" "), line[paranthesis_index+1:-1].split(" "))

    return list(map(parse_line, data.split("\n")))

def find_candidates(parsed_input, all_allergens):
    def intersection(lst1, lst2): 
        return list(set(lst1) & set(lst2)) 

    retval = {}

    for allergene in all_allergens:
        all_ingridience_lists_that_match = reduce(lambda acc, cur: acc + [cur[0]], (x for x in parsed_input if allergene in x[1]), [])
        overlaps = reduce(intersection, all_ingridience_lists_that_match)
        retval[allergene] = overlaps

    return retval

def get_candidates(parsed_input):
    all_allergens = set(reduce(lambda acc,cur: acc + cur[1], parsed_input, []))
    return find_candidates(parsed_input, all_allergens)

def part1(data):
    parsed_input = parse_input(data)
    candidates = get_candidates(parsed_input)

    all_candidates = reduce(lambda acc, cur: acc + cur[1], candidates.items(), [])
    all_ingridience = reduce(lambda acc, cur: acc + cur[0], parsed_input, [])
    return len([x for x in all_ingridience if x not in all_candidates])

def part2(data):
    def assign_ingridient_to_allergene(candidates):
        candidates = list(candidates.items())
        used = []
        assignments = {}
        done = False
        while not done:
            done = True
            for candidate in candidates:
                left_to_use = [x for x in candidate[1] if x not in used]

                if len(left_to_use) == 1:
                    used.append(left_to_use[0])
                    assignments[candidate[0]] = left_to_use[0]
                    done = False
                    break

        return assignments

    parsed_input = parse_input(data)
    candidates = get_candidates(parsed_input)
    assignments = assign_ingridient_to_allergene(candidates)
    return ",".join((assignments[key] for key in sorted(assignments)))

tests = [
    ("21_test1", 5, part1),
    ("21_test1", "mxmxvkd,sqjhc,fvjkl", part2),
]
