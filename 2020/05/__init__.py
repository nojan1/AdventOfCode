import math

def partition(instructions, original_value):
    size = original_value
    upper = original_value - 1
    lower = 0

    for i in instructions:
        size = math.ceil(size / 2)

        if i == "R" or i == "B":
            lower += size
        else:
            upper -= size 

    return lower

def get_seat_id(pass_string):
    row = partition(pass_string[0:7], 128)
    column = partition(pass_string[7:11], 8)
    return (row * 8) + column

def get_seat_ids(data):
    return list(map(get_seat_id, data.split("\n")))

def part1(data):
    seat_ids = get_seat_ids(data)
    return max(seat_ids)

def part2(data):
    seat_ids = get_seat_ids(data)
    seat_ids.sort()

    for i,id in enumerate(seat_ids):
        if i == 0:
            continue

        if seat_ids[i-1] != id - 1:
            return id - 1

    return None

tests = [
    ("05_test2", 357, part1),
    ("05_test1", 820, part1),
]
