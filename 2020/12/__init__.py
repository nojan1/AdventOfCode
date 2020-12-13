import math

def export_path(filename, positions):
    offset = 1000
    f = open(filename, "w")

    f.write(
        f'<svg viewBox="0 0 {offset * 2} {offset * 2}" xmlns="http://www.w3.org/2000/svg">')
    f.write('<circle cx="0" cy="0" r="3" fill="red"/>')

    for i in range(1, len(positions)):
        f.write(
            f'<line x1="{positions[i-1][0] + offset}" y1="{positions[i-1][1] + offset}" x2="{positions[i][0] + offset}" y2="{positions[i][1] + offset}" stroke="black" />')

    f.write("</svg>")

movements = {
    "N": (0, -1),
    "E": (1, 0),
    "S": (0, 1),
    "W": (-1, 0)
}

def parse_instructions(data):
    return [(x[0], int(x[1:])) for x in data.split("\n")]

def part1(data):
    def get_boat_path(instructions):
        headings = ["N", "E", "S", "W"]

        heading = 1
        positionX = 0
        positionY = 0

        yield (0, 0)

        for (instruction, amount) in instructions:
            if instruction == "L":
                heading = (heading - int(amount / 90)) % 4
            elif instruction == "R":
                heading = (heading + int(amount / 90)) % 4
            elif instruction == "F":
                headingKey = headings[heading]
                positionX += movements[headingKey][0] * amount
                positionY += movements[headingKey][1] * amount

                yield (positionX, positionY)
            else:
                change = movements[instruction]
                positionX += change[0] * amount
                positionY += change[1] * amount

                yield (positionX, positionY)

    instructions = parse_instructions(data)
    path = list(get_boat_path(instructions))
    return abs(path[-1][0]) + abs(path[-1][1])

def part2(data):
    def follow_waypoints(instructions):
        def rotate_waypoint(degrees, waypointX, waypointY):
            times = int(degrees / 90)
            for i in range(times):
                backup = waypointY
                waypointY = waypointX
                waypointX = -backup

            return waypointX, waypointY
        
        waypointX = 10
        waypointY = -1
        shipX = 0
        shipY = 0

        yield (0, 0)

        for (instruction, amount) in instructions:
            if instruction == "L":
                waypointX, waypointY = rotate_waypoint(360 - amount, waypointX, waypointY)
            elif instruction == "R":
                waypointX, waypointY = rotate_waypoint(amount, waypointX, waypointY)
            elif instruction == "F":
                shipX += waypointX * amount
                shipY += waypointY * amount

                yield(shipX, shipY)
            else:
                change = movements[instruction]
                waypointX += change[0] * amount
                waypointY += change[1] * amount

                print("Waypoint moved to ", waypointX, waypointY)


    instructions = parse_instructions(data)
    path = list(follow_waypoints(instructions))
    print(path)
    return abs(path[-1][0]) + abs(path[-1][1])


tests = [
    ("12_test1", 25, part1),
    ("12_test1", 286, part2),
    # ("<input file name minus txt>", <expected result>, part1),
]
