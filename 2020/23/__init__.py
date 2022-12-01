def play(cups, rounds):
    current_cup = 0

    for round in range(rounds):
        to_pickup = [cups[(current_cup + i) % len(cups)] for i in range(1, 4)]
        current_label = int(cups[current_cup])
        min_cup = min(int(x) for x in cups)
        max_cup = max(int(x) for x in cups)

        print(f"-- move {round + 1} --")
        print(f"cups: {' '.join([f'({x})' if i == current_cup else x for i,x in enumerate(cups)])}")
        print(f"pick up: {', '.join(to_pickup)}")

        for x in to_pickup:
            cups.remove(x)

        destination = current_label - 1
        
        while str(destination) not in cups:
            destination -= 1

            if destination < min_cup:
                destination = max_cup

        print(f"destination: {destination}")

        place_index = cups.index(str(destination))
        print(place_index)
        for i,x in enumerate(to_pickup):
            cups.insert(place_index + 1 + i, x)

        current_cup = (current_cup + 1) % len(cups)

        input("")

    return cups

def part1(data):
    cups = list(data)
    cups = play(cups, 100)
    print(cups)
    cup_string = "".join(cups)
    return cup_string[cup_string.index("1")+1:]

def part2(data):
    return None

tests = [
    ("23_test1", "67384529", part1),
    # ("<input file name minus txt>", <expected result>, part1),
]
