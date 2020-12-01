
def yieldEntries(entries):
    for i in entries:
        for y in entries:
            for z in entries:
                yield (i,y,z)

def part1(data):
    entries = [int(x.strip()) for x in data.split("\n")]
    (a,b) = next((a,b) for (a,b,c) in yieldEntries(entries) if a + b == 2020)
    return a * b

def part2(data):
    entries = [int(x.strip()) for x in data.split("\n")]
    (a,b,c) = next((a,b,c) for (a,b,c) in yieldEntries(entries) if a + b + c== 2020)
    return a * b * c



tests = [
    ("01_test1", 514579, part1),
    ("01_test1", 241861950, part2)
]
