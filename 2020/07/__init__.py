def parse(data):
    data = data.replace("bags", "").replace("bag", "").replace(".", "")
    root_bags = dict()

    for l in data.split("\n"):
        root, contains = l.split(" contain ")

        if "no other" in l:
            root_bags[root.strip()] = []
        else:
            bags = map(lambda x: (x[2:].strip(), int(x.strip()[0])), contains.strip().split(","))
            root_bags[root.strip()] = list(bags)

    return root_bags

def part1(data):
    def reduce_bags(bags):
        def count_shiny_bags(bag, count = 0):
            for sub_bag in bags[bag]:
                if sub_bag[0] == "shiny gold":
                    count += 1
                elif sub_bag[0] in bags:
                    count = count_shiny_bags(sub_bag[0], count) 

            return 1 if count >= 1 else 0

        return sum(count_shiny_bags(x, 0) for x in bags)

    bags = parse(data)
    return reduce_bags(bags)

def part2(data):
    def count_recursive(bags, bag):
        count = 0
        if bag[0] in bags:
            if len(bags[bag[0]]) == 0:
                return bag[1]

            count = sum(count_recursive(bags, sub_bag) for sub_bag in bags[bag[0]])
            return bag[1] + (bag[1] * count)

        return bag[1]

    bags = parse(data)
    return count_recursive(bags, ("shiny gold", 1)) - 1


tests = [
    ("07_test1", 4, part1),
    ("07_test2", 126, part2)
]
