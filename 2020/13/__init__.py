from functools import reduce
import math, operator

def parse_input(data):
    lines = data.split("\n")
    return (int(lines[0]), [int(x) if x != "x" else None for x in lines[1].split(",")])

def part1(data):
    def get_actual_departure(earliest, buses):
        while True:
            earliest += 1
            for bus in buses:
                if bus == None:
                    continue

                if earliest % bus == 0:
                    return (earliest, bus)

    earliest, buses = parse_input(data)
    departure, bus = get_actual_departure(earliest, buses)
    diff = departure - earliest
    return bus * diff

def part2(data):
    def find_magic_timestamp(buses):
        pos = 0
        increment = 1
        for offset,time in enumerate(buses):
            if time == None:
                continue

            while (pos + offset) % time != 0:
                pos += increment

            increment *= time

        return pos

    _, buses = parse_input(data)
    return find_magic_timestamp(buses)

tests = [
    ("13_test1", 295, part1),
    ("13_test1", 1068781, part2),
    # ("<input file name minus txt>", <expected result>, part1),
]
