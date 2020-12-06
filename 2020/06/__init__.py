

def parse(data):
    answers = []
    groups = [answers]

    for l in data.split("\n"):
        if l == "":
            answers = []
            groups.append(answers)
        else:
            answers.append(l)

    return groups
    

def part1(data):
    def count_group(answers):
        unique = set()
        for a in answers:
            for x in a:
                unique.add(x)

        return len(unique)
        

    groups = parse(data)
    counts = map(count_group, groups)
    return sum(counts)

def part2(data):
    def count_group(answers):
        counts = dict()
        for a in answers:
            for x in a:
                if x in counts:
                    counts[x] += 1
                else:
                    counts[x] = 1

        return len([x for x in counts if counts[x] == len(answers)])

    
    groups = parse(data)
    counts = map(count_group, groups)
    return sum(counts)

tests = [
    ("06_test1", 11, part1),
    ("06_test1", 6, part2),
]
