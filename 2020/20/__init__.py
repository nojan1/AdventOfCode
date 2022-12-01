def parse_input(data):
    def get_connector_code(rows):
        def pattern_to_int(pattern):
            return int(pattern.replace("#","1").replace(".","0"), 2)

        return [
            pattern_to_int(rows[0]),
            pattern_to_int("".join([x[0] for x in rows])),
            pattern_to_int("".join([x[-1] for x in rows])),
            pattern_to_int(rows[-1]),
        ]

    tiles = []
    current_tileid = None
    current_tile_rows = []

    for line in data.split("\n"):
        if line.startswith("Tile"):
            current_tileid = int(line[5:-1])
        elif line == "":
            tiles.append((current_tileid, get_connector_code(current_tile_rows), current_tile_rows))
            current_tile_rows = []
        else:
            current_tile_rows.append(line)

    return tiles

def solve(arrangement):
    pass

def part1(data):
    parsed_input = parse_input(data)
    print(parsed_input)
    return None

def part2(data):
    return None

tests = [
    ("20_test1", 20899048083289, part1),
    # ("<input file name minus txt>", <expected result>, part1),
]
